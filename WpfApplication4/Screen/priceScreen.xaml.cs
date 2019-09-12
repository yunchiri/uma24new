using System.Windows;
using System.Timers;
using UMA24.Manager;
using UMA24.ModelControl;
using System.Windows.Controls;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for priceScreen.xaml
    /// </summary>
    public partial class priceScreen : Window
    {
        public string Data { get; set; }
        public Timer timer;
     

        public priceScreen()
        {
            InitializeComponent();

            timer = new Timer(Config.Envoriment.SelectScreenTime);
            
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            init();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Action dele = () => this.Hide();
            this.Dispatcher.BeginInvoke(dele);
        }

        void init()
        {
            ModelCreater btnCreator = new ModelCreater();

            foreach (var p in DataManager.ProductListbyCategories(true))
            {
                 this.pricePanel.Children.Add(btnCreator.BtnAddProductInfo(110, 156, 25, p));
            }
        }

        public void ShowPriceScreen()
        {
            timer.Start();
            this.ShowDialog();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                timer.Stop();
                (sender as priceScreen).Hide();
            }
        }

    
      
    }
}
