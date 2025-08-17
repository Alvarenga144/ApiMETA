# ApiMETA

ApiMETA es una API REST construida con **ASP.NET Web API** sobre el framework **.NET Framework 4.7.2**. Provee servicios para la gestión de comercios, usuarios y transacciones dentro del ecosistema META.

## Módulos principales

- **Autenticación** (`api/Authentication`) – genera tokens JWT para acceder al resto de los servicios.
- **Usuarios** (`api/Users`) – búsqueda, listado y administración de usuarios.
- **Roles de usuarios** (`api/RoleUsers`) – asignación y eliminación de roles a los usuarios.
- **Comercios** (`api/Commerces`) – consulta y administración de comercios asociados.
- **Transacciones** (`api/Transaction`) – búsqueda de transacciones, consulta de detalle y envío de vouchers.
- **Monitor** (`api/Monitor`) – monitoreo de transacciones en tiempo real.
- **Estadísticas** (`api/Statistics`) – reportes agrupados por tipo de compra, comercio, adquirente y terminal.

## Tecnologías

- ASP.NET Web API 2
- .NET Framework 4.7.2
- JWT para autenticación
- SQL Server (acceso mediante procedimientos almacenados)
- Paquetes NuGet como `Newtonsoft.Json` y `System.IdentityModel.Tokens.Jwt`

## Requisitos

- Windows o Linux con [Mono](https://www.mono-project.com/) o Visual Studio.
- .NET Framework 4.7.2
- SQL Server con las cadenas de conexión definidas en `Web.config`.

## Uso básico

1. **Obtener token**
   - Endpoint: `POST /api/Authentication/Token`
   - Cuerpo: `{ "Usuario": "nombre", "Password": "secreto" }`
2. **Consumir otros endpoints**
   - Enviar el token en el encabezado `Authorization: Bearer <token>`

## Compilación

```bash
nuget restore
xbuild ApiMETA.sln
```

> En entornos sin Windows se puede compilar con [Mono](https://www.mono-project.com/). Asegúrate de restaurar los paquetes NuGet antes de compilar.

## Estructura del proyecto

- `Controllers/` – Controladores Web API.
- `Models/` – Lógica de acceso a datos y utilidades.
- `Areas/HelpPage/` – Documentación automática de la API.

## Versionado

El proyecto se encuentra en **.NET Framework 4.7.2**. Para nuevas características o migración a .NET moderno se recomienda crear ramas específicas.

## Licencia

Este proyecto se distribuye únicamente con fines educativos. Ajusta esta sección según corresponda.

