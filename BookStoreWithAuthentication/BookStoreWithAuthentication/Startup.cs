using Microsoft.Owin;
using Owin;
using BookStoreWithAuthentication.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using BookStoreWithAuthentication.Models;

[assembly: OwinStartupAttribute(typeof(BookStoreWithAuthentication.Startup))]
namespace BookStoreWithAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }
        
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "admin@poczta.pl";
                user.Email = "admin@poczta.pl";
                user.FirstName = "Adminek";
                user.LastName = "Adminowski";
                string userPass = "Haslo123";

                var chkUser = userManager.Create(user, userPass);
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
