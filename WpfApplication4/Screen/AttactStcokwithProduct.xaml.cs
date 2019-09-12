using System.Windows;
using System;
using System.Timers;
using UMA24.Manager;
using System.Windows.Controls;
using System.Data;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for AttactStcokwithProduct.xaml
    /// </summary>
    public partial class AttactStcokwithProduct : Window
    {
        uma24DataContext db;
        public Timer timer;
        public int Count { get; set; }
       

        public AttactStcokwithProduct()
        {
            InitializeComponent();
            timer = new Timer(200000);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            db = new uma24DataContext();
            this.girdProductSource.DataContext = db.ProductSourceDetails;
     
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            txtQuantity.Text = ChangeValue(txtQuantity.Text, true);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtQuantity.Text = ChangeValue(txtQuantity.Text, false);
        }

        private string ChangeValue(string target, bool isPlus)
        {
            int s = Convert.ToInt32(txtQuantity.Text);
            if (isPlus == true)
            {
                s++;

            }
            else
            {
                if (s > 0) s--;
            }
            return s.ToString();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action dele = () => this.Close();
            this.Dispatcher.BeginInvoke(
               dele
                );
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            SaveNewData();
            db.SubmitChanges();
            
               Close();
        }

        private void SaveNewData()
        {
            

              ProductSourceDetail newProductSource = new ProductSourceDetail();
           

            if (this.listProduct.SelectedIndex > 0 && this.listStock.SelectedIndex > 0)
            {
                int productID = DataManager.GetProductID(this.listProduct.SelectedValue.ToString());
                int stockID = DataManager.GetStockID(this.listStock.SelectedValue.ToString());
                 int quantity = Convert.ToInt32(this.txtQuantity.Text);
                
                db.ProductSourceDetails.InsertOnSubmit(
                    new ProductSourceDetail { 
                        ProductID = productID,
                        StockID = stockID, 
                        Quantity = quantity });                            
            }
        }

        private void listStock_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            var stockListData = (e.OriginalSource as ListBox);

            if (stockListData.SelectedIndex > -1)
            {

                this.girdAssisiatInfo.ItemsSource = DataManager.ProductSourceGet(stockListData.SelectedValue.ToString());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Dispose();
        }




    }
}
