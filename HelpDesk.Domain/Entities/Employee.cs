using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

/// <summary>
/// Employee.cs:  será una Entity de dominio
/// Representa la identidad/acceso al sistema
/// </summary>
public class Employee : BaseEntity
{
    public Guid UserId { get; private set; }

    public string EmployeeNumber { get; private set; }

    public int DepartmentId { get; private set; }

    public string Position { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }


    // Relación
    public Department Department { get; private set; } = null!;

    private Employee()
    {
        // Constructor requerido por EF Core
    }

    private Employee(
        Guid userId,
        string employeeNumber,
        int departmentId,
        string position)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        EmployeeNumber = employeeNumber;
        DepartmentId = departmentId;
        Position = position;
        IsActive = true;
    }

    public static Employee Create(
        Guid userId,
        string employeeNumber,
        int departmentId,
        string position)
    {
        if (userId == Guid.Empty)
            throw new DomainException(
                "El usuario asociado es obligatorio.");

        if (departmentId < 0)
            throw new DomainException(
                "El departamento es obligatorio.");

        if (string.IsNullOrWhiteSpace(employeeNumber))
            throw new DomainException(
                "El número de empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(position))
            throw new DomainException(
                "El puesto del empleado es obligatorio.");

        employeeNumber = employeeNumber.Trim();
        position = position.Trim();

        if (employeeNumber.Length > 20)
            throw new DomainException(
                "El número de empleado no puede superar los 20 caracteres.");

        if (position.Length > 100)
            throw new DomainException(
                "El puesto no puede superar los 100 caracteres.");

        return new Employee(
            userId,
            employeeNumber,
            departmentId,
            position);
    }

    public void ChangeDepartment(int departmentId)
    {
        if(departmentId <= 0)
            throw new DomainException(
                "El departamento es obligatorio.");

        DepartmentId = departmentId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePosition(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
            throw new DomainException(
                "El puesto del empleado es obligatorio.");

        position = position.Trim();

        if (position.Length > 100)
            throw new DomainException(
                "El puesto no puede superar los 100 caracteres.");

        Position = position;
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
