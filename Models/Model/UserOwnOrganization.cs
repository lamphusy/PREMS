using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserOwnOrganization
    {
        [Key]
        [Column(Order =1)]
        public string IdORegister { get; set; }
        [Key]
        [Column(Order = 2)]
        public string IdOrganization { get; set; }

        [ForeignKey("IdORegister")]
        [JsonIgnore]
        public virtual ORegister ORegister { get; set; }
        [ForeignKey("IdOrganization")]
        [JsonIgnore]
        public virtual Organization Organization { get; set; }
    }
}
