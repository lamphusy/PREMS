using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class SemesterViewModel
    {
        public int IdSemester { get; set; }

        public int SemesterNum { get; set; }

        public bool IsNow { get; set; }

        public string IDOrganization { get; set; }
        [DefaultValue(true)]
        public bool CanDelete { get; set; }

       
    }
}
