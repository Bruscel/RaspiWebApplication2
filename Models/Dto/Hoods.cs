using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspiWebApplication2.Models.Dto
{
    public class Hoods
    {
        public string neighborhood_name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string feet_radius { get; set; }
    }
}
