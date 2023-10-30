// модель актуальной валюты
class Currency
{
    public string CurrencyId { get; set; }    // уникальный код валюты, поэтому без Guid
    public string Name { get; set; }
    
    public string NumCode { get; set; }
    public string CharCode { get; set; }
    public double Value { get; set; }
}

