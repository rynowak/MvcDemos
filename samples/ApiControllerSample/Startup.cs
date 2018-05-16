
using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiControllerSample
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<AddPetDto, Pet>();
                c.CreateMap<EditPetDto, Pet>();
            });
            services.AddSingleton<IMapper>(new Mapper(mapperConfiguration));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API Controller Sample", Version = "v1" });
            });

            services.AddEntityFrameworkSqlServer().AddDbContext<BasicApiContext>(options =>
            {
                var connectionString = Configuration["Data:DefaultConnection:ConnectionStringApiControllerSample"];
                options.UseSqlServer(connectionString);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                CreateDatabase(app.ApplicationServices, env.ContentRootPath);
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Controller Sample");
                c.RoutePrefix = "";
            });

            app.UseMvc();
        }

        private void CreateDatabase(IServiceProvider services, string contentRoot)
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<BasicApiContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                using (var connection = dbContext.Database.GetDbConnection())
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = File.ReadAllText(Path.Combine(contentRoot, "seed.sql"));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
