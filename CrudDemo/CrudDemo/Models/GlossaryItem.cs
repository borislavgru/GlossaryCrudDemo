using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Models
{
    public class GlossaryItem
    {
        public int Id { get; set; }
        [Required, MaxLength(512)]
        public string Term { get; set; }
        [Required]
        public string Definition { get; set; }
    }
}
