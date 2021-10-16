using Newtonsoft.Json;
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

        [Column("id")]
        public int id { set; get; }
        
        [Column("empCode")]
        public string empCode { set; get; }
        [Column("email")]
        public string email { set; get; }
        [Column("name")]
        public string name { set; get; }
        [Column("password")]
        public string password { set; get; }
        [Column("phone")]
        public string phone { set; get; }
        [Column("locationKey")]
        public Nullable<int> locationKey { set; get; }
        [Column("role")]
        public Nullable<int> role { set; get; }
        [JsonIgnore]
        public roles _role { get; set; }

    }
}
