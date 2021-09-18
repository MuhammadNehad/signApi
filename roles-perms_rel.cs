using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace locationRecordeapi
{
    [Table("roles_perms_rel")]

    public class roles_perms_rel
    {
        [Key]
        public int id { set; get; }
        public int role_id { set; get; }
        public int perm_id { set; get; }


    }
}
