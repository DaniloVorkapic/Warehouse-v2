using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;
using WarehouseWeb.Model.Enums;
using WarehouseWeb.Repositories;

namespace WarehouseWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<CustomerRole> _customerRoleRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Claims> _claimsRepository;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<RoleClaims> _roleClaimsRepository;
        

        public UserService(IUnitOfWork unitOfWork,SignInManager<User> signInManager,UserManager<User> userManager,IGenericRepository<Customer> customerRepository,IGenericRepository<CustomerRole> customerRoleRepository,IGenericRepository<Role> roleRepository,IGenericRepository<Claims> claimsRepository,IConfiguration config,IGenericRepository<RoleClaims> roleClaimsRepository)
        {
            _unitOfWork = unitOfWork; 
            _signInManager = signInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _roleRepository = roleRepository;
            _claimsRepository = claimsRepository;
            _config = config;
            _roleClaimsRepository = roleClaimsRepository;
            
        }
        public async Task<Result> Login(LoginDto request)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError; ;
                var result = Result.Create(null, statusCode);

                var user = await _userManager.FindByNameAsync(request.Username);
                
                if(user == null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;

                }
                var correctPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                
                if (correctPassword == true)
                {
                    

                    var customer = GetCustomerByUsername(request);
                    customer.Token = GenerateToken(user);
                   
                    statusCode = StatusCodes.Status200OK;
                    result.StatusCode = statusCode;
                    result.Value = customer;
                    return result;

                }
                else
                {
                    result = Result.Create("Los password", statusCode);
                    return result;
                }
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
        }

        public async Task<Result> Register(RegistrationDTO request)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null,statusCode);

                var userExists = await _userManager.FindByNameAsync(request.Username);
                if(userExists != null)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create(null, statusCode);
                    return result;

                }
                Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase);
                   
                if (!emailRegex.IsMatch(request.Email))
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    result = Result.Create("Los format emaila", statusCode);
                    return result;
                }
               
                var user = new User();
                user.Id = Guid.NewGuid().ToString();
                user.UserName  = request.Username;
                user.Email = request.Email;                       
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                
                var customerRoleResult = GetCustomerRole(request,request.RoleName);
                var customerRole = customerRoleResult.Value as CustomerRole;

                var myTransaction = _unitOfWork.myTransaction();
                try
                {                 

                    var customer = AddCustomer(request, user.Id, customerRole);

                    if (customer == null)
                    {
                        myTransaction.Rollback();
                        result = Result.Create("Cutomer nije uspesnio naprvljen napravljen", statusCode);
                        return result;

                    }
                    var addUserResult = await _userManager.CreateAsync(user, request.Password);    
                    
                    if(addUserResult.Succeeded == false)
                    {
                        myTransaction.Rollback();
                        result = Result.Create("Failed: Password required nonAlphaNumeric", statusCode);
                        return result;

                    }

                    statusCode = StatusCodes.Status200OK;
                    _unitOfWork.commit();
                    result = Result.Create("Uspesno registrovani korisnik", statusCode);                  
                    myTransaction.Commit();
                    return result;

                }
                catch (Exception ex)
                {

                    myTransaction.Rollback();
                    statusCode = StatusCodes.Status500InternalServerError;
                    result = Result.Create(null, statusCode);
                    return result;
                }
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode);
                return result;
            }
            }
        public async Task<Result> Logout()
        {
            var statusCode = StatusCodes.Status200OK;
            await _signInManager.SignOutAsync();
            var result = Result.Create("Log outovanje uspesno", statusCode);
            return result;
        }

        public LoginActivityDto GetCustomerByUsername(LoginDto request)
        {

           var query = (from customer in _customerRepository.GetQueryable<Customer>().Include(x => x.CustomerRoleList)                         
                                      where (customer.Username == request.Username)

                                      select new LoginActivityDto
                                      {
                                          CustomerId = customer.Id,
                                          Username = customer.Username,
                                          ClaimsList =  (from roleCustomer in  customer.CustomerRoleList
                                                        join  roleActivities in _roleClaimsRepository.GetQueryable<RoleClaims>()
                                                        on roleCustomer.RoleId equals roleActivities.RoleId
                                                        select roleActivities.Claims.Name).ToList(),             
                                           Token = null,

                                      }).FirstOrDefault();                                   

            return query;
        }

        public  Result<CustomerRole> GetCustomerRole(RegistrationDTO request, string roleName)
        {
            var roleId = GetRoleIdByName(roleName);
            var status = StatusCodes.Status500InternalServerError;
            var result = new Result<CustomerRole>();

            if (roleId == 0)
            {
                status = StatusCodes.Status400BadRequest;
                return result;
            }
            var customerRole = new CustomerRole();
            customerRole.RoleId = roleId;
            customerRole.Description = GetCustomerRoleDesciption(request.FirstName, request.LastName, roleName);

            result.Value = customerRole;

             status = StatusCodes.Status200OK;
            result.StatusCode = status;

              return result ;
           
        }

        public long GetRoleIdByName(string roleName)
        {
           // var role = new Role();
            long roleId = 0;
            roleId = _roleRepository.GetQueryable<Role>()
                 .Where(x => x.Name == roleName)
                 .Select(x =>x.Id)
                 .FirstOrDefault();        

            return roleId;
        }

        public Task<Customer> AddCustomer(RegistrationDTO request,string userId, CustomerRole customerRole)
        {
            var customer = new Customer();
            customer.UserId = userId;
            customer.Username = request.Username;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.CustomerType = request.CustomerType;
            if (customer.CustomerType == CustomerType.LegalEntity)
                customer.CompanyId = request.CompanyId;

            customer.CustomerRoleList.Add(customerRole);

            var addCustomer =  _customerRepository.AddEntity(customer);
            return addCustomer;

        }
        public string GenerateToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JWT:ValidIssuer"],
                _config["ValidAudience"],
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GetCustomerRoleDesciption(string firstName,string lastName,string roleName)
        {
            switch (roleName)
            {
                case "Administrator":
                    return $"{firstName} {lastName} is administrator";

                case "Customer":
                    return $"{firstName} {lastName} is customer";
                default:
                    return "Greska!!";
            }
        }
    }
}
