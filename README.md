# 🏥 InventarioMedico API

API REST desarrollada en **.NET 8** para la gestión de inventario médico. Permite consultar productos, realizar pedidos y visualizar estadísticas en tiempo real.

🔗 **API en producción (Azure):**
[https://inventario-medico-api-fegcarceh3b7gchd.centralus-01.azurewebsites.net/swagger](https://inventario-medico-api-fegcarceh3b7gchd.centralus-01.azurewebsites.net/swagger)

---

## 🚀 ¿Cómo usar la API?

No necesitas instalar nada. Entra al enlace de arriba y verás una interfaz llamada **Swagger** donde puedes probar todos los endpoints directamente desde el navegador.

### Pasos:
1. Abre el enlace de Swagger
2. Haz clic en el endpoint que quieres probar
3. Clic en **Try it out**
4. Completa los campos si los pide
5. Clic en **Execute**
6. Verás la respuesta en pantalla

---

## 📋 Endpoints disponibles

### 📦 Productos

| Método | Endpoint | ¿Qué hace? |
|--------|----------|------------|
| GET | `/api/Productos` | Lista productos de forma paginada (50 por página) |
| GET | `/api/Productos/{id}` | Busca un producto por su ID |
| GET | `/api/Productos/buscar?categoria=X` | Filtra productos por categoría |

**Categorías disponibles:** Medicamentos, Insumos, Equipos, Laboratorio, Emergencia

**Ejemplo:**
```
GET /api/Productos?pagina=1&porPagina=50
GET /api/Productos/1
GET /api/Productos/buscar?categoria=Medicamentos
```

### 🛒 Pedidos

| Método | Endpoint | ¿Qué hace? |
|--------|----------|------------|
| POST | `/api/Pedidos` | Crea un pedido y descuenta el stock automáticamente |
| GET | `/api/Pedidos/dashboard` | Muestra estadísticas generales del inventario |

**Ejemplo para crear un pedido:**
```
POST /api/Pedidos?productoId=1&cantidad=10
```

---

## 🧠 Conceptos técnicos aplicados

| Concepto | Dónde se aplica |
|----------|-----------------|
| **async / await** | Todos los endpoints son asíncronos para no bloquear el servidor |
| **Paginación** | `GET /api/Productos` devuelve los datos en páginas para no sobrecargar la memoria |
| **IQueryable vs IEnumerable** | El filtro por categoría se ejecuta en la base de datos (IQueryable), no en memoria |
| **Caché** | `GET /api/Productos/{id}` guarda el resultado 10 minutos para responder más rápido |
| **Transacciones** | Al crear un pedido, se descuenta el stock de forma atómica — si algo falla, se revierte todo |
| **Concurrencia** | El dashboard ejecuta 3 consultas en paralelo con `Task.WhenAll` |
| **Índices** | La base de datos tiene índices en `Categoria` y `Activo` para búsquedas más rápidas |

---

## ☁️ Despliegue en Azure

La API está desplegada en **Azure App Service** con integración continua desde GitHub:

1. Código fuente en **GitHub**
2. Cada push a `master` dispara un **GitHub Actions workflow**
3. El workflow compila y despliega automáticamente en **Azure App Service (Plan F1 Gratis)**
4. La API queda disponible en la URL pública

---

## 🛠️ Stack tecnológico

- **Backend:** C# / .NET 8
- **ORM:** Entity Framework Core (InMemory)
- **Documentación:** Swagger / OpenAPI
- **Cloud:** Microsoft Azure App Service
- **CI/CD:** GitHub Actions

---

## 📁 Estructura del proyecto

```
InventarioMedicoAPI/
├── Controllers/
│   ├── ProductosController.cs
│   └── PedidosController.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── Producto.cs
│   ├── Pedido.cs
│   ├── ApiResponse.cs
│   ├── PaginadoResponse.cs
│   └── DashboardResponse.cs
├── Services/
│   ├── ProductoService.cs
│   └── PedidoService.cs
└── Program.cs
```

---

## 👤 Autor

**Angel Vargas Apolaya**
Programador de Sistemas | .NET Developer
📧 angelsk814@gmail.com
📱 992122293
🔗 [Portfolio](https://angeldevportfolio.netlify.app)
🔗 [LinkedIn](https://linkedin.com/in/angel-vargas-apolaya-615074262/)
