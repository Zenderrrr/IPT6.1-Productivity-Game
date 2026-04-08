using FocusUp.Application.Services;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        public BadgeRepository _badgeRepository;
        public BadgeService _badgeService;

        public BadgeController(BadgeRepository badgeRepository, BadgeService badgeService)
        {
            _badgeRepository = badgeRepository;
            _badgeService = badgeService;
        }

        //[HttpPost]
        //public 
    }
}