﻿using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class RemoveExecutorProfileValidator : AbstractValidator<RemoveExecutorProfileQuery>
    {
        private readonly ApplicationDbContext _context;
        public RemoveExecutorProfileValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var executorService = await _context.ExecutorProfiles.FirstOrDefaultAsync(x => x.Id == id);

                return executorService != null;
            }).WithMessage("Executor profile with Id: {PropertyValue} doesn't exist");
        }
    }
}
