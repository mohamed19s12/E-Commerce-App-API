﻿using Ecom.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IEmailServices
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
