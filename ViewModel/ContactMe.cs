using System.ComponentModel.DataAnnotations;

namespace AngelusTBlog.ViewModel
{
    public class ContactMe
    {
        // The contact me form data.
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Subject { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Message { get; set; }
    }
}
