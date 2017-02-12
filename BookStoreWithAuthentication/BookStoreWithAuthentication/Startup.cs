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
                var role = new IdentityRole();
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

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                string userPass = "Haslo123";

                var user = new ApplicationUser();
                user.UserName = "kowalski@poczta.pl";
                user.Email = "kowalski@poczta.pl";
                user.FirstName = "Jan";
                user.LastName = "Kowalski";

                var chkUser = userManager.Create(user, userPass);
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "User");
                }

                var user2 = new ApplicationUser();
                user2.UserName = "nowak@poczta.pl";
                user2.Email = "nowak@poczta.pl";
                user2.FirstName = "Mirek";
                user2.LastName = "Nowak";

                chkUser = userManager.Create(user2, userPass);
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user2.Id, "User");
                }
            }
        }
    }
}
