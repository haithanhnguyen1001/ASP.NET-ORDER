using Dapper;
using SV21T1020324.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.DataLayers.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {

        }

        public int Add(Supplier data)
        {
            int id = 0;
            using (var connection = OpenConection())
            {
                var sql = @"INSERT INTO Suppliers(SupplierName, ContactName, Provice, Address, Phone, Email, Logo)
                            VALUES(@SupplierName, @ContactName, @Provice, @Address, @Phone, @Email, @Logo);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Provice = data.Provice ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Logo = data.Logo ?? ""
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConection())
            {
                var sql = @"select count(*) 
		                    from Suppliers
		                    where (SupplierName like @searchValue) or (ContactName like @searchValue)";
                var parameters = new { searchValue = $"%{searchValue}%" };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"DELETE FROM Suppliers WHERE SupplierId = @SupplierId";
                var parameters = new
                {
                    SupplierId = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Supplier? Get(int id)
        {
            Supplier? data = null;
            using (var connection = OpenConection())
            {
                var sql = @"SELECT * FROM Suppliers WHERE SupplierId = @SupplierId";
                var parameters = new { SupplierId = id };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"IF EXISTS(SELECT * FROM Products WHERE SupplierId = @SupplierId)
                                SELECT 1
                            ELSE 
                                SELECT 0";
                var parameters = new
                {
                    SupplierId = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data = new List<Supplier>();
            using (var connection = OpenConection())
            {
                var sql = @"select *
                            from(
                                    select * ,
				                       ROW_NUMBER() over (order by SupplierName) as RowNumber
		                            from Suppliers
		                            where (SupplierName like @searchValue) or (ContactName like @searchValue)
	                            ) as t
                            where (@pageSize = 0)
	                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber;";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue = $"%{searchValue}%"
                };
                data = connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public bool Update(Supplier data)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"UPDATE Suppliers
                            SET SupplierName = @SupplierName,
                                ContactName = @ContactName,
                                Provice = @Provice,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                Logo = @Logo,
                            WHERE SupplierId = @SupplierId";
                var parameters = new
                {
                    SupplierId = data.SupplierID,
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Provice = data.Provice ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Logo = data.Logo ?? ""
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }

}
