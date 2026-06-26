using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class BookingRepository : GenaricRepository<MemberSession> , IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MemberSession> GetSessionById(int sessionId)
        {
            return _dbContext.MembersSessions
                             .Where(MS => MS.SessionId == sessionId)
                             .Include(MS => MS.Member)
                             .ToList();
        }
    }
}
