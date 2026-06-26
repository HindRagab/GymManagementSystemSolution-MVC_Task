using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        // Ask CLR For Creating Object From Services
        // CLR Will Inject Address Of Object In Instructor
        public MemberServices(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                // If One Of Them Exists Return False
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;

                var PhotoName = _attachmentService.Upload("members", createMember.PhotoFile);
                if (string.IsNullOrEmpty(PhotoName)) return false;

                // If Not Add Member And Return True If Added
                var MemberEntity = _mapper.Map<Member>(createMember);
                MemberEntity.Photo = PhotoName;
                _unitOfWork.GetRepository<Member>().Add(MemberEntity);
                var IsCreated = _unitOfWork.SaveChanges() > 0;
                if (!IsCreated)
                {
                    _attachmentService.Delete(PhotoName, "members");
                    return false;
                }
                else
                    return IsCreated;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];
            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;

        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = _mapper.Map<MemberViewModel>(Member);

            // Active MemberShip
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId && X.Status == "Active")
                                                        .FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
            }

            return ViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);

            if (MemberHealthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(Member);

        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;
            var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(X => X.MemberId == MemberId ).Select(x => x.SessionId);

            var HasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

            if (HasFutureSessions) return false;

            var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();
            var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                    {
                        MemberShipRepo.Delete(memberShip);
                    }
                }
                MemberRepo.Delete(Member);
                var IsDeleted = _unitOfWork.SaveChanges() > 0;
                if (IsDeleted)
                    _attachmentService.Delete(Member.Photo, "members");

                return IsDeleted;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdateMember)
        {
            var emailExists = _unitOfWork.GetRepository<Member>()
                .GetAll(X => X.Email == UpdateMember.Email && X.Id != Id);

            var PhoneExists = _unitOfWork.GetRepository<Member>()
                .GetAll(X => X.Phone == UpdateMember.Phone && X.Id != Id);

            if (emailExists.Any() || PhoneExists.Any()) return false;

            var Repo = _unitOfWork.GetRepository<Member>();

            var Member = Repo.GetById(Id);
            if (Member is null) return false;

            _mapper.Map(UpdateMember, Member);
            Repo.Update(Member);
            return _unitOfWork.SaveChanges() > 0;
        }


        #region Helper Method

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }

        #endregion
    }
}
