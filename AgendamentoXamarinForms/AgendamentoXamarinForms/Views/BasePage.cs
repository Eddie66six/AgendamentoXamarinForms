using AgendamentoXamarinForms.ViewModels;
using Xamarin.Forms;

namespace AgendamentoXamarinForms.Views
{
    public class BasePage : ContentPage
    {
        private BaseViewModel ViewModel => BindingContext as BaseViewModel;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel == null) return;
            await ViewModel.LoadAsync();
        }
    }
}
