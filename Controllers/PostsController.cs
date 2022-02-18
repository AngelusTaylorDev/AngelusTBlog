using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngelusTBlog.Data;
using AngelusTBlog.Models;
using Microsoft.AspNetCore.Identity;
using AngelusTBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AngelusTBlog.Enums;
using X.PagedList;

namespace AngelusTBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ISlugService _slugService;
        private readonly BlogSearchService _blogSearchService;

        public PostsController(ApplicationDbContext context,
            UserManager<BlogUser> userManager,
            IImageService imageService,
            ISlugService slugService, BlogSearchService blogSearchService)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
            _slugService = slugService;
            _blogSearchService = blogSearchService;
        }

        // Search results 
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            // Search Term
            ViewData["SearchTerm"] = searchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 6;

            var posts = _blogSearchService.Search(searchTerm);

            
            return View(await posts.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        // BlogPostIndex
        public async Task<IActionResult> BlogPostIndex(int? id, int? page)
        {
            if(id == null)
            {
                return NotFound();
            }

            var pageNumber = page ?? 1;
            var pageSize = 6;

            // Ceate a page list with a order set. - where post are production ready
            var posts = await _context.Posts.Where(p => p.BlogId == id && p.ReadyStatus == ReadyStatus.ProductionReady)
                .Include(p => p.Author)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(m => m.Slug == slug);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            ViewData["AuthorID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,BlogId,Title,Summary,Content,ReadyStatus,Slug,Image")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                // Recoarding the created date.
                post.Created = DateTime.Now;

                // Create the Author Id.
                var AuthoriD = _userManager.GetUserId(User);
                post.AuthorID = AuthoriD;

                // Getting the Image File
                post.ImageData = await _imageService.EncodeImageAsync(post.Image);

                // Getting the Image File type.
                post.ImageType = _imageService.ContentType(post.Image);

                // Creating the slug and see if it is unique
                var slug = _slugService.UrlFriendly(post.Title);

                // Getting the Image File
                post.ImageData = await _imageService.EncodeImageAsync(post.Image);

                // Getting the Image File type.
                post.ImageType = _imageService.ContentType(post.Image);

                // See if a slug error has occared
                var VallidationError = false;

                // If the slug is mpty
                if (string.IsNullOrEmpty(slug))
                {
                    VallidationError = true;
                    ModelState.AddModelError("", "The title that you have provided can't be used as it is empty.");
                }

                // Detect incoming duplicate slugs
                else if (!_slugService.IsUnique(slug))
                {
                    VallidationError = true;
                    // Add a model state error and retrun the user to the index page.
                    ModelState.AddModelError("Title", "The title that you have provided has already been used. Please use another title.");
                }

                // If a slug error has occared
                if (VallidationError)
                {
                    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
                    return View(post);
                }

                // Store the slug to post . slug
                post.Slug = slug;


                _context.Add(post);
                await _context.SaveChangesAsync();

                // Loop over each tag list of string
                foreach (var tag in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        AuthorID = AuthoriD,
                        Text = tag
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BlogPostIndex), new { Id = post.BlogId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorID);
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Slug == slug);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Summary,Content,ReadyStatus,Image,Created")] Post post, IFormFile newImage, List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Getting the original post
                    var editPost = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == post.Id);

                    // Recoarding the Updated date
                    post.Updated = DateTime.Now;
                    editPost.Updated = post.Updated;
                    editPost.Summary = post.Summary;
                    editPost.Title = post.Title;
                    editPost.Content = post.Content;
                    editPost.ReadyStatus = post.ReadyStatus;
                    editPost.Tags = post.Tags;

                    // Make a new slug 
                    var newSlug = _slugService.UrlFriendly(post.Title);

                    // Is the tittle new?
                    if(newSlug != editPost.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            editPost.Title = post.Title;
                            editPost.Slug = newSlug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "This Title can not be used as it resules in a duplicate slug");
                            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
                            return View(post);
                        }
                    }

                    if (editPost.Title != post.Title)
                    {
                        // Recoarding the Name
                        editPost.Title = post.Title;
                    }

                    if (editPost.Summary != post.Summary)
                    {
                        editPost.Summary = post.Summary;
                    }

                    if (editPost.Content != post.Content)
                    {
                        editPost.Content = post.Content;
                    }

                    if (newImage != null)
                    {
                        // Getting the Image File
                        editPost.ImageData = await _imageService.EncodeImageAsync(newImage);
                        editPost.ImageType = _imageService.ContentType(newImage);
                    }

                    // Remove all previously associated with this post
                    _context.Tags.RemoveRange(editPost.Tags);
                    
                    // Add new tags 
                    foreach(var tagText in tagValues)
                    {
                        _context.Add(new Tag()
                        {
                            PostId = post.Id,
                            AuthorID = editPost.AuthorID,
                            Text = tagText
                        });
                    }

                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Users, "Id", "Id", post.AuthorID);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}