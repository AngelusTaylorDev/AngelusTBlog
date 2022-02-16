using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AngelusTBlog.Enums;

// My Code First Database build - Post Model
namespace AngelusTBlog.Models
{
    public class Post
    {
        // Post ID -  Primary Key
        public int Id { get; set; }

        // Blog ID - Forign key
        [Display(Name = "Blog Name")]
        public string BlogId { get; set; }

        // Post Author ID - Forign key
        public string AuthorID { get; set; }

        // Post Title - Required - min leng:8  max leng:100 - error message
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Title { get; set; }

        // Post Summary - Required - min leng:8  max leng:200 - error message
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string Summary { get; set; }

        // Post Content - Required
        [Required]
        public string Content { get; set; }

        // Post Creation time and date - using the display type Date - display name: Created Date
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        // Post Update time and date ( ? - Nullable ) - - using the display type Date - display name: Updated Date
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        // Is the blog post ready to be viewed by the public - using the ReadyStatus Enums
        [Display(Name = "Status")]
        public ReadyStatus ReadyStatus { get; set; }

        // Used for SEO and custom routing 
        public string Slug { get; set; }

        // Post Image File
        [Display(Name = "Image")]
        public byte[] ImageData { get; set; }

        // Post Image file Type
        public string ImageType { get; set; }

        // Selecting hte image using the IFormFile - for tthe backend server 
        [NotMapped]
        public IFormFile Image { get; set; }


        // Navigation Properties 
        // Reference the percent of Post - Blog (BlogId)
        public virtual Blog Blog { get; set; }

        // hold the record of AuthorID
        public virtual BlogUser Author { get; set; }

        // Child elements of the Post Class - a collection of Tags - and initializing a new list of Tags
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        // Child elements of the Post Class - a collection of Comments - and initializing a new list of Comments
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
