using System.Threading.Tasks;
using auth_service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace auth_service.Services
{
    // Stub email sender for development and testing purposes
    // TODO: Implement a real email sender service
    public class NoOpEmailSender : IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(
            ApplicationUser user,
            string email,
            string confirmationLink
        )
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            return Task.CompletedTask;
        }
    }
}
