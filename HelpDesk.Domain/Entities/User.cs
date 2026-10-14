using HelpDesk.Domain.Enums;
using HelpDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class User : BaseEntity
{
    public PersonName Name { get; private set; }

    public Email Email { get; private set; }

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    private User()
    {
        // Constructor requerido por EF Core
    }

    private User(
        PersonName name,
        Email email,
        UserRole role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(
        PersonName name,
        Email email,
        UserRole role)
    {
        return new User(name, email, role);
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
