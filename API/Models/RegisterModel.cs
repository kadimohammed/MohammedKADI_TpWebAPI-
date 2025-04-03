using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le nom d'utilisateur doit comporter entre 2 et 20 caractères.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail  est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe  est obligatoire.")]
        [Length(8, 100, ErrorMessage = "Le mot de passe doit comporter au moins 8 caractères.")]
        public string Password { get; set; }
    }
}
