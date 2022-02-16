using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// My Code First Database build - Blog Model
namespace AngelusTBlog.Models
{
    public class Blog
    {
        // Blog ID -  Primary Key
        public int Id { get; set; }

        // Author ID - Forign key
        [Display(Name = "Author")]
        public string AuthorId { get; set; }

        // Blog Name - Required - min leng:8  max leng:100 - error message
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Name { get; set; }

        // Blog Description - Required - min leng:8  max leng:500 - error message
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Description { get; set; }

        // Blog Creation time and date - using the display type Date - display name: Created Date
        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }

        // Blog Update time and date ( ? - Nullable ) - - using the display type Date - display name: Updated Date
        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        // Blog Image File
        [Display(Name = "Blog Image")]
        public byte[] ImageData { get; set; }

        // Blog Image file Type
        [Display(Name = "Image type")]
        public string ImageType { get; set; }

        // Selecting hte image using the IFormFile - for tthe backend server 
        [NotMapped]
        public IFormFile Image { get; set; }


        // Navigation Properties 
        // Reference the percent of Blogs - Author (AutherId)
        public virtual BlogUser Author { get; set; }

        // Child elements of the Blog Class - a collection of Posts - and initializing a new list of Posts
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
