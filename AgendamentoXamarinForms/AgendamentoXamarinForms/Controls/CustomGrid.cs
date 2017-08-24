using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AgendamentoXamarinForms.Controls
{
    public class CustomGrid : Grid
    {
        //linhas e colunas
        public static readonly BindableProperty QtdeColumnsProperty =
            BindableProperty.Create("QtdeColumns", typeof(int), typeof(CustomGrid), 0, propertyChanged: propertyChanged);

        public int QtdeColumns
        {
            get { return (int)GetValue(QtdeColumnsProperty); }
            set { SetValue(QtdeColumnsProperty, value); }
        }
        public static readonly BindableProperty QtdeRowsProperty =
            BindableProperty.Create("QtdeRows", typeof(int), typeof(CustomGrid), 0, propertyChanged: propertyChanged);
        public int QtdeRows
        {
            get { return (int)GetValue(QtdeRowsProperty); }
            set { SetValue(QtdeRowsProperty, value); }
        }

        //largura e altura
        public static readonly BindableProperty ItemHeightProperty =
            BindableProperty.Create("ItemHeight", typeof(double), typeof(CustomGrid), 0.0, propertyChanged: propertyChanged);
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public static readonly BindableProperty ItemWidthProperty =
            BindableProperty.Create("ItemWidth", typeof(double), typeof(CustomGrid), 0.0, propertyChanged: propertyChanged);
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        //total de celulas, ignora linhas e considera o parametro colunas como max
        public static readonly BindableProperty QtdeTotalProperty =
            BindableProperty.Create("QtdeTotal", typeof(int), typeof(CustomGrid), 0, propertyChanged: propertyChanged);

        public int QtdeTotal
        {
            get { return (int)GetValue(QtdeTotalProperty); }
            set { SetValue(QtdeTotalProperty, value); }
        }

        //string com separados por ',' com os index que seram desativados 
        public static readonly BindableProperty DisableIndexProperty =
            BindableProperty.Create("DisableIndex", typeof(string), typeof(CustomGrid), null, propertyChanged: propertyChanged);

        public string DisableIndex
        {
            get { return (string)GetValue(DisableIndexProperty); }
            set { SetValue(DisableIndexProperty, value); }
        }

        //click
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(CustomGrid), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public CustomGrid()
        {
        }

        private static void propertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var binding = (CustomGrid)bindable;
            if (oldValue != newValue)
            {
                binding.MontarGrid();
            }
        }
        protected void LimparBotoes()
        {
            foreach (var item in Children.Where(p=>p.GetType() == typeof(Button)))
            {
                var btn = (Button)item;
                btn.BackgroundColor = Color.Default;
            }
        }
        public void MontarGrid()
        {
            Children.Clear();
            if (QtdeTotal == 0)
            {
                for (int column = 0; column < QtdeColumns; column++)
                {
                    for (int row = 0; row < QtdeRows; row++)
                    {
                        Children.Add(new StackLayout()
                        {
                            HeightRequest = ItemHeight,
                            WidthRequest = ItemWidth,
                            BackgroundColor = Color.Aqua
                        }, column, row);
                    }
                }
            }
            else
            {
                var disableIndex = new int[0];
                if (!string.IsNullOrEmpty(DisableIndex))
                    disableIndex = DisableIndex.Split(',').Select(int.Parse).ToArray();
                var celulasRestantes = QtdeTotal;
                var indexAtual = 0;
                for (int row = 0; row < (QtdeTotal / QtdeColumns) + 1; row++)
                {
                    for (int column = 0; column < QtdeColumns; column++)
                    {
                        if (celulasRestantes > 0)
                        {
                            indexAtual++;
                            celulasRestantes--;
                            var btn = new Button()
                            {
                                Text = indexAtual.ToString(),
                                HeightRequest = ItemHeight,
                                WidthRequest = ItemWidth,
                                IsEnabled = !(disableIndex.Contains(indexAtual))
                            };
                            if (!(disableIndex.Contains(indexAtual)))
                            {
                                btn.Command = Command;
                                btn.CommandParameter = indexAtual;
                                btn.Clicked += (e, s) =>
                                {
                                    LimparBotoes();
                                    btn.BackgroundColor = Color.Blue;
                                };
                            }
                            Children.Add(btn, column, row);
                        }
                        else
                        {
                            Children.Add(new StackLayout { BackgroundColor = Color.Transparent, HeightRequest = ItemHeight, WidthRequest = ItemWidth }, column, row);
                        }
                    }
                }
            }
        }
    }
}
