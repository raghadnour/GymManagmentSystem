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

        //private readonly IGenericRepo<Member> _memberRepo;
        //private readonly IGenericRepo<Membership> _membershipRepo;
        //private readonly IPlanRepo _planRepo;
        //private readonly IGenericRepo<HealthRecord> _healthRecordRepo;
        //private readonly IGenericRepo<MemberSession> _memberSessionRepo;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                if (IsEmailExists(createMemberViewModel.Email) || IsPhoneExists(createMemberViewModel.Phone)) return false;
                var member = new Member
                {
                    Name = createMemberViewModel.Name,
                    Email = createMemberViewModel.Email,
                    Phone = createMemberViewModel.Phone,
                    Gender = createMemberViewModel.Gender,
                    DateOfBirth = createMemberViewModel.DateOfBirth,
                    Address = new Adderss
                    {
                        BuildingNumber = createMemberViewModel.BuildingNumber,
                        Street = createMemberViewModel.Street,
                        City = createMemberViewModel.City,
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = createMemberViewModel.HealthRecord.Height,
                        Weight = createMemberViewModel.HealthRecord.Weight,
                        BloodType = createMemberViewModel.HealthRecord.BloodType,
                        Notes = createMemberViewModel.HealthRecord.Note
                    }


                };
                 _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch
            {
                return false;

            }


        }

        public bool RemoveMember(int id)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();
            var _membershipRepo = _unitOfWork.GetRepository<Membership>();

            var member = _memberRepo.GetById(id);
            if (member is  null) return false;
            var memberSessions = _unitOfWork.GetRepository<MemberSession>().GetAll(X => X.MemberId == id && X.Session.StartTime>DateTime.Now).Any();
            if (memberSessions) return false;
            var ActiveMemberShip = _membershipRepo.GetAll(x => x.MemberId == id);

            try
            {
                if (ActiveMemberShip.Any())
                {
                    foreach (var membership in ActiveMemberShip)

                        _membershipRepo.Delete(membership);

                }

                 _memberRepo.Delete(member);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;

            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()

            });
            return memberViewModels;

        }

        public HealthRecordViewModel? GetHealthRecordDetails(int id)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
            if(memberHealthRecord is not null)
            {
                HealthRecordViewModel healthRecord = new HealthRecordViewModel
                {
                    Height = memberHealthRecord.Height,
                    Weight = memberHealthRecord.Weight,
                    BloodType = memberHealthRecord.BloodType,
                    Note = memberHealthRecord.Notes
                };
                return healthRecord;

            }
            return null;
        }

        public MemberViewModel? GetMemberDetails(int Id)
        {
            try
            {
                var member = _unitOfWork.GetRepository<Member>().GetById(Id);
                if (member == null) return null;
                var memberViewModel = new MemberViewModel
                {
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString(),
                    Photo = member.Photo,
                    Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
                    DateOfBirth = member.DateOfBirth.ToShortDateString(),

                };

                var Activemembership = _unitOfWork.GetRepository<Membership>().GetAll(X => X.Id == Id && X.Statues == "Active").FirstOrDefault();
                if (Activemembership is not null)
                {
                    memberViewModel.MembershipStartDate = Activemembership.CreatedAt.ToShortDateString();
                    memberViewModel.MembershipEndDate = Activemembership.EndDate.ToShortDateString();

                    var memberPlan = _unitOfWork.GetRepository<Plan>().GetById(Activemembership.PlanId);
                    if (memberPlan is not null)
                        memberViewModel.PlanName = memberPlan.Name;

                }
                return memberViewModel;
            }
            catch
            {
                return null;
            }

        }

        public MemberUpdateViewModel? GetMemberForUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is  null) return null;
            var memberForUpdate = new MemberUpdateViewModel
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
            return memberForUpdate;
        }

        public bool UpdateMember(int id, MemberUpdateViewModel updateMemberViewModel)
        {
            try
            {

                if (IsEmailExists(updateMemberViewModel.Email) || IsPhoneExists(updateMemberViewModel.Phone)) return false;
                var member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (member == null) return false;
                member.Email= updateMemberViewModel.Email;
                member.Phone= updateMemberViewModel.Phone;
                member.Address.BuildingNumber= updateMemberViewModel.BuildingNumber;
                member.Address.Street= updateMemberViewModel.Street;
                member.Address.City= updateMemberViewModel.City;
                member.UpdatedAt= DateTime.Now;
                _unitOfWork.GetRepository<Member>().Update(member) ;
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
            

        }


        #region Helper Methods
        bool IsEmailExists(string email) => _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        #endregion
    }
}
