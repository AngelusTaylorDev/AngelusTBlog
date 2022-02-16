using AngelusTBlog.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


// My Code First Database build - Comment Model
namespace AngelusTBlog.Models
{
    public class Comment
    {
        // Comment ID -  Primary Key
        public int Id { get; set; }

        // Post ID - Forign key
        [Display(Name = "Post")]
        public int PostId { get; set; }

        // Post Author ID - Forign key
        public string AuthorID { get; set; }

        // Post Moderator ID - Forign key
        public string ModeratorID { get; set; }

        // Comment - Required - min leng:8  max leng:100 - error message
        [Required]
        [Display(Name = "Comment")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string CommentBody { get; set; }

        // Comment Creation time and date - using the display type Date
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        // Comment Update time and date ( ? - Nullable ) - - using the display type Date
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        // Comment Moderated time and date ( ? - Nullable ) - - using the display type Date
        [DataType(DataType.Date)]
        public DateTime? Moderated { get; set; }

        // Comment Deleted time and date ( ? - Nullable ) - - using the display type Date
        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }

        // Comment of the moderator
        [Display(Name = "Moderated Comment")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at Max {1} Characters", MinimumLength = 8)]
        public string ModeratedCommentBody { get; set; }

        // Moderator Comment type - using enoms
        [Display(Name = "Moderated Type")]
        public ModerationType ModerationType { get; set; }


        // Navigation Properties 

        // Reference the percent of Comment - Post (PostId)
        public virtual Post Post { get; set; }

        // hold the record of AuthorID
        public virtual BlogUser Author { get; set; }

        // hold the record of ModeratorId
        public virtual BlogUser Moderator { get; set; }
    }
}
