using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Repositories.Interfaces;
using GymManagmentDAL.Repositories.classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GymManagmentDAL.Data.DataSeed;
using GymManagmentBLL;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.AttachmentService.Interface;
using GymManagmentBLL.Service.AttachmentService.Class;
using Microsoft.AspNetCore.Identity;
using GymManagmentDAL.Entities;
namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            //builder.Services.AddScoped(typeof(IGenericRepo<>),typeof(GenericRepo<>));
            //builder.Services.AddScoped<IPlanRepo,PlanRepo>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepo, SessionRepo>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IMemberShipRepo, MembershipRepo>();
            builder.Services.AddScoped<IMemberSessionService, MemberSessionService>();
            builder.Services.AddScoped<IMemberSessionRepo, MemberSessionRepo>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();
        

            var app = builder.Build();

            #region DataSeed _ MigrateDatabase
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations != null && pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }
            GymDbContextSeeding.DataSeed(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
