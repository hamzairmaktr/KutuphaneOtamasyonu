using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Email
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, bool isHtml = true);
    }
}
