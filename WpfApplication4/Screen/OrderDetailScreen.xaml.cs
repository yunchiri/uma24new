using System;
using System.Text;
using System.Windows;
using System.Timers;
using UMA24.Manager;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using NLog;
//using System.Threading;
namespace UMA24.Screen
{
   
    public partial class OrderDetailScreen : Window
    {
     
      public Order OrderData { get; set; }
      private Timer timer;
      private bool deleOrder = false;
      private bool isEdited;
        ObservableCollection<OrderDetail> detailsList = new ObservableCollection<OrderDetail>();
        

        public OrderDetailScreen()
        {
            InitializeComponent();
        }

        public OrderDetailScreen(Order _order)
        {
            InitializeComponent();
            OrderData = _order;
            
            BindXaml(OrderData);
            timer = new Timer(Config.Envoriment.OrderScreenDetailTime);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            if(_order.Customer.AddrExtra != null && _order.Customer.AddrExtra.Contains("모텔"))
            {
                this.chcIsMotel.IsChecked = true;
            }

            this.Background = DataManager.AddrColor(_order.Customer);//backgournd color
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action dele = () => this.Close();
            this.Dispatcher.BeginInvoke(
               dele
                );

        }
    
    
        private void BindXaml(Order order)
        {
            base.DataContext = order;
            
        }
    
        private void LoadingMap()
        {
            #region loading map
            StringBuilder sb = new StringBuilder();
            sb.Append("대구 ");
            sb.Append(txtAddrGu.Text + " ");
            sb.Append(txtAddrDong.Text + " ");

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
 
        private void ClosingProcess(bool? isAlreadyfinished)
        {
            if (isAlreadyfinished != true)
            {
                DataManager.CalculateRating(OrderData);
                
                
                //2011-3-2 업데이트되면 1시간 전에 갔을때 아 귀찮다
                //업데이트되면 전에 정보도 업데이트 
                
            }

            MainWindow mainScreen = Application.Current.MainWindow as MainWindow;
            if (mainScreen.OrderIndexTime != 1)
            {
                ScreenManager.UpdateMainUIBeforeData(mainScreen.OrderIndexTime);
            }
            else
            {
                ScreenManager.UpdateMainUI();
            }

            this.RatingStar.Clear();
            this.timer.Dispose();
            this.DataContext = null;
            this.OrderData = null;
            this.OrderDetailList.ItemsSource = null;
            this.gridBeforeData.ItemsSource = null;
            this.Close();
            detailsList.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(detailsList_CollectionChanged);
            detailsList = null;
           
           
        }

        #region Event
        private void OrderDetailScreen_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var p in  OrderData.OrderDetails)
            {
                detailsList.Add( p);
             //   OrderData.OrderDetails[0].Product.ProductName
            }
            detailsList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(detailsList_CollectionChanged);

          
            this.EmployListBox.ItemsSource = DataManager.EmployeeGet(); //employee name loading
            this.EmployListBox2.ItemsSource = DataManager.EmployeeGet();
            
            //이전 주문 목록
            if (OrderData.Customer.AddrGugun != "홀주문")
            {
                //count
                BeforeOrderCountLabel.Content = DataManager.OrderGetFomerDataCount(OrderData.CustomerID).ToString();
               // this.gridBeforeData.ItemsSource = DataManager.OrderGetFomerData(OrderData.CustomerID);
            }
            this.OrderDetailList.ItemsSource = detailsList;
            
            this.txtTotalPrice.Text = OrderManager.OrderPriceUpdate(detailsList);
            if (txtPhoneNumber.Text.Length > 4)
            {
                //LoadingMap();
                this.RatingStar.SetRating(DataManager.GetRating(OrderData.CustomerID));
            }
        }

        void detailsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
            switch (e.Action.ToString())
            {
                 
                case "Add":
                    OrderDetail newItems = e.NewItems[0] as OrderDetail;                  
                    OrderData.OrderDetails.Add(newItems);
                    break;

                case "Replace":
                    OrderData.OrderDetails[e.NewStartingIndex]
                        = e.NewItems[0] as OrderDetail;
                    
                    break;
                case "Remove":
                    OrderData.OrderDetails.RemoveAt(e.OldStartingIndex);
                    break;

            }
            isEdited = true;
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

