# NutriData

Es una aplicación diseñada para facilitar la gestión y seguimiento de productos alimenticios mediante la lectura de códigos de barras, brindando información nutricional detallada y ofreciendo feedback sobre los productos escaneados. Este proyecto está desarrollado como parte de un curso de desarrollo móvil usando MAUI, C#, y SQL Server.

## Descripción del Proyecto
NutriData permite a los usuarios escanear productos utilizando la cámara de un dispositivo móvil para obtener información detallada sobre ellos, como el contenido nutricional y otras características. El proyecto también incluye funcionalidades para gestionar perfiles de usuarios (adultos mayores, cuidadores y familiares), historial de escaneos, registros médicos y feedback sobre los productos.

### Características
- Escaneo de códigos de barras de productos.
- Consulta de información nutricional de los productos utilizando la API de OpenFoodFacts.
- Gestión de usuarios.
- Registro de historial de escaneos y feedback de productos.
- Sistema de manejo de errores en la base de datos para garantizar la fiabilidad del sistema.

## Estructura del Proyecto
El proyecto se divide en varias capas para mejorar la organización y la escalabilidad:

- **Entidades:** Contiene las clases que representan los objetos de dominio (Usuario, Producto, Feedback, etc.).
- **Acceso a Datos:** Gestión de las operaciones con la base de datos a través de procedimientos almacenados (SP).
- **Lógica de Negocio:** Contiene la lógica para procesar las solicitudes y las reglas del negocio.
- **API y Servicios Externos:** Comunicación con servicios externos como OpenFoodFacts para obtener información adicional de los productos.
- **Interfaz de Usuario (Frontend):** A desarrollar usando MAUI para iOS y Android.

## Instalación
### Requisitos
- **Visual Studio** con soporte para MAUI (desarrollo móvil).
- **SQL Server** para la base de datos.
- **API de OpenFoodFacts** para la obtención de datos nutricionales.
- **.NET 7.0** o superior.
## Configuración
Clona el repositorio
`git clone https://github.com/adrianqe/NutriApp.git`
## Licencia
Este proyecto está bajo la Licencia MIT. Puedes ver más detalles en el archivo LICENSE.
