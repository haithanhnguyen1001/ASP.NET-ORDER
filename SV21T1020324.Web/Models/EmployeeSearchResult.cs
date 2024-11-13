using SV21T1020324.DomainModels;

namespace SV21T1020324.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm nhân viên
    /// </summary>
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
