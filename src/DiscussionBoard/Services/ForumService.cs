using DiscussionBoard.Data;
using DiscussionBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Services
{
    public class ForumService
    {
        private readonly ApplicationDbContext _dbContext;

        public ForumService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Forum>> GetAllAsync()
        {
            return await _dbContext.Forums.Where(f => !f.IsDeleted).ToListAsync();
        }
    }

}
