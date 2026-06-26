using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        // HiringDate - CreatedAT Of BaseEntity
        public Specialties specialties { get; set; }

        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
