using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace locationRecordeapi
{
    [Table("permissions")]
    public class Permissions
    {
        [Key]
        public int Id { set; get; }

        public string name { set; get; }

        public string en_display_name { set; get; }
        public string ar_display_name { set; get; }
        [JsonIgnore]
        public ICollection<roles_perms_rel> _roles_perms_rel { get; set; }

    }
}
