using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TBoard.Entities;
using TBoard.Entities.Auth;
using TBoard.Infrastructure.Middlewares;
using TBoard.WebApi.Extensions;
using TBoard.WebApi.Repositories.Implementation;
using TBoard.WebApi.Repositories.Interfaces;
using TBoard.WebApi.Services.Implementation;
using TBoard.WebApi.Services.Interfaces;

namespace TBoard.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<TournamentContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Player, Role>(options =>
            {
                options.Password.RequiredLength = 5;
            })
           .AddEntityFrameworkStores<TournamentContext>();

            var authOptions = services.ConfigureAuthOptions(Configuration);
            services.AddJwtAuthentication(authOptions);
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());
            //});


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IGameRepository), typeof(GameRepository));
            services.AddScoped(typeof(ITournamentRepository), typeof(TournamentRepository));
            services.AddScoped(typeof(IPlayerRepository), typeof(PlayerRepository));
            services.AddTransient<ITournamentService, TournamentService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddSwaggerGen(setupAction => {
                setupAction.SwaggerDoc("TournamentOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "TournamentApi",
                        Version = "1"
                    });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
