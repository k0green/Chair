using Chair.BLL.Dto.ExecutorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public interface IExecutorServiceBusinessLogic
    {
        Task<List<ExecutorServiceDto>> GetAllServicesByExecutorId(string executorId);
        Task<ExecutorServiceDto> GetExecutorServiceById(Guid id);

        Task<Guid> AddAsync(AddExecutorServiceDto dto);

        Task UpdateAsync(UpdateExecutorServiceDto dto);

        Task RemoveAsync(Guid id);
    }
}
