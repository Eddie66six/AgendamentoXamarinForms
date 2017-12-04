using AgendamentoXamarinForms.Models;
using AgendamentoXamarinForms.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AgendamentoXamarinForms.ViewModels
{
    public class TodasAtividadesPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        public ICommand DetalhesAtividadeCommand { get; }
        public ICommand AcaoCommand { get; }
        public ICommand AlterDateCommand { get; }
        public ICommand AlterarDisponivelCommand { get; set; }

        private List<Atividade> _atividades;

        public List<Atividade> Atividades
        {
            get { return _atividades; }
            set { SetProperty(ref _atividades, value); }
        }

        private bool _apenasDisponiveis;

        public bool ApenasDisponiveis
        {
            get { return _apenasDisponiveis; }
            set { SetProperty(ref _apenasDisponiveis, value); }
        }

        private DateTime _data;

        public DateTime Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }

        private string _disponivel;

        public string Disponivel
        {
            get { return _disponivel; }
            set { SetProperty(ref _disponivel, value); }
        }

        //w1NvI32XNXWIHfnkYACxQA==
        //txyy5LcO+xUgHZb+ohZSDw==
        private string token = "txyy5LcO+xUgHZb+ohZSDw==";
        public TodasAtividadesPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            DetalhesAtividadeCommand = new Command<Atividade>(DetalhesAtividade);
            AcaoCommand = new Command<Atividade>(Acao);
            AlterDateCommand = new Command<string>(AlterDate);
            AlterarDisponivelCommand = new Command(AlterarDisponivel);
            ApenasDisponiveis = true;
            Data = DateTime.Now;
            Disponivel = "Mostrar todos";
        }

        private void AlterarDisponivel()
        {
            AlterDate("");
        }

        private async void AlterDate(string obj)
        {
            if (obj == "-")
                Data = Data.AddDays(-1);
            else if (obj == "+")
                Data = Data.AddDays(1);
            AtivarLoad(true);
            var api = new EvoApi();
            var result = await api.ObterTodasAtividades(token, Data, ApenasDisponiveis);
            if (result == null || result.Item1 != null)
            {
                await _dialogService.DisplayAlertAsync("Erro", result?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
            }
            else
            {
                Atividades = result.Item2;
            }
            AtivarLoad(false);
        }
        private async void Acao(Atividade obj)
        {
            var api = new EvoApi();
            switch (obj.Botao.Value)
            {
                case ButtonValue.Participar:
                    if (obj.flNumerarVagas)
                    {
                        var json = JsonConvert.SerializeObject(obj).Replace("/", "[barra]").Replace("#", "[sharp]");
                        var pToken = token.Replace("/", "[barra]").Replace("#", "[sharp]");
                        await _navigationService.NavigateAsync($"NumeracaoVagaPage?obj={json}&token={pToken}",null, true);
                        return;
                    }
                    else
                    {
                        AtivarLoad(true);
                        var resultP = await api.ParticiparDaAtividade(token, obj.idAtividadeSessao.GetValueOrDefault(), Data, null);
                        if (resultP == null || resultP.Item1 != null)
                        {
                            AtivarLoad(false);
                            await _dialogService.DisplayAlertAsync("Erro", resultP?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                        }
                        else
                        {
                            AtivarLoad(false);
                            AlterDate("");
                        }
                    }
                    break;
                case ButtonValue.Fila:
                    AtivarLoad(true);
                    var resultF = await api.EntrarNaFilaDaAtividade(token, Data, obj.idAtividadeSessao.GetValueOrDefault());
                    if (resultF == null || resultF.Item1 != null)
                    {
                        AtivarLoad(false);
                        await _dialogService.DisplayAlertAsync("Erro", resultF?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                    }
                    else
                    {
                        AtivarLoad(false);
                        AlterDate("");
                    }
                    break;
                case ButtonValue.Sair:
                    AtivarLoad(true);
                    var resultS = await api.SairDaAtividade(token, Data, obj.idAtividadeSessao.GetValueOrDefault());
                    if (resultS == null || resultS.Item1 != null)
                    {
                        AtivarLoad(false);
                        await _dialogService.DisplayAlertAsync("Erro", resultS?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
                    }
                    else
                    {
                        AtivarLoad(false);
                        AlterDate("");
                    }
                    break;
            }
        }

        private async void DetalhesAtividade(Atividade obj)
        {
            var json = JsonConvert.SerializeObject(obj).Replace("/", "[barra]").Replace("#", "[sharp]");
            var pToken = token.Replace("/", "[barra]").Replace("#", "[sharp]");
            await _navigationService.NavigateAsync($"DetalhesAtividadePage?obj={json}&token={pToken}");
        }

        public override async void MyOnNavigatedTo(NavigationParameters parameters)
        {
            if(parameters.ContainsKey("obj"))
            {
                token = parameters["obj"].ToString().Replace("[barra]", "/").Replace("#", "[sharp]");
            }

            AtivarLoad(true);
            var api = new EvoApi();
            var result = await api.ObterTodasAtividades(token, Data, ApenasDisponiveis);
            if (result == null || result.Item1 != null)
            {
                await _dialogService.DisplayAlertAsync("Erro", result?.Item1.errors[0].value ?? "Ocorreu um erro", "OK");
            }
            else
            {
                Atividades = result.Item2;
            }
            AtivarLoad(false);
        }
    }
}
