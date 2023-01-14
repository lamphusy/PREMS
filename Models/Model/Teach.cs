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
    [Table("Teach")]
    public class Teach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string IDTeacher { get; set; }

        public string IDClass { get; set; }

        public int IDSchoolYear { get; set; }
        [Required]
        public int IDPeriod { get; set; }
        [Required]
        public int WeekDay { get; set; }

        public string IDSubject { get; set; }

        [JsonIgnore]
        [ForeignKey("IDSchoolYear")]
        public virtual SchoolYear SchoolYear { get; set; }
        [JsonIgnore]
        [ForeignKey("IDSubject")]
        public virtual Subject Subject { get; set; }
        [JsonIgnore]
        [ForeignKey("IDClass")]
        public virtual Class Class { get; set; }
        [JsonIgnore]
        [ForeignKey("IDTeacher")]
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        [ForeignKey("IDPeriod")]
        public virtual OPeriodLesson Period { get; set; }
    }
}
