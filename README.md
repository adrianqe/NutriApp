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
## Capturas
![Bienvenido a NutriApp](https://github.com/user-attachments/assets/9651066a-c989-4082-a0b6-fb696c967b89)
![Inicio de Sesion NutriApp](https://github.com/user-attachments/assets/0135ce17-7ac8-4e99-bd80-53dd66ac7113)
![Escanear Producto](https://github.com/user-attachments/assets/8054ee40-d375-4dcf-a084-d59e3910c0b2)
![PringlesQueso Producto Escaneado](https://github.com/user-attachments/assets/896cfc26-fac2-49d5-9485-762b75753764)
![Feedback de Usuarios](https://github.com/user-attachments/assets/15ba2901-2897-4a4a-8243-c5f639778442)
![Alergenos](https://github.com/user-attachments/assets/094aae86-4e96-4add-bbf8-333f769c0655)


### Requisitos

- **Visual Studio** con soporte para MAUI (desarrollo móvil).
- **SQL Server** para la base de datos.
- **.NET 7.0** o superior.
## Configuración
Clona el repositorio
`git clone https://github.com/adrianqe/NutriApp.git`
