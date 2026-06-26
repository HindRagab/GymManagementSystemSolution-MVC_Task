using AutoMapper;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllMembershipsWithMembersAndPlans(M => M.Status == "Active");

            var MembershipViewModel = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return MembershipViewModel;
        }

        public bool CreateMembership(CreateMembershipViewModel createMembership)
        {
            if(!IsMemberExists(createMembership.MemberId) || !IsPlanExists(createMembership.PlanId) || HasActiveMemberShips(createMembership.MemberId))
                return false;

            var membership = _unitOfWork.MembershipRepository;

            var membershipMap = _mapper.Map<MemberShip>(createMembership);

            var plan = _unitOfWork.GetRepository<Plan>().GetById(createMembership.PlanId);
            membershipMap.EndDate = DateTime.UtcNow.AddDays(plan!.DurationDays);

            membership.Add(membershipMap);
            return _unitOfWork.SaveChanges() > 0;

        }

        public IEnumerable<PlanSelectViewModel> GetPlansForDropDown()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll(P=>P.IsActive);
            return _mapper.Map<IEnumerable<PlanSelectViewModel>>(Plans);
        }

        public IEnumerable<MemberSelectViewModel> GetMembersForDropDown()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(Members);
        }

        public bool DeleteMembership(int MemberId)
        {
            var Repo = _unitOfWork.MembershipRepository;
            var membership = Repo.GetFirstOrDefault(M => M.MemberId == MemberId && M.Status == "Active");
            if (membership == null) return false;
            Repo.Delete(membership);
            return _unitOfWork.SaveChanges() > 0;
        }


        #region Helper Method

        private bool IsMemberExists(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            return member != null;
        }

        private bool IsPlanExists(int planId)
        {
            var membershipPlan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            return membershipPlan != null;
        }

        private bool HasActiveMemberShips(int memberId)
        {
            var ActiveMemberShips = _unitOfWork.MembershipRepository
                .GetAllMembershipsWithMembersAndPlans(X => X.MemberId == memberId && X.Status == "Active");
            return ActiveMemberShips.Any();
        }

        #endregion

    }
}
