using GymManagementDAL.Entities;
namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenaricRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetAllMembershipsWithMembersAndPlans(Func<MemberShip , bool>? filter = null);

        MemberShip? GetFirstOrDefault(Func<MemberShip, bool>? filter = null);
    }
}
