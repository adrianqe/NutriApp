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

        // Obtener el nombre del al�rgeno desde ClassId
        string allergenName = checkBox.ClassId;

        // Depuraci�n: Mostrar ClassId del checkbox y el estado
        System.Diagnostics.Debug.WriteLine($"Checkbox ClassId: {allergenName}, IsChecked: {checkBox.IsChecked}");

        // Verificar si el al�rgeno est� en el diccionario
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
            // Depuraci�n: Mostrar un mensaje si el al�rgeno no se encuentra en el diccionario
            System.Diagnostics.Debug.WriteLine($"Al�rgeno no encontrado en el diccionario: {allergenName}");
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
            await DisplayAlert("Atenci�n", "Por favor, selecciona al menos un alergeno", "Aceptar");
            return;
        }

        try
        {
            // Convertir los IDs de al�rgenos a una lista de strings
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
                        await DisplayAlert("�xito", res.mensaje.FirstOrDefault() ?? "Alergias guardadas correctamente", "Aceptar");
                        // Guardar sesi�n
                        SessionManager.SaveSession(_token, _userId);

                        // Cambiar p�gina principal
                        Application.Current.MainPage = new NavigationPage(new HomePage(_userId, _token));
                    }
                    else
                    {
                        await DisplayAlert("Error", res.mensaje.FirstOrDefault() ?? "No se pudieron guardar las alergias", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error de conexi�n", "No fue posible conectar con el servidor", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "Aceptar");
        }
    }
}