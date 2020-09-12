using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _rpo;
        public AuthController(IAuthRepository rpo)
        {
            _rpo = rpo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto dto)
        {
            // //validate request//当不是controller的时候需要手动写检查，并且需要加[FormBaby]
            // if (!ModelState.IsValid)
            //     return BadRequest(modelState);

            dto.Username = dto.Username.ToLower ();

            User createToUser = new User{
                Username = dto.Username
            };

            if (await _rpo.UserExists(dto.Username))
                return BadRequest("Username is already exists!");

            User UserCreate =await  _rpo.Register(createToUser,dto.Password);

            // CreatedAtRoute("getUser",new {Username =UserCreate.Username }, UserCreate);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var loginUser = await _rpo.Login(dto.Username,dto.Password);
            if(loginUser == null)
                return Unauthorized();

            

        }

        

        
    }
}