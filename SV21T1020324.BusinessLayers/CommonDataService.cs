using SV21T1020324.DataLayers;
using SV21T1020324.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.BusinessLayers
{
    public static class CommonDataService
    {
        static readonly ICommonDAL<Province> provinceDB;
        static readonly ICommonDAL<Customer> customerDB;
        static readonly ICommonDAL<Supplier> supplierDB;
        static readonly ICommonDAL<Shipper> shipperDB;
        static readonly ICommonDAL<Employee> employeeDB;
        static readonly ICommonDAL<Category> categoryDB;
        static CommonDataService()
        {
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(Configuration.ConnectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(Configuration.ConnectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(Configuration.ConnectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(Configuration.ConnectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(Configuration.ConnectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(Configuration.ConnectionString);
        }

        //Province
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }

        //Customer
        public static List<Customer> ListOfCustomers( out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Customer> ListOfCustomers(string searchValue= "")
        {
            return customerDB.List(1, 0, searchValue).ToList();
        }

        public static Customer? GetCustomer(int id)
        {
            if (id <= 0)
                return null;
            return customerDB.Get(id);
        }

        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }

        public static bool UpdateCustomer(Customer data) 
        {
            return customerDB.Update(data);
        } 

        public static bool DeleteCustomer(int id)
        {
            return customerDB.Delete(id);
        }

        public static bool IsUsedCustomer(int id)
        {
            return customerDB.InUsed(id);
        }

        //Employee
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Employee> ListOfEmployees(string searchValue = "")
        {
            return employeeDB.List(1, 0, searchValue).ToList();
        }

        public static Employee? GetEmployee(int id)
        {
            if (id <= 0)
                return null;
            return employeeDB.Get(id);
        }

        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }

        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }

        //Supplier
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Supplier> ListOfSuppliers(string searchValue = "")
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }

        public static Supplier? GetSupplier(int id)
        {
            if (id <= 0)
                return null;
            return supplierDB.Get(id);
        }

        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }

        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }

        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }

        //Shipper
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Shipper> ListOfShippers(string searchValue = "")
        {
            return shipperDB.List(1, 0, searchValue).ToList();
        }

        public static Shipper? GetShipper(int id)
        {
            if (id <= 0)
                return null;
            return shipperDB.Get(id);
        }

        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }

        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }

        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }

        public static bool IsUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }

        //Category
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Category> ListOfCategories(string searchValue = "")
        {
            return categoryDB.List(1, 0, searchValue).ToList();
        }

        public static Category? GetCategory(int id)
        {
            if (id <= 0)
                return null;
            return categoryDB.Get(id);
        }

        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }

        public static bool IsUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }
    }
}
