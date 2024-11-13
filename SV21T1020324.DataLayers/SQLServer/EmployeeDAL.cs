using Dapper;
using SV21T1020324.DomainModels;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.DataLayers.SQLServer
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConection())
            {
                var sql = @"INSERT INTO Employees(FullName, BirthDate, Address, Phone, Email, Photo, IsWorking)
                            VALUES(@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
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
		                    from Employees
		                    where (FullName like @searchValue)";
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
                var sql = @"DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                var parameters = new
                {
                    EmployeeId = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;

        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConection())
            {
                var sql = @"SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
                var parameters = new { EmployeeId = id };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;

        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"IF EXISTS(SELECT * FROM Orders WHERE EmployeeId = @EmployeeId)
                                SELECT 1
                            ELSE 
                                SELECT 0";
                var parameters = new
                {
                    EmployeeId = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            using (var connection = OpenConection())
            {
                var sql = @"select *
                            from(
                                    select * ,
				                       ROW_NUMBER() over (order by FullName) as RowNumber
		                            from Employees
		                            where (FullName like @searchValue)
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
                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;

        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConection())
            {
                var sql = @"UPDATE Employees
                            SET FullName = @FullName,
                                BirthDate = @BirthDate,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                Photo = @Photo,
                                IsWorking = @IsWorking
                            WHERE EmployeeId = @EmployeeId";
                var parameters = new
                {
                    EmployeeId = data.EmployeeID,
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
