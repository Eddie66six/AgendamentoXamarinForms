using AgendamentoXamarinForms.Models;
using AgendamentoXamarinForms.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace AgendamentoXamarinForms.ViewModels
{
    public class DetalhesAtividadePageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        public ICommand AcaoCommand { get; }

        private Atividade _atividade;

        public Atividade Atividade
        {
            get { return _atividade; }
            set { SetProperty(ref _atividade, value); }
        }

        public DetalhesAtividadePageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            AcaoCommand = new Command(Acao);
        }

        public override void MyOnNavigatedTo(NavigationParameters parameters)
        {
            AtivarLoad(true);
            if (parameters.ContainsKey("cancelado")) return;
            if (!parameters.ContainsKey("obj")) _navigationService.GoBackAsync();
            var json = (string)parameters["obj"];
            if (!string.IsNullOrEmpty(json))
                Atividade = JsonConvert.DeserializeObject<Atividade>(json.Replace("[barra]", "/").Replace("#", "[sharp]"));
            AtivarLoad(false);
        }

        private async void Acao()
        {
            var api = new EvoApi();
            switch (Atividade.Botao.Value)
            {
                case ButtonValue.Participar:
                    if (Atividade.flNumerarVagas)
                    {
                        var json = JsonConvert.SerializeObject(Atividade).Replace("/", "[barra]").Replace("#", "[sharp]");
                        await _navigationService.NavigateAsync($"NumeracaoVagaPage?obj={json}", null, true);
                        return;
                    }
                    else
                    {
                        AtivarLoad(true);
                        var resultP = await api.ParticiparDaAtividade("txyy5LcO+xUgHZb+ohZSDw==", Atividade.idAtividadeSessao.GetValueOrDefault(), Atividade.data, null);
                        if (resultP == null || resultP.Item1 != null)
                        {
                            await _dialogService.DisplayAlertAsync("Erro", resultP?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                        }
                        else
                        {
                            await _navigationService.GoBackAsync();
                        }
                        AtivarLoad(false);
                    }
                    break;
                case ButtonValue.Fila:
                    AtivarLoad(true);
                    var resultF = await api.EntrarNaFilaDaAtividade("txyy5LcO+xUgHZb+ohZSDw==", Atividade.data, Atividade.idAtividadeSessao.GetValueOrDefault());
                    if (resultF == null || resultF.Item1 != null)
                    {
                        await _dialogService.DisplayAlertAsync("Erro", resultF?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                    }
                    else
                    {
                        await _navigationService.GoBackAsync();
                    }
                    AtivarLoad(false);
                    break;
                case ButtonValue.Sair:
                    AtivarLoad(true);
                    var resultS = await api.SairDaAtividade("txyy5LcO+xUgHZb+ohZSDw==", Atividade.data, Atividade.idAtividadeSessao.GetValueOrDefault());
                    if (resultS == null || resultS.Item1 != null)
                    {
                        await _dialogService.DisplayAlertAsync("Erro", resultS?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                    }
                    else
                    {
                        await _navigationService.GoBackAsync();
                    }
                    AtivarLoad(false);
                    break;
            }
        }
    }
}
