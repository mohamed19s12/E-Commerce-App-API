using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        //component is the frontend url if reset password or confirm email
        public static string SendEmail(string email , string token , string component , string message)
        {
            string encodedToken = Uri.EscapeDataString(token);
            return $@"
                <html>
                    <head>
                        <style>
                            .button {{
                                /* background-color: #4CAF50; /* Green */ */
                                background: lenear-gradient(45deg, rgba(2,0,36,1) 0%, rgba(9,121,113,1) 35%, rgba(0,212,255,1) 100%);
                                color: white;
                                border: none;
                                border-radius: 10px;
                                padding: 15px 32px;
                                display: inline-block;
                                text-align: center;
                                text-decoration: none;
                                box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
                                transition: all 0.4s ease;
                                font-size: 16px;
                                font-family: Arial, sans-serif;
                                margin: 4px 2px;
                                cursor: pointer;    
                        }}
                        </style>

                    
                    </head>
                    <body>
                        <div>
                            <h2>{message}</h2>
                            <a class=""button"" href=""http://localhost:4200/Account/component?email={email}&code={encodedToken}"">Confirm Email</a>
                        </div>
                    </body>                
                </html>
            ";  
        }

    }
}
