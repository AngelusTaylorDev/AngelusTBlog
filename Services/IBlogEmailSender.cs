using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace AngelusTBlog.Services
{
    public interface IBlogEmailSender : IEmailSender
    {
        // Send the contact email.
        Task SendContactEmailAsync(string emailForm, string name, string subject, string message);
    }
}
