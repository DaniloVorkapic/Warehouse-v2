using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WarehouseWeb.Configuration;
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
        private readonly JwtConfig _jwtConfig;
      //  private readonly IConfiguration _config;
        private readonly IGenericRepository<RoleClaims> _roleClaimsRepository;
        private readonly IGenericRepository<Company> _companyRepository;

        public UserService(IUnitOfWork unitOfWork, SignInManager<User> signInManager,
            UserManager<User> userManager, IGenericRepository<Customer> customerRepository,
            IGenericRepository<CustomerRole> customerRoleRepository, IGenericRepository<Role> roleRepository,
            IGenericRepository<Claims> claimsRepository, IOptionsMonitor<JwtConfig> optionMonitor,
            IGenericRepository<RoleClaims> roleClaimsRepository, IGenericRepository<Company> companyRepository)
        {
            _unitOfWork = unitOfWork; 
            _signInManager = signInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _roleRepository = roleRepository;
            _claimsRepository = claimsRepository;
            _jwtConfig = optionMonitor.CurrentValue;
           // _config = config;
            _roleClaimsRepository = roleClaimsRepository;
            _companyRepository = companyRepository;
        }
        public async Task<Result> Login(LoginDto request)
        {
            try
            {
                int statusCode = StatusCodes.Status500InternalServerError;
                string errorMessage = "Greeska";
                var result = Result.Create(null, statusCode, errorMessage,0);


                var user = await _userManager.FindByNameAsync(request.Username);
                
                if(user == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Nije pornadjen user";
                    return result;
                    

                }
                var correctPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                
                if (correctPassword == true)
                {


                    var customer = GetCustomerByUsername(request);
                    customer.Token = GenerateToken(customer.CustomerId,customer.Username);
                  //  var customer = _customerRepository.GetQueryable<Customer>().FirstOrDefault(x => x.UserId == user.Id);



                   
                   
                    result.StatusCode = StatusCodes.Status200OK;
                    result.ErrorMessage = "Uspesno ste se ulogovali";
                    result.Value = customer;
                    
                    return result;

                }
                else
                {
                    result.ErrorMessage = "Password pogresan";
                    result.StatusCode = StatusCodes.Status401Unauthorized;
                    result.Value = null;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
        }

        public async Task<Result> Register(RegistrationDTO request)
        {
            try
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null,statusCode,null,0);

                var userExists = await _userManager.FindByNameAsync(request.Username);
                if(userExists != null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "user  postoji";
                    return result;

                }
                string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                Regex emailRegex = new Regex(emailPattern,RegexOptions.IgnoreCase);
                
                   
                if (!emailRegex.IsMatch(request.Email))
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.ErrorMessage = "Los format pasworda";
                    return result;
                }
               
                var user = new User();
                user.Id = Guid.NewGuid().ToString();
                user.UserName  = request.Username;
                user.Email = request.Email;                       
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                
                var customerRoleResult = GetCustomerRole(request,request.RoleName);
                var customerRole = customerRoleResult.Value as CustomerRole;

                var myTransaction = _unitOfWork.myTransaction();
                try
                {                 

                    var customer =  await AddCustomer(request, user.Id, customerRole) ;
                    
                    if (customer == false)
                    {
                        myTransaction.Rollback();
                        result.ErrorMessage = "Customer nije uspesno napravljen";
                        return result;

                    }
                    var addUserResult = await _userManager.CreateAsync(user, request.Password);    
                    
                    if(addUserResult.Succeeded == false)
                    {
                        myTransaction.Rollback();
                        result.ErrorMessage = "Failed: Password required nonAlphaNumeric";
                        return result;

                    }

                    result.StatusCode = StatusCodes.Status200OK;
                    result.ErrorMessage = "Uspesno registrovani korisnik";
                    _unitOfWork.commit();
                    myTransaction.Commit();
                    
                    return result;

                }
                catch (Exception ex)
                {

                    myTransaction.Rollback();
                    statusCode = StatusCodes.Status500InternalServerError;
                    result = Result.Create(null, statusCode,"Greska",0);
                    return result;
                }
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = Result.Create(null, statusCode,"Greska",0);
                return result;
            }
            }
        public async Task<Result> Logout()
        {
            var statusCode = StatusCodes.Status200OK;
            var errorMessage = "Izlogovali ste se";
            await _signInManager.SignOutAsync();
            var result = Result.Create(null, statusCode,errorMessage,0);
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

        public async Task<bool> AddCustomer(RegistrationDTO request,string userId, CustomerRole customerRole)
        {
            var customer = new Customer();
            customer.UserId = userId;
            customer.Username = request.Username;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.CustomerType = request.CustomerType;
           // var myTransactionCompany = _unitOfWork.myTransaction();
            if (customer.CustomerType == CustomerType.LegalEntity)
            {
              

                var company =    AddCompany(request);
                customer.Company = company;
                //customer.Company = company;




                //if (company == null)
                //{
                //    myTransactionCompany.Rollback();
                    
                //    return false;

                //}

            }



            customer.CustomerRoleList.Add(customerRole);

            var addCustomer =  await _customerRepository.AddEntity(customer) ;
            return addCustomer;

        }

        private  Company AddCompany(RegistrationDTO request)
        {
            var company = new Company
            {
               
                Name = request.CompanyName,
                Pib = request.Pib
            };
          //  var addedCompany = await _companyRepository.AddEntity(company);

            return company;
        }

        public string GenerateToken(long customerId, string username)
        {

            string id = customerId.ToString();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { 
            
                new(JwtRegisteredClaimNames.Sub,id),
                new(JwtRegisteredClaimNames.UniqueName,username),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            


            };


            var token = new JwtSecurityToken(
               _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                null,
                expires: DateTime.UtcNow.AddMinutes(45),
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

        public async Task<Result> GetAllClaims(long customerId)
        {
            Result result = Result.Create(null, StatusCodes.Status500InternalServerError, null, 0);

            var claims = await _customerRoleRepository.GetQueryable<CustomerRole>()
                .Include(x => x.Role)
                .ThenInclude(y => y.RoleClaimsList)
                .ThenInclude(u => u.Claims)
                .Where(x => x.CustomerId == customerId)
                .Select(x => x.Role.RoleClaimsList.Select(y => y.Claims.Name))
                .ToListAsync();


            result.Value = claims;
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
