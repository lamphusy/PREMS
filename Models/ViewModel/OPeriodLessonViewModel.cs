using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OPeriodLessonViewModel
    {
        
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
        [Required]

        public int PeriodStartTime { get; set; }
        [Required]
        public int PeriodEndTime { get; set; }

        public int IDShift { get; set; }

        [ForeignKey("IDShift")]
        public virtual OShift OShift { get; set; }
    }
}
