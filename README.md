![NutriAppBanner](https://github.com/user-attachments/assets/e90d87e3-4a95-42e9-b8d6-c78e6e3e1a78)

Es una aplicación diseñada para facilitar la gestión y seguimiento de productos alimenticios mediante la lectura de códigos de barras, brindando información nutricional detallada y ofreciendo feedback sobre los productos escaneados. Este proyecto está desarrollado como parte de un curso de desarrollo móvil usando MAUI, C#, y SQL Server.

## Descripción del Proyecto
NutriApp permite a los usuarios escanear productos utilizando la cámara de un dispositivo móvil para obtener información detallada sobre ellos, como el contenido nutricional y otras características. El proyecto también incluye funcionalidades para gestionar perfiles de usuarios, historial de escaneos y feedback sobre los productos.

### Características
- Escaneo con ZXing.Net.Maui para cualquier formato de código de barras.
- Consulta de información nutricional de los productos utilizando la API de OpenFoodFacts.
- Gestión de usuarios.
- Registro de historial de escaneos y feedback de productos.
- Sistema de manejo de errores en la base de datos para garantizar la fiabilidad del sistema.

## Estructura del Proyecto
El proyecto se divide en varias capas para mejorar la organización y la escalabilidad:

- **Entidades:** Contiene las clases que representan los objetos de dominio (Usuario, Producto, Feedback, etc.).
- **Acceso a Datos:** Gestión de las operaciones con la base de datos a través de procedimientos almacenados (SP).
- **Lógica de Negocio:** Contiene la lógica para procesar las solicitudes y respuestas.
- **API y Servicios Externos:** Comunicación con servicios externos como OpenFoodFacts para obtener información adicional de los productos.
- **Interfaz de Usuario (Frontend):** A desarrollar usando MAUI para iOS y Android.

## Instalación
### Requisitos
- **Visual Studio** con soporte para MAUI (desarrollo móvil).
- **SQL Server** para la base de datos.
- **.NET 7.0** o superior.
## Configuración
Clona el repositorio
`git clone https://github.com/adrianqe/NutriApp.git`
## Licencia
Este proyecto está bajo la Licencia MIT. Puedes ver más detalles en el archivo LICENSE.
