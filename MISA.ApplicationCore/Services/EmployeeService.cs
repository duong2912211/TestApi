using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;   
        }
        #endregion

        #region Method

        public IEnumerable<Employee> GetEmployees()
        {
            var employees = _employeeRepository.GetEmployees();
            return employees;
        }
        public ServiceResult GetEmployeeById(Guid employeeId)
        {
            var serviceResult = new ServiceResult();
            serviceResult.Data = _employeeRepository.GetEmployeeById(employeeId);
            return serviceResult;
        }
        public ServiceResult GetEmployeeByCode(string employeeCode)
        {
            var serviceResult = new ServiceResult();
            serviceResult.Data = _employeeRepository.GetEmployeeByCode(employeeCode);
            return serviceResult;
        }
        public ServiceResult AddEmployee(Employee employee)
        {
            var serviceResult = new ServiceResult();
            //Validate dữ liệu:
            //Check trường bắt buộc nhập, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi 
            var employeeCode = employee.EmployeeCode;
            var employeeName = employee.EmployeeName;
            //var employeeIdCard = employee.IdCard;
            //var employeeEmail = employee.Email;
            //var employeePhoneNumber = employee.PhoneNumber;
            //var employeePositionId = employee.PositionId;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeId", msg = "Mã nhân viên không được phép để trống " },
                    userMsg = "Mã nhân viên không được phép để trống ",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = " Mã nhân viên không được để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }
            else if(string.IsNullOrEmpty(employeeName))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeName", msg = "Tên nhân viên không được phép để trống " },
                    userMsg = "Tên nhân viên không được phép để trống ",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = " Tên nhân viên không được để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }    

            //Check trùng mã 
            var res = _employeeRepository.GetEmployeeByCode(employeeCode);
            if (res != null)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã nhân viên đã tồn tại" },
                    userMsg = "Mã nhân viên đã tồn tại",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = "Mã nhân viên đã tồn tại";
                serviceResult.Data = msg;
                return serviceResult;

            }

            // Thêm mới khi dữ liệu đã hợp lệ
            var rowAffects = _employeeRepository.AddEmployee(employee);
            serviceResult.MISACode = MISACode.IsValid;
            serviceResult.Messenger = "Thêm thành công";
            serviceResult.Data = rowAffects;
            return serviceResult;
        }
        public ServiceResult UpdateEmployee(Employee employee)
        {
            var serviceResult = new ServiceResult();
            //Validate dữ liệu:
            //Check trường bắt buộc nhập, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi 
            var employeeCode = employee.EmployeeCode;
            var employeeName = employee.EmployeeName;
            //var employeeIdCard = employee.IdCard;
            //var employeeEmail = employee.Email;
            //var employeePhoneNumber = employee.PhoneNumber;
            //var employeePositionId = employee.PositionId;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeId", msg = "Mã nhân viên không được phép để trống " },
                    userMsg = "Mã nhân viên không được phép để trống ",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = " Mã nhân viên không được để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }
            else if (string.IsNullOrEmpty(employeeName))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeName", msg = "Tên nhân viên không được phép để trống " },
                    userMsg = "Tên nhân viên không được phép để trống ",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = " Tên nhân viên không được để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }

            //Check trùng mã 
            var res = _employeeRepository.GetEmployeeByCode(employeeCode);
            if (res != null)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã nhân viên đã tồn tại" },
                    userMsg = "Mã nhân viên đã tồn tại",
                    Code = MISACode.NotValid,
                };
                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = "Mã nhân viên đã tồn tại";
                serviceResult.Data = msg;
                return serviceResult;
            }
            var rowAffects = _employeeRepository.UpdateEmployee(employee);
            serviceResult.MISACode = MISACode.IsValid;
            serviceResult.Messenger = "Thêm thành công";
            serviceResult.Data = rowAffects;
            return serviceResult;
        }
        public ServiceResult DeleteEmployee(Guid employeeId)
        {
            var serviceResult = new ServiceResult();
            serviceResult.Data = _employeeRepository.DeleteEmployee(employeeId);
            return serviceResult;
        }
        #endregion
    }
}
