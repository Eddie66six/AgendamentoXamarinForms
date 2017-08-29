using Prism.Unity;
using AgendamentoXamarinForms.Views;
using Xamarin.Forms;

namespace AgendamentoXamarinForms
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<TodasAtividadesPage>();
            Container.RegisterTypeForNavigation<DetalhesAtividadePage>();
            Container.RegisterTypeForNavigation<NumeracaoVagaPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
        }
    }
}
