# ProjectAPI - Sistema de Gestión de Clientes y Posts

## 📋 Descripción

ProjectAPI es un microservicio desarrollado en .NET que proporciona APIs RESTful para la gestión de clientes (customers) y publicaciones (posts). El proyecto implementa una arquitectura limpia con separación de responsabilidades y utiliza patrones como CQRS con MediatR.

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas con los siguientes componentes:

```
ProjectAPI/
├── API/                    # Capa de presentación (Controllers, Middleware)
├── Business/              # Lógica de negocio (Commands, Queries, Handlers)
├── DataAccess/           # Acceso a datos (Repositories, DbContext)
├── Domain/               # Entidades de dominio y DTOs
└── TestApi/              # Pruebas unitarias e integración
```

### Capas del Sistema

- **API**: Controladores REST, middleware personalizado y configuración de la aplicación
- **Business**: Implementación de CQRS con MediatR, validaciones de negocio
- **DataAccess**: Repositorios, Entity Framework DbContext
- **Domain**: Entidades, DTOs, enums y validadores
- **TestApi**: Pruebas automatizadas

## 🚀 Características Principales

### Gestión de Clientes (Customers)
- ✅ Crear nuevos clientes con validación de nombre único
- ✅ Obtener todos los clientes
- ✅ Obtener cliente por ID
- ✅ Actualizar información del cliente
- ✅ Eliminar cliente (incluye eliminación en cascada de posts)
- ✅ Obtener posts asociados a un cliente

### Gestión de Posts
- ✅ Crear posts individuales con validaciones automáticas
- ✅ Crear múltiples posts simultáneamente
- ✅ Obtener todos los posts
- ✅ Obtener post por ID
- ✅ Actualizar posts
- ✅ Eliminar posts
- ✅ Obtener posts por cliente

### Validaciones Implementadas
- **Clientes únicos**: No se permiten clientes con el mismo nombre
- **Validación de usuario**: Verificación de existencia del cliente antes de crear posts
- **Truncado de texto**: Posts con body > 20 caracteres se truncan a 97 caracteres + "..."
- **Categorización automática**: 
  - Type 1 → "Farándula"
  - Type 2 → "Política" 
  - Type 3 → "Futbol"
  - Otros → Categoría personalizada

## 🛠️ Tecnologías Utilizadas

- **.NET 6/7**: Framework principal
- **ASP.NET Core**: Web API
- **Entity Framework Core**: ORM para acceso a datos
- **SQL Server**: Base de datos
- **MediatR**: Patrón CQRS y mediador
- **Serilog**: Logging estructurado
- **FluentValidation**: Validaciones (si está implementado)

## 📦 Prerrequisitos

- .NET 6.0 SDK o superior
- SQL Server LocalDB o SQL Server Express
- Visual Studio 2022 o VS Code

## ⚙️ Configuración

### 1. Clonar el Repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd ProjectAPI
```

### 2. Configurar Base de Datos

#### Opción A: Restaurar desde Backup
```bash
# Restaurar el archivo JujuTests.bak en SQL Server Management Studio
```

#### Opción B: Ejecutar Script SQL
```bash
# Ejecutar el archivo JujuTests.Script.sql en tu instancia de SQL Server
```

### 3. Configurar Cadena de Conexión

Editar `appsettings.json` en el proyecto API:

```json
{
  "ConnectionStrings": {
    "Development": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=JujuTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  }
}
```

### 4. Restaurar Dependencias
```bash
dotnet restore
```

### 5. Ejecutar Migraciones (si es necesario)
```bash
dotnet ef database update --project DataAccess --startup-project API
```

## 🚀 Ejecución

### Desde Visual Studio
1. Establecer `API` como proyecto de inicio
2. Presionar F5 o Ctrl+F5

### Desde Línea de Comandos
```bash
cd API
dotnet run
```

La aplicación estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## 📚 API Endpoints

### Customers

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/Customer` | Obtener todos los clientes |
| GET | `/Customer/{id}` | Obtener cliente por ID |
| POST | `/Customer` | Crear nuevo cliente |
| PUT | `/Customer/{id}` | Actualizar cliente |
| DELETE | `/Customer/{id}` | Eliminar cliente |
| GET | `/Customer/{id}/posts` | Obtener posts del cliente |

### Posts

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/Post` | Obtener todos los posts |
| GET | `/Post/{id}` | Obtener post por ID |
| POST | `/Post` | Crear nuevo post |
| POST | `/Post/multiple` | Crear múltiples posts |
| PUT | `/Post/{id}` | Actualizar post |
| DELETE | `/Post/{id}` | Eliminar post |
| GET | `/Post/customer/{customerId}` | Obtener posts por cliente |

## 📝 Ejemplos de Uso

### Crear Cliente
```json
POST /Customer
{
  "name": "Juan Pérez"
}
```

### Crear Post
```json
POST /Post
{
  "title": "Mi primer post",
  "body": "Este es el contenido del post que será validado automáticamente",
  "type": 1,
  "customerId": 1
}
```

### Crear Múltiples Posts
```json
POST /Post/multiple
{
  "posts": [
    {
      "title": "Post 1",
      "body": "Contenido del primer post",
      "type": 2,
      "customerId": 1
    },
    {
      "title": "Post 2", 
      "body": "Contenido del segundo post",
      "type": 3,
      "customerId": 1
    }
  ]
}
```

## 🧪 Pruebas

### Ejecutar Pruebas
```bash
dotnet test
```

### Ejecutar Pruebas con Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Logging

El sistema utiliza Serilog para logging estructurado:

- **Consola**: Logs en tiempo real durante desarrollo
- **Archivo**: Logs diarios en la carpeta `logs/`
- **Base de datos**: Logs almacenados en tabla `Logs` (SQL Server)

## 🔧 Mejoras Implementadas

1. **Validaciones de negocio robustas**
2. **Manejo de errores centralizado con middleware**
3. **Logging estructurado con Serilog**
4. **Arquitectura CQRS con MediatR**
5. **Separación clara de responsabilidades**
6. **Eliminación en cascada para integridad referencial**
7. **API para creación masiva de posts**
8. **Validación automática de modelos**

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles.

## 📞 Contacto

**Equipo de Desarrollo:**
- andres.rodriguez@juju.com.co

**Equipo de Innovación:**
- cristian.moreno@juju.com.co

---

## 🔍 Notas Técnicas

### Estructura de Base de Datos

```sql
-- Tabla Customer
Customer (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
)

-- Tabla Post
Post (
    PostId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255),
    Body NVARCHAR(MAX),
    Type INT,
    Category NVARCHAR(100),
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId)
)
```

### Patrones Implementados

- **Repository Pattern**: Para abstracción del acceso a datos
- **CQRS**: Separación de comandos y consultas
- **Mediator Pattern**: Desacoplamiento de controladores y lógica de negocio
- **Dependency Injection**: Inversión de control
- **Clean Architecture**: Separación de capas y responsabilidades

---

*Desarrollado con ❤️ para Post Ltda.*