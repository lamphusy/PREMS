using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class StudyListViewModel
    {
        public string IDClass { get; set; }

        public string ClassName { get; set; }

        public List<StudyViewModel> Students { get; set; }
    }
}
