using FluentValidation;
using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Validators;

/// Validador para ContaDto
public class ContaValidator : AbstractValidator<ContaDto>
{
    public ContaValidator()
    {
        RuleFor(x => x.NomeContaContabil)
            .NotEmpty().WithMessage("Nome da conta é obrigatório")
            .MaximumLength(70).WithMessage("Nome não pode ter mais de 70 caracteres")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Nome contém caracteres inválidos");

        RuleFor(x => x.Tipo)
            .Must(tipo => tipo == 'R' || tipo == 'D')
            .WithMessage("Tipo deve ser 'R' (Receita) ou 'D' (Despesa)");
    }
}

/// Validador para CriarContaDto
public class CriarContaValidator : AbstractValidator<CriarContaDto>
{
    public CriarContaValidator()
    {
        RuleFor(x => x.NomeContaContabil)
            .NotEmpty().WithMessage("Nome da conta é obrigatório")
            .MaximumLength(70).WithMessage("Nome não pode ter mais de 70 caracteres")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Nome contém caracteres inválidos");

        RuleFor(x => x.Tipo)
            .Must(tipo => tipo == 'R' || tipo == 'D')
            .WithMessage("Tipo deve ser 'R' (Receita) ou 'D' (Despesa)");
    }
}

/// Validador para AtualizarContaDto
public class AtualizarContaValidator : AbstractValidator<AtualizarContaDto>
{
    public AtualizarContaValidator()
    {
        RuleFor(x => x.NomeContaContabil)
            .NotEmpty().WithMessage("Nome da conta é obrigatório")
            .MaximumLength(70).WithMessage("Nome não pode ter mais de 70 caracteres")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Nome contém caracteres inválidos");

        RuleFor(x => x.Tipo)
            .Must(tipo => tipo == 'R' || tipo == 'D')
            .WithMessage("Tipo deve ser 'R' (Receita) ou 'D' (Despesa)");
    }
}
