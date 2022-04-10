using AutoMapper;
using FriendsApi.Data;
using FriendsApi.DTOs;
using FriendsApi.Interface;
using FriendsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FriendsApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.userName))
                return BadRequest("User Name is Taken!");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.userName = registerDto.userName.ToLower();
            user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
            user.passwordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.userName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.userName==loginDto.userName);
            if(user==null) return Unauthorized("Invalid username");
            using var hmac=new HMACSHA512(user.passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for(int i = 0; i< computedHash.Length; i++)
            {
                if (computedHash[i] != user.passwordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto
            {
                UserName = user.userName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl= user.Photos?.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs = user.KnownAs
                
            };
        }





        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.userName == userName.ToLower());
        }
    }
}
