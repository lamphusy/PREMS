using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OrganizationViewModel
    {
        [MaxLength(128)]
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
        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }

        [DataType(DataType.MultilineText)]
        public string Information { get; set; }

        public string LogoPath { get; set; }

        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string LinkedinLink { get; set; }
    }
}
