using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BlogProjesi.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık boş bırakılamaz.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [ValidateNever]
        public User User { get; set; } = default!;

        [ValidateNever]
        public List<Comment> Comments { get; set; } = new();
        public int? CategoryId { get; set; } 

        [ValidateNever]
        public Category? Category { get; set; }


    }
}
