using Dapper;
using SV21T1020324.DataLayers.SQLServer;
using SV21T1020324.DataLayers;
using SV21T1020324.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL, IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Order data)
        {
            int id = 0;
            using (var connection = OpenConection())
            {
                var sql = @"INSERT INTO Orders(CustomerId, OrderTime, DeliveryProvince, DeliveryAddress, EmployeeID, Status)
                            VALUES(@CustomerID, GETDATE(), @DeliveryProvince, @DeliveryAddress, @EmployeeID, @Status);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    CustomerID = data.CustomerID,
                    DeliveryProvince = data.DeliveryProvince ?? "",
                    DeliveryAddress = data.DeliveryAddress ?? "",
                    EmployeeID = data.EmployeeID,
                    Status = 1,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConection())
            {
                var sql = @"select count(*)
                        from Orders as o
                             left join Customers as c on o.CustomerID = c.CustomerID
                             left join Employees as e on o.EmployeeID = e.EmployeeID
                             left join Shippers as s on o.ShipperID = s.ShipperID
                        where (@Status = 0 or o.Status = @Status)
                            and (@FromTime is null or o.OrderTime >= @FromTime)
                            and(@ToTime is null or o.OrderTime <= @ToTime)
                            and(@SearchValue = N''
                                or c.CustomerName like @SearchValue
                                or e.FullName like @SearchValue
                                or s.ShipperName like @SearchValue)";
                var parameters = new
                {
                    status,
                    fromTime,
                    toTime,
                    searchValue = $"%{searchValue}%",
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int orderID)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"delete from OrderDetails where OrderID = @OrderID;
                        delete from Orders where OrderID = @OrderID";
                var parameters = new { OrderID = orderID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteDetail(int orderID, int productID)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"delete from OrderDetails
                        where OrderID = @OrderID and ProductID = @ProductID";
                var parameters = new { OrderID = orderID, ProductID = productID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Order? Get(int orderID)
        {
            Order? data = null;
            using (var connection = OpenConection())
            {
                var sql = @"select o.*,
                            c.CustomerName,
                            c.ContactName as CustomerContactName,
                            c.Address as CustomerAddress,
                            c.Phone as CustomerEmail,
                            c.Email as CustomerEmail,
                            e.FullName as EmployeeName,
                            s.ShipperName,
                            s.Phone as ShipperPhone
                        from Orders as o   
                             left join Customers as c on o.CustomerID = c.CustomerID
                             left join Employees as e on o.EmployeeID = e.EmployeeID
                             left join Shippers as s on o.ShipperID = s.ShipperID
                        where o.OrderID = @OrderID";

                var parameters = new { OrderID = orderID };
                data = connection.QueryFirstOrDefault<Order>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public OrderDetail? GetDetail(int orderID, int productID)
        {
            OrderDetail? data = null;
            using (var connection = OpenConection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit
                        from OrderDetails as od
                             join Products as p on od.ProductID = p.ProductID
                        where od.OrderID = @OrderID and od.ProductID = @ProductID";
                var parameters = new { OrderID = orderID, ProductID = productID };
                data = connection.QueryFirstOrDefault<OrderDetail>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Order> List(int page = 1, int pageSize = 0,
                                int status = 0, DateTime? fromTime = null, DateTime? toTime = null,
                                string searchValue = "")
        {
            List<Order> list = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConection())
            {
                var sql = @"with cte as
                (
                    select row_number() over(order by o.OrderTime desc) as RowNumber,
                            o.*,
                            c.CustomerName,
                            c.ContactName as CustomerContactName,
                            c.Address as CustomerAddress,
                            c.Phone as CustomerPhone,
                            c.Email as CustomerEmail,
                            e.FullName as EmployeeName,
                            s.ShipperName,
                            s.Phone as ShipperPhone
                    from Orders as o
                            left join Customers as c on o.CustomerID = c.CustomerID
                            left join Employees as e on o.EmployeeID = e.EmployeeID
                            left join Shippers as s on o.ShipperID = s.ShipperID
                    where (@Status = 0 or o.Status = @Status)
                        and (@FromTime is null or o.OrderTime >= @FromTime)
                        and (@ToTime is null or o.OrderTime <= @ToTime)
                        and (@SearchValue = N''
                            or c.CustomerName like @SearchValue
                            or e.FullName like @SearchValue
                            or s.ShipperName like @SearchValue)
                 )
                 select * from cte
                 where (@PageSize = 0)
                    or (RowNumber between (@Page - 1) * @PageSize + 1 and @Page * @PageSize)
                 order by RowNumber";
                var parameters = new
                {
                    page,
                    pageSize,
                    status,
                    fromTime,
                    toTime,
                    searchValue = $"%{searchValue}%"
                };
                list = connection.Query<Order>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return list;
        }

        public IList<Order> List(int page = 1, int pageSize = 0, string searchValues = "")
        {
            throw new NotImplementedException();
        }
        public IList<OrderDetail> ListDetails(int orderID)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            using (var connection = OpenConection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit
                        from OrderDetails as od
                            join Products as p on od.ProductID = p.ProductID
                        where od.OrderID = @OrderID";
                var parameters = new { OrderID = orderID };
                list = connection.Query<OrderDetail>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return list;
        }

        public bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"if exists(select * from OrderDetails
                                    where OrderID = @OrderID and ProductID = @ProductID)
                            update OrderDetails
                            set Quantity = @Quantity,
                                SalePrice = @SalePrice
                            where OrderID = @OrderID and ProductID = @ProductID
                        else
                            insert into OrderDetails(OrderID, ProductID, Quantity, SalePrice)
                            values(@OrderID, @ProductID, @Quantity, @SalePrice)";
                var parameters = new { OrderID = orderID, ProductID = productID, Quantity = quantity, SalePrice = salePrice };
                result = connection.ExecuteScalar<bool>(sql, parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public bool Update(Order data)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"update Orders
                        set CustomerID = @CustomerID,
                            OrderTime = @OrderTime,
                            DeliveryProvince = @DeliveryProvince,
                            DeliveryAddress = @DeliveryAddress,
                            EmployeeID = @EmployeeID,
                            AcceptTime = @AcceptTime,
                            ShipperID = @ShipperID,
                            ShippedTime = @ShippedTime,
                            FinishedTime = @FinishedTime,
                            Status = @Status
                        where OrderID = @OrderID";
                var parameters = new
                {
                    data.OrderID,
                    data.CustomerID,
                    data.OrderTime,
                    data.DeliveryProvince,
                    data.DeliveryAddress,
                    data.EmployeeID,
                    data.AcceptTime,
                    data.ShipperID,
                    data.FinishedTime,
                    data.Status,
                    data.ShippedTime
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}