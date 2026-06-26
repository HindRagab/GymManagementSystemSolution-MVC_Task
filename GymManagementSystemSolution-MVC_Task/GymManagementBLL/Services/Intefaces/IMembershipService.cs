using GymManagementBLL.ViewModels.MembershipViewModels;
namespace GymManagementBLL.Services.Intefaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();
        bool CreateMembership(CreateMembershipViewModel createMembership);
        IEnumerable<PlanSelectViewModel> GetPlansForDropDown();
        IEnumerable<MemberSelectViewModel> GetMembersForDropDown();
        bool DeleteMembership(int MemberId);

    }
}
