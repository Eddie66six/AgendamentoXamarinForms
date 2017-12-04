using AgendamentoXamarinForms.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Linq;
using Prism.Services;
using AgendamentoXamarinForms.Services;

namespace AgendamentoXamarinForms.ViewModels
{
    public class NumeracaoVagaPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        public ICommand ClickButtonCommand { get; }
        public ICommand ParticiparCommand { get; }
        public ICommand CancelarCommand { get; }

        private int _column;

        public int Column
        {
            get { return _column; }
            set { SetProperty(ref _column, value); }
        }
        private int _row;

        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }

        private int _total;

        public int Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private double _size;

        public double Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        private string _disableIndex;

        public string DisableIndex
        {
            get { return _disableIndex; }
            set { SetProperty(ref _disableIndex, value); }
        }

        private int? _selecionado = null;
        private int _idConfiguracao;
        private DateTime _data;
        private string token = null;
        private bool numerarVagas = false;

        public NumeracaoVagaPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            ClickButtonCommand = new Command(ClickButton);
            ParticiparCommand = new Command(Participar);
            CancelarCommand = new Command(Cancelar);
            Column = 5;
            Row = 2;
            Size = 50;
        }

        private async void Cancelar()
        {
            var navParams = new NavigationParameters("cancelado=true");
            await _navigationService.GoBackAsync(navParams);
        }

        private async void Participar()
        {
            if (_selecionado == null && numerarVagas)
            {
                await _dialogService.DisplayAlertAsync("Erro", "Selecione uma vaga", "OK");
                return;
            }
            AtivarLoad(true);
            var api = new EvoApi();
            var resultP = await api.ParticiparDaAtividade(token, _idConfiguracao, _data, _selecionado);
            if (resultP == null || resultP.Item1 != null)
            {
                AtivarLoad(false);
                await _dialogService.DisplayAlertAsync("Erro", resultP?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                return;
            }
            AtivarLoad(false);
            await _navigationService.GoBackAsync();
        }

        private void ClickButton(object obj)
        {
            _selecionado = (int?)obj;
        }

        public override void MyOnNavigatedTo(NavigationParameters parameters)
        {
            AtivarLoad(true);
            if (parameters.ContainsKey("obj"))
            {
                token = ((string)parameters["token"])?.Replace("[barra]", "/").Replace("#", "[sharp]");
                var json = (string)parameters["obj"];
                if (!string.IsNullOrEmpty(json))
                {
                    var atividade = JsonConvert.DeserializeObject<Atividade>(json.Replace("[barra]", "/").Replace("#", "[sharp]"));
                    Total = atividade.capacidade;
                    _data = atividade.data;
                    _idConfiguracao = atividade.idAtividadeSessao.GetValueOrDefault();
                    numerarVagas = atividade.flNumerarVagas;
                    if (atividade.lugares != null)
                        DisableIndex = string.Join(",", atividade.lugares.Where(p => p.disponivel == false).Select(p => p.numero));
                }
            }
            AtivarLoad(false);
        }
    }
}
