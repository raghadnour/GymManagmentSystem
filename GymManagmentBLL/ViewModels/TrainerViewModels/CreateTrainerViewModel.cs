﻿using GymManagmentDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "The name lenght must be between 2 and 50 charactars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The name must contain only letters and spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "The Email lenght must be between 5 and 100 charactars")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Ohone number must be valid Egyption number")]
        public string Phone { get; set; } = null!;



        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }


        [Required(ErrorMessage = "Date of birth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }



        [Required(ErrorMessage = "BuildingNumber is Required")]
        [Range(1, 9000, ErrorMessage = "Building Number must be between 1,9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Street lenght must be between 2 and 30 chars")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "City lenght must be between 2 and 30 chars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The name must contain only letters and spaces")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "You must have one speciality assign")]
        public Specialities Specialization { get; set; }
    }
}
