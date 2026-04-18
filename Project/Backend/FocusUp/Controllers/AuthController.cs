using FocusUp.Application.DTOs;
using FocusUp.Application.Services.Auth;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;

        public AuthController(AuthService authService, UserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                int userId = _authService.Register(registerRequest.Username, registerRequest.Email, registerRequest.Password);
                return StatusCode(201, userId );
            }
            catch (InvalidDataException)
            {
                return Conflict();
            }catch(Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                string token = _authService.Login(loginRequest.UsernameOrEmail, loginRequest.Password);
                return StatusCode(200, new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            } catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null) throw new UnauthorizedAccessException();

            User? user = _userRepository.GetById(int.Parse(userIdClaim));
            if (user == null) return NotFound();

            return StatusCode(200, new { user.Id, user.Email, user.Username });
        }

        [HttpDelete("me")]
        public IActionResult DeleteUser()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedAccessException();

            User? user = _userRepository.GetById(int.Parse(userIdClaim));
            if(user == null) return NotFound();

            try
            {
                _userRepository.Delete(user.Id);
                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}