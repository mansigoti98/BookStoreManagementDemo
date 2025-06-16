using BookStoreManagement_1.ViewModels.Customer;
using FluentValidation;

namespace BookStoreManagement_1.Models
{
    public class Validator:AbstractValidator<CustomerCreateViewModel>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is Required.")
                .Length(5, 30).WithMessage("Name must be between 5 and 30 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is Required.")
                .EmailAddress().WithMessage("Valid Email Address is Required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required.")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

            RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(10).WithMessage("Phone number must be between 10 digits.");


            RuleFor(x => x.BillingAddress)
                .NotEmpty().WithMessage("Address is Required.")
                .MaximumLength(20).WithMessage("Maximum 20 characters.");

        }
    }
}
