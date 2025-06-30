# 🗂️ InventarioXWY

**InventarioXWY** es una aplicación web desarrollada con **ASP.NET Core MVC** y **Entity Framework Core**, que permite gestionar el inventario de una empresa, incluyendo el control de artículos, préstamos y usuarios con distintos roles.

---

## 📌 Objetivo del Proyecto

Brindar una solución de software organizada, segura y escalable para la administración de inventarios, préstamos de artículos y usuarios, permitiendo:

- Control por roles: Administrador y Operador.
- Registro de préstamos y devoluciones.
- Mantenimiento de artículos.
- Generación de reportes (PDF/Excel, opcional).
- Seguridad en el acceso y protección de datos.

---

## 🧱 Arquitectura del Proyecto

El proyecto sigue una arquitectura en **N capas**, facilitando el mantenimiento y escalabilidad:

InventarioXWY.sln
│
├── MyApp.Presentation     => Capa de presentación (ASP.NET Core MVC)
├── MyApp.Business         => Lógica de negocio (services)
├── MyApp.DataAccess       => Acceso a datos con EF Core
├── MyApp.Entities         => Entidades del dominio (POCOs)

---

## 🔐 Gestión de Usuarios y Roles

### Funcionalidades:

- **Registro de usuario:** cualquier persona puede registrarse. Por defecto se asigna el rol `Operador`.
- **Inicio de sesión:** autenticación por email y contraseña.
- **Asignación de roles:** solo el usuario con rol `Administrador` puede cambiar roles entre `Administrador` y `Operador`.
- **Control de acceso:** páginas protegidas según el rol. Los intentos no autorizados redirigen a una página de "Acceso Denegado".

---

## 📦 Gestión de Artículos

Permite a usuarios autenticados:

- Crear nuevos artículos con código, nombre, categoría, estado y ubicación.
- Editar artículos (excepto el ID).
- Eliminar artículos solo si no están en préstamo activo.
- Buscar y filtrar por nombre, código, categoría o estado.
- Paginación y ordenamiento automático.

---

## 🔄 Gestión de Préstamos

- Los operadores pueden **solicitar préstamos** de artículos disponibles.
- Los administradores pueden **aprobar o rechazar** solicitudes.
- Al devolver un artículo, se cambia su estado a `Disponible` y se registra la fecha de devolución.
- Vista de **historial** con filtros por usuario, artículo o estado.

---

## 🖥️ Tecnologías Usadas

- **Backend:** ASP.NET Core MVC (.NET 8)
- **ORM:** Entity Framework Core
- **Base de datos:** SQL Server 2019+
- **Frontend:** Bootstrap 5, Razor Views
- **Seguridad:** ASP.NET Identity
- **DI:** Inyección de dependencias incorporada
- **Autenticación:** Cookies y Identity
- **Logger:** Console Logging (extensible a Serilog)
- **Idioma:** Español

---

## 🔐 Seguridad

- Contraseñas hasheadas con ASP.NET Identity (BCrypt).
- Protección contra CSRF y XSS.
- Autorización basada en roles.
- Página de acceso denegado personalizada.

---

## 🔧 Configuración Inicial

1. Clona el repositorio:
   git clone https://github.com/tuusuario/InventarioXWY.git

2. Configura la cadena de conexión en appsettings.json:
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=InventarioXWY;Trusted_Connection=True;TrustServerCertificate=True;"
   }

3. Ejecuta las migraciones:
   Add-Migration InitialCreate
   Update-Database

4. Ejecuta el proyecto (Ctrl + F5 en Visual Studio).

5. Registra un usuario o usa los datos de prueba insertados.

---

## 🧪 Datos de prueba para SQL Server (opcional)

USE InventarioXWY;

-- Roles (si no usas ASP.NET Identity)
INSERT INTO Roles (Nombre) VALUES ('Administrador'), ('Operador');

-- Usuarios simulados
INSERT INTO Usuarios (Nombre, Email, PasswordHash, Rol)
VALUES ('Ana Pérez', 'ana@xwy.com', 'HASH_ADMIN', 'Administrador');

-- Artículos
INSERT INTO Articulos (Codigo, Nombre, Categoria, Estado, Ubicacion)
VALUES ('EQP001', 'Laptop Dell', 'Electrónica', 'Disponible', 'Bodega A');

-- Préstamos
INSERT INTO Prestamos (ArticuloId, UsuarioId, FechaEntrega, Estado)
VALUES (1, 1, '2025-07-01', 'Pendiente');

---

## 🧪 Pruebas

- Se pueden agregar pruebas unitarias en el proyecto MyApp.Tests.
- Para pruebas de integración se recomienda usar InMemoryDatabase.

---

## 📈 Futuras mejoras

- Generación de reportes en PDF y Excel.
- Soporte para múltiples idiomas con archivos .resx.
- Contenerización con Docker.
- Registro de logs de actividad con Serilog.

---

## 📄 Licencia

Este proyecto es de uso académico y puede ser modificado libremente con fines educativos.
