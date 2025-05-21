using Newtonsoft.Json;
using System.Text;
using NutriApp.DTO;
using NutriApp.DTO.Response;

namespace NutriApp;

public partial class DetalleProductoPage : ContentPage
{
    private int _productoID;
    private readonly int _usuarioID;
    private int _calificacion = 1; // Valor inicial del slider

    public DetalleProductoPage(Producto producto, int usuarioID)
    {
        InitializeComponent();

        // Cargar los datos del producto
        CargarDatosProducto(producto);
        SetNutriScoreImage(producto.NutriScore);
    }

    private void SetNutriScoreImage(int nutriScore)
    {
        // Assuming you have images named: 
        // nutriScore_1.png, nutriScore_2.png, nutriScore_3.png, nutriScore_4.png, nutriScore_5.png
        string imageName = $"nutriscore_{nutriScore}.png";

        try
        {
            imgNutriScore.Source = ImageSource.FromFile(imageName);
        }
        catch
        {
            // Fallback to a default image or logging if needed
            imgNutriScore.Source = ImageSource.FromFile("nutriscore_default.png");
        }
    }

    private async void CargarDatosProducto(Producto producto)
    {
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
        // Datos básicos
        lblNombre.Text = producto.Nombre;
        lblMarca.Text = producto.Marca;
        _productoID = producto.productoID;

        // Ingredientes
        lblIngredientes.Text = producto.Ingredientes;

        // Código de barras
        lblCodigoBarras.Text = producto.Codigo_Barras;

        // Alergenos
        lblAlergenos.Text = producto.Alergenos?.Any() == true
            ? string.Join(", ", producto.Alergenos)
            : "No se detectaron alergenos";

        // Información nutricional
        lblInfoNutricional.Text = FormatearInformacionNutricional(producto.InformacionNutricional);

        // Cargar los comentarios
        await CargarComentarios(producto.productoID);
    }

    private string FormatearInformacionNutricional(string informacionNutricional)
    {
        try
        {
            // Parsear el JSON con Newtonsoft.Json
            var nutricionDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(informacionNutricional);

            if (nutricionDict == null)
                return "Información nutricional no disponible";

            var infoBuilder = new StringBuilder();

            // Función auxiliar para obtener valor de manera segura
            object GetValueSafely(string key)
            {
                return nutricionDict.TryGetValue(key, out var value) ? value : null;
            }

            // Energía
            var energia = GetValueSafely("energy-kcal_value");
            if (energia != null)
                infoBuilder.AppendLine($"Energía: {energia} kcal");

            // Carbohidratos
            var carbohidratos = GetValueSafely("carbohydrates_value");
            if (carbohidratos != null)
                infoBuilder.AppendLine($"Carbohidratos: {carbohidratos} g");

            // Proteínas
            var proteinas = GetValueSafely("proteins_value");
            if (proteinas != null)
                infoBuilder.AppendLine($"Proteínas: {proteinas} g");

            // Grasas
            var grasas = GetValueSafely("fat_value");
            if (grasas != null)
                infoBuilder.AppendLine($"Grasas: {grasas} g");

            // Azúcares
            var azucares = GetValueSafely("sugars_value");
            if (azucares != null)
                infoBuilder.AppendLine($"Azúcares: {azucares} g");

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
            return $"No se pudo procesar la información nutricional: {ex.Message}";
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
                            // Crear la visualización del comentario
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

                                    // Calificación con estrellas
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

                        // Ocultar el mensaje de "sé el primero en dar un feedback"
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

    // Método para crear las estrellas basadas en la calificación
    private StackLayout CreateStars(int rating)
    {
        var starLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 5
        };

        // Invertir la lógica para las estrellas
        int invertedRating = 6 - rating;  // Si rating es 1, invertedRating será 5, si es 5, invertedRating será 1

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

    private void OnStarClicked(object sender, EventArgs e)
    {
        var selectedStar = (ImageButton)sender;
        int selectedRating = 0;
        int selectedRt = 0;

        // Determinar qué estrella fue seleccionada
        if (selectedStar == star1) { selectedRating = 1; selectedRt = 5; }
        else if (selectedStar == star2) { selectedRating = 2; selectedRt = 4; }
        else if (selectedStar == star3) { selectedRating = 3; selectedRt = 3; }
        else if (selectedStar == star4) { selectedRating = 4; selectedRt = 2; }
        else if (selectedStar == star5) { selectedRating = 5; selectedRt = 1; }

        // Actualizar las imágenes de las estrellas
        UpdateStarImages(selectedRating);

        // Guardar la valoración en las preferencias
        Preferences.Set("UserRating", selectedRt);
    }

    private void UpdateStarImages(int rating)
    {
        // Cambiar las imágenes de las estrellas en función de la calificación
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
                usuarioID = _usuarioID,
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
                    await DisplayAlert("Éxito", "Feedback enviado correctamente.", "Aceptar");
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
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
        }
    }
}