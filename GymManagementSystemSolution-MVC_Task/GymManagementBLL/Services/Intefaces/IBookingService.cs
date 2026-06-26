using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Intefaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessionsWithTrainersAndCategory();
        IEnumerable<MemberSessionViewModel> GetAllMembersForSession(int id);

        public bool CreateBooking(CreateBookingViewModel createBookingViewModel);
        IEnumerable<MemberSelectViewModel> GetMemberForDropDown(int id);
        bool MemberAttended(MemberAttendOrCancelViewModel model);
        bool CancelBooking(MemberAttendOrCancelViewModel model);
    }
}
