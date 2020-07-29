using System.ComponentModel.DataAnnotations;

namespace CrudDemo.DTO
{
    public class GlossaryItemDto
    {
        public int Id { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
    }
}
