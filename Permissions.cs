using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace locationRecordeapi
{
    [Table("permissions")]
    public class Permissions
    {
        [Key]
        public string Id { set; get; }

        public string name { set; get; }
    }
}
