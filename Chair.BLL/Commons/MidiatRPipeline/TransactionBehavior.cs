using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using System.Transactions;
using Chair.DAL.Data;

namespace Chair.BLL.Commons.MidiatRPipeline
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ApplicationDbContext _database; // replace with UoW

        public TransactionBehavior(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (IsCommand() == false)
                return await next();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var response = await next();
            await _database.SaveChangesAsync(cancellationToken);
            scope.Complete();

            return response;
        }

        private static bool IsCommand()
        {
            return typeof(TRequest).Name.EndsWith("Command");
        }
    }
}
