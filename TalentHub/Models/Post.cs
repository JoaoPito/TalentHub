using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TalentHub.Models
{
    public class Post
    {
        [Key]
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The title is required.")]
        [DisplayName("Title")]
        [StringLength(80, ErrorMessage = "The title must have at most 80 characters.")]
        [MinLength(1, ErrorMessage = "Title must not be empty.")]
        public string Title { get; set; } = string.Empty;

        [DisplayName("Description")]
        [StringLength(120, ErrorMessage = "Description should have at most 120 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Post has no content")]
        [StringLength(1000, ErrorMessage = "The post's content should have at most 1000 characters.")]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [DisplayName("Created At")]
        [Required(ErrorMessage = "Invalid Creation Date.")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Modified At")]
        [Required(ErrorMessage = "Invalid Modification Date.")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}