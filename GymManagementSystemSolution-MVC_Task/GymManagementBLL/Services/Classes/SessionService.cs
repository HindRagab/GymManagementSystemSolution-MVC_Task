using AutoMapper;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreateSession)
        {
            try
            {
                // Check If Trainer Exists
                if (!IsTrainerExist(CreateSession.TrainerId)) return false;
                // Check If Category Exists
                if (!IsCategoryExist(CreateSession.CategoryId)) return false;
                // Check If StartDate is before EndDate
                if (!IsDateTimeValid(CreateSession.StartDate, CreateSession.EndDate)) return false;

                if (CreateSession.Capacity > 25 || CreateSession.Capacity < 0) return false;

                var SessionEntity = _mapper.Map<Session>(CreateSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Create Session Failed: {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Session = _unitOfWork.SessionRepository.GetAllSesionWithTrainerandCategory();
            if (!Session.Any()) return [];


            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Session);
            foreach (var session in MappedSessions)
                session.AvilableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (Session is null) return null;

            var MappedSessions = _mapper.Map<Session , SessionViewModel>(Session);
            MappedSessions.AvilableSlots = MappedSessions.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSessions.Id);
            return MappedSessions;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (!IsSessionAvilableForUpdating(Session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel UpdateSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (!IsSessionAvilableForUpdating(Session!)) return false;
                if (!IsTrainerExist(UpdateSession.TrainerId)) return false;
                if (!IsDateTimeValid(UpdateSession.StartDate , UpdateSession.EndDate)) return false;


                _mapper.Map(UpdateSession , Session);
                Session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed: {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (!IsSessionAvilableForRemoving(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed: {ex}");
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var Categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(Categories);
        }


        #region Helper Method

        private bool IsSessionAvilableForUpdating(Session session)
        {
            // If Session Completed - Not Updated Allowed
            if (session.EndDate < DateTime.Now) return false;

            // If Session Started - Not Updated Allowed
            if (session.StartDate <= DateTime.Now) return false;

            // If Session Has Active Booking - Not Updated Allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0; 
            if (HasActiveBooking) return false;

            return true;
        }

        private bool IsSessionAvilableForRemoving(Session session)
        {
            // If Session Completed - Not Delete Allowed
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            // If Session Upcoming - Not Delete Allowed
            if (session.StartDate > DateTime.Now) return false;

            // If Session Has Active Booking - Not Delete Allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }

        private bool IsTrainerExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate , DateTime EndDate)
        {
            return EndDate > StartDate && DateTime.Now > StartDate ;
        }


        #endregion

    }
}
