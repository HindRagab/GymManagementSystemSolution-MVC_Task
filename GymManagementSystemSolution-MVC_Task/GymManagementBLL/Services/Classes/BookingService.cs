using AutoMapper;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
namespace GymManagementBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessionsWithTrainersAndCategory()
        {
            var SessionRepository = _unitOfWork.SessionRepository;
            var sessions = SessionRepository.GetAllSesionWithTrainerandCategory();

            var sessionViewModel = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionViewModel)
                session.AvilableSlots = session.Capacity - SessionRepository.GetCountOfBookedSlots(session.Id);

            return sessionViewModel;
        }

        public IEnumerable<MemberSessionViewModel> GetAllMembersForSession(int id)
        {
            var BookingRepoo = _unitOfWork.BookingRepository;
            var MemberSession = BookingRepoo.GetSessionById(id);

            var memberSessionViewModel = _mapper.Map<IEnumerable<MemberSessionViewModel>>(MemberSession);
            return memberSessionViewModel;
        }

        public bool CreateBooking(CreateBookingViewModel createBookingViewModel)
        {
            var Sessions = _unitOfWork.SessionRepository.GetById(createBookingViewModel.SessionId);
            if (Sessions is null || Sessions.StartDate <= DateTime.Now) return false;

            var MembershipRepository = _unitOfWork.MembershipRepository;

            var ActiveMembership = MembershipRepository.GetFirstOrDefault(M => M.Status == "Active" && M.MemberId == createBookingViewModel.MemberId);
            if (ActiveMembership is null) return false;

            var SessionRepository = _unitOfWork.SessionRepository;
            var SessionBookedSlots = SessionRepository.GetCountOfBookedSlots(createBookingViewModel.SessionId);
            var AvilableBookedSlots = Sessions.Capacity - SessionBookedSlots;
            if (AvilableBookedSlots == 0) return false;

            var Booking = _mapper.Map<MemberSession>(createBookingViewModel);
            Booking.IsAttended = false;
            _unitOfWork.BookingRepository.Add(Booking);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool MemberAttended(MemberAttendOrCancelViewModel model)
        {
            var memberSession = _unitOfWork.GetRepository<MemberSession>()
                                           .GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                           .FirstOrDefault();
            if (memberSession is null) return false;

            memberSession.IsAttended = true;
            memberSession.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<MemberSession>().Update(memberSession);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool CancelBooking(MemberAttendOrCancelViewModel model)
        {
            var session = _unitOfWork.SessionRepository.GetById(model.SessionId);
            if (session is null || session.StartDate <= DateTime.Now) return false;

            var Booking = _unitOfWork.BookingRepository.GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId).FirstOrDefault();
            if (Booking is null) return false;
            _unitOfWork.BookingRepository.Delete(Booking);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region Helper Methods

        public IEnumerable<MemberSelectViewModel> GetMemberForDropDown(int id)
        {
            var BookedRepository = _unitOfWork.BookingRepository;
            var BookedMember = BookedRepository.GetAll(S => S.Id == id)
                                               .Select(MS => MS.MemberId)
                                               .ToList();
            var AvailableMemberBooking = _unitOfWork.GetRepository<Member>().GetAll(M => !BookedMember.Contains(M.Id));
            var MemberForSelect = _mapper.Map<IEnumerable<MemberSelectViewModel>>(AvailableMemberBooking);
            return MemberForSelect;
        }

        #endregion
    }
}
