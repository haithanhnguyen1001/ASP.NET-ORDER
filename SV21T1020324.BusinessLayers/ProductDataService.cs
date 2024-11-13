using SV21T1020324.DataLayers;
using SV21T1020324.DataLayers.SQLServer;
using SV21T1020324.DomainModels;
using SV21T1020324.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.BusinessLayers
{
    public class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng ( không phân trang )
        /// </summary>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List().ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice).ToList();
        }

        /// <summary>
        /// Lấy thông tin của 1 sản phẩm dựa vào mã sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Product? GetProduct(int productID)
        {
            if (productID <= 0)
            {
                return null;
            }
            return productDB.Get(productID);
        }

        /// <summary>
        /// Bổ sung 1 sản phẩm mới. Hàm trả về id của sản phẩm được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }

        /// <summary>
        /// Cập nhật thông tin của 1 sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }

        /// <summary>
        /// Xóa 1 sản phẩm dựa vào mã sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }

        /// <summary>
        /// Kiểm tra xem sản phẩm có mã í có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool IsUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }

        /// <summary>
        /// Lấy danh sách ảnh mặt hàng có mã id
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return productDB.ListPhotos(productID).ToList();
        }

        public static ProductPhoto? GetPhoto(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }

        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }

        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }

        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }

        public static List<ProductAttribute> ListAttributes(int productID)
        {
            return productDB.ListAttributes(productID).ToList();
        }

        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }

        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }

        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }

        public static ProductAttribute? GetAttribute(int attributeId)
        {
            if (attributeId <= 0)
            {
                return null;
            }
            return productDB.GetAttribute(attributeId);
        }
    }
}
