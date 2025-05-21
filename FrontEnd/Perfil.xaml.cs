using Newtonsoft.Json;
using NutriApp.DTO;
using System.Net.Http.Headers;
using System.Text;
using System.IO;

namespace NutriApp;

public partial class Perfil : ContentPage
{
    private int _usuarioId;
    private readonly string _token;

    public Perfil(int usuarioId, string token)
    {
        InitializeComponent();

        _usuarioId = usuarioId;

        _token = token;
        MostrarNombreUsuario();
        CargarImagenPerfil();
    }

    private void CargarImagenPerfil()
    {
        // Ruta para almacenar la imagen de perfil localmente
        string perfilImagePath = Path.Combine(FileSystem.AppDataDirectory, $"perfil_{_usuarioId}.jpg");

        if (File.Exists(perfilImagePath))
        {
            imgPerfil.Source = ImageSource.FromFile(perfilImagePath);
        }
        else
        {
            // Mantener la imagen por defecto si no hay imagen guardada
            imgPerfil.Source = "perfil.png";
        }
    }

    private void MostrarNombreUsuario()
    {
        try
        {
            // Decodificar el token para obtener el payload
            JwtPayload payload = JwtHelper.DecodeToken(_token);

            // Asignar el nombre al Label
            lblNombreUsuario.Text = payload.unique_name ?? "Usuario";
        }
        catch (Exception ex)
        {
            lblNombreUsuario.Text = "Error al obtener el nombre";
            System.Diagnostics.Debug.WriteLine($"Error al decodificar el token: {ex.Message}");
        }
    }

    private async void OnAlergiaButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlergiaPage(_usuarioId, _token));
    }

    private async void OnEditarPerfilButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ActualizarPerfilPage(_usuarioId, _token));
    }
    private async void OnEliminarPerfilButtonClicked(object sender, EventArgs e)
    {
        bool confirmacion = await DisplayAlert(
            "Eliminar Perfil",
            "¿Está seguro de que desea eliminar su perfil? Esta acción no se puede deshacer.",
            "Eliminar",
            "Cancelar"
        );

        if (confirmacion)
        {
            try
            {
                if (_usuarioId <= 0)
                {
                    await DisplayAlert("Error", "ID de usuario inválido. No se puede eliminar el perfil.", "Aceptar");
                    return;
                }

                var eliminarUsuarioRequest = new ReqEliminarUsuario
                {
                    Usuario_ID = _usuarioId
                };

                string jsonRequest = JsonConvert.SerializeObject(eliminarUsuarioRequest);

                string url = $"{ApiConfig.UrlBase}api/usuario/eliminar";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                    var request = new HttpRequestMessage(HttpMethod.Delete, url)
                    {
                        Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                    };

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<ResEliminarUsuario>(responseContent);
                        if (res != null && res.exito)
                        {
                            await DisplayAlert("Éxito", "Perfil eliminado correctamente.", "Aceptar");

                            SessionManager.ClearSession();
                            Application.Current.MainPage = new MainPage();
                        }
                        else
                        {
                            string mensajeError = res?.mensaje?.FirstOrDefault()
                                ?? "Error desconocido al intentar eliminar el perfil.";
                            await DisplayAlert("Error", mensajeError, "Aceptar");
                        }
                    }
                    else
                    {
                        string errorDetalle = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error de conexión", $"No fue posible conectar con el servidor. Detalle: {errorDetalle}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error inesperado: {ex.Message}", "Aceptar");
            }
        }
    }
}