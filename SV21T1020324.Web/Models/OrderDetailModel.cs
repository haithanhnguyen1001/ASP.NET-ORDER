using SV21T1020324.DomainModels;

namespace SV21T1020324.Web.Models
{
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
