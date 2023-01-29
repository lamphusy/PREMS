using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ScoreTeacherViewModel
    {
        public string IDStudent { get; set; }
        public string StudentName { get; set; }
        public List<ScoreDetail> ScoreDetails { get; set; }

        public ScoreTeacherViewModel()
        {
            ScoreDetails = new List<ScoreDetail>();
        }
    }
}
