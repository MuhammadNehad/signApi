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
        public int Id { set; get; }

        public string name { set; get; }
        public ICollection<roles_perms_rel> _roles_perms_rel { get; set; }

    }
}
