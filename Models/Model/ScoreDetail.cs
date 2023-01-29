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
    [Table("ScoreDetail")]
    public class ScoreDetail
    {
        [Key]
        public int ID { get; set; }
   
        public string IDStudent { get; set; }

        public string IDSubject { get; set; }

        public int IDSemester { get; set; }
        [Required]

        public int IDScoreType { get; set; }
        [DefaultValue(0)]
        public float Score { get; set; }

        [ForeignKey("IDStudent")]
        [JsonIgnore]
        virtual public Student Student { get; set; }
        [ForeignKey("IDSubject")]
        [JsonIgnore]
        virtual public Subject Subject { get; set; }
        [ForeignKey("IDSemester")]
        [JsonIgnore]
        virtual public Semester Semester { get; set; }
        [ForeignKey("IDScoreType")]
        [JsonIgnore]
        public TypeScore TypeScore { get; set; }


    }
}
