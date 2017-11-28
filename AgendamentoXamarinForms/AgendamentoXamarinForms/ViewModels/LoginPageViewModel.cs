using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using AgendamentoXamarinForms.Services;
using Newtonsoft.Json;

namespace AgendamentoXamarinForms.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        public ICommand LoginCommand { get; }

        private string _usuario;
        public string Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }
        private string _senha;
        public string Senha
        {
            get { return _senha; }
            set { SetProperty(ref _senha, value); }
        }
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            LoginCommand = new Command(Login);
            Usuario = "heitorteste@mail.com";
            Senha = "hei24112017";
        }

        private async void Login()
        {
            if(string.IsNullOrEmpty(Usuario) && string.IsNullOrEmpty(Senha))
            {
                await _navigationService.NavigateAsync("TodasAtividadesPage");
                return;
            }
            AtivarLoad(true);
            var api = new EvoApi();
            var result = await api.Logar(Usuario, Senha);
            if (result == null || result.Item1 != null)
            {
                await _dialogService.DisplayAlertAsync("Erro", result?.Item1.messageResult ?? "Ocorreu um erro", "OK");
            }
            else
            {
                var token = result.Item2.token.Replace("/", "[barra]").Replace("#", "[sharp]");
                await _navigationService.NavigateAsync("TodasAtividadesPage?obj=" + token);
            }
            AtivarLoad(false);
        }
    }
}
