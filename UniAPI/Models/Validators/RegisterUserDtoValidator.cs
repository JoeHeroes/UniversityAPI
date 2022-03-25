using FluentValidation;
using System.Linq;
using UniAPI.Entites;

namespace UniAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {

        public RegisterUserDtoValidator(UniversityDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) => 
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email ==value);
                    if (emailInUse)
                    {
                        context.AddFailure("Emial", "That email is taken");
                    }
                });
            
        }
    }
}