            PrintManager.PrintOrderInfo(OrderData, chcIsMotel.IsChecked, chcIsVAT.IsChecked);
                
            //todo 임시로 처리하게 만들어야함
           DataManager.SaveToDBOrderFinish(OrderData, "임시");
          
            this.btnPrint.Content = "출력했음\n임시처리";
            ScreenManager.UpdateMainUI();
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OrderData.Finish == true) return;
            if (deleOrder == true)
            {
                //btn delete
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Info("Order Cancled : " + OrderData.ID);
               
                DataManager.DeleteOrder(OrderData);
                ClosingProcess(true);
                //ScreenManager.UpdateMainUI();
                
            }
            else
            {
                btnDelOrder.Content = "삭제합니다?";
                deleOrder = true;
            }
            
       
        
        }

        private void OrderDetailScrn_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            timer.Interval += Config.Envoriment.OrderScreenDetailTimeStay;
           
            if ((e.Source is System.Windows.Documents.Run) == false)
            {
                FrameworkElement feSource = e.Source as FrameworkElement;

                switch (feSource.Name)
                {

                    case "txtAddrGu":
                        ScreenManager.CreateNewAddrGuScreen(sender as OrderDetailScreen);
                        
                        break;
                    case "txtAddrDong":
                        ScreenManager.CreateNewAddrDongScreen(sender as OrderDetailScreen);
                      
                        break;
                }
                e.Handled = false;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SubMitProcess(1);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SubMitProcess(2);
        }

        private void SubMitProcess(int id)
        {
            bool? AlreadyFinish = OrderData.Finish;
            if (isEdited == true) OrderManager.UpdateOrderDetails();

            switch (id)
            {
                case 1:
                    ScreenManager.SelectedEmployeeName = this.EmployListBox.SelectedItem.ToString();
                    DataManager.SaveToDBOrderFinish(OrderData, EmployListBox.SelectedValue.ToString());
                    break;
                case 2: ScreenManager.SelectedEmployeeName2 = this.EmployListBox2.SelectedItem.ToString();
                    DataManager.SaveToDBOrderFinish(OrderData, EmployListBox2.SelectedValue.ToString());
                    break;
                default: ScreenManager.SelectedEmployeeName = this.EmployListBox.SelectedItem.ToString();
                    DataManager.SaveToDBOrderFinish(OrderData, EmployListBox.SelectedValue.ToString());
                    break;
            }
           
            
            ClosingProcess(AlreadyFinish);
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            bool? AlreadyFinish = OrderData.Finish;
            if (isEdited == true) OrderManager.UpdateOrderDetails();
            //OrderManager.UpdateOrderDetails();
            ClosingProcess(AlreadyFinish);
         }

        private void EmployListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (ScreenManager.SelectedEmployeeName != null)
            {
                this.EmployListBox.SelectedItem = ScreenManager.SelectedEmployeeName;

            }
        }
        private void EmployListBox2_Loaded(object sender, RoutedEventArgs e)
        {
            if (ScreenManager.SelectedEmployeeName2 != null)
            {
                this.EmployListBox2.SelectedItem = ScreenManager.SelectedEmployeeName2;
            }
        }

        
        private void OrderDetailList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
           // OrderManager.UpdateNewOrderDetails(this.OrderDetailList,detailsList );
            OrderManager.UpdateNewOrderDetails3(this.OrderDetailList, detailsList);
            this.txtAddrExtra.Focus();
            this.txtTotalPrice.Text = OrderManager.OrderPriceUpdate(detailsList);
          
        }
     
        private void txtAddrGu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //OrderData.Customer.AddrGugun = (e.Source as TextBox).Text;
               }

        private void txtAddrDong_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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

        private void txtAddrExtra_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //OrderData.Customer.AddrExtra = (e.Source as TextBox).Text;
        }

        private void txtExpends_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //OrderData.OrderExtend = (e.Source as TextBox).Text;
        }
        #endregion

       

        private void OnMapButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingMap();
        }

        private void chcIsMotel_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
