using Chair.BLL.Dto.ExecutorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.BLL.Dto.Base;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension.Models;
using opr_lib;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public interface IExecutorServiceBusinessLogic
    {
        Task<List<ExecutorServiceDto>> GetAllServicesByExecutorId(Guid executorId);
        Task<List<GroupExecutorServiceDto>> GetAllServicesByTypeId(Guid executorId);
        Task<List<GroupExecutorServiceDto>> GetAllServices(FilterModelWithPeriods filter);
        Task<ExecutorServiceDto> GetExecutorServiceById(Guid id);

        Task<Guid> AddAsync(AddExecutorServiceDto dto);

        Task UpdateAsync(UpdateExecutorServiceDto dto);

        Task RemoveAsync(Guid id);
        Task<List<LookupDto>> GetAllServicesNamesByUserId();
        Task<List<LookupDto>> GetAllServicesNames();
        Task<ExecutorServiceDto> GetOptimizeService(FilterModelWithPeriods filter, Guid serviceTypeId, List<Condition> conditions);
    }
}
