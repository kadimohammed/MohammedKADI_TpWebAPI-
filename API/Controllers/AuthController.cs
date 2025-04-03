using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOS;
using Service.Exceptions;
using Service.Services;
using Service.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            
            try
            {
                // Vérification si l'utilisateur existe déjà avec le nom d'utilisateur
                UserDto existingUser = await _userService.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Tentative d'inscription avec un nom d'utilisateur déjà pris : {model.Username}");
                    return BadRequest("Nom d'utilisateur déjà exist.");
                }
            }
            catch (InvalidUserException ex)
            {
                _logger.LogInformation($"Tentative dinscription pour l'utilisateur : {model.Username}");
                UserDto user = await _userService.RegisterUserAsync(model.Username, model.Email, model.Password);
                if (user is not null)
                {
                    _logger.LogInformation($"Utilisateur {model.Username} bien Enregister.");
                    return Ok(new { Id = user.Id });
                }
            }

            _logger.LogWarning($"Échec d'inscription pour l'utilisateur : {model.Username}");
            return BadRequest("Erreur d'inscription.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation($"Tentative de connexion pour l'utilisateur : {model.Username}");

            UserDto user = await _userService.GetUserByUsernameAsync(model.Username);
            if (!_userService.VerifyPassword(user, model.Password))
            {
                _logger.LogWarning($"Échec de connexion pour l'utilisateur : {model.Username}");
                return Unauthorized();
            }

            _logger.LogInformation($"Connexion reussie pour l'utilisateur : {model.Username}");

            _jwtService.AddRoles("User");
            string token = _jwtService.GenerateToken(user.Username);

            _logger.LogInformation($"Token JWT géneré avec succès pour utilisateur : {model.Username}");
            return Ok(new {Token = token });
        }


        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            _logger.LogInformation($"Accès réussi à l'endpoint protégé par l'utilisateur : {User.Identity?.Name}");
            return Ok("You have accessed a protected endpoint!");
        }


    }
}
