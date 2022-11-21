using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public string IDStudent { get; set; }
        public string IDOrganization { get; set; }

        [Required]
        public string FullName { get; set; }
        public string CreateBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public string AvatarPath { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<Study> Studies { get; set; }
        public virtual ICollection<TotalScore> TotalScores { get; set; }
        public virtual ICollection<TotalScoreSubject> TotalScoreSubjects { get; set; }
        public virtual ICollection<ScoreDetail> ScoreDetails { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }


    }
}
