using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
