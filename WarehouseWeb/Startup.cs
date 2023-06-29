using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Data;
using WarehouseWeb.Model;
using WarehouseWeb.Repositories;
using WarehouseWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarehouseWeb.Middlewares;
using WarehouseWeb.Configuration;
using Microsoft.AspNetCore.Authorization;
using WarehouseWeb.Authentication;

namespace WarehouseWeb
{

    public class Startup
    {
        // string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WarehouseWeb", Version = "v1" });

            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //  {
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          ValidateIssuer = true,
            //          ValidateAudience = true,
            //          ValidateLifetime = true,
            //          ValidIssuer = Configuration["JWT:ValidIssuer"],
            //          ValidAudience = Configuration["JWT:ValidAudience"],
            //          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
            //      };

            //  });

            //services.AddAuthentication(option =>
            //{
            //    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(jwt =>
            //    {
                     
            //        var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:SecretKey"]);

            //        jwt.SaveToken = true;
            //        jwt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = true, //toDo Update
            //            ValidIssuer = Configuration["JwtConfig:Issuer"],
            //            ValidateAudience = true,
            //            ValidAudience = Configuration["JwtConfig:Audience"],
            //            RequireExpirationTime = true, //toDo Update
            //            ValidateLifetime = true
            //        };
            //    });
            //services.AddAuthorization();


            services.AddDbContext<DataContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

           // services.AddIdentity<User,IdentityRole>().
            services.AddIdentity<User, IdentityRole>(options =>
             {
                 options.Password.RequireDigit = false;
                 options.Password.RequiredLength = 5;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireUppercase = false;

             })
                   .AddEntityFrameworkStores<DataContext>()
                   .AddDefaultTokenProviders();
            

            services.AddCors(options => options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
           // services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddTransient<GlobalExceptionHandler>();



            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               // option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {

                    var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:SecretKey"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true, //toDo Update
                        ValidIssuer = Configuration["JwtConfig:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtConfig:Audience"],
                        RequireExpirationTime = true, //toDo Update
                        ValidateLifetime = true
                    };
                });
            services.AddAuthorization();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WarehouseWeb v1"));
            }

            app.UseHttpsRedirection();



            app.UseCors();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.ConfigureExceptionMiddleware();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

