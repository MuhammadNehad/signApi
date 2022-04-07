using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace locationRecordeapi
{
    [Table("role_perms_rel")]

    public class roles_perms_rel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public int role_id { set; get; }
        public int perm_id { set; get; }

        //[JsonIgnore]
        //public Permissions perm { get; set; }

        //[JsonIgnore] 
        //public roles role{ get; set; }

    }
}
