using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using NutriApp.DTO;

namespace NutriApp;

public partial class HistorialPage : ContentPage
{
    //Variables para almacenar el ID del usuario
    private int _idUsuario;
    private static readonly HttpClient client = new HttpClient();

    // Colecci�n observable para el ListView
    public ObservableCollection<Producto> ProductosHistorial { get; set; }

    // Indicador de carga
    public bool IsLoading { get; set; }

    public HistorialPage(int usuarioID)
    {
        InitializeComponent();

        _idUsuario = usuarioID;

        // Inicializar la colecci�n de productos
        ProductosHistorial = new ObservableCollection<Producto>();

        // Establecer el BindingContext
        BindingContext = this;

        // Inicializar el indicador de carga
        IsLoading = false;

        // Cargar el historial cuando se crea la p�gina
        CargarHistorialProductos();
    }

    private bool _isHistorialVacio = true;
    public bool IsHistorialVacio
    {
        get => _isHistorialVacio;
        set
        {
            _isHistorialVacio = value;
            OnPropertyChanged(nameof(IsHistorialVacio));
        }
    }

    private async void CargarHistorialProductos()
    {
        try
        {
            // Mostrar el indicador de carga
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            // Preparar la solicitud de historial
            ReqHistorial req = new()
            {
                usuarioID = _idUsuario
            };

            var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            // Realizar la solicitud GET
            var response = await client.GetAsync($"{ApiConfig.UrlBase}api/usuario/historialEscaneos?usuarioID={_idUsuario}");

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();
                var resHistorial = JsonConvert.DeserializeObject<ResHistorial>(responseContent);

                // Verificar si hay productos en el historial
                if (resHistorial?.codigoBarras != null && resHistorial.codigoBarras.Any())
                {
                    // Limpiar la colecci�n actual
                    ProductosHistorial.Clear();

                    // Agregar los productos al historial con manejo de im�genes
                    foreach (var producto in resHistorial.codigoBarras)
                    {
                        // Manejar la imagen del producto
                        if (!string.IsNullOrWhiteSpace(producto.Imagen))
                        {
                            try
                            {
                                // Intentar asignar la imagen desde la URL
                                producto.ImagenProducto = new Uri(producto.Imagen);
                            }
                            catch
                            {
                                // Si falla, usar imagen por defecto
                                producto.ImagenProducto = ImageSource.FromFile("default_product_image.png");
                            }
                        }
                        else
                        {
                            // Si no hay imagen, mostrar imagen por defecto
                            producto.ImagenProducto = ImageSource.FromFile("default_product_image.png");
                        }
                        ProductosHistorial.Add(producto);
                    }

                    // Actualizar la visibilidad del mensaje de historial vac�o
                    IsHistorialVacio = false;
                }
                else
                {
                    // No hay productos en el historial
                    txtHistorial.Text = "No has escaneado ning�n producto a�n";
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error de red o de deserializaci�n
            await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "Aceptar"); //Se disparo este mensaje
        }
        finally
        {
            // Ocultar el indicador de carga
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    // M�todo para manejar la selecci�n de un producto del historial
    private async void OnProductoHistorialSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Producto productoSeleccionado)
        {
            // Navegar a la p�gina de detalles del producto o mostrar m�s informaci�n
            await Navigation.PushAsync(new DetalleProductoPage(productoSeleccionado, _idUsuario));

            // Desseleccionar el elemento
            ((ListView)sender).SelectedItem = null;
        }
    }

    // M�todo para limpiar el historial
    private async void OnLimpiarHistorialClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Limpiar Historial",
            "�Est�s seguro de que quieres borrar tu historial de productos?",
            "S�", "No");

        if (confirmar)
        {
            try
            {
                // Implementar la l�gica para limpiar el historial en el servidor
                string url = $"{ApiConfig.UrlBase}/limpiar-historial";
                var reqLimpiar = new ReqHistorial { usuarioID = _idUsuario };
                var jsonRequest = JsonConvert.SerializeObject(reqLimpiar);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Limpiar la colecci�n local
                    ProductosHistorial.Clear();
                    IsVisible = false;
                    await DisplayAlert("�xito", "Historial limpiado correctamente", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo limpiar el historial", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "Aceptar");
            }
        }
    }
}