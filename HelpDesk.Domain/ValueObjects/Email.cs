using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.ValueObjects;

/// <summary>
/// 
/// </summary>
public sealed class Email
{
    public string Value { get; private set; }

    private Email()
    {
        // Constructor requerido por EF Core
    }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "El email es obligatorio.");
        }

        value = value.Trim();

        if (value.Length > 254)
        {
            throw new DomainException(
                "El email no puede superar los 254 caracteres.");
        }

        if (!IsValidEmail(value))
        {
            throw new DomainException(
                $"El email '{value}' no tiene un formato válido.");
        }

        return new Email(value);
    }

    private static bool IsValidEmail(string value)
    {
        try
        {
            var mailAddress = new MailAddress(value);

            return mailAddress.Address.Equals(
                value,
                StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Email other)
        {
            return false;
        }

        return string.Equals(
            Value,
            other.Value,
            StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase
            .GetHashCode(Value);
    }
}
