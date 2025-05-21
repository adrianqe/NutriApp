using System.Text;
using NutriApp.DTO;
using Newtonsoft.Json;

namespace NutriApp;

public partial class InicioSesionPage : ContentPage
{
    public InicioSesionPage()
    {
        InitializeComponent();
    }

    private void btnRegistro_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegistroPage());
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        if (this.validarDatos())
        {
            // Preparar la solicitud
            ReqIniciarSesion req = new ReqIniciarSesion
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };

            var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync($"{ApiConfig.UrlBase}api/usuario/iniciarSesion", contenidoJSON);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResIniciarSesion>(responseContent);

                    if (res.exito)
                    {
                        // Decodificar el token para obtener el payload
                        JwtPayload payload = JwtHelper.DecodeToken(res.token);

                        // Obtener el NameId del token
                        int nameId = int.Parse(payload.nameid);

                        // Validar que NameId se haya obtenido correctamente
                        System.Diagnostics.Debug.WriteLine($"NameId del token: {nameId}");

                        // Guardar la sesión
                        SessionManager.SaveSession(res.token, nameId);

                        // Cambiar a la página principal, pasando nameId y el token
                        await Application.Current.MainPage.Navigation.PushAsync(new HomePage(nameId, res.token));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Email o contraseña incorrectos", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error de conexión", "No fue posible conectar con el servidor", "Cancelar");
                }
            }
        }
        else
        {
            await DisplayAlert("Faltan datos", "Por favor ingrese su email y contraseña", "Aceptar");
        }
    }


    private bool validarDatos()
    {
        if (String.IsNullOrEmpty(txtEmail.Text) || String.IsNullOrEmpty(txtPassword.Text))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}