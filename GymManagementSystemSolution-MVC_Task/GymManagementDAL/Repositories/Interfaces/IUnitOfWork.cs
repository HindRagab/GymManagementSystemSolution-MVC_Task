using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ISessionRepository SessionRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IBookingRepository BookingRepository { get; }

        IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();

        int SaveChanges();
    }
}
