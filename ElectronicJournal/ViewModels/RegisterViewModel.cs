using ElectronicJournal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Група")]
        public int? GroupID { get; set; }
        public Group Group { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} має мати довжину мінімум {2} символів", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")] 
        public string PasswordConfirm { get; set; }
    }
}
