using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NutriApp.DTO;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using NutriApp.DTO.Response;

namespace NutriApp;

public partial class ProductoEscaneado : ContentPage
{
    private readonly string _codigoBarras;
    private readonly int _idUsuario;
    private readonly int _nutriScore;
    private int _productoID;
    private int _calificacion = 1; // Valor inicial del slider
    private ObservableCollection<Feedback> _feedbacks = new();

    public ProductoEscaneado(string codigoBarras, int idUsuario)
    {
        InitializeComponent();
        _codigoBarras = codigoBarras;
        _idUsuario = idUsuario;
        lblCodigoEnviado.Text = $"C�digo de barras: {_codigoBarras}";
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        StatusLabel.IsVisible = true;
        StatusLabel.Text = "Consultando producto, por favor espera...";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                ReqEscanearProducto req = new ReqEscanearProducto
                {
                    Codigo_Barras = _codigoBarras,
                    UsuarioID = _idUsuario
                };

                var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                // Llamada a la API
                var response = await client.PostAsync($"{ApiConfig.UrlBase}api/escanear/producto", contenidoJSON);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResEscanearProducto>(responseContent);

                    // Verificar si hay productos y si es exitoso
                    if (res.exito && res.CodigoBarras != null && res.CodigoBarras.Any())
                    {
                        Producto producto = res.CodigoBarras.First();
                        _productoID = producto.productoID;
                        // Manejar la imagen del producto
                        if (!string.IsNullOrWhiteSpace(producto.Imagen))
                        {
                            try
                            {
                                // Intentar cargar la imagen desde la URL
                                imgProducto.Source = new Uri(producto.Imagen);
                            }
                            catch
                            {
                                // Si falla, usar imagen por defecto
                                imgProducto.Source = "default_product_image.png";
                            }
                        }
                        else
                        {
                            // Si no hay imagen, mostrar imagen por defecto
                            imgProducto.Source = "default_product_image.png";
                        }
                        // Mostrar los datos b�sicos del producto
                        lblNombreProducto.Text = producto.Nombre;
                        lblMarcaProducto.Text = producto.Marca ?? "Marca no especificada";
                        lblCategoria.Text = producto.Categoria ?? "Categor�a no especificada";

                        // Ingredientes
                        lblIngredientes.Text = string.IsNullOrWhiteSpace(producto.Ingredientes)
                            ? "Ingredientes no disponibles"
                            : producto.Ingredientes;

                        // Alergenos
                        lblAlergenos.Text = producto.Alergenos?.Any() == true
                            ? string.Join(", ", producto.Alergenos)
                            : "No se detectaron alergenos";

                        string imageName = $"nutriscore_{producto.NutriScore}.png";

                        try
                        {
                            imgNutriScore.Source = ImageSource.FromFile(imageName);
                        }
                        catch
                        {
                            // Fallback to a default image or logging if needed
                            imgNutriScore.Source = ImageSource.FromFile("nutriscore_default.png");
                        }

                        // Informaci�n Nutricional
                        if (!string.IsNullOrWhiteSpace(producto.InformacionNutricional))
                        {
                            try
                            {
                                lblInformacionNutricional.Text = FormatearInformacionNutricional(producto.InformacionNutricional);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error al procesar informaci�n nutricional: {ex.Message}");
                                lblInformacionNutricional.Text = "Informaci�n nutricional no disponible";
                            }
                        }
                        else
                        {
                            lblInformacionNutricional.Text = "Informaci�n nutricional no disponible";
                        }

                        // C�digo de Barras
                        lblCodigoEnviado.Text = $"C�digo de barras: {_codigoBarras}";

                        // Cargar los comentarios
                        await CargarComentarios(producto.productoID);

                        // Ocultar el indicador de carga
                        LoadingIndicator.IsVisible = false;
                        LoadingIndicator.IsRunning = false;
                        StatusLabel.IsVisible = false;
                    }
                    else
                    {
                        // Manejar caso cuando no hay productos o no es exitoso
                        StatusLabel.Text = "No se encontraron productos o la b�squeda no fue exitosa.";

                        // Si hay mensajes, mostrarlos
                        if (res.mensaje != null && res.mensaje.Any())
                        {
                            StatusLabel.Text += $" Detalles: {string.Join(", ", res.mensaje)}";
                        }
                    }
                }
                else
                {
                    throw new Exception($"API Error: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error al consultar el producto: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Excepci�n completa: {ex}");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task CargarComentarios(int productoId)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Llamada a la API para obtener los comentarios del producto
                var response = await client.GetAsync($"{ApiConfig.UrlBase}api/feedback/obtener?pruductoID={productoId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResObtenerFeeback>(responseContent);

                    if (res != null && res.feedback.Any())
                    {
                        // Mostrar los comentarios
                        StackComentarios.Children.Clear(); // Limpiar comentarios previos
                        foreach (var comentario in res.feedback)
                        {
                            // Crear la visualizaci�n del comentario
                            var comentarioFrame = new Frame
                            {
                                BorderColor = Color.FromArgb("#e4e4e4"),
                                BackgroundColor = Color.FromArgb("#ffffff"),
                                CornerRadius = 10,
                                Padding = 10,
                                Margin = new Thickness(0, 10),
                                Content = new StackLayout
                                {
                                    Children =
                                {
                                    // Nombre del usuario
                                    new Label
                                    {
                                        Text = comentario.nombreUsuario,
                                        FontAttributes = FontAttributes.Bold,
                                        HorizontalOptions = LayoutOptions.Start,
                                        FontSize = 16
                                    },

                                    // Calificaci�n con estrellas
                                    CreateStars(comentario.calificacion),

                                    // Comentario
                                    new Label
                                    {
                                        Text = comentario.comentario,
                                        FontSize = 14,
                                        TextColor = Color.FromArgb("#424242"),
                                        HorizontalOptions = LayoutOptions.StartAndExpand
                                    }
                                }
                                }
                            };

                            // Agregar el Frame con el comentario al StackLayout
                            StackComentarios.Children.Add(comentarioFrame);
                        }

                        // Ocultar el mensaje de "s� el primero en dar un feedback"
                        lblNoComentarios.IsVisible = false;
                    }
                    else
                    {
                        // Si no hay comentarios, mostrar el mensaje
                        lblNoComentarios.IsVisible = true;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron obtener los comentarios", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar los comentarios: {ex.Message}", "Aceptar");
        }
    }

