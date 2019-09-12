using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Timers;
using System;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for AddressFindScreen.xaml
    /// </summary>
    public partial class ButtonSelectScreen : Window
    {
     //   public OrderScreen Target { get; set; }
        public string Data { get; set; }
        public Timer timer;
        public ButtonSelectScreen()
        {
            InitializeComponent();
            Data = "취소";
            timer = new Timer(Config.Envoriment.SelectScreenTime);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
          Action dele =() => this.Close();
              this.Dispatcher.BeginInvoke(dele);
        }
     
        

        private void ScreenSelectAddr_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.timer.Dispose();
        }

     
    }
}
