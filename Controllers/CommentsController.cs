using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngelusTBlog.Data;
using AngelusTBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AngelusTBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _UserManager;

        public CommentsController(ApplicationDbContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _UserManager = userManager;
        }

        // GET: Comments all Comments: GET: Comments/Details/5
        public async Task<IActionResult> Index()
        {
            var allComments = await _context.Comments.ToListAsync();
            return View(allComments);
        }

        // GET: Comments all the Original Comments
        public async Task<IActionResult> OriginalComments()
        {
            var originalComments = _context.Comments.ToListAsync();
            return View("Index", await originalComments);
        }

        // GET: Comments all the Moderated Comments
        public async Task<IActionResult> ModeratedIndex()
        {
            var moderatedIndex = _context.Comments.Where(c => c.Moderated != null).ToListAsync();
            return View("Index", await moderatedIndex);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PostId,CommentBody")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Getting the user ID
                comment.AuthorID = _UserManager.GetUserId(User);

                // Recoarding the created date.
                comment.Created = DateTime.Now;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorID);
            ViewData["ModeratorID"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorID);
            ViewData["PostId"] = new SelectList(_context.Users, "Id", "Summary", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorID);
            ViewData["ModeratorID"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentBody")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == comment.Id);
                
                try
                {
                    newComment.CommentBody = comment.CommentBody;
                    newComment.Updated = DateTime.Now;
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentsSection");
            }
            return View(comment);
        }

        // POST: Comments/Moderate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Moderate(int id, [Bind("Id,CommentBody,ModeratedCommentBody,ModerationType")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == comment.Id);

                try
                {
                    newComment.ModeratedCommentBody = comment.ModeratedCommentBody;
                    newComment.ModerationType = comment.ModerationType;

                    newComment.Moderated = DateTime.Now;
                    newComment.ModeratorID = _UserManager.GetUserId(User);

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentSection");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Moderator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
