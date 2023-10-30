using CurrencyDemo;
using CurrencyDemo.Models;
using System.Text;
using System.Xml.Serialization;

InitializeEnviroment();

var mapper = MapperConfig.InitializeAutomapper();

Currency byn = new Currency { CurrencyId = "R01090B", CharCode = "BYN", Name = "Белорусский рубль", NumCode = "933", Value = 29.1323 };
Currency eur = new Currency { CurrencyId = "R01239", CharCode = "EUR", Name = "Евро", NumCode = "978", Value = 98.3504 };
Currency usd = new Currency { CurrencyId = "R01235", CharCode = "USD", Name = "Доллар США", NumCode = "840", Value = 93.2174 };
Currency azn = new Currency { CurrencyId = "R01020A", CharCode = "AZN", Name = "Азербайджанский манат", NumCode = "944", Value = 54.8338 };

var moneyAll = new List<Money>() { new Money(eur, 10), new Money(azn, 5), new Money(usd, 20) }.ToArray();

await Sum(byn, moneyAll);




// api method 
// в api можно было б положить CurrencyManager в DI и после использовать в методе 
async Task<CurrencySumDto> Sum(Currency resultCurrency, Money[] money)
{
    // получили все актуальные данные
    var valCurs = await GetActualValCurs();
    var availableCurrencies = mapper.Map<List<Currency>>(valCurs.Valute);
    var currencyManager = new CurrencyManager(availableCurrencies);

    double sum = currencyManager.Sum(resultCurrency, money); 


    var result = new CurrencySumDto
    {
        Sum = money,
        ResultCurrency = resultCurrency,
        ResultSum = sum
    };

    return result;
}


void InitializeEnviroment()
{
    // https://stackoverflow.com/questions/33579661/encoding-getencoding-cant-work-in-uwp-app
    // для работы с кириллицей в xml
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    Encoding.GetEncoding("windows-1251");
}

async Task<ValCurs> GetActualValCurs()
{
    string url = $"http://www.cbr.ru/scripts/XML_daily.asp";

    ValCurs valCurs = new ValCurs();

    using var _httpClient = new HttpClient();
    var response = await _httpClient.GetAsync(url);

    var xmlSerializer = new XmlSerializer(typeof(ValCurs));
    string sXml = await response.Content.ReadAsStringAsync();

    using var reader = new StringReader(sXml);
    valCurs = xmlSerializer.Deserialize(reader) as ValCurs;

    return valCurs;
}

class CurrencyManager
{
    private readonly List<Currency> _currencies;

    public CurrencyManager(List<Currency> currencies)
    {
        _currencies = currencies;
    }
    
    public double GetConversionValue(Currency currencyTo, Money money)
    {            
        var baseCurrency = _currencies.FirstOrDefault(c => c.CurrencyId == money.Currency.CurrencyId);        
        var selectedCurrency = _currencies.FirstOrDefault(c => c.CurrencyId == currencyTo.CurrencyId);

        if (baseCurrency is null || selectedCurrency is null)
            throw new ArgumentException($"Currency not found");

        var x = Math.Round(baseCurrency.Value / selectedCurrency.Value * money.Amount, 2);

        return Math.Round(baseCurrency.Value / selectedCurrency.Value * money.Amount, 2);
    }

    public double Sum(Currency currencyTo, params Money[] money)
    {
        double sum = 0;
        foreach (var moneyItem in money)
            sum += GetConversionValue(currencyTo, moneyItem);

        return Math.Round(sum, 2);
    }
}

