using Xamarin.Forms;

namespace AgendamentoXamarinForms.Views
{
    public partial class TodasAtividadesPage : BasePage
    {
        public TodasAtividadesPage()
        {
            InitializeComponent();
            listView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };
        }
    }
}
