using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Timers;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for AddOnScr.xaml
    /// </summary>
  


    public partial class AddOnScr : Window
    {
        Timer syncTimer;
        Action updateScreen;
        //Delegate updateScreen;
        StringBuilder itemText;
        List<Order> orderList;

        public AddOnScr()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            itemText = new StringBuilder();
             orderList = new List<Order>();
            //updateScreen = delegate() { AddScreenItem(); };
            updateScreen = new Action(AddScreenItem);
            //  updateScreen = new Delegate(AddScreenItem);
            syncTimer = new Timer(6000);
            syncTimer.Start();
            syncTimer.Elapsed += new ElapsedEventHandler(syncTimer_Elapsed);

        }


        void syncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            update();
        }

        void update()
        {
            this.stackScreen.Dispatcher.Invoke(updateScreen, null);
        }

        void AddScreenItem()
        {

            try
            {
                List<Order> orderList = UMA24.Manager.DataManager.GetExistOrder();
               
                stackScreen.Children.Clear();
                foreach (Order p in orderList)
                {
                    stackScreen.Children.Add(MakeText(p));
                }
                orderList = null;
            }
            catch (InvalidOperationException)
            {
            }


        }

        

        private TextBlock MakeText(Order _item)
        {
            
            TextBlock newTextBox = new TextBlock();
            newTextBox.FontSize = 64;
            newTextBox.FontWeight = FontWeights.ExtraBold;
            newTextBox.TextWrapping = TextWrapping.Wrap;
           // newTextBox.Padding.Left = 0;
            newTextBox.Background = UMA24.Manager.DataManager.AddrColor(_item.Customer); //AddrColor(_item.Customer);

            itemText.Clear();
            

            if (_item.Customer.AddrExtra != null && _item.Customer.AddrExtra.Contains("그릇") == true)
            {
                itemText.Append("그릇]");
            }
            foreach (var p in _item.OrderDetails)
            {
                itemText.Append(" " + p.Product.ProductName + p.Quantity);
            }

            itemText.Append("<"+_item.Customer.AddrDong); //동추가
            newTextBox.Text = itemText.ToString();
            
            return newTextBox;

        }
    

     
    }
}
