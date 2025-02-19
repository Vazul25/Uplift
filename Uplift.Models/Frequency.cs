﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Uplift.Models
{
    public class Frequency
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName ("Frequency Name")]
        public String Name { get; set; }

        [Required]
        [DisplayName ("Frequency Count")]
        public int FrequencyCount { get; set; }
    }
}
