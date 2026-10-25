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
            return await _dbContext.Posts.Where(p => p.DiscussionThreadId == threadId && !p.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException("post");
            }
            await _dbContext.Posts.AddAsync(post);
        }
    }
}
