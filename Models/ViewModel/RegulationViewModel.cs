using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RegulationViewModel
    {
        public string IdOrganization { get; set; }
        public List<OShiftViewModel> ShiftList { get; set; }
        public List<OPeriodLessonViewModel> PeriodLessonList { get; set; }
        
    }
}
