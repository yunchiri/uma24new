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
using UMA24.Manager;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for StockScreen.xaml
    /// </summary>
    public partial class StockScreen : Window
    {
        uma24DataContext db;
        public StockScreen()
        {
            InitializeComponent();
            db= new uma24DataContext();
            this.StocksGrid.DataContext = db.Stocks;
        }
        public StockScreen(Stock stocks)
        {
            InitializeComponent();
            base.DataContext = stocks;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
         
            db.SubmitChanges();
            db = null;
          this.Close();
        }
    }
}
