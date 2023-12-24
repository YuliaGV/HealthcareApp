using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.Entities;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    
    public class UserController : BaseAPIController
    {

        private readonly ApplicationDbContext _db;

        private readonly ITokenService _tokenService;


        public UserController(ApplicationDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;

        }


        [Authorize]
        [HttpGet] //  api/user
        public async Task<ActionResult<IEnumerable<User> >> GetUsers()
        {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }


        [Authorize]
        [HttpGet("{id}")]  //  api/user/1
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) 
        {
            if (await DoesUserExist(registerDTO.Email)) return BadRequest("Email is already registered");

     

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Name = registerDTO.Name,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return new UserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDTO.Email.ToLower());
            if (user == null) return Unauthorized("User doesn't exist");
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i=0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Wrong password!");
            }

            return new UserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };



        }


        private async Task<bool> DoesUserExist(string email)
        {
            return await _db.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }



    }
}
