using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberShipViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CreateMemberShip(CreateMemberShipViewModel model)
        {
            try
            {
                var PalnRepo = _unitOfWork.GetRepository<Plan>();
                var MemberRepo = _unitOfWork.GetRepository<Member>();

                bool PlanIsActive = PalnRepo.GetById(model.PlanId)?.IsActive ?? false;
                if (!PlanIsActive)return false;

                bool MemberExists = MemberRepo.GetById(model.MemberId) is not null;
                if (!MemberExists) return false;

                bool PlanExists = PalnRepo.GetById(model.PlanId) is not null;
                if (!PlanExists) return false;
                bool ActiveMembershipExists = _unitOfWork.MemberShipRepo
                    .GetAll()
                    .Any(m => m.MemberId == model.MemberId  && m.Statues=="Active");
                if (ActiveMembershipExists) return false;


                var membership = _mapper.Map<Membership>(model);
                membership.CreatedAt= DateTime.Now;
                var endDate = DateTime.Now.AddDays(
                    PalnRepo.GetById(model.PlanId)?.DurationDays ?? 0);
                membership.EndDate= endDate;
                _unitOfWork.GetRepository<Membership>().Add(membership);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public IEnumerable<MemberShipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MemberShipRepo.GetAllMembershipsWithMembersAndPlans();
            if(memberships is null || !memberships.Any()) 
                return Enumerable.Empty<MemberShipViewModel>();
            var mappedMemberships = _mapper.Map<IEnumerable<Membership>, IEnumerable<MemberShipViewModel>>(memberships);
            return mappedMemberships;
        }

        public IEnumerable<MemberSelectViewModel> GetMembersForDropDown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public IEnumerable<PlanSelectViewModel> GetPlansForDropDown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            return _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);

        }

        public bool RemoveMemberShip(int memberId, int planId)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Membership>();
                var membership = _unitOfWork.MemberShipRepo
                    .GetAllMembershipsWithMembersAndPlans()
                    .FirstOrDefault(m => m.MemberId == memberId && m.PlanId == planId);

                if (membership is null)
                    return false;

                Repo.Delete(membership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
