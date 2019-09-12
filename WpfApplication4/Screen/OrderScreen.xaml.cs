using System;
using System.Text;
using System.Windows;
using System.Web;
using System.Timers;
using UMA24.Manager;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Media;


namespace UMA24.Screen
{
    public partial class OrderScreen : Window
    {
        public  Order OrderData {get;set;}
        public bool isMapOn = false;  
        public Timer timer;
         ObservableCollection<OrderDetail> detailsList = new ObservableCollection<OrderDetail>();
         private bool isCancleCliked = false;       
        public OrderScreen(Order newOrder)
        {
        
            InitializeComponent();
            OrderData = newOrder;
            BindXaml(OrderData);
            this.txtTimeOrder.Text = OrderData.Date.ToShortTimeString();

            List<string> PhoneNumberList = DataManager.GetExistOrderPhoneNumber();

            //todo did 2010-11-4

            if (PhoneNumberList.Contains(newOrder.Customer.PNumberID))
            {
                timer = new Timer(Config.Envoriment.OrderScreenTimeAlreadyExistORder);
            }
            else
            {
                timer = new Timer(Config.Envoriment.OrderScreenTime); 
            }
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }
        public OrderScreen(Order newOrder,short PhoneLineNumber)
        {

            InitializeComponent();
            OrderData = newOrder;
            BindXaml(OrderData);
            this.txtTimeOrder.Text = OrderData.Date.ToShortTimeString();
            this.txtPhoneLineNo.Text = PhoneLineNumber.ToString();
            List<string> PhoneNumberList = DataManager.GetExistOrderPhoneNumber();

            //todo did 2010-11-4

            if (PhoneNumberList.Contains(newOrder.Customer.PNumberID))
            {
                timer = new Timer(Config.Envoriment.OrderScreenTimeAlreadyExistORder);
            }
            else
            {

                timer = new Timer(Config.Envoriment.OrderScreenTime);

                //새로 주문왔을때 프린트
                
                
                    string preInformationSring = DateTime.Now.ToShortTimeString() + "<" + newOrder.PhoneLineNo.ToString() + ">"
                         + newOrder.Customer.PNumberID
                        + Environment.NewLine + ">" + newOrder.Customer.AddrGugun + " " + newOrder.Customer.AddrDong
                        + Environment.NewLine+ ">"  + newOrder.Customer.AddrExtra;

                    PrintManager.PrintPreInformation(preInformationSring, "preInfo");
                
            }



            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public OrderScreen(Order newOrder, short PhoneLineNumber, bool _isExistOrder)
        {

            InitializeComponent();
            OrderData = newOrder;
            BindXaml(OrderData);
            this.txtTimeOrder.Text = OrderData.Date.ToShortTimeString();
            this.txtPhoneLineNo.Text = PhoneLineNumber.ToString();

                this.Background = DataManager.AddrColor(newOrder.Customer);
          
            if (_isExistOrder)
            {
                timer = new Timer(Config.Envoriment.OrderScreenTimeAlreadyExistORder);
            }
            else
            {
                timer = new Timer(Config.Envoriment.OrderScreenTime);
            }
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }
    
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action dele = () => CloseProcess();
            this.Dispatcher.BeginInvoke(dele);
        }

        private void BindXaml(Order target_order)
        {
            //TODO After learn Binding...
            base.DataContext = target_order;
            this.OrderList.ItemsSource = detailsList;
        }

      

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
                if(txtCallNumber.Text.Length >5)      LoadingMap();
        }

        private void LoadingMap()
        {
            #region loading map
            StringBuilder sb = new StringBuilder();
            sb.Append("대구 ");
            sb.Append(txtAddrGu.Text);sb.Append(" ");
            sb.Append(txtAddrDong.Text);sb.Append(" ");

            int divideCountIndex = txtAddrExtra.Text.IndexOf("-");
            int divideSpaceIndex = txtAddrExtra.Text.IndexOf(" ");
            if (divideCountIndex > 0)
            {
                if (divideCountIndex < divideSpaceIndex)
                {
                    sb.Append(txtAddrExtra.Text.Substring(0, divideSpaceIndex));
                }
                else
                {
                    sb.Append(txtAddrExtra.Text);
                }
            }

            ScreenManager.MapScr.OpenUrl(sb.ToString());
            #endregion

        }

        private void ScreenOrder_Loaded(object sender, RoutedEventArgs e)
        {
            //2011-3-7 별로안써서 잠시 없앰 
            //this.OrderlistBefore.ItemsSource = DataManager.OrderGetFomerData(OrderData.CustomerID);

            this.txtOrderCount.Text = DataManager.OrderGetFomerDataCount(OrderData.CustomerID).ToString();

            if (this.txtCallNumber.Text.Length > 6)
            {
                //LoadingMap();

                this.RatingStar.SetRating(DataManager.GetRating(OrderData.CustomerID));
            }
        }
     

        private void btnOrdCanc_Click(object sender, RoutedEventArgs e)
        {
            if (isCancleCliked)
            {
                DataManager.DeleteOrderFromOrderScreen(OrderData);
                CloseProcess();
        }
            else
            {
                isCancleCliked = true;
            }
            //DataManager.DeleteOrder(this.OrderData);
            //CloseProcess();
        }

