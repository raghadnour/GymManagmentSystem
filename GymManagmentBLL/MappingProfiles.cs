using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.ViewModels.SessionViewModels;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()
                );
            CreateMap<CreateSessionViewModel, Session>().ReverseMap();
        }
    }
}
