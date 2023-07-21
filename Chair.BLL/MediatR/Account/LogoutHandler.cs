using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class LogoutHandler : IRequestHandler<LogoutQuery, Unit>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public LogoutHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<Unit> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            await _accountBusinessLogic.Logout();

            return Unit.Value;
        }
    }
}
