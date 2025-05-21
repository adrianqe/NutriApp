using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json;
using NutriApp.DTO;

namespace NutriApp;

public partial class ActualizarProducto : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    public ActualizarProducto()
    {
        InitializeComponent();
    }

    // Método para regresar a la MainPage
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Evento del botón Actualizar
    public async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // Crear el objeto de solicitud
        var reqActualizarProducto = new ReqActualizarProducto
        {
            Producto_ID = int.Parse(txtProductoID.Text),
            Nombre = txtNombre.Text.Trim(),
            Categoria = txtCategoria.Text.Trim(),
            Marca = txtMarca.Text.Trim(),
            Informacion_Nutricional = txtInformacionNutricional.Text.Trim(),
            Ingredientes = txtIngredientes.Text.Trim()
        };

        try
        {
            // URL del backend (No se si la conexion va a funcionar )
            string url = $"{ApiConfig.UrlBase}api/productos/actualizarProducto";

            // Serializar el objeto de solicitud
            string jsonContent = JsonConvert.SerializeObject(reqActualizarProducto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Enviar la solicitud HTTP POST
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Procesar la respuesta
                string responseBody = await response.Content.ReadAsStringAsync();
                var resActualizarProducto = JsonConvert.DeserializeObject<ResActualizarProducto>(responseBody);

                if (resActualizarProducto != null && resActualizarProducto.Exito)
                {
                    await DisplayAlert("Éxito", "Producto actualizado correctamente.", "OK");
                }
                else
                {
                    string mensaje = resActualizarProducto?.Mensaje ?? "Ocurrió un error desconocido.";
                    await DisplayAlert("Error", mensaje, "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar el producto.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores generales
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}