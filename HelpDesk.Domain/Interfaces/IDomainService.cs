using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HelpDesk.Domain.Interfaces;

/// <summary>
/// Interfaz marcadora para identificar Servicios de Dominio (DDD).
/// 
/// En arquitectura de software y DDD (Domain-Driven Design), IDomainService se utiliza como una 
/// interfaz marcadora (Marker Interface).
/// 
/// FUNCIONES:
/// ---------
/// Aunque no tenga código, cumple tres funciones clave:
/// Inyección de Dependencias Automática(IoC/DI):
/// Te permite registrar en el contenedor de dependencias todos los servicios de dominio en una sola línea mediante 
/// reflection o librerías como Scrutor, buscando todas las interfaces que implementen IDomainService.
/// 
/// Categorización Arquitectónica:
/// Al leer public interface ITicketAssignmentService : IDomainService, cualquier desarrollador o herramienta de 
/// análisis estático de código identifica de inmediato que ITicketAssignmentService es un Servicio de Dominio y 
/// no un servicio de aplicación, de infraestructura o de base de datos.
/// 
/// Reglas de Reflexión y Pruebas Arquitectónicas:
/// Te permite hacer pruebas con librerías como NetArchTest para asegurar reglas del tipo: 
/// "Todos los servicios que implementen IDomainService deben estar ubicados únicamente dentro del ensamblado HelpDesk.Domain".
/// 
/// </summary>
public interface IDomainService
{

}
