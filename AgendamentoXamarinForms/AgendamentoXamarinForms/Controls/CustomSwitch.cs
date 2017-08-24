using System.Windows.Input;
using Xamarin.Forms;

namespace AgendamentoXamarinForms.Controls
{
    public class CustomSwitch: Switch
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(CustomSwitch), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public CustomSwitch()
        {
            Toggled += Selecionado;
        }

        private void Selecionado(object sender, ToggledEventArgs e)
        {
            if (Command == null) return;
            if (Command.CanExecute(e))
                Command.Execute(e);
        }
    }
}
