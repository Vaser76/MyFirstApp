using System.ComponentModel.DataAnnotations;


namespace DeliveyApp.ViewModels
{
    public class RegisterDriverModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Не указан паспорт")]
        public string Passport { get; set; }
        [Required(ErrorMessage = "Не указано ФИО")]
        public string Fio { get; set; }
        [Required(ErrorMessage = "Не указан номер лицензии")]
        public string Numberdriverlicense { get; set; }
        [Required]
        public string Salt { get; set; }
    }
}
