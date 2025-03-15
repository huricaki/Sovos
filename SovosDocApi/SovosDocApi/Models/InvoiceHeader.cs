

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class InvoiceHeader
{
    [Key]
    [JsonPropertyName("InvoiceId")]
    public string? InvoiceId { get; set; } 

    [JsonPropertyName("SenderTitle")]
    public string? SenderTitle { get; set; }

    [JsonPropertyName("ReceiverTitle")]
    public string? ReceiverTitle { get; set; } 

    [JsonPropertyName("Date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("EMail")]
    public string? Email { get; set; } 

    [JsonIgnore]
    public bool IsSendMail { get; set; } = false;

    // InvoiceLine için 1-n ilişki
    [JsonIgnore]
    public List<InvoiceLine>? InvoiceLines { get; set; } = new();
}
