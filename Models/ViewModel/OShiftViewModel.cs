using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OShiftViewModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string ShiftName { get; set; }
        [Required]
        public int ShiftStartTime { get; set; }

        [Required]
        public int ShiftEndTime { get; set; }

      
    }
}
