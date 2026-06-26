namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // JoinDate == CreatedAt Of BaseEntity
        public string Photo { get; set; } = null!;

        #region Relationship

        #region Member - HealthRecord 
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion

        #region Member - Membership

        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        #endregion

        #region Member - Session

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;

        #endregion

        #endregion

    }
}
