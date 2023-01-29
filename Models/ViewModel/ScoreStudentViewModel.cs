using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ScoreStudentViewModel
    {
        public string IDSubject { get; set; }
        public string SubjectName { get; set; }
        public List<ScoreDetail> ScoreDetails { get; set; }

        public ScoreStudentViewModel()
        {
            ScoreDetails = new List<ScoreDetail>();
        }
    }
}
