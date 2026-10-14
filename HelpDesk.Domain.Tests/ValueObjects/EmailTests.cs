using HelpDesk.Domain.Exceptions;
using HelpDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("usuario@dominio.com")]
    [InlineData("test.user@empresa.co.es")]
    public void Create_WithValidEmail_ShouldCreateInstance(string validEmail)
    {
        // Act
        var email = Email.Create(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyEmail_ShouldThrowDomainException(string? invalidEmail)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => Email.Create(invalidEmail!));
        Assert.Equal("El email es obligatorio.", exception.Message);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0057")]
    public void Create_ExceedingMaxLength_ShouldThrowDomainException()
    {
        // Arrange: Generamos una cadena válida pero de 255 caracteres
        // 242 letras 'a' + '@domain.com' (11 chars) = 253 chars (válido)
        // 244 letras 'a' + '@domain.com' (11 chars) = 255 chars (excede los 254)
        var longEmail = new string('a', 244) + "@domain.com";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => Email.Create(longEmail));
        Assert.Equal("El email no puede superar los 254 caracteres.", exception.Message);
    }

    [Theory]
    [InlineData("email-invalido")]
    [InlineData("usuario@")]
    [InlineData("@dominio.com")]
    [InlineData("usuario@dominio .com")] // Se cambia el caso con puntos dobles por espacio interno
    public void Create_WithInvalidFormat_ShouldThrowDomainException(string invalidFormat)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Email.Create(invalidFormat));
    }

    [Fact]
    public void Equals_WithSameEmailDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var email1 = Email.Create("SOPORTE@Empresa.com");
        var email2 = Email.Create("soporte@empresa.com");

        // Act
        var result = email1.Equals(email2);

        // Assert
        Assert.True(result);
    }
}
