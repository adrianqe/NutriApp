using NutriApp.DTO;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Maui.Storage;
using System.IO;

namespace NutriApp;

public partial class ActualizarPerfilPage : ContentPage
{
    private readonly int _usuarioId;
    private readonly string _token;
    private string _imagenPerfilPath;

    public ActualizarPerfilPage(int usuarioId, string token)
    {
        InitializeComponent();
        _usuarioId = usuarioId;
        _token = token;

        BindingContext = this;

        CargarImagenPerfilGuardada();
        CargarDatosUsuario();
    }
    private void CargarImagenPerfilGuardada()
    {
        // Ruta para almacenar la imagen de perfil localmente
        string perfilImagePath = Path.Combine(FileSystem.AppDataDirectory, $"perfil_{_usuarioId}.jpg");

        if (File.Exists(perfilImagePath))
        {
            imgPerfil.Source = ImageSource.FromFile(perfilImagePath);
            _imagenPerfilPath = perfilImagePath;
        }
    }
    private void CargarDatosUsuario()
    {
        try
        {
            // Decodificar el token para obtener información del usuario
            JwtPayload payload = JwtHelper.DecodeToken(_token);

            // Llenar los campos con la información actual
            txtNombre.Text = payload.unique_name ?? "";
            txtCorreo.Text = payload.email ?? "";
        }
        catch (Exception ex)
        {
            // Manejar cualquier error al cargar los datos
            DisplayAlert("Error", $"No se pudieron cargar los datos del usuario. Detalle: {ex.Message}", "OK");
        }
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            await DisplayAlert("Error", "El nombre no puede estar vacío.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtCorreo.Text))
        {
            await DisplayAlert("Error", "El correo electrónico no puede estar vacío.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            await DisplayAlert("Error", "La contraseña no puede estar vacía.", "OK");
            return;
        }

        if (txtContrasena.Text != txtConfirmarContrasena.Text)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
            return;
        }

        var reqActualizarUsuario = new ReqActualizarUsuario
        {
            Usuario_ID = _usuarioId,
            Nombre = txtNombre.Text.Trim(),
            Email = txtCorreo.Text.Trim(),
            Password = txtContrasena.Text.Trim()
        };

        try
        {
            // URL del backend
            string url = $"{ApiConfig.UrlBase}api/usuario/actualizar";

            // Serializar el objeto de solicitud
            string jsonContent = JsonConvert.SerializeObject(reqActualizarUsuario);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                // Configurar encabezados de autorización
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                // Enviar la solicitud HTTP PUT
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Procesar la respuesta
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var resActualizarUsuario = JsonConvert.DeserializeObject<ResActualizarUsuario>(responseBody);

                    if (resActualizarUsuario != null && resActualizarUsuario.exito)
                    {
                        await DisplayAlert("Éxito", "Perfil actualizado correctamente.", "OK");
                        await Navigation.PopAsync(); // Regresar a la página anterior
                    }
                    else
                    {
                        string mensaje = resActualizarUsuario?.mensaje?.FirstOrDefault() ?? "Ocurrió un error desconocido.";
                        await DisplayAlert("Error", mensaje, "OK");
                    }
                }
                else
                {
                    // Mostrar detalles del error del servidor
                    await DisplayAlert("Error", $"No se pudo actualizar el perfil. Código de estado: {response.StatusCode}. Detalle: {responseBody}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores generales
            await DisplayAlert("Error", $"Ocurrió un error inesperado: {ex.Message}", "OK");
        }
    }

    private async void OnCambiarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            // Verificar si la selección de medios está soportada
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Error", "Selección de fotos no es compatible con este dispositivo.", "OK");
                return;
            }

            // Seleccionar foto de la galería
            var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Selecciona tu foto de perfil"
            });

            if (result != null)
            {
                // Ruta para guardar la imagen
                string perfilImagePath = Path.Combine(FileSystem.AppDataDirectory, $"perfil_{_usuarioId}.jpg");

                // Copiar la imagen seleccionada a la carpeta de datos de la aplicación
                using (var stream = await result.OpenReadAsync())
                using (var newStream = File.OpenWrite(perfilImagePath))
                {
                    await stream.CopyToAsync(newStream);
                }

                // Actualizar la imagen en la interfaz
                imgPerfil.Source = ImageSource.FromFile(perfilImagePath);
                _imagenPerfilPath = perfilImagePath;

                // Opcional: Mostrar un mensaje de éxito
                await DisplayAlert("Éxito", "Foto de perfil actualizada correctamente.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al seleccionar la foto: {ex.Message}", "OK");
        }
    }

    private void EliminarImagenPerfil()
    {
        if (!string.IsNullOrEmpty(_imagenPerfilPath) && File.Exists(_imagenPerfilPath))
        {
            File.Delete(_imagenPerfilPath);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}