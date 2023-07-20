﻿using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class RemoveServiceTypeValidator : AbstractValidator<RemoveServiceTypeQuery>
    {
        private readonly ApplicationDbContext _context;
        public RemoveServiceTypeValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(x=> x.Id == id);

                return serviceType != null;
            }).WithMessage("Service with Id: {PropertyValue} doesn't exist");
        }
    }
}