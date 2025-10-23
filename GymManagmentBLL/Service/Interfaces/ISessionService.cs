using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel createSessionViewModel);
        UpdateSessionViewModel? GetSessionToUpdate(int id);
        bool UpdateSession(int id, UpdateSessionViewModel updateSessionViewModel);
        bool RemoveSession(int id);
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();

    }
}
