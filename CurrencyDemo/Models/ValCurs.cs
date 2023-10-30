using System.Xml.Serialization;

namespace CurrencyDemo.Models;

[XmlRoot(ElementName = "ValCurs")]
public class ValCurs
{

    [XmlElement(ElementName = "Valute")]
    public List<Valute> Valute { get; set; }

    [XmlAttribute(AttributeName = "Date")]
    public string Date { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
}
