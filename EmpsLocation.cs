using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    [Table("EmpsLocation")]
    public class EmpsLocation
    {
        [Key]
        public int Id { set; get; }
        public Double latitude { set; get; }
        public Double lngtude { set; get; }
        public string address { set; get; }

        //public Emplyees emp { set; get; }
    }
}
