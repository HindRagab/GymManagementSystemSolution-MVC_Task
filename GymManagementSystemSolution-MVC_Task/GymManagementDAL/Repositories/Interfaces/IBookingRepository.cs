using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenaricRepository<MemberSession>
    {
        IEnumerable<MemberSession> GetSessionById(int sessionId);
    }
}
