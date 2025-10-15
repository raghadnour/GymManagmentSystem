using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.TrainerViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel trainer)
        {
            try
            {

                if (IsEmailExists(trainer.Email) || IsPhoneExists(trainer.Phone)) return false;
                var newTrainer = new Trainer
                {
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Phone = trainer.Phone,
                    Gender = trainer.Gender,
                    DateOfBirth = trainer.DateOfBirth,
                    Address = new Adderss
                    {
                        BuildingNumber = trainer.BuildingNumber,
                        Street = trainer.Street,
                        City = trainer.City,
                    },
                    Specialities = trainer.Specialization

                };
                _unitOfWork.GetRepository<Trainer>().Add(newTrainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<TrainersViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if(!trainers.Any() || trainers is null ) return Enumerable.Empty<TrainersViewModel>();
            var trainersViewModel = trainers.Select(t => new TrainersViewModel
            {
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialities.ToString()
            });
            return trainersViewModel;
        }

        public TrainersViewModel? GetTrainerById(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) return null;
            var trainerViewModel = new TrainersViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialities.ToString(),
                Address = trainer.Address != null ? $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}" : null,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString()
            };
            return trainerViewModel;
        }

        public TrainerUpdateViewModel? GetTrainerForUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) return null;
            var trainerUpdateViewModel = new TrainerUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address?.BuildingNumber ?? 0,
                Street = trainer.Address?.Street ?? string.Empty,
                City = trainer.Address?.City ?? string.Empty,
                Specialization = trainer.Specialities
            };
            return trainerUpdateViewModel;
        }

        public bool RemoveTrainer(int id)
        {
            try
            {
                var existingTrainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (existingTrainer == null || HasActiveSession(id)) return false;
                _unitOfWork.GetRepository<Trainer>().Delete(existingTrainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateTrainer(int id, TrainerUpdateViewModel trainer)
        {
            try
            {
                if (IsEmailExists(trainer.Email) || IsPhoneExists(trainer.Phone)) return false;
                var existingTrainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (existingTrainer == null) return false;
                existingTrainer.Email = trainer.Email;
                existingTrainer.Phone = trainer.Phone;
                existingTrainer.Address = new Adderss
                {
                    BuildingNumber = trainer.BuildingNumber,
                    Street = trainer.Street,
                    City = trainer.City,
                };
                existingTrainer.Specialities = trainer.Specialization;
                existingTrainer.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Trainer>().Update(existingTrainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }


        #region Helper
        bool IsEmailExists(string email) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        
        bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();

        bool HasActiveSession(int trainerId) => _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.StartTime > DateTime.Now).Any();
        #endregion
    }
}
