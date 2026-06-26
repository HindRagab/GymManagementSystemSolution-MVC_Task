using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        IEnumerable<Session> GetAllSesionWithTrainerandCategory();

        int GetCountOfBookedSlots(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
