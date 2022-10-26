﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Semester
    {
        [Key]
        public int IdSemester { get; set; }
        [Required]
        public int SemesterNum { get; set; }
        [Required]
        public int LastYear { get; set; }
        [Required]
        public int NextYear { get; set; }
        [DefaultValue(false)]
        public bool IsNow { get; set; }
        [Required]
        public string IDOrganization { get; set; }

        [ForeignKey("IDOrganization")]
        public Organization Organization { get; set; }

    }
}