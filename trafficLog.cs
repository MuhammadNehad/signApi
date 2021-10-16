using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    public class trafficLog
    {
        [Key]
        public int Id { get; set; }
        public int fromLocId { get; set; }
        public int tolocId { get; set; }
        public Int64 duration { get; set; }
        public DateTime time { get; set; }


    }
}
