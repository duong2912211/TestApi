using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity:BaseEntity
    {
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult;
        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = Enums.MISACode.Success };
        }
        #endregion
        #region Method
        //lấy danh sách phòng ban
        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _baseRepository.GetEntities();
            return entities;
        }

        //Lấy thông tin phòng ban theo Id 
        public TEntity GetEntityById(Guid entityId)
        {
            
         var entities = _baseRepository.GetEntityById(entityId);
            return entities;
            
        }

        //Lấy thông tin phòng ban theo mã phòng ban
        public TEntity GetEntityByCode(string entityCode)
        {
            var entities = _baseRepository.GetEntityByCode(entityCode);
            return entities;
        }
        //Thêm mới phòng ban
        public virtual ServiceResult Add(TEntity entity)
        {
            entity.EntityState = Enums.EntityState.AddNew;  
            ServiceResult serviceResult = new ServiceResult();
            //Thực hiện validate:
            var isValidate = Validate(entity);
            if(isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Add(entity);
                _serviceResult.MISACode = Enums.MISACode.IsValid;
                return _serviceResult;
            }
            else
            {
                return _serviceResult;
            }
        }

        //Sửa thông tin phòng ban
        public ServiceResult Update(TEntity entity)
        {
            entity.EntityState = Enums.EntityState.Update;
            ServiceResult serviceResult = new ServiceResult();
            //Thực hiện validate:
            var isValidate = Validate(entity);
            if (isValidate == true)
            {
                serviceResult.Data = _baseRepository.Update(entity);
                serviceResult.MISACode = Enums.MISACode.IsValid;
                return serviceResult;
            }
            else
            {
                serviceResult.Data = _baseRepository.Update(entity);
                serviceResult.MISACode = Enums.MISACode.NotValid;
                return serviceResult;
            }
        }

        //Xóa phòng ban
        public ServiceResult Delete(Guid entityId)
        {
            _serviceResult.Data = _baseRepository.Delete(entityId);
            return _serviceResult;
        }

        private bool Validate(TEntity entity)
        {
            var messError = new List<string>();
            var isValidate = true;
            var serviceResult = new ServiceResult();
            //đọc các properties
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entity);
                var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                //Ktra xem có attribute cần validate ko 
                if (property.IsDefined(typeof(Required), false))
                {
                    //Check bắt buộc nhập
                    if (propertyValue == null)
                    {
                        isValidate = false;
                        messError.Add($"Thông tin {displayName} không được để trống ");
                        _serviceResult.MISACode = Enums.MISACode.NotValid;
                        _serviceResult.Messenger = "Dữ liệu không hợp lệ";
                    }
                }
                else if (property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //Check trùng dữ liệu 
                    var propertyName = property.Name;
                    var entityDuplicate = _baseRepository.GetEntityByProperty(entity,property);
                        if(entityDuplicate != null)
                        {
                            isValidate = false;
                            messError.Add($"Thông tin {displayName} đã có trên hệ thống ");
                            _serviceResult.MISACode = Enums.MISACode.NotValid;
                            _serviceResult.Messenger = "Dữ liệu không hợp lệ";
                    }    
                 }
            }
            _serviceResult.Data = messError;
            return isValidate;
        }
        #endregion
    }
}
