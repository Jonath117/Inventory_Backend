# Sistema de Gestión Empresarial - Módulo de Inventario

## Descripción breve del sistema
El Módulo de Inventario es un componente central del Sistema de Gestión Empresarial diseñado para controlar y administrar de manera eficiente el flujo de productos en las bodegas. Construido bajo los principios de **Arquitectura Limpia (Clean Architecture)** y **Diseño Guiado por el Dominio (DDD)**, este módulo garantiza un código escalable, mantenible y altamente cohesivo, separando claramente las reglas de negocio del acceso a datos y la interfaz de usuario.

## Alcance funcional implementado
El alcance actual del proyecto abarca la gestión integral de catálogos base (categorías, unidades de medida y productos) y el control transaccional del stock a través de movimientos de inventario. El sistema soporta un entorno multi-empresa (Multi-tenant), aislando la información de cada compañía mediante la validación estricta de un identificador de empresa en cada petición.

## Lista de funcionalidades desarrolladas
- **Gestión de Catálogos Maestro:**
  - **Categorías:** Creación y listado de categorías para agrupar productos.
  - **Unidades de Medida:** Creación de unidades (ej. Litros, Cajas, Porciones) con validación de duplicados para evitar inconsistencias de datos.
  - **Productos:** Registro de productos vinculados a sus respectivas categorías y unidades de medida (SKU, nombre, stock mínimo, etc.).
- **Control de Bodegas e Inventario:**
  - Listado de bodegas disponibles por empresa.
  - **Movimientos de Inventario (Kárdex):**
    - **Entradas (IN):** Registro de ingresos por compras a proveedores con número de factura.
    - **Salidas (OUT):** Registro de bajas, mermas o ventas con número de ticket.
    - Validación de reglas de negocio en tiempo real (ej. no permitir cantidades menores o iguales a cero).

## Tecnologías utilizadas
**Backend:**
- **Plataforma:** .NET 10 (C#)
- **Base de Datos:** PostgreSQL
- **ORM:** Entity Framework Core
- **Arquitectura:** Clean Architecture, Repository Pattern, DTOs (Data Transfer Objects).

**Frontend:**
- **Librería Core:** React (con Vite)
- **Lenguaje:** TypeScript
- **Estilos:** Tailwind CSS
- **Estructura:** Modular (Features, Services, Components, Types).

## Instrucciones de ejecución

### Prerrequisitos
- .NET SDK 10.0+
- Node.js v18+
- PostgreSQL en ejecución (puerto 5432)

### Configuración del Backend (API)
1. Navegar a la carpeta del backend: `cd backend/Backend.API`
2. Configurar la cadena de conexión a PostgreSQL en el archivo `appsettings.json`.
3. Aplicar las migraciones a la base de datos:
   ```bash
   dotnet ef database update --project "..\src\Modules\Inventory\Inventory.Infrastructure" --context InventoryDbContext
   ```


### Ejecutar la API:
```
dotnet run
```

La API estará disponible en http://localhost:5153

### Configuración del Frontend (React)
Navegar a la carpeta del frontend: cd frontend

Instalar las dependencias:

````
npm install
````
### Configurar las variables de entorno en el archivo .env:

````
VITE_API_URL=http://localhost:5153/api
````
Iniciar el servidor de desarrollo:

````
npm run dev
````

## Estructura general del repositorio
El repositorio está dividido en dos partes principales, siguiendo estrictamente Clean Architecture en el backend:
```
/
├── backend/
│   ├── Backend.API/                  # Capa de Presentación (Controladores, inyección dedependencias)
│   └── src/Modules/Inventory/
│       ├── Inventory.Domain/         # Entidades puras (Product, Unit, Movement) e Interfaces
│       ├── Inventory.Application/    # Casos de uso (Servicios, DTOs, validaciones de negocio)
│       └── Inventory.Infrastructure/ # Acceso a datos (DbContext, Repositories, Migraciones)
│
└── frontend/
    └── src/
        ├── components/               # Componentes UI reutilizables (Botones, Toasts, Layouts)
        ├── features/                 # Módulos de la aplicación (ej. /in-out para movimientos)
        ├── services/                 # Peticiones HTTP a la API (MovementService.ts, etc.)
        └── types/                    # Interfaces de TypeScript (IRegisterMovement, etc.)
```