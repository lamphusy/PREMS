using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ScoreViewModel
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public List<float> Scores { get; set; }
        public List<string> Subjects { get; set; }

        public ScoreViewModel()
        {
            Scores = new List<float>();
            Subjects = new List<string>();
        }
    }
}
