using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using QAConnect.Models;
using System.Net.Mail;
using System;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using Microsoft.Owin.Infrastructure;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace QAConnect
{

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            MailMessage mail = new MailMessage();

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            //recipient address
            mail.To.Add(new MailAddress(message.Destination));

            //Formatted mail body
            mail.IsBodyHtml = true;
            mail.Body = message.Body;
            mail.Subject = message.Subject;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser, long>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, long> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser, long>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser, long>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, long>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    //custom class to get Identity object properties
    public class AppUserPrincipal : ClaimsPrincipal
    {
        public AppUserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public long UserID
        {
            get
            {
                return Identity.GetUserId<long>();
            }
        }

        public string UserName
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string FullName
        {
            get
            {
                return this.FindFirst(ClaimTypes.GivenName).Value;
            }
        }

        public string Gender
        {
            get
            {
                return this.FindFirst(ClaimTypes.Gender).Value;
            }
        }

        public string ProfilePicture
        {
            get
            {
                return this.FindFirst(ClaimTypes.Dns).Value;
            }
            set
            {
                var identity = (HttpContext.Current.User as ClaimsPrincipal).Identity as ClaimsIdentity;
                identity.RemoveClaim(this.FindFirst(ClaimTypes.Dns));

                var AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                identity.AddClaim(new Claim(ClaimTypes.Dns, value));
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );


            }
        }

        public string CoverPicture
        {
            get
            {
                return this.FindFirst(ClaimTypes.Dsa).Value;
            }

            set
            {
                var identity = (HttpContext.Current.User as ClaimsPrincipal).Identity as ClaimsIdentity;
                identity.RemoveClaim(this.FindFirst(ClaimTypes.Dsa));

                var AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                identity.AddClaim(new Claim(ClaimTypes.Dsa, value));
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );
            }
        }

        public byte StatusID
        {
            get
            {
                return Convert.ToByte(this.FindFirst(ClaimTypes.Sid).Value);
            }

            set
            {
                var identity = (HttpContext.Current.User as ClaimsPrincipal).Identity as ClaimsIdentity;
                identity.RemoveClaim(this.FindFirst(ClaimTypes.Sid));

                var AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                identity.AddClaim(new Claim(ClaimTypes.Sid, value.ToString()));
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );
            }
        }


        public DateTime LastStatusOn
        {
            get
            {
                return DateTime.Now;
                //return Convert.ToDateTime(this.FindFirst(ClaimTypes.Anonymous).Value);
            }
        }


        public string APIToken
        {
            get
            {
                var identity = (HttpContext.Current.User as ClaimsPrincipal).Identity as ClaimsIdentity;
                return ApplicationUser.Token(identity);
                //  return this.FindFirst(ClaimTypes.Thumbprint).Value;
            }
        }

        public int PageType { get { return this.PageType; } set { this.PageType = value; } }
    }
}
