using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace locationRecordeapi
{
    [Table("roles")]
    public class roles
    {
        [Key]
        public int Id { set; get; }
        public string name { set; get; }
        [JsonIgnore]
        public List<roles_perms_rel> _roles_perms_rel { get; set; }
        [JsonIgnore]
        public Emplyees emplyees { get; set; }
    }
}
