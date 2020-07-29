using System.ComponentModel.DataAnnotations;

namespace CrudDemo.DTO
{
    public class GlossaryItemAddDto
    {
        [Required]
        public string Term { get; set; }
        [Required]
        public string Definition { get; set; }
    }
}
