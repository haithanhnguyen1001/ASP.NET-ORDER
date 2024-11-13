namespace SV21T1020324.Web.Models
{
    public class ProductSearchInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
    }
}