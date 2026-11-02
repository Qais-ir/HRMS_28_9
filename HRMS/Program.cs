
using HRMS.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Token Configuration
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               var key = Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"); // Define the secret key
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false, // The Source Where The Token Is Issued
                   ValidateAudience = false, // The Users Whome Can Use This Token
                   ValidateIssuerSigningKey = true, // Make Sure That The Token Is Using My Secret Key
                   IssuerSigningKey = new SymmetricSecurityKey(key), // Generate The Token Using Our Key
               };
           });


            // Container : DI
            builder.Services.AddDbContext<HRMSContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSContext"))
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middlewares pipe
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
