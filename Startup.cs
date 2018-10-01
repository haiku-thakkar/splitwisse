using SecondSplitWise.DBContext;
using SecondSplitWise.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecondSplitWise
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
            services.AddDbContext<SecondSplitWiseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SecondSplitWise")));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ISettlementRepository, SettlementRepository>();

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors((corsPolicyBuilder) =>
            {
                corsPolicyBuilder.AllowAnyOrigin();
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.AllowAnyHeader();
                corsPolicyBuilder.WithExposedHeaders("X-InlineCount");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
           

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
