using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TeachViewModel
    {
        public int ID { get; set; }

        public string TeacherName { get; set; }

        public string WeekDay { get; set; }

        public string SubjectName { get; set; }
   
        //-----------//
        public string IDClass { get; set; }

        public int IDSchoolYear { get; set; }
      
        public int IDPeriod { get; set; }
    }
}
