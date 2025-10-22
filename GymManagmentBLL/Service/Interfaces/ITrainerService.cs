using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        TrainerViewModel? GetTrainerDetails(int id);
        bool CreateTrainer(CreateTrainerViewModel trainer);
        bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel trainer);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int id);

        bool RemoveTrainer(int id);
    }
}