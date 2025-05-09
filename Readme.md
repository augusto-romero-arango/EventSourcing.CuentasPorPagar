# Solution template: Cosmos EventSourcing

## Descripción

Nuget de Plantilla de solución para event sourcing.

Construye los siguientes proyectos:

- **Dominio**: Contiene los agregados, comandos, eventos, commandHandlers y proyecciones.
- **API**: API REST de la aplicación con minimal API, healthchecks y Open API.
- **EventStore**: Implementación de `Marten` como event store.'
- **Dominio.Tests**: Abstracción del StoreEvent para pruebas y CommandHandlerTest.
- **AcceptanceTests**: Proyecto de pruebas de integración que ejecutan el API y una base de datos con TestContainers.

## Instalación

```bash
dotnet new install Cosmos.EventSourcing.Template
```

## Forma de uso

Después de instalado, la plantilla estará disponible `Cosmos EventSourcing Solution` funciona para visual studio y Rider.

Por línea de comandos:

```bash
dotnet new Cosmos.EventSourcing.Solution -n nombre-del-proyecto
```

### Configuraciones avanzadas

En el caso que necesite correr varias aplicaciones a la vez puede configurar los puertos de cada una de ellas para evitar colisiones.

Cuando cree la solución en su IDE, en la sección **Configuraciones avanzadas** puede cambiar los puertos.

En caso que no lo haya hecho, debe cambiar los puertos en:

- `docker-compose.override.yml` que se encuentra en la raíz de la solución.
- `appsettings.json` que se encuentra en el proyecto API.'

## Correr la aplicación

Para correr la aplicación, docker desktop debe estar instalado y ejecutándose.
En el directorio raíz de la solución, ejecute el siguiente comando:

```bash
docker-compose up -d
```

### API

La API está expuesta en `http://localhost:8080`

### Acceder a la base de datos

Para consultar la base de datos, puede conectarse desde el IDE a un proveedor de Postgres.
Como se ve en el docker-compose, la base de datos está expuesta en el puerto 5432.

Datos de conexión:

POSTGRES_USER: CuentasPorPagarUser

POSTGRES_PASSWORD: CuentasPorPagarPassword

POSTGRES_DB: cuentasporpagardb

POSTGRES_PORT: 5432

### Revisar los logs y métricas en .NetAspire

.NetAspires queda expuesto en `http://localhost:18888`

## Tecnologías utilizadas

- **.NET 9**
- **Marten**: Event Store basado en PostgreSQL.
- **Wolverine**: Bus de mensajes para manejar comandos y eventos.
- **OpenAPI**: Documentación interactiva de la API.
- **Health Checks**: Supervisión de la salud de la aplicación.

## Características principales

- **Event Sourcing**: Uso de **Marten** para almacenar eventos, rehidratar los agregados y crear proyecciones.
- **Enrutamiento de comandos**: Mediador que invoca los handlers dependiendo del comando o evento recibido.
- La implementación  `WolverineCommandRouter` es una abstracción de Wolverine como mediador.
- **Transacciones automáticas**: `SaveChanges()` automático al finalizar la ejecución del handler.

## Configuración

### Requisitos previos

- **.NET 9 SDK**: Asegúrate de tener instalado el SDK de .NET 9.
- **Docker desktop**: La solución incluye un docker compose para levantar los contenedores de base de datos y aplcación.
