using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GimpySoftwareNewTemplate.Models
{
    public class CompanyParameters
    {
        public String Name { get; set; }
        public String Country { get; set; }
        public String Region { get; set; }
        public List<int> Genres { get; set; }
        public List<int> Categories { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
    }
}