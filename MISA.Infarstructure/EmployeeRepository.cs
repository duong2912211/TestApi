using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MISA.Infarstructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        #endregion

        #region Constructor
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Method

        /// <summary>
        /// Lấy ra thông tin phòng ban
        /// </summary>
        /// <returns></returns>
        /// CreatedBy : NPDUONG (12/01/2021)
        public IEnumerable<Employee>GetEmployees()
        {
            //Kết nối tới CSDL
            //Khởi tạo các CommandText
            var employees = _dbConnection.Query<Employee>("Proc_GetEmployees", commandType: CommandType.StoredProcedure);
            //Trả về dữ liệu
            return employees;
        }

        /// <summary>
        /// Lấy ra thông tin phòng ban qua ID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// CreatedBy :NPDUONG (12/01/2021)
        public Employee GetEmployeeById(Guid employeeId)
        {
            //Khởi tạo các CommandText
            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeeById", new { EmployeeId = employeeId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            //Trả về dữ liệu
            return employees;
        }

        /// <summary>
        /// Láy thông tin phòng ban qua mã phòng ban
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        /// CreatedBy : NPDUONG (12/01/2021)
        public Employee GetEmployeeByCode(string employeeCode)
        {
            //Khởi tạo các CommandText
            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeeByCode", new { EmployeeCode = employeeCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            //Trả về dữ liệu
            return employees;
        }

        /// <summary>
        /// Thêm mới phòng ban
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// CreatedBy : NPDUONG (12/01/2021)
        public int AddEmployee(Employee employee)
        {
            //Khởi tạo kết nối tới CSDL
            var param = MappingDbType(employee);
            //Thực thi commandText
            var rowAffects = _dbConnection.Execute("Proc_InsertEmployee", param, commandType: CommandType.StoredProcedure);
            //Trả về kết quả ()
            return rowAffects;
        }

        /// <summary>
        /// Sửa thông tin phòng ban
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// CreatedBy : NPDUONG (12/01/2021)
        public int UpdateEmployee(Employee employee)
        {
            //Khởi tạo kết nối tới CSDL
            var param = MappingDbType(employee);
            //Thực thi commandText
            var rowAffects = _dbConnection.Execute("Proc_UpdateEmployee", param, commandType: CommandType.StoredProcedure);
            //Trả về kết quả ()
            return rowAffects;
        }

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// CreatedBy : NPDUONG (12/01/2021)
        public int DeleteEmployee(Guid employeeId)
        {
            //Khởi tạo các CommandText
            var res = _dbConnection.Execute("Proc_DeleteEmployee", new { EmployeeId = employeeId }, commandType: CommandType.StoredProcedure);
            //Trả về dữ liệu
            return res;
        }

        /// <summary>
        /// Đinh dạng dữ liệu 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DynamicParameters MappingDbType<TEntity>(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var param = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    param.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    param.Add($"@{propertyName}", propertyValue);
                }
            }
            return param;
        }
        #endregion
    }
}
