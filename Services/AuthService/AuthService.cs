using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8.Data;
using Week8.Dto;
using Week8.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Week8.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(LoginDto user)
        {
            var response = new ServiceResponse<string>();
            try{
                var userDb = await _context.Users.Where(u => u.Email == user.Email).FirstAsync();
                if(!VerifyPassword(user.Password,userDb.PasswordHash,userDb.PasswordSalt)){
                    response.Success =false;
                    response.Message = "Wrong Email or Password";
                    return response;
                }
                response.Data = CreateToken(userDb);
            }catch{
                response.Success =false;
                response.Message = "User not found";
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Register(RegisterDto user)
        {
            var response = new ServiceResponse<string>();
            GenerateHash(user.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            var register = new User{
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                Role = user.Role,
            };
            await _context.Users.AddAsync(register);
            await _context.SaveChangesAsync();
            response.Data = "Successfully registered";
            return response;
        }

        private string CreateToken(User user){
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
        private void GenerateHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt){
            using(var hash = new System.Security.Cryptography.HMACSHA512()){
                PasswordSalt = hash.Key;
                PasswordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }

        private bool VerifyPassword(string Password,byte[] PasswordHash,byte[] PasswordSalt){
            using(var hash = new System.Security.Cryptography.HMACSHA512(PasswordSalt)){
                var computedHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}