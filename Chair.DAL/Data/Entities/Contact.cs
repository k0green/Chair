using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.DAL.Enums;

namespace Chair.DAL.Data.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public ContactsType Type { get; set; }
        public Guid ExecutorProfileId { get; set; }

        public ExecutorProfile Executor { get; set; }
    }
}
