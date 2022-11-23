using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8.Dto;

namespace Week8.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(RegisterDto user);
        Task<ServiceResponse<string>> Login(LoginDto user);
        
    }
}