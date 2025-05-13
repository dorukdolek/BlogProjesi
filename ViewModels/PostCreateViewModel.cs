using System.ComponentModel.DataAnnotations;

namespace BlogProjesi.ViewModels
{
    public class PostCreateViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
    }
}
