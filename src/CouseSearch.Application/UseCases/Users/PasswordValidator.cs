using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CourseSearch.Application.UseCases.Users;
public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{ErrorMessage}";
    }
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        if (Regex.IsMatch(password, @"[A-Z]+") == false)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        if (Regex.IsMatch(password, @"[a-z]+") == false)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        if (Regex.IsMatch(password, @"[0-9]+") == false)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        if (Regex.IsMatch(password, @"[\!\@\#\$\%\^\&\*\(\)\+\{\}\[\]\|\\\\\:\;\<\>\,\.\?\/\~\`]+") == false)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter especial.");
            return false;
        }

        return true;
    }
}