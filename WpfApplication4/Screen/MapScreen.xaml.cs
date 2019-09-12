using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for MapScreen.xaml
    /// </summary>
    public partial class MapScreen : Window
    {
        Timer closingTimer;
        Action hidingWindowDelegate;
        string gmap;
        public MapScreen()
        {
            InitializeComponent();
            closingTimer = new Timer(Config.Envoriment.MapOnTime);
            closingTimer.Elapsed += new ElapsedEventHandler(closingTimer_Elapsed);
            closingTimer.AutoReset = true;
            hidingWindowDelegate = new Action(HideWindowd);
        }

        void closingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.MapWindow.Dispatcher.Invoke(hidingWindowDelegate, null);
        }

        private void HideWindowd()
        {
            closingTimer.Stop();
            this.Hide();

        }
      

        public void OpenUrl(string _sourceUrl)
        {
           
             //gmap = string.Format("http://maps.google.com/maps?f=q&source=s_q&hl=ko&geocode=&q={0}", System.Web.HttpUtility.UrlEncode(_sourceUrl));
             //gmap = string.Format("http://maps.naver.com/?query={0}", System.Web.HttpUtility.UrlEncode(_sourceUrl));
            gmap = string.Format("http://maps.kakao.com/?q={0}", System.Web.HttpUtility.UrlEncode(_sourceUrl));
            // UriBuilder urlBuilder = new UriBuilder();

            MapBrowser.Source = new Uri(gmap);
            this.ShowDialog();
            closingTimer.Start();
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
           
            closingTimer.Stop();

            this.Hide();
        }
    }
}
