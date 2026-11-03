using DiscussionBoard.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Services
{
    public class UserProfileService: IUserProfileService
    {
        ApplicationDbContext _db;

        public UserProfileService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<bool> IsDisplayNameAvailableAsync(string displayName, string? excludeUserId = null)
        {
            displayName = displayName.Trim() ?? "";
            if (displayName.Length < 2 || displayName.Length > 60)
            {
                return Task.FromResult(false);
            }

            try
            {
                return Task.FromResult(!_db.Users.Any(u => u.DisplayName.ToUpper().Equals(displayName.ToUpper()) && (excludeUserId == null || u.Id != excludeUserId)));
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }
    }
}
