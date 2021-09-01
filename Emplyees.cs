using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    [Table("Emplyees")]
    public class Emplyees
    {
        [Key]
        public int id { set; get; }
        public string empCode { set; get; }
        public string email { set; get; }
        public string name { set; get; }
        public string phone { set; get; }
        [ForeignKey("EmpsLocation")]
        public int? locationKey { set; get; }


    }
}
