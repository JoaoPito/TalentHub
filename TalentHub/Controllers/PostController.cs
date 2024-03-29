using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentHub.Data;
using TalentHub.Models;

namespace TalentHub
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Post
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] string tags="")
        {
              return _context.Posts != null ? 
                          View(await _context.Posts.Where(p => p.Tags
                                                                .Contains(tags.ToLower()))
                                                                .OrderByDescending(p => p.CreatedDate)
                                                                .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
        }

        // GET: Post/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var postContent = Markdig.Markdown.ToHtml(post.Content);
            ViewBag.HtmlContent = postContent;

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Title,Description,Content,CreatedDate,ModifiedDate,Tags")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserName = User.Identity!.Name!;
                post.Tags = post.Tags.ToLower();

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (User.Identity != null && !HasPermissions(post, User.Identity.Name))
            {
                return Forbid();
            }

            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Title,Description,Content,CreatedDate,ModifiedDate,Tags")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldPost = _context.Posts.FirstOrDefault(x => x.Id == id)!;

                    if (User.Identity != null && !HasPermissions(oldPost, User.Identity.Name))
                    {
                        return Forbid();
                    }

                    oldPost.Title = post.Title;
                    oldPost.Description = post.Description;
                    oldPost.Content = post.Content;
                    oldPost.ModifiedDate = DateTime.Now;
                    oldPost.Tags = post.Tags.ToLower();

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
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            if (User.Identity != null && !HasPermissions(post, User.Identity.Name))
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                if (User.Identity != null && !HasPermissions(post, User.Identity.Name))
                    return Forbid();

                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool HasPermissions(Post post, string currentUser){
            if (post.UserName != currentUser)
            {
                return false;
            }
            return true;
        }
    }
}
