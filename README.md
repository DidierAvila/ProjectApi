# ProjectAPI - Sistema de Gestión de Clientes y Posts

## 📋 Descripción

ProjectAPI es un microservicio moderno desarrollado en **.NET 8.0** que proporciona APIs RESTful para la gestión de clientes (customers) y publicaciones (posts). El proyecto implementa una **arquitectura limpia** con separación de responsabilidades, utiliza patrones como **CQRS con MediatR**, y está completamente containerizado con **Docker**.

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas con los siguientes componentes:

```
ProjectAPI/
├── API/                    # Capa de presentación (Controllers, Middleware, Auth)
├── Business/              # Lógica de negocio (Commands, Queries, Handlers)
├── DataAccess/           # Acceso a datos (Repositories, DbContext, Migrations)
├── Domain/               # Entidades de dominio, DTOs y validadores
└── TestApi/              # Pruebas unitarias e integración (xUnit)
```

### Capas del Sistema

- **API**: Controladores REST, middleware personalizado, autenticación JWT y configuración
- **Business**: Implementación de CQRS con MediatR, validaciones de negocio y handlers
- **DataAccess**: Repositorios, Entity Framework DbContext y migraciones
- **Domain**: Entidades, DTOs, enums y validadores con FluentValidation
- **TestApi**: Pruebas automatizadas con xUnit, Moq y FluentAssertions

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

### Seguridad y Autenticación
- ✅ **Autenticación JWT** implementada
- ✅ Endpoints protegidos
- ✅ Configuración de tokens segura

### Validaciones Implementadas
- **Clientes únicos**: No se permiten clientes con el mismo nombre
- **Validación de usuario**: Verificación de existencia del cliente antes de crear posts
- **Truncado de texto**: Posts con body > 20 caracteres se truncan a 97 caracteres + "..."
- **Categorización automática**: 
  - Type 1 → "Farándula"
  - Type 2 → "Política" 
  - Type 3 → "Futbol"
  - Otros → Categoría personalizada

### 🚫 Sistema de Cancelación
- **Estados de entidad**: Active (1), Cancelled (2), Inactive (3)
- **Cancelación soft**: Los registros se marcan como cancelados, no se eliminan
- **Validaciones de cancelación**:
  - Verificación de existencia de la entidad
  - Prevención de cancelación doble
  - Mantenimiento de integridad referencial
- **Trazabilidad completa**: Logs detallados de todas las operaciones de cancelación

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 8.0**: Framework principal
- **ASP.NET Core**: Web API
- **Entity Framework Core 8.0**: ORM para acceso a datos
- **SQL Server**: Base de datos
- **MediatR 13.0**: Patrón CQRS y mediador
- **AutoMapper 12.0**: Mapeo de objetos
- **FluentValidation**: Validaciones robustas
- **Serilog**: Logging estructurado

### Seguridad
- **JWT Authentication**: Autenticación basada en tokens
- **HTTPS**: Comunicación segura

### Containerización y DevOps
- **Docker**: Containerización multi-stage
- **Docker Compose**: Orquestación de servicios
- **SQL Server 2022**: Base de datos containerizada

### Testing
- **xUnit**: Framework de pruebas
- **Moq**: Mocking framework
- **FluentAssertions**: Assertions fluidas
- **Coverlet**: Cobertura de código

## 📦 Prerrequisitos

### Opción 1: Desarrollo Local
- .NET 8.0 SDK o superior
- SQL Server LocalDB o SQL Server Express
- Visual Studio 2022 o VS Code

### Opción 2: Docker (Recomendado)
- Docker Desktop
- Docker Compose

## ⚙️ Configuración y Ejecución

### 🐳 Opción 1: Usando Docker (Recomendado)

#### 1. Clonar el Repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd ProjectAPI
```

#### 2. Ejecutar con Docker Compose
```bash
# Construir y ejecutar todos los servicios
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d --build
```

La aplicación estará disponible en:
- **API**: `http://localhost:8080`
- **SQL Server**: `localhost:1433`

#### 3. Verificar el Estado
```bash
# Ver logs
docker-compose logs -f projectapi

# Ver contenedores en ejecución
docker-compose ps
```

#### 4. Detener los Servicios
```bash
docker-compose down

# Eliminar volúmenes (datos de BD)
docker-compose down -v
```

### 💻 Opción 2: Desarrollo Local

