using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using TeddySmith.API.DTOs.Account;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Models;
using TeddySmith.API.Service;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager )
        {
            _userManager = userManager;
            _tokenService = tokenService; 
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
           if(!ModelState .IsValid) 
                return BadRequest(ModelState);

           var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if(!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
            return Ok(
                new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        //tao token
                        return Ok(
                            new NewUserDTO
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)

                            });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
/*
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                    var appUser = new AppUser
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email

                    };
                    var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                    if (createdUser.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                        if (roleResult.Succeeded)
                        {
                            return Ok("User Created");
                        }
                        else
                        {
                            return StatusCode(500, roleResult.Errors);
                        }

                    }
                    else
                    {
                        return StatusCode(500, createdUser.Errors);
                    }

                }
            }
            catch (Exception e)
            {

                return StatusCode(500, e);
            }
            return Ok();
        }
    }
}
*/