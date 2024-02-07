using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Layer.IServices;
using Business_Layer.Services;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Innovura_TaskCraft.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Innovura_TaskCraft.IServices;
using Innovura_TaskCraft.Services;

namespace Innovura_TaskCraft
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddControllersWithViews();
            services.AddSession();
            var tokenManager = services.BuildServiceProvider().GetService<ITokenManager>();
            var dbContext = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            var jwtSettings = services.BuildServiceProvider().GetService<Jwt>();
            var userManager = services.BuildServiceProvider().GetService<IUserManager>();
            services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator(tokenManager, userManager, dbContext, jwtSettings));
            
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ILabelManager, LabelManager>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); 

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<IHomeEssentials, HomeEssentials>();

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var _jwtSettings = Configuration.GetSection("Jwt");
            services.Configure<Jwt>(_jwtSettings);

            var authKey = Configuration.GetValue<string>("Jwt:SecretKey");

            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseMiddleware<JwtMiddleware>();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innovura TaskCraft API");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
