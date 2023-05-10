using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Services
{
    public interface IUserService
    {
        Task<Result> Login(LoginDto request);
        Task<Result> Register(RegistrationDTO request);
        Task<Result> Logout(); 
    }
}
