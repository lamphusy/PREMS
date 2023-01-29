using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Organization")]
    public class Organization {
        [MaxLength(128)]
        [Key]
        public string IdOrganization { get; set; }

        [MaxLength(256)]
        [Required]
        [DisplayName("Organization name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        [DataType(DataType.MultilineText)]
        public string Information { get; set; }

        [DefaultValue(false)]
        public bool IsPaid { get; set; }

        public string LogoPath { get; set; }

        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string LinkedinLink { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<UserOwnOrganization> UserOwnOrganizations { get; set; }
       
    }
}
