namespace NutriApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Cargar sesión guardada
            SessionManager.LoadSavedSession();

            // Definir página principal
            MainPage = new NavigationPage(
                SessionManager.IsLoggedIn
                ? new HomePage(SessionManager.UserId, SessionManager.AccessToken)
                : new MainPage()
            );
        }
    }
}
