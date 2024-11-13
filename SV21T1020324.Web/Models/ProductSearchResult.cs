using SV21T1020324.DomainModels;

namespace SV21T1020324.Web.Models
{
    public class ProductSearchResult
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; } = 0;
        public int PageCount
        {
            get
            {
                if (PageSize <= 0)
                    return 1;

                int n = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    n++;
                }
                return n;
            }
        }
        public int CategoryId { get; set; } = 0;
        public int SupplierId { get; set; } = 0;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = 0;
        public required List<Product> Data { get; set; }
    }
}
