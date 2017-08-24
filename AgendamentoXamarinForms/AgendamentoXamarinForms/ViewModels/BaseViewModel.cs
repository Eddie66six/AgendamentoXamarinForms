using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace AgendamentoXamarinForms.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MyOnNavigatedFrom(parameters);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            MyOnNavigatedTo(parameters);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public virtual Task LoadAsync()
        {
            return Task.FromResult(0);
        }

        public virtual void MyOnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void MyOnNavigatedFrom(NavigationParameters parameters)
        {

        }

        private bool _isVisibleOrEnable = true;
        public bool IsVisibleOrEnable
        {
            get { return _isVisibleOrEnable; }
            set { SetProperty(ref _isVisibleOrEnable, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private decimal _opacity;
        public decimal Opacity
        {
            get { return _opacity == 0 ? 1.0m : _opacity; }
            set { SetProperty(ref _opacity, value); }
        }

        public void AtivarLoad(bool ativar)
        {
            Opacity = ativar == true ? 0.5m : 1.0m;
            IsBusy = ativar;
            IsVisibleOrEnable = !ativar;
        }
    }
}
