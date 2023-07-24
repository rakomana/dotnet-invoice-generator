namespace learnApi.Models
{
    public class Quotation
    {
        public int Id {get; set;}
        public string? EntityName {get; set;}
        public string? EntityDescription {get; set;}
        public string? DateCreated {get; set;}
        public string? QuotationNumber {get; set;}
        public int UserId {get; set;}
        public User? User {get; set;}
    }
}