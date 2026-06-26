using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class MembershipRepository : GenaricRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<MemberShip> GetAllMembershipsWithMembersAndPlans(Func<MemberShip, bool>? filter = null)
        {
            var memberships = _context.MembersShips.Include(M => M.Member).Include(P => P.Plan).Where(filter ?? (_ => true));

            return memberships;
        }

        public MemberShip? GetFirstOrDefault(Func<MemberShip, bool>? filter = null)
        {
            var Membership = _context.MembersShips.FirstOrDefault(filter ?? (_ => true));
            return Membership;
        }
    }
}
