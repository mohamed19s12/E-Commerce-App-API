using Ecom.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task SendEmailAsync(string email, string token, string component, string subject, string message);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<bool> SendEmailForGorgetPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ActiveAccountAsync(ActiveEmailDTO activeEmailDTO);

    }
}
