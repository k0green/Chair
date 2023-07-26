﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chair.DAL.Data.Entities
{
    public class Image : BaseEntity
    {
        public string URL { get; set; }
        public Guid? ObjectId { get; set; }

        public ExecutorService ExecutorService { get; set; }
    }
}