#### 1. Clonar el Repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd ProjectAPI
```

#### 2. Configurar Base de Datos

**Opción A: Restaurar desde Backup**
```bash
# Restaurar el archivo JujuTests.bak en SQL Server Management Studio
```

**Opción B: Ejecutar Script SQL**
```bash
# Ejecutar el archivo JujuTests.Script.sql en tu instancia de SQL Server
```

#### 3. Configurar Cadena de Conexión

Editar `appsettings.json` en el proyecto API:

```json
{
  "ConnectionStrings": {
    "Development": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=JujuTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  }
}
```

#### 4. Restaurar Dependencias y Ejecutar
```bash
# Restaurar paquetes NuGet
dotnet restore

# Ejecutar migraciones (si es necesario)
dotnet ef database update --project DataAccess --startup-project API

# Ejecutar la aplicación
cd API
dotnet run
```

La aplicación estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## 📚 API Endpoints

### 🔐 Autenticación
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/Security/login` | Autenticación y obtención de JWT token |

### 👥 Customers
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/Customer` | Obtener todos los clientes | ✅ |
| GET | `/Customer/{id}` | Obtener cliente por ID | ✅ |
| POST | `/Customer` | Crear nuevo cliente | ✅ |
| PUT | `/Customer/{id}` | Actualizar cliente | ✅ |
| DELETE | `/Customer/{id}` | Eliminar cliente | ✅ |
| PUT | `/Customer/{id}/cancel` | **Cancelar cliente** | ✅ |
| GET | `/Customer/{id}/posts` | Obtener posts del cliente | ✅ |

### 📝 Posts
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/Post` | Obtener todos los posts | ✅ |
| GET | `/Post/{id}` | Obtener post por ID | ✅ |
| POST | `/Post` | Crear nuevo post | ✅ |
| POST | `/Post/multiple` | Crear múltiples posts | ✅ |
| PUT | `/Post/{id}` | Actualizar post | ✅ |
| DELETE | `/Post/{id}` | Eliminar post | ✅ |
| PUT | `/Post/{id}/cancel` | **Cancelar post** | ✅ |
| GET | `/Post/customer/{customerId}` | Obtener posts por cliente | ✅ |

## 📝 Ejemplos de Uso

### Autenticación
```json
POST /Security/login
{
  "username": "admin",
  "password": "password"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-01T12:00:00Z"
}
```

### Crear Cliente
```json
POST /Customer
Authorization: Bearer {token}
{
  "name": "Juan Pérez"
}
```

### Crear Post
```json
POST /Post
Authorization: Bearer {token}
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
Authorization: Bearer {token}
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

### 🚫 Cancelar Cliente
```json
PUT /Customer/{id}/cancel
Authorization: Bearer {token}

Response:
{
  "customerId": 1,
  "name": "Juan Pérez",
  "status": 2  // 2 = Cancelled
}
```

### 🚫 Cancelar Post
```json
PUT /Post/{id}/cancel
Authorization: Bearer {token}

Response:
{
  "postId": 1,
  "title": "Mi primer post",
  "body": "Este es el contenido del post...",
  "status": 2,  // 2 = Cancelled
  "customerId": 1
}
```

## 🧪 Pruebas

### Ejecutar Pruebas
```bash
# Todas las pruebas
dotnet test

