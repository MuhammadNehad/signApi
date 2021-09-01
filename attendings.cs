using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    [Table("attendings")]
    public class attendings
    {
        [Key]
        public int Id { set; get; }
        public int empKey { set; get; }

        public int locationKey { set; get; }
        public DateTime atdt { set; get; }
        public bool entering { set; get; }

        public Int64 leaveAfter { set; get; }


    }
}
