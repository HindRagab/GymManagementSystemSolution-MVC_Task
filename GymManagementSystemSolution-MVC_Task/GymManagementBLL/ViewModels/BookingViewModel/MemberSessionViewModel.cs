namespace GymManagementBLL.ViewModels.BookingViewModel
{
    public class MemberSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public string BookingDate { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
