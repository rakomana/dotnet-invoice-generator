namespace learnApi.Models
{
    public class Item
    {
        public int Id {get; set;}
        public string? ItemDescription {get; set;}
        public string? ItemQuantity {get; set;}
        public string? ItemPrice {get; set;}
        public string? ItemDiscount {get; set;}
        public string? DateCreated {get; set;}
        public int QuotationId {get; set;}
        public Quotation? Quotation {get; set;}
    }
}