using HelpDesk.Domain.Exceptions;
using HelpDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Tests.ValueObjects;

public class PersonNameTests
{
    [Theory]
    [InlineData("Juan", "Pérez")]
    [InlineData("María Carmen", "García López")]
    public void Create_WithValidFirstAndLastName_ShouldCreateInstance(string firstName, string lastName)
    {
        // Act
        var personName = PersonName.Create(firstName, lastName);

        // Assert
        Assert.NotNull(personName);
        Assert.Equal(firstName, personName.FirstName);
        Assert.Equal(lastName, personName.LastName);
        Assert.Equal($"{firstName} {lastName}", personName.FullName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidFirstName_ShouldThrowDomainException(string? invalidFirstName)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => PersonName.Create(invalidFirstName!, "Pérez"));
        Assert.Equal("El nombre es obligatorio.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidLastName_ShouldThrowDomainException(string? invalidLastName)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => PersonName.Create("Juan", invalidLastName!));
        Assert.Equal("El apellido es obligatorio.", exception.Message);
    }

    [Fact]
    public void Create_ExceedingFirstNameMaxLength_ShouldThrowDomainException()
    {
        // Arrange (Nombre de más de 50 caracteres)
        var longFirstName = new string('a', 51);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => PersonName.Create(longFirstName, "Pérez"));
        Assert.Equal("El nombre no puede superar los 50 caracteres.", exception.Message);
    }

    [Fact]
    public void Create_ExceedingLastNameMaxLength_ShouldThrowDomainException()
    {
        // Arrange (Apellido de más de 50 caracteres)
        var longLastName = new string('a', 51);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => PersonName.Create("Juan", longLastName));
        Assert.Equal("El apellido no puede superar los 50 caracteres.", exception.Message);
    }

    [Fact]
    public void Equals_WithSameFirstAndLastName_ShouldReturnTrue()
    {
        // Arrange
        var name1 = PersonName.Create("Juan", "Pérez");
        var name2 = PersonName.Create("Juan", "Pérez");

        // Act
        var result = name1.Equals(name2);

        // Assert
        Assert.True(result);
    }
}