# Con cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Pruebas específicas por proyecto
dotnet test TestApi/TestApi.csproj
```

### Estructura de Pruebas
```
TestApi/
├── Customers/          # Pruebas de lógica de customers
├── Posts/             # Pruebas de lógica de posts
└── TestApi.csproj     # Configuración con xUnit, Moq, FluentAssertions
```

## 📊 Logging y Monitoreo

El sistema utiliza **Serilog** para logging estructurado:

### Configuración de Logs
- **Consola**: Logs en tiempo real durante desarrollo
- **Archivo**: Logs diarios en la carpeta `logs/` (Docker volume)
- **Base de datos**: Logs almacenados en tabla `Logs` (SQL Server)

### Niveles de Log
- **Warning**: Nivel mínimo configurado
- **Error**: Errores de aplicación
- **Information**: Información general
- **Debug**: Información detallada (desarrollo)

## 🐳 Docker Configuration

### Dockerfile Features
- **Multi-stage build**: Optimización del tamaño de imagen
- **Non-root user**: Seguridad mejorada
- **Health checks**: Monitoreo de salud del contenedor
- **.NET 8.0 Runtime**: Imagen optimizada para producción

### Docker Compose Services
- **projectapi**: Aplicación principal (Puerto 8080)
- **sqlserver**: SQL Server 2022 Express (Puerto 1433)
- **Volumes**: Persistencia de datos y logs
- **Networks**: Red aislada para comunicación segura

## 🔧 Mejoras Implementadas

### Arquitectura y Patrones
1. **Arquitectura limpia** con separación de responsabilidades
2. **CQRS con MediatR** para separar comandos y consultas
3. **Repository Pattern** para abstracción del acceso a datos
4. **Dependency Injection** nativo de .NET
5. **AutoMapper** para mapeo automático de DTOs

### Seguridad
6. **Autenticación JWT** implementada
7. **Middleware de manejo de errores** centralizado
8. **Validaciones robustas** con FluentValidation
9. **HTTPS** configurado por defecto

### DevOps y Deployment
10. **Containerización completa** con Docker
11. **Docker Compose** para orquestación
12. **Multi-stage builds** para optimización
13. **Logging estructurado** con Serilog

### Calidad de Código
14. **Pruebas unitarias** con xUnit
15. **Mocking** con Moq para pruebas aisladas
16. **Cobertura de código** con Coverlet
17. **Eliminación en cascada** para integridad referencial

## 🚀 Deployment en Producción

### Variables de Entorno
```bash
# Configurar en producción
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
ConnectionStrings__DefaultConnection="Server=sqlserver;Database=JujuTests;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;"
```

### Consideraciones de Seguridad
- Cambiar contraseñas por defecto
- Configurar certificados SSL/TLS
- Implementar rate limiting
- Configurar CORS apropiadamente
- Usar secrets management

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

### Estándares de Código
- Seguir las convenciones de C# y .NET
- Escribir pruebas para nuevas funcionalidades
- Mantener cobertura de código > 80%
- Documentar APIs con XML comments

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles.

## 📞 Contacto

**Equipo de Desarrollo:**
- andres.rodriguez@juju.com.co

**Equipo de Innovación:**
- cristian.moreno@juju.com.co

---

## 🔍 Información Técnica

### Estructura de Base de Datos

```sql
-- Tabla Customer
Customer (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL UNIQUE
)

-- Tabla Post
Post (
    PostId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255),
    Body NVARCHAR(MAX),
    Type INT,
    Category NVARCHAR(100),
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId) ON DELETE CASCADE
)

-- Tabla Logs (Serilog)
Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Message NVARCHAR(MAX),
    Level NVARCHAR(128),
    TimeStamp DATETIME,
    Exception NVARCHAR(MAX),
    Properties NVARCHAR(MAX)
)
```

### Patrones y Principios Implementados

- **Clean Architecture**: Separación de capas y dependencias
- **SOLID Principles**: Principios de diseño orientado a objetos
- **CQRS**: Separación de comandos y consultas
- **Mediator Pattern**: Desacoplamiento con MediatR
- **Repository Pattern**: Abstracción del acceso a datos
- **Dependency Injection**: Inversión de control
- **Unit of Work**: Transacciones y consistencia

### Tecnologías de Desarrollo

| Categoría | Tecnología | Versión |
|-----------|------------|---------|
| Framework | .NET | 8.0 |
| Web API | ASP.NET Core | 8.0 |
| ORM | Entity Framework Core | 8.0.18 |
| Database | SQL Server | 2022 |
| Mediator | MediatR | 13.0.0 |
| Mapping | AutoMapper | 12.0.1 |
| Validation | FluentValidation | 11.3.0 |
| Logging | Serilog | 2.10.0 |
| Testing | xUnit | 2.5.3 |
| Mocking | Moq | 4.20.70 |
| Container | Docker | Latest |

---

## 🎯 Roadmap Futuro

### Próximas Funcionalidades
- [ ] **API Versioning**: Versionado de APIs
- [ ] **Swagger/OpenAPI**: Documentación interactiva mejorada
- [ ] **Rate Limiting**: Control de velocidad de requests
- [ ] **Caching**: Implementación de Redis
- [ ] **Health Checks**: Endpoints de salud detallados
- [ ] **Metrics**: Integración con Prometheus/Grafana
- [ ] **CRUD Avanzado**: Filtros, paginación y ordenamiento
- [ ] **Notificaciones**: Sistema de eventos y notificaciones

### Mejoras Técnicas
- [ ] **Integration Tests**: Pruebas de integración completas
- [ ] **Performance Tests**: Pruebas de carga y rendimiento
- [ ] **CI/CD Pipeline**: Integración y despliegue continuo
- [ ] **Infrastructure as Code**: Terraform/ARM templates
- [ ] **Kubernetes**: Orquestación avanzada de contenedores

---

*Desarrollado con ❤️ por el equipo de Post Ltda.*

**Última actualización:** Diciembre 2024  
**Versión del proyecto:** 2.0.0  
**Compatibilidad:** .NET 8.0+