using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace EF.ASP.NET.CORE
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddDbContext<UniContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<UniContext>();

            services.Configure<RepositoryOptions>(Configuration);
            services.AddScoped<StudentService>();
            services.AddScoped<CourseService>();
            services.AddScoped<LecturerService>();
            services.AddScoped<HomeTaskService>();

            services.AddScoped(typeof(UniversityRepository<>));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

           //services.AddDbContext<ApplicationDbContext>(optionsAction: options => options.UseSqlServer(@"Data Source = KYURRII07\SQLEXPRESS; Initial Catalog = BaseCourseAUTH; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"));

            services.AddDbContext<ApplicationDbContext>(optionsAction: options => options.UseInMemoryDatabase());


          services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(p =>
            {
                p.LoginPath = "/Security/Login";
                p.Cookie.Name = "EF.ASP.NET.CORE";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
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

            app.UseStaticFiles();
            app.UseAuthentication();
            this.CreateAdminUser(userManager, roleManager);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");  // "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void CreateAdminUser(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            IdentityUser identityUser = new IdentityUser("Admin@test.com") { Email = "Admin@test.com" };
            var userRes = userManager.CreateAsync(identityUser, "Qwerty1234!");
            var rol = roleManager.CreateAsync(new IdentityRole("Admin"));
            var res = userManager.AddToRoleAsync(identityUser, "Admin");
        }
    }
}
