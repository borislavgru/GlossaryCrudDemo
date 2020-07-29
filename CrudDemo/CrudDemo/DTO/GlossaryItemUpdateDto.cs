using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.DTO
{
    public class GlossaryItemUpdateDto
    {
        [Required]
        public string Definition { get; set; }
    }
}
