using Chair.BLL.Dto.Account;
using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chair.BLL.BusinessLogic.Account
{
    public interface IAccountBusinessLogic
    {
        Task Register(RegisterDto model);
        Task Login(LoginDto model);
        Task Logout();
    }
}
