using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHub.Models
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; } = default!;

        [DisplayName("Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Title")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "The title must have 5 to 80 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Invalid Creation Date")]
        [DisplayName("Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Invalid Modification Date")]
        [DisplayName("Modified At")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    
        [StringLength(1000, ErrorMessage = "Content must have at most 1000 characters")]
        public string Content { get; set; } = string.Empty;

        [DisplayName("Tags")]
        [StringLength(150, ErrorMessage = "The tags must have at most 150 characters.")]
        public string Tags { get; set; } = string.Empty;
    }
}