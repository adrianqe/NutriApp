using Newtonsoft.Json;
using System.Text;
using NutriApp.DTO;

namespace NutriApp;

public partial class VerificacionPage : ContentPage
{
    // Campo para almacenar el email
    private readonly string _email;
    private readonly int _codigoVerificacion;
    private readonly int _idUsuario;
    private readonly string _password;
    private readonly string _token;
    public VerificacionPage(string email, int pinVerificacion,string token,int idUsuario)
    {
        InitializeComponent();
        _email = email;
        _codigoVerificacion = pinVerificacion;
        _token = token;
        _idUsuario = idUsuario;

        lblPin.Text = $"Hemos enviado un c�digo de verificaci�n a \n {email}";
    }

    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        if (this.validarDatosFaltantes())
        {
            if (int.TryParse(txtPin.Text, out int pinIngresado) && _codigoVerificacion == pinIngresado)
            {
                ReqVerificar req = new ReqVerificar
                {
                    Email = _email
                };

                // Serializamos el objeto completo, no solo el string del email
                var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync($"{ApiConfig.UrlBase}api/usuario/verificar", contenidoJSON);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<ResVerificar>(responseContent);

                        if (res.exito)
                        {
                            await Navigation.PushAsync(new AlergiaPage(_idUsuario, _token));
                        }
                        else
                        {
                            await DisplayAlert("Error", "C�digo de verificaci�n incorrecto", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error de conexi�n", "No fue posible conectar con el servidor", "Cancelar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Intenta de nuevo", "El codigo de verificaci�n es incorrecto", "Aceptar");
            }
        }
        else
        {
            await DisplayAlert("Faltan datos", "Por favor ingrese el c�digo de verificaci�n", "Aceptar");
        }
    }

    private bool validarDatosFaltantes()
    {
        if (String.IsNullOrEmpty(txtPin.Text))
        {
            return false;
        }
        return true;
    }
}