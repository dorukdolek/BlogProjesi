using BlogProjesi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogProjesi.ViewModels
{
    public class CommentViewModel
    {
        [ValidateNever]
        public Post Post { get; set; } = default!;

        [ValidateNever]
        public List<Comment> Comments { get; set; } = new();

        [Required(ErrorMessage = "Yorum alanı boş bırakılamaz.")]
        public string Content { get; set; } = string.Empty;
    }
}
