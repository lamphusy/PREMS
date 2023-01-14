using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ClassViewModel
    {
        public SchoolYear SchoolYear { get; set; }
        public List<Class> Classes { get; set; }
    }
}
