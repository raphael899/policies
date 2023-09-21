using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using policies.Services;
using policies.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace policies.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UsersController : ControllerBase
    {
        public  UsersService _usersService;
        private readonly IConfiguration _configuration;


        public UsersController(UsersService usersService , IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public IActionResult CreateUser([FromBody] User user)
        {
            // Verifica si el usuario ya existe en la base de datos.
            var existingUser = _usersService.GetByUsername(user.UserName);

            if (existingUser != null)
            {
                // Si el usuario ya existe, responde con un mensaje de error.
                return BadRequest(new { message = "El usuario ya existe." });
            }

            try
            {
                // Hashea la contraseña antes de guardarla en la base de datos.
                var hashedPassword = new PasswordHasher<User>().HashPassword(user, user.HashPassword);
                user.HashPassword = hashedPassword;

                _usersService.CreateUser(user);

                var token = GenerateJwtToken(user.UserName);


                // Responde con el usuario creado o un mensaje de éxito.
                return Ok(new { message = "Usuario registrado exitosamente.", token });

            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                // You should add appropriate logging here
                Console.WriteLine($"Excepción: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return StatusCode(500, new { message = "Error interno del servidor.", errorMessage = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // Find the user by username
            var existingUser = _usersService.GetByUsername(user.UserName);

            if (existingUser == null)
            {
                return BadRequest(new { message = "El usuario no existe." });
            }

            // Verify the hashed password
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(existingUser, existingUser.HashPassword, user.HashPassword);

            if (result == PasswordVerificationResult.Success)
            {
                // Password is correct, generate and return a JWT token
                var token = GenerateJwtToken(existingUser.UserName);

                return Ok(new { message = "Inicio de sesión exitoso.", token });
            }
            else
            {
                return BadRequest(new { message = "Contraseña incorrecta." });
            }
        }



        private string GenerateJwtToken(string userName)
        {
            var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, userName), // Puedes incluir reclamaciones adicionales aquí
         };

            var token = new JwtSecurityToken(
                issuer: null, // Establece el emisor deseado si es necesario
                audience: null, // Establece la audiencia deseada si es necesario
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}

