using AngelusTBlog.Models;
using MailKit.Search;
using System.Linq;
using AngelusTBlog.Enums;
using AngelusTBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace AngelusTBlog.Services
{
    public class BlogSearchService
    {
        // Communicate with the DB
        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Making a Iqueariable
        public IQueryable<Post> Search(string searchTerm)
        {
            // get a Queryable list of post
            var posts = _context.Posts.Where(p => p.ReadyStatus == ReadyStatus.ProductionReady)
                .Include(p => p.Author)
                .AsQueryable();

            // Searching all the post and comments 
            if (searchTerm != null)
            {
                // Changeing the search term to lowercase
                searchTerm = searchTerm.ToLower();

                posts = posts.Where(
                    p => p.Title.ToLower().Contains(searchTerm) ||
                    p.Summary.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(c => c.CommentBody.ToLower().Contains(searchTerm) ||
                                                     c.ModeratedCommentBody.ToLower().Contains(searchTerm) ||
                                                     c.Author.FirstName.ToLower().Contains(searchTerm) ||
                                                     c.Author.LastName.ToLower().Contains(searchTerm) ||
                                                     c.Author.Email.ToLower().Contains(searchTerm)));
            }
            return posts.OrderByDescending(p => p.Created);
        }
    }
}
