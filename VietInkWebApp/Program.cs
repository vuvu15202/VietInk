using Microsoft.EntityFrameworkCore;
using VietInkWebApp.Entities;
using Microsoft.AspNetCore.Identity;
using VietInkWebApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;

namespace VietInkWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<TattooshopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

                builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<TattooshopContext>();

            //session
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            //send mail
            builder.Services.AddOptions();                                        // Kích hoạt Options
            var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
            builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject
            builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();;

            app.UseAuthorization();

            //Session
            app.UseSession();
            //
            app.MapRazorPages();

            app.Run();
        }
    }
}