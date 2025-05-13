using System.ComponentModel.DataAnnotations;

namespace BlogProjesi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new();
    }
}
