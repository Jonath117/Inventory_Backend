# Sistema de Gestión de Inventario y Ventas
## Módulos Core: Inventario & Ventas (Backend)

Este repositorio contiene el núcleo transaccional del sistema, desarrollado en una arquitectura de **Monolito Modular** utilizando **.NET 8** y **PostgreSQL**. El sistema está compuesto principalmente por dos módulos altamente integrados pero desacoplados a nivel de lógica de negocio: **Sales (Ventas)** e **Inventory (Inventario)**.

---

## Justificación del Repositorio Único (Monorepo)

La decisión de mantener ambos módulos dentro de un mismo repositorio físico responde a principios clave de ingeniería de software orientados al desarrollo ágil y la consistencia del sistema:

1. **Arquitectura de Monolito Modular:** Aunque el sistema está lógicamente dividido en módulos con límites de contexto claros (*Bounded Contexts*), compartir un mismo repositorio facilita la centralización del **Shared Kernel** (componentes compartidos como el `GlobalExceptionHandler`, proveedores de contexto comunes y utilidades transversales), evitando la duplicación de código (Principio DRY).
2. **Sincronización de Contratos API:** El contrato de integración (`contrato-api.yaml` bajo OpenAPI 3.0) define cómo se comunican Ventas e Inventario. Al estar en el mismo repositorio, cualquier cambio en los DTOs de comunicación se versiona en simultáneo, eliminando el riesgo de desalineación de contratos entre equipos.
3. **Simplicidad de Orquestación y Despliegue Local:** Permite compilar, ejecutar y realizar pruebas de integración de punta a punta (End-to-End) de forma local sin la complejidad de gestionar múltiples repositorios, submódulos de Git o registros de paquetes NuGet privados.

---

## Descripción de los Módulos

### 1. Módulo de Ventas (`Sales`)
Encargado de la gestión comercial y el ciclo de vida de las transacciones con los clientes en el Punto de Venta (POS).
* **Responsabilidades:** Apertura y asignación de meseros a mesas/tickets, adición incremental de productos al catálogo central, edición en tiempo real de cantidades mediante lógica de *Aggregate Root*, cálculo automatizado de impuestos y totales, y procesamiento seguro de pagos.
* **Patrones Clave:** Implementación de **CQRS** mediante **MediatR** para desacoplar las lecturas de las mutaciones, asegurando que los controladores expongan endpoints delgados y limpios de lógica de control.

### 2. Módulo de Inventario (`Inventory`)
Funciona como la única fuente de la verdad (*Single Source of Truth*) respecto a las existencias físicas y el control de stock de la empresa.
* **Responsabilidades:** Control de existencias por bodegas/almacenes, validación atómica de requerimientos de stock previa a la venta, registro cronológico de movimientos de inventario (`IN` / `OUT`) bajo transacciones aisladas, y gestión de unidades de medida.
* **Garantía Transaccional:** Implementa un sistema anti-sobreventas que revierte el inventario (Rollback) si ocurre algún fallo de infraestructura a mitad de la operación.

---

## Instrucciones para Correr los Proyectos desde Cero

### Prerrequisitos
Antes de iniciar, asegúrate de contar con las siguientes herramientas instaladas en tu máquina de desarrollo:
* **.NET 8.0 SDK** o superior.
* **PostgreSQL Server** (activo y escuchando en el puerto estándar `5432`).
* Herramientas de terminal o un IDE como *VS Code*, *Visual Studio 2022* o *JetBrains Rider*.

---

### Paso 1: Configuración de Variables de Entorno

Ambos módulos requieren archivos de configuración para interactuar de forma segura sin exponer credenciales críticas en el historial de Git.

#### Configuración en Ventas (`src/Modules/Sales/Sales.Api/`)
Crea un archivo `.env` o renombra el `.env.example` con las siguientes claves:
```env
# URL base donde estará corriendo el módulo de Inventario
INVENTORY_API_URL=http://localhost:5224

# Cadena de conexión a la Base de Datos de Ventas
ConnectionStrings__DefaultConnection=Host=localhost;Database=CampeandoSalesDb;Username=postgres;Password=tu_contraseña_aqui

### Configuración en Inventario (`src/Modules/Inventory/Inventory.Api/`)

Crea un archivo `.env` o renombra el `.env.example` en la raíz correspondiente con la siguiente estructura:

```ini
# Cadena de conexión a la Base de Datos de Inventario
ConnectionStrings__DefaultConnection=Host=localhost;Database=CampeandoInventoryDb;Username=postgres;Password=tu_contraseña_aqui
```

---

### Paso 2: Ejecución de los Proyectos por Consola

Abre dos terminales separadas en la raíz del proyecto para levantar ambos servidores en paralelo:

#### Terminal 1: Ejecutar Módulo de Inventario
```bash
# Navegar a la carpeta del proyecto de entrada de Inventario
cd src/Modules/Inventory/Inventory.Api/

# Restaurar dependencias de NuGet
dotnet restore

# Compilar y levantar el servidor
dotnet run --urls "http://localhost:5224"
```
> El Swagger de Inventario estará disponible en: http://localhost:5224/swagger/index.html

#### Terminal 2: Ejecutar Módulo de Ventas
```bash
# Navegar a la carpeta del proyecto de entrada de Ventas
cd src/Modules/Sales/Sales.Api/

# Restaurar dependencias de NuGet
dotnet restore

# Compilar y levantar el servidor
dotnet run --urls "http://localhost:5000"
```
> El Swagger de Ventas estará disponible en: http://localhost:5000/swagger/index.html

### Paso 2: Ejecución de los Proyectos por Consola

Para que el sistema funcione de manera integral, es necesario levantar **tres servicios** en paralelo. 

> **IMPORTANTE: El rol del servicio Shared**
> Este sistema opera bajo un modelo multi-empresa (Multi-Tenant). Para que el **filtrado de compañías** funcione correctamente y las APIs puedan resolver el contexto de la empresa actual (procesando headers como x-company-id), es **obligatorio** levantar el proyecto Shared primero. Si este servicio está apagado, las consultas de Ventas e Inventario fallarán al no poder validar a qué compañía pertenecen los datos.

Abre tres terminales separadas en la raíz del proyecto y ejecuta los siguientes comandos en orden:

#### Terminal 1: Ejecutar el Módulo Shared (Base)
```bash
# Navegar a la carpeta del proyecto Shared
cd src/Modules/Shared/Shared.Api/

# Restaurar dependencias y levantar el servidor
dotnet restore
dotnet run --urls "http://localhost:5005"
```
> *Nota: Revisa bien la ruta src/Modules/Shared/Shared.Api/ y el puerto 5005 en el bloque de código de arriba, y ajústalos si tu proyecto Shared está en otra carpeta o usa otro puerto.*

#### Terminal 2: Ejecutar Módulo de Inventario
```bash
# Navegar a la carpeta del proyecto de Inventario
cd src/Modules/Inventory/Inventory.Api/

# Restaurar dependencias y levantar el servidor
dotnet restore
dotnet run --urls "http://localhost:5224"
```
> El Swagger de Inventario estará disponible en: http://localhost:5224/swagger/index.html

#### Terminal 3: Ejecutar Módulo de Ventas
```bash
# Navegar a la carpeta del proyecto de Ventas
cd src/Modules/Sales/Sales.Api/

# Restaurar dependencias y levantar el servidor
dotnet restore
dotnet run --urls "http://localhost:5000"
```
> El Swagger de Ventas estará disponible en: http://localhost:5000/swagger/index.html

