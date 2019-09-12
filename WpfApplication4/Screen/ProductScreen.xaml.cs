using System.Windows;
using System.Timers;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for ProductScreen.xaml
    /// </summary>
    public partial class ProductScreen : Window
    {
        public string Data { get; set; }
        public Timer timer;
     public   bool isEditing;
     
       public ProductScreen()
        {
            InitializeComponent();
            Data = "취소";

            timer = new Timer(Config.Envoriment.SelectScreenTime);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

     
        
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
          System.Action dele =() => this.Close();
              this.Dispatcher.BeginInvoke(dele);
        }
       

    }
}
