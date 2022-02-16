using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngelusTBlog.Models
{
    public class BlogUser : IdentityUser
    {
        // User Information
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be less then {1} and the {2} must be more then 2 Charactors long", MinimumLength =2)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be less then {1} and the {2} must be more then 2 Charactors long", MinimumLength = 2)]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be less then {1} and the {2} must be more then 2 Charactors long", MinimumLength = 2)]
        public string DisplayName { get; set; }
        
        // Full Name String
        [NotMapped]
        public string FullName 
        { 
            get 
            {
                // Getting the values of First name and Last name
                return $"{FirstName} {LastName}";
            } 
        }


        //Byte array for the profile image
        public byte[] BlogUserImage { get; set; }
        public string ContentType { get; set; }

        // External website Links
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }


        // A collection of all of the blogs written by the user
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();

        // A collection of all of the Posts written by the user
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