    // M�todo para crear las estrellas basadas en la calificaci�n
    private StackLayout CreateStars(int rating)
    {
        var starLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 5
        };

        // Invertir la l�gica para las estrellas
        int invertedRating = 6 - rating;  // Si rating es 1, invertedRating ser� 5, si es 5, invertedRating ser� 1

        for (int i = 1; i <= 5; i++)
        {
            var starImage = new Image
            {
                Source = i <= invertedRating ? "star_filled.png" : "star_empty.png",
                WidthRequest = 20,
                HeightRequest = 20
            };
            starLayout.Children.Add(starImage);
        }

        return starLayout;
    }

    private string FormatearInformacionNutricional(string informacionNutricional)
    {
        try
        {
            // Parsear el JSON con Newtonsoft.Json
            var nutricionDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(informacionNutricional);

            if (nutricionDict == null)
                return "Informaci�n nutricional no disponible";

            var infoBuilder = new StringBuilder();

            // Funci�n auxiliar para obtener valor de manera segura
            object GetValueSafely(string key)
            {
                return nutricionDict.TryGetValue(key, out var value) ? value : null;
            }

            // Energ�a
            var energia = GetValueSafely("energy-kcal_value");
            if (energia != null)
                infoBuilder.AppendLine($"Energ�a: {energia} kcal");

            // Carbohidratos
            var carbohidratos = GetValueSafely("carbohydrates_value");
            if (carbohidratos != null)
                infoBuilder.AppendLine($"Carbohidratos: {carbohidratos} g");

            // Prote�nas
            var proteinas = GetValueSafely("proteins_value");
            if (proteinas != null)
                infoBuilder.AppendLine($"Prote�nas: {proteinas} g");

            // Grasas
            var grasas = GetValueSafely("fat_value");
            if (grasas != null)
                infoBuilder.AppendLine($"Grasas: {grasas} g");

            // Az�cares
            var azucares = GetValueSafely("sugars_value");
            if (azucares != null)
                infoBuilder.AppendLine($"Az�cares: {azucares} g");

            // Sal
            var sal = GetValueSafely("salt_value");
            if (sal != null)
                infoBuilder.AppendLine($"Sal: {sal} g");

            // Fibra
            var fibra = GetValueSafely("fiber_value");
            if (fibra != null)
                infoBuilder.AppendLine($"Fibra: {fibra} g");

            // Grasas saturadas
            var grasasSaturadas = GetValueSafely("saturated-fat_value");
            if (grasasSaturadas != null)
                infoBuilder.AppendLine($"Grasas saturadas: {grasasSaturadas} g");

            // Grupo NOVA (procesamiento)
            var grupoNova = GetValueSafely("nova-group");
            if (grupoNova != null)
                infoBuilder.AppendLine($"Grupo NOVA: {grupoNova}");

            return infoBuilder.ToString();
        }
        catch (Exception ex)
        {
            // Opcional: loguear el error real
            return $"No se pudo procesar la informaci�n nutricional: {ex.Message}";
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        var selectedStar = (ImageButton)sender;
        int selectedRating = 0;
        int selectedRt = 0;

        // Determinar qu� estrella fue seleccionada
        if (selectedStar == star1) { selectedRating = 1; selectedRt = 5; } 
        else if (selectedStar == star2) { selectedRating = 2; selectedRt = 4; }
        else if (selectedStar == star3) { selectedRating = 3; selectedRt = 3; }
        else if (selectedStar == star4) { selectedRating = 4; selectedRt = 2; }
        else if (selectedStar == star5) { selectedRating = 5; selectedRt = 1; }

        // Actualizar las im�genes de las estrellas
        UpdateStarImages(selectedRating);

        // Guardar la valoraci�n en las preferencias
        Preferences.Set("UserRating", selectedRt);
    }

    private void UpdateStarImages(int rating)
    {
        // Cambiar las im�genes de las estrellas en funci�n de la calificaci�n
        star1.Source = rating >= 1 ? "star_filled.png" : "star_empty.png";
        star2.Source = rating >= 2 ? "star_filled.png" : "star_empty.png";
        star3.Source = rating >= 3 ? "star_filled.png" : "star_empty.png";
        star4.Source = rating >= 4 ? "star_filled.png" : "star_empty.png";
        star5.Source = rating >= 5 ? "star_filled.png" : "star_empty.png";
    }


    private async void OnEnviarFeedbackClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFeedback.Text))
        {
            await DisplayAlert("Error", "Por favor, escribe un comentario antes de enviar.", "Aceptar");
            return;
        }

        try
        {
            var feedback = new
            {
                usuarioID = _idUsuario,
                comentario = txtFeedback.Text,
                calificacion = _calificacion,
                productoID = _productoID
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync($"{ApiConfig.UrlBase}api/feedback/insertar", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("�xito", "Feedback enviado correctamente.", "Aceptar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo enviar el feedback.", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "Aceptar");
        }
    }
}
