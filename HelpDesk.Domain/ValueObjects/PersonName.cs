using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.ValueObjects;

public sealed class PersonName
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    private PersonName()
    {
        // Constructor requerido por EF Core
    }

    private PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException(
                "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException(
                "El apellido es obligatorio.");

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > 50)
            throw new DomainException(
                "El nombre no puede superar los 50 caracteres.");

        if (lastName.Length > 50)
            throw new DomainException(
                "El apellido no puede superar los 50 caracteres.");

        return new PersonName(firstName, lastName);
    }

    public override string ToString()
    {
        return FullName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not PersonName other)
            return false;

        return FirstName == other.FirstName &&
               LastName == other.LastName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }
}
