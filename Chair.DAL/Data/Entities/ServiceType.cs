using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chair.DAL.Data.Entities
{
    public class ServiceType : BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public ICollection<ExecutorService> ExecutorServices { get; set; }
    }
}
