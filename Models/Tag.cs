using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

// My Code First Database build - Tag Model
namespace AngelusTBlog.Models
{
    public class Tag
    {
        // Tag ID -  Primary Key
        public int Id { get; set; }

        // post ID - Forign key
        public int PostId { get; set; }

        // Tag Author ID - Forign key
        public string AuthorID { get; set; }

        // Tag Text
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 5)]
        public string Text { get; set; }

        // Navigation Properties 

        // Reference the percent of Tags - Post (PostId)
        public virtual Post Post { get; set; }

        // hold the record of AuthorID
        public virtual BlogUser Author { get; set; }
    }
}
