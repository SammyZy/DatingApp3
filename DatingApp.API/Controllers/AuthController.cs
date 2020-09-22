using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _rpo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository rpo, IConfiguration config)
        {
            _config = config;
            _rpo = rpo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto dto)
        {
            // //validate request//当不是controller的时候需要手动写检查，并且需要加[FormBaby]
            // if (!ModelState.IsValid)
            //     return BadRequest(modelState);

            dto.Username = dto.Username.ToLower();

            User createToUser = new User
            {
                Username = dto.Username
            };

            if (await _rpo.UserExists(dto.Username))
                return BadRequest("Username is already exists!");

            User UserCreate = await _rpo.Register(createToUser, dto.Password);

            // CreatedAtRoute("getUser",new {Username =UserCreate.Username }, UserCreate);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var loginUser = await _rpo.Login(dto.Username.ToString(), dto.Password);
            if (loginUser == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginUser.Id.ToString()),
                new Claim(ClaimTypes.Name,loginUser.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }




    }
}