namespace SV21T1020324.DomainModels
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public String ContactName { get; set; } = string.Empty;
        public String Province { get; set; } = string.Empty;
        public String Address { get; set; } = string.Empty;
        public String Phone { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public string? SupplierName { get; set; }
        public string? Provice { get; set; }
    }
}