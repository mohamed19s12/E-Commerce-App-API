using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailServices emailServices;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateTokenService token;

        public AuthRepository(UserManager<AppUser> userManager, IEmailServices emailServices, SignInManager<AppUser> signInManager, IGenerateTokenService token)
        {
            this.userManager = userManager;
            this.emailServices = emailServices;
            this.signInManager = signInManager;
            this.token = token;
        }

        // Implement authentication methods here
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return null;

            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
                return "Username is already taken";
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
                return "Email is already registered";

            //map dto to user
            AppUser user = new()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName
            };

            //create user
            var result = await userManager.CreateAsync(user, registerDTO.Password);

            //check for errors
            if (!result.Succeeded)
                //get first error
                return result.Errors.ToList()[0].Description;

            //send confirmation email
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            return "done"; //success
        }

        // Implement Send Email method
        public async Task SendEmailAsync(string email, string token, string component, string subject, string message)
        {
            var result = new EmailDTO(
                email,
                "mohammeed19s12@gmail.com",
                subject,
                EmailStringBody.SendEmail(email, token, component, message));

            await emailServices.SendEmail(result);
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return null;

            // Check Email
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (!user.EmailConfirmed)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendEmailAsync(user.Email, token, "Active", "Confirm your email", "Please confirm your email by clicking the link below:");
                return "Email not confirmed. A new confirmation email has been sent to your email address.";
            }
            //Check Password
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, true);

            //If success , generate token
            if (result.Succeeded)
                return token.GetAndCreateToken(user);


            return "Invalid email or password";


        }

        public async Task<bool> SendEmailForGorgetPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmailAsync(user.Email, token, "ResetPassword", "Reset your password", "Please reset your password by clicking the link below:");
            return true;
        }

        //reset password
        public async Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
                return null;
            var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);
            if (result.Succeeded)
                return "Password changed Successfully";

            return result.Errors.ToList()[0].Description;
        }

        //active account
        public async Task<bool> ActiveAccountAsync(ActiveEmailDTO activeEmailDTO)
        {
            //find user by email
            var user = await userManager.FindByEmailAsync(activeEmailDTO.Email);
            if (user == null)
                return false;

            //-----confirm email------
            var result = await userManager.ConfirmEmailAsync(user, activeEmailDTO.Token);
            if (result.Succeeded)
                return true;

            //if failed , generate new token and send email again
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmailAsync(user.Email, token, "Active", "Active your email", "Please Active your email by clicking the link below:");
            return false;


        }
    }
}
