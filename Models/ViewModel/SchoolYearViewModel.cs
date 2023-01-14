using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class SchoolYearViewModel
    {
        public SchoolYear SchoolYear { get; set; }

        public List<SemesterViewModel> Semesters { get; set; }
    }
}
