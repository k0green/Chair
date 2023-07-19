using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chair.DAL.Data.Entities
{
    public class ExecutorService : BaseEntity
    {

        public Guid ServiceTypeId { get; set; }
        public string ExecutorId { get; set; }
        public string Description { get; set; }

        public ServiceType ServiceType { get; set; }
        public User Executor { get; set; }
    }
}
