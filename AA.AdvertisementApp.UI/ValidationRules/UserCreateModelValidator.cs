using AA.AdvertisementApp.UI.Models;
using FluentValidation;
using System;

namespace AA.AdvertisementApp.UI.ValidationRules
{
    public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateModelValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş olamaz");
            RuleFor(x => x.Password).MinimumLength(3).WithMessage("Parolanız minimum 3 karakter olmalıdır");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Parolalar eşleşmiyor");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı kısmı boş olamaz");
            RuleFor(x => x.Username).MinimumLength(3).WithMessage("Kullanıcı adı minimum 3 karakterden oluşmalıdır.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad kısmı boş geçilemez");
            RuleFor(x => x.SecondName).NotEmpty().WithMessage("Soy ad boş geçilemez");
            RuleFor(x => new
            {
                x.Username,
                x.FirstName
            }).Must(x =>CanNotFirstname(x.Username,x.FirstName)).
            WithMessage("Kullanıcı adı,adınzı içeremez").When(x=>x.Username 
            !=null && x.FirstName !=null);
            RuleFor(x => x.GenderId).NotEmpty().WithMessage("Lütfen cinsiyet seçiniz");

        }

        private bool CanNotFirstname(string username,string firstname)
        {
            return !username.Contains(firstname);
        }
    }
}
