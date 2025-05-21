using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Text;
using NutriApp.DTO;
using Microsoft.Maui.Controls;
using ZXing.Net.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ZXing;
using ZXing.Net.Maui;

namespace NutriApp;


public partial class HomePage : ContentPage
{
    //Variables para almacenar el ID del usuario y el token
    private readonly int _idUsuario;
    private readonly string _token;

    private static readonly HttpClient client = new HttpClient();
    public HomePage(int IdUsuario, string token)
    {
        InitializeComponent();
        _idUsuario = IdUsuario;
        barcodeReader.Options = new BarcodeReaderOptions()
        {
            AutoRotate = true,
            Multiple = false,
            Formats = BarcodeFormats.All

        };
        _token = token;
        MostrarNombreUsuario();
        CargarImagenPerfil();
    }
    private void CargarImagenPerfil()
    {
        // Ruta para almacenar la imagen de perfil localmente
        string perfilImagePath = Path.Combine(FileSystem.AppDataDirectory, $"perfil_{_idUsuario}.jpg");

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

    // Evento para buscar el producto
    public async void OnBuscarProductoClicked(object sender, EventArgs e)
    {
        string nombreProducto = txtProducto.Text; // el trim elimina espacios en blanco al principio y al final
        if (string.IsNullOrEmpty(nombreProducto))
        {
            await DisplayAlert("Error", "Por favor, ingresa el nombre de un producto.", "OK");
            return;
        }

        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Crear la solicitud
                ReqBuscarProducto req = new ReqBuscarProducto
                {
                    NombreProducto = nombreProducto
                };

                var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                // Llamada a la API
                var response = await client.PostAsync($"{ApiConfig.UrlBase}api/productos/buscarProducto", contenidoJSON);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResBuscarProducto>(responseBody);

                    if (res != null && res.ProductosEncontrados.Count > 0)
                    {
                        // Mostrar los resultados
                        listProductos.ItemsSource = res.ProductosEncontrados;
                        lblResultado.IsVisible = true;
                        listProductos.IsVisible = true;
                        lblResultado.Text = $"Se encontraron {res.ProductosEncontrados.Count} producto(s):";
                    }
                    else
                    {
                        // No hay resultados
                        lblResultado.IsVisible = true;
                        lblResultado.Text = "No se encontraron productos.";
                        listProductos.IsVisible = false;
                    }
                }
                else
                {
                    // Error en la respuesta
                    await DisplayAlert("Error", "No se pudo realizar la búsqueda. Inténtalo más tarde.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar errores generales
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void OnProductoSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Producto productoSeleccionado)
        {
            // Desseleccionar el elemento
            ((ListView)sender).SelectedItem = null;

            // Navegar a la página de detalles del producto
            await Navigation.PushAsync(new DetalleProductoPage(productoSeleccionado, _idUsuario));
        }
    }

    private void OnHomeClicked(object sender, EventArgs e)
    {
        // Lógica del evento
        DisplayAlert("Home", "Home button clicked!", "OK");
    }

    // Evento para la barra de navegación - Historial
    private async void OnHistorialClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistorialPage(_idUsuario));

    }

    // Evento para la barra de navegación - Perfil
    private async void OnPerfilClicked(object sender, EventArgs e)
    {
        // Parte de carlos :)
        await Navigation.PushAsync(new Perfil(_idUsuario, _token));
    }

    private bool _isProcessingBarcode = false;
    private string _lastScannedCode = null;
    private DateTime _lastScanTime = DateTime.MinValue;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reinicia el escáner de manera más explícita
        if (barcodeReader != null)
        {
            barcodeReader.IsDetecting = true;
            barcodeReader.Options = new BarcodeReaderOptions()
            {
                AutoRotate = true,
                Multiple = false,
                Formats = BarcodeFormats.All
            };
        }

        // Resetear estado de escaneo
        lblBarcodeResult.Text = "Escanea un código de barras";
        _lastScannedCode = null;
        _isProcessingBarcode = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (barcodeReader != null)
        {
            barcodeReader.IsDetecting = false;
        }
    }

    public async void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // Early exit if already processing or no results
        if (_isProcessingBarcode || e?.Results?.FirstOrDefault() == null)
            return;

        string? detectedCode = e.Results.FirstOrDefault()?.Value;

        // Prevent rapid repeated scans of same code
        if (string.IsNullOrEmpty(detectedCode) ||
            (detectedCode == _lastScannedCode && (DateTime.Now - _lastScanTime).TotalSeconds < 3))
            return;

        try
        {
            _isProcessingBarcode = true;
            _lastScannedCode = detectedCode;
            _lastScanTime = DateTime.Now;

            // Stop detection briefly
            barcodeReader.IsDetecting = false;

            // Update UI
            Dispatcher.Dispatch(() =>
            {
                lblBarcodeResult.Text = $"Código de barras: {detectedCode}";
            });

            // Navigate to product page
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PushAsync(new ProductoEscaneado(detectedCode, _idUsuario));
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            });
        }
        finally
        {
            // Reactivate detection after a short delay
            await Task.Delay(3000);
            barcodeReader.IsDetecting = true;
            _isProcessingBarcode = false;
        }
    }

    private void ReiniciarEscaner()
    {
        if (barcodeReader != null)
        {
            barcodeReader.IsDetecting = true;
            barcodeReader.Options = new BarcodeReaderOptions()
            {
                AutoRotate = true,
                Multiple = false,
                Formats = BarcodeFormats.All
            };
        }
    }
}