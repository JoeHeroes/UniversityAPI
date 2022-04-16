using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniAPI.Entites;

namespace UniAPI.Models.Validators
{
    public class UniversityQueryValidator : AbstractValidator<UniversityQuery>
    {


        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortColumnNames = { nameof(University.Name), nameof(University.Description), nameof(University.Type) };
        public UniversityQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must by in [{string.Join(",", allowedSortColumnNames)}]");
        
        
        }
    }
}
