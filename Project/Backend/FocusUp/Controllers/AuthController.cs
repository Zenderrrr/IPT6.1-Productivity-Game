using FocusUp.Application.DTOs;
using FocusUp.Application.Services;
using FocusUp.Application.Services.Auth;
using FocusUp.Domain.Models;
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
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(AuthService authService, UserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
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

                User? user = _userRepository.GetByEmail(loginRequest.UsernameOrEmail) ?? _userRepository.GetByUsername(loginRequest.UsernameOrEmail);

                if (user == null)
                    return Unauthorized();

                var (refreshToken, plainToken) = _authService.CreateRefreshToken(user.Id);

                Response.Cookies.Append("refreshToken", plainToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = refreshToken.ExpiresAt
                });

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

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var oldToken = Request.Cookies["refreshToken"];

            if (String.IsNullOrEmpty(oldToken))
                return Unauthorized("No refresh token");

            var dbToken = _authService.GetRefreshToken(oldToken);

            if (dbToken == null || !dbToken.IsActive())
                return Unauthorized("Invalid Refresh Token");


            var user = _userRepository.GetById(dbToken.UserId);
            if(user == null)
                return Unauthorized();

            var newAccessToken = _jwtTokenService.CreateToken(user);

            var (userRefreshToken, plainToken) = _authService.RotateRefreshToken(oldToken);

            Response.Cookies.Append("refreshToken", plainToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = userRefreshToken.ExpiresAt
            });

            return Ok(newAccessToken);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var oldToken = Request.Cookies["refreshToken"];

            try
            {
                if (!String.IsNullOrEmpty(oldToken))
                {
                    var dbToken = _authService.GetRefreshToken(oldToken);

                    if (dbToken != null)
                    {
                        dbToken.SetRevokedAt(DateTime.UtcNow);
                        _authService.SetRevokedAtRefreshToken(dbToken);
                    }
                }
                Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}