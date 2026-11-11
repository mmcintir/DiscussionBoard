using DiscussionBoard.Data;
using DiscussionBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Services
{
    public class PostService
    {
        ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetForThreadAsync(int threadId)
        {
            return await _dbContext.Posts.Include(p => p.ApplicationUser ).Where(p => p.DiscussionThreadId == threadId && !p.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            await _dbContext.Posts.AddAsync(post);
        }

        public async Task<Post?> GetAsync(int id)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Post post)
        {
            Post existing = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existing == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            existing.Title = post.Title;
            existing.Content = post.Content;
            await _dbContext.SaveChangesAsync();
        }
    }
}
