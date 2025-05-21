using NutriApp.DTO;
using Newtonsoft.Json;
using System.Text;

namespace NutriApp;

public partial class AlergiaPage : ContentPage
{
    private List<string> _selectedAllergens = new List<string>();
    private int _userId;
    private readonly string _token;
    private readonly Dictionary<string, string> _allergenMapping = new Dictionary<string, string>
{
    {"Gluten", "1"},
    {"Lactosa", "2"},
    {"Nueces", "3"},
    {"Huevos", "4"},
    {"Soja", "5"},
    {"Pescado", "6"},
    {"Mariscos", "7"},
    {"Cacahuates", "8"}
};
    public AlergiaPage(int UsuarioID, string token)
    {
        InitializeComponent();
        this._userId = UsuarioID;
        _token = token;
    }

    private void OnAllergenCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;

        // Obtener el nombre del alérgeno desde ClassId
        string allergenName = checkBox.ClassId;

        // Depuración: Mostrar ClassId del checkbox y el estado
        System.Diagnostics.Debug.WriteLine($"Checkbox ClassId: {allergenName}, IsChecked: {checkBox.IsChecked}");

        // Verificar si el alérgeno está en el diccionario
        if (_allergenMapping.ContainsKey(allergenName))
        {
            if (checkBox.IsChecked)
            {
                _selectedAllergens.Add(_allergenMapping[allergenName]);
            }
            else
            {
                _selectedAllergens.Remove(_allergenMapping[allergenName]);
            }

            UpdateSelectedAllergensLabel();
        }
        else
        {
            // Depuración: Mostrar un mensaje si el alérgeno no se encuentra en el diccionario
            System.Diagnostics.Debug.WriteLine($"Alérgeno no encontrado en el diccionario: {allergenName}");
        }
    }



    private void UpdateSelectedAllergensLabel()
    {
        if (_selectedAllergens.Count != 0)
        {
            var allergenNames = _selectedAllergens
                .Select(id => _allergenMapping.FirstOrDefault(x => x.Value == id).Key)
                .Where(name => !string.IsNullOrEmpty(name)) // Filter out any null/empty names
                .ToList();
        }
    }

    private async void OnGuardarAlergias(object sender, EventArgs e)
    {
        if (_selectedAllergens.Count == 0)
        {
            await DisplayAlert("Atención", "Por favor, selecciona al menos un alergeno", "Aceptar");
            return;
        }

        try
        {
            // Convertir los IDs de alérgenos a una lista de strings
            var allergenIds = _selectedAllergens.ToList();

            // Realizar la solicitud HTTP
            using (HttpClient client = new HttpClient())
            {
                ReqInsertarAlergias req = new ReqInsertarAlergias
                {
                    userID = _userId,
                    alergias = allergenIds
                };

                var contenidoJSON = new StringContent(JsonConvert.SerializeObject(req),Encoding.UTF8,"application/json");

                var response = await client.PostAsync($"{ApiConfig.UrlBase}api/usuario/registrarAlergias",contenidoJSON);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResInsertarAlergias>(responseContent);

                    if (res.exito)
                    {
                        await DisplayAlert("Éxito", res.mensaje.FirstOrDefault() ?? "Alergias guardadas correctamente", "Aceptar");
                        // Guardar sesión
                        SessionManager.SaveSession(_token, _userId);

                        // Cambiar página principal
                        Application.Current.MainPage = new NavigationPage(new HomePage(_userId, _token));
                    }
                    else
                    {
                        await DisplayAlert("Error", res.mensaje.FirstOrDefault() ?? "No se pudieron guardar las alergias", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error de conexión", "No fue posible conectar con el servidor", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
        }
    }
}