using GymManagmentBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainersViewModel> GetAllTrainers();
        TrainersViewModel? GetTrainerDetails(int id);
        bool CreateTrainer(CreateTrainerViewModel trainer);
        bool UpdateTrainerDetails(int id, TrainerUpdateViewModel trainer);
        TrainerUpdateViewModel? GetTrainerToUpdate(int id);

        bool RemoveTrainer(int id);
    }
}
