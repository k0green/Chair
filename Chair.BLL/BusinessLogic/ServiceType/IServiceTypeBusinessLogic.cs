using Chair.BLL.Dto.ExecutorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public interface IServiceTypeBusinessLogic
    {
        Task<List<ServiceTypeDto>> GetAllServiceTypes();
        Task<ServiceTypeDto> GetServiceTypeById(Guid id);
        Task<Guid> AddAsync(AddServiceTypeDto dto);
        Task UpdateAsync(ServiceTypeDto dto);
        Task RemoveAsync(Guid id);
    }
}
