using System;
using System.Collections.Generic;
using System.Text;

namespace Chair.DAL.Extension.Models
{
    public class SimpleFilter
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string SearchValue { get; set; }
    }
}
