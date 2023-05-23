using System;
using System.Text.RegularExpressions;
using FluentValidation;
using logistics.Dtos;

namespace logistics.Validators
{
    public class AdminRegisterValidator : AbstractValidator<AdminDto>
    {
        public AdminRegisterValidator()
        {
            RuleFor(a => a.UserPhone).NotEmpty().WithMessage("手机号不能为空");
            RuleFor(a => a.UserPhone).Matches(new Regex(@"^1[3456789]\d{9}$")).WithMessage("请验证您的手机号");
            //RuleFor(a => a.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(a => a.Password).MaximumLength(12).WithMessage("密码长度最大为12位");
            RuleFor(a => a.Password).MinimumLength(6).WithMessage("密码长度最小为6位");
        }
    }
}

