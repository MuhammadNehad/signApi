using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace locationRecordeapi
{
    [Table("roles")]
    public class roles
    {
        [Key]
        public int Id { set; get; }
        public string name { set; get; }
    }
}
