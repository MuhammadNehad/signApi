using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace locationRecordeapi
{
    [Table("roles")]
    public class roles
    {
        [Key]
        public int Id { set; get; }
        public string name { set; get; }
        [Column("parent_id")]
        public int? proleId { set; get; }
        //[JsonIgnore]
        //public List<roles_perms_rel> _roles_perms_rel { get; set; }
        //[JsonIgnore]
        //public Emplyees emplyees { get; set; }
        //[JsonIgnore]
        //public roles prole { get; set; }
    }
}
