using DiscussionBoard.Data;
using DiscussionBoard.Models;
using DiscussionBoard.Pages;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Services
{
    public class DiscussionThreadService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DiscussionThreadService> _logger;


        public DiscussionThreadService(ApplicationDbContext dbContext, ILogger<DiscussionThreadService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public DiscussionThreadService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DiscussionThread?> GetAsync(int id)
        {
            return await _dbContext.DiscussionThreads.FirstOrDefaultAsync(thread => thread.Id == id);
        }

        public async Task<List<DiscussionThread>> GetForForumAsync(int forumId)
        {
            return await _dbContext.DiscussionThreads.Where(t => t.ForumId == forumId && !t.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(DiscussionThread thread)
        {
            try
            {
                _dbContext.DiscussionThreads.Add(thread);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to add thread to forum {ForumId}", thread.ForumId);
                throw;
            }
        }

        public async Task UpdateAsync(DiscussionThread thread)
        {
            var dbThread =  await GetAsync(thread.Id);

            if (dbThread == null)
            {
                _logger.LogWarning("Thread {ThreadId} not found for update", thread.Id);
                throw new NullReferenceException("Thread not found");
            }

            dbThread.Title = thread.Title;
            dbThread.Content = thread.Content;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update thread {ThreadId}", thread.Id);
                throw;
            }

        }
    }
}