        private void btnOrdAcpt_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtCallNumber.Text.Length > 0 && this.OrderList.Items.Count > 1)
            {
                if (this.OrderData.Customer.PNumberID.Length > 11)
                {
                    //전화번호가 잘못들어오면 수정
                    //11자리 넘어오면 11개까지 끊음
                    this.OrderData.Customer.PNumberID = this.OrderData.Customer.PNumberID.Substring(0, 10);
                    
                }

                foreach (var p in detailsList)
                {
                    if(p.Quantity != 0)                    OrderData.OrderDetails.Add(p);
                }
                OrderManager.SaveToDBNewOrder(OrderData);
                CloseProcess();
                ScreenManager.UpdateMainUI();
            //    PrintManager.PrintOrderInfo(OrderData, false);
            }

        }
   

       

        private void ScreenOrder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            timer.Interval += Config.Envoriment.OrderScreenTimeStay;
            if ((e.Source is System.Windows.Documents.Run) == false )
            {
                FrameworkElement feSource = e.Source as FrameworkElement;

                switch (feSource.Name)
                {

                    case "txtAddrGu":
                        ScreenManager.CreateNewAddrGuScreen(sender as OrderScreen);
                     //   e.Handled = true;
                        break;
                    case "txtAddrDong":
                        ScreenManager.CreateNewAddrDongScreen(sender as OrderScreen);
                    //    e.Handled = true;
                        break;       
                }
                e.Handled = false;
            }
        }

        private void txtCallNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
         //   OrderData.Customer.PNumberID = (e.Source as TextBox).Text;
            if ((sender as TextBox).Text.Length >= 10)
            {
                var x = DataManager.CustomerGetID2((sender as TextBox).Text);
                if (x > 0)
                {
                    this.OrderData.Customer = DataManager.CustomerGetInfo(x);
                }
                else
                {
                    if(OrderData.CustomerID >0)      this.OrderData.Customer = new Customer();
                }
            }
        }

        private void txtAddrExtra_TextChanged(object sender, TextChangedEventArgs e)
        {
       //     OrderData.Customer.AddrExtra = (e.Source as TextBox).Text;
        }

        private void txtExpend_TextChanged(object sender, TextChangedEventArgs e)
        {
         //   OrderData.OrderExtend = (e.Source as TextBox).Text;
        }

        private void OrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderManager.UpdateNewOrderDetails3(sender as DataGrid , this.detailsList);
            #region test
            //int selIndex = (e.Source as DataGrid).SelectedIndex;
                //int diffIndex = (e.Source as DataGrid).Items.Count - selIndex;

                ////e.AddedItems.ToString();
            
                //if (diffIndex == 1)
                //{
                //    //add item
               
                //    OrderDetail newOrderDetail = OrderManager.GetNewOrderDetail(true);
                //    if (newOrderDetail != null)  detailsList.Add(newOrderDetail);
                //    this.OrderList.SelectedIndex = -1;
                //}
                //else if (selIndex > -1)
                //{
                //     OrderDetail newOrderDetail = OrderManager.GetNewOrderDetail(false);
                //     if (newOrderDetail.Product.ProductName != "취소")
                //     {
                //         detailsList[selIndex] = newOrderDetail;
                //     }
                //     else
                //     {
                //         detailsList.RemoveAt(selIndex);
                //     }
                   
                   
                //    //detailsList.Add(newOrderDetail);
                //    //remove or edit item
            //}
            #endregion
            this.txtAddrExtra.Focus();
            this.txtOrderPrice.Text = OrderManager.OrderPriceUpdate(detailsList);

        }

        private void txtAddrGu_TextChanged(object sender, TextChangedEventArgs e)
        {
          //  OrderData.Customer.AddrGugun = (e.Source as TextBox).Text;
        }
        private void CloseProcess()
        {
           
           
            this.timer.Dispose();
            this.DataContext = null;
            this.OrderList.ItemsSource = null;
            this.OrderlistBefore.ItemsSource = null;
                this.OrderData = null;
                this.detailsList.Clear();
                this.RatingStar.Clear();
            this.Close();
            
        }

        private void txtAddrDong_TextChanged(object sender, TextChangedEventArgs e)
        {
            var source = (e.OriginalSource as TextBox).Text;
            if (source.Length > 0)
            {
                if (this.OrderData.Customer.AddrGugun != null)
                {
                    if (OrderData.Customer.AddrGugun.Contains("홀") || OrderData.Customer.AddrGugun.Contains("부대"))
                    {
                        OrderData.Customer.PNumberID = DataManager.AddrGetID(this.OrderData.Customer.AddrDong).ToString();
                        OrderData.Customer = DataManager.CustomerGetInfo(DataManager.CustomerGetID2(OrderData.Customer.PNumberID));
                    }
                }
                else
                {
                    OrderData.Customer.AddrGugun = DataManager.AddrGetGubyDong(source);
                }
            }
      
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //btn map on
            LoadingMap();
        }

       
    }
}
