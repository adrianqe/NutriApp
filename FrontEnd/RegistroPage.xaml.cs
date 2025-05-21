using Newtonsoft.Json;
using System.Text;
using NutriApp.DTO;

namespace NutriApp;

public partial class RegistroPage : ContentPage
{
    int count = 0;

    public RegistroPage()
    {
        InitializeComponent();
    }

    private async void btnRegistrarse_Clicked(object sender, EventArgs e)
    {
        if (this.validarDatos())
        {
            if (this.validarContrasena())
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        ReqRegistrar req = new ReqRegistrar
                        {
                            Nombre = txtNombre.Text,
                            Email = txtEmail.Text,
                            Password = txtPassword.Text
                        };

                        var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");


                        var response = await client.PostAsync($"{ApiConfig.UrlBase}api/usuario/registrar", contenidoJSON);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();

                            if (string.IsNullOrEmpty(responseContent))
                            {
                                await DisplayAlert("Error", "La respuesta del servidor está vacía", "Aceptar");
                                return;
                            }

                            var res = JsonConvert.DeserializeObject<ResRegistrar>(responseContent);
                            if (res == null || string.IsNullOrEmpty(res.Token))
                            {
                                await DisplayAlert("Error", "El servidor no devolvió un token válido", "Aceptar");
                                return;
                            }

                            // Lógica del token
                            JwtPayload payload = JwtHelper.DecodeToken(res.Token);

                            if (string.IsNullOrEmpty(payload.CodigoVerificacion) || string.IsNullOrEmpty(payload.nameid))
                            {
                                await DisplayAlert("Error", "El token no contiene los datos esperados", "Aceptar");
                                return;
                            }

                            int cogigoVerificacion = int.Parse(payload.CodigoVerificacion);
                            int idUsuario = int.Parse(payload.nameid);

                            if (res.Exito)
                            {
                                await Navigation.PushAsync(new VerificacionPage(txtEmail.Text, cogigoVerificacion, res.Token, idUsuario));
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se pudo registrar", "Aceptar");
                            }
                        }

                        else
                        {
                            await DisplayAlert("Error de conexión", "No fue posible conectar con el servidor", "Cancelar");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden", "Aceptar");
            }
        }
        else
        {
            await DisplayAlert("Faltan datos", "Por favor llene todos los campos", "Aceptar");
        }
    }

    private bool validarDatos()
    {
        return !(string.IsNullOrEmpty(txtNombre.Text) ||
                 string.IsNullOrEmpty(txtEmail.Text) ||
                 string.IsNullOrEmpty(txtPassword.Text) ||
                 string.IsNullOrEmpty(txtVerifiedPassword.Text));
    }

    private bool validarContrasena()
    {
        return txtPassword.Text == txtVerifiedPassword.Text;
    }
}