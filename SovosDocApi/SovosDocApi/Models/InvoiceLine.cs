using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class InvoiceLine
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("Id")]
    public int LineId { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("UnitCode")]
    public string UnitCode { get; set; } = string.Empty;


    [JsonPropertyName("UnitPrice")]
    public decimal UnitPrice { get; set; }

    // InvoiceHeader için ilişki kuruldu 
    [JsonIgnore]
    public string? InvoiceId { get; set; }
    [JsonIgnore]
    public InvoiceHeader? InvoiceHeader { get; set; } = null!;

}