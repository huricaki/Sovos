using System.Text.Json.Serialization;

namespace SovosDocApi.Models
{
    public class InvoiceDto
    {
        [JsonPropertyName("InvoiceHeader")]
        public InvoiceHeader InvoiceHeader { get; set; }

        [JsonPropertyName("InvoiceLine")]
        public List<InvoiceLine> InvoiceLines { get; set; } 
    }
}
