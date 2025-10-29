using AutoMapper;
using GymManagmentBLL.Service.AttachmentService.Interface;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        //private readonly IGenericRepo<Member> _memberRepo;
        //private readonly IGenericRepo<Membership> _membershipRepo;
        //private readonly IPlanRepo _planRepo;
        //private readonly IGenericRepo<HealthRecord> _healthRecordRepo;
        //private readonly IGenericRepo<MemberSession> _memberSessionRepo;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper ,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Member>();

                if (IsEmailExists(createMemberViewModel.Email))
                    return false;
                if (IsPhoneExists(createMemberViewModel.Phone))
                    return false;
                var photoFileName = _attachmentService.Upload("members", createMemberViewModel.Photo);
                if(string.IsNullOrEmpty(photoFileName) ) return false;
                var member = _mapper.Map<Member>(createMemberViewModel);
                member.Photo = photoFileName;
                Repo.Add(member);
                bool iscreated = _unitOfWork.SaveChanges() > 0;
                if (iscreated)
                {
                    
                    return iscreated;
                }
                else
                {
                    _attachmentService.Delete("members", photoFileName);
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveMember(int MemberId)
        {
            var Repo = _unitOfWork.GetRepository<Member>();
            var Member = Repo.GetById(MemberId);
            if (Member is null) return false;
            var sessionIds = _unitOfWork.GetRepository<MemberSession>().GetAll(
               b => b.MemberId == MemberId).Select(S => S.SessionId);

            var hasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(S => sessionIds.Contains(S.Id) && S.StartTime > DateTime.Now).Any();

            if (hasFutureSessions) return false;

            var MemberShips = _unitOfWork.GetRepository<Membership>().GetAll(X => X.MemberId == MemberId);

            try
            {
                if (MemberShips.Any())
                {
                    foreach (var membership in MemberShips)
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                }
                _unitOfWork.GetRepository<Member>().Delete(Member);
                bool isDeleted = _unitOfWork.SaveChanges() > 0;
                if (isDeleted)
                {
                    _attachmentService.Delete("members", Member.Photo);
                    return isDeleted;
                }
                else
                {
                    return isDeleted;
                }
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {

            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (!Members.Any()) return [];
            return _mapper.Map<IEnumerable<MemberViewModel>>(Members);

        }

        public HealthRecordViewModel? GetHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);

            if (member is null) return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMemberShip = _unitOfWork.GetRepository<Membership>()
                .GetAll(MP => MP.MemberId == MemberId && MP.Statues == "Active").FirstOrDefault();

            if (activeMemberShip is not null)
            {
                var activePlan = _unitOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);

                viewModel.PlanName = activePlan?.Name;
                viewModel.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();
            }

            return viewModel;

        }

        public MemberUpdateViewModel? GetMemberForUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;
            return _mapper.Map<MemberUpdateViewModel>(member);
        }

        public bool UpdateMember(int Id, MemberUpdateViewModel updateMemberViewModel)
        {
            var emailExist = _unitOfWork.GetRepository<Member>().GetAll(
                 m => m.Email == updateMemberViewModel.Email && m.Id != Id);

            var PhoneExist = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Phone == updateMemberViewModel.Phone && m.Id != Id);

            if (emailExist.Any() || PhoneExist.Any()) return false;

            var Repo = _unitOfWork.GetRepository<Member>();
            var Member = Repo.GetById(Id);
            if (Member is null) return false;
            _mapper.Map(updateMemberViewModel, Member);

            Repo.Update(Member);
            return _unitOfWork.SaveChanges() > 0;

        }


        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Email == email);
            return existing.Any();
        }
        private bool IsPhoneExists(string phone)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Phone == phone);
            return existing.Any();
        }
        #endregion
    }
}
