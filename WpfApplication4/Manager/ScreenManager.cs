using System.Collections.Generic;
using UMA24.Screen;
using UMA24.Model;
using UMA24.ModelControl;
using System.Windows;
using System.Windows.Controls;
using System;
using NLog;
namespace UMA24.Manager
{
    public static class ScreenManager
    {
        #region ScreenSlot
        static List<ScreenPositionModel> ScreenSlot = new List<ScreenPositionModel>
        { 
           
           new ScreenPositionModel(200,160), 
            new ScreenPositionModel(5, 160),
         
        };
        #endregion

        static private List<OrderScreen> ListOrderScreen = new List<OrderScreen>();

        static ModelCreater btnCreator = new ModelCreater();
        static Logger logger = LogManager.GetCurrentClassLogger();
        static public string SelectedEmployeeName, SelectedEmployeeName2;

        static public AddOnScr AddonScr = new AddOnScr();
        static public MapScreen MapScr = new MapScreen();
        static public priceScreen priceScr = new priceScreen();
        //        static public CallerInformation callerInfoScr = new CallerInformation();
        #region priceScreen
        public static void ShowPriceScreen()
        {
            priceScr.ShowPriceScreen();
            
        }

        #endregion

        #region CallerInforScreen

        #endregion

        #region NumberScreen
        public static void CreateNewNumberScreen(OrderDetail target)
        {



            ProductScreen numberScreen = new ProductScreen();
            numberScreen.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ProductScreen_PreviewMouseDown);

            //CreateNewNumberScreen(310, 310, 50);
            for (int i = 1; i < 30; i++)
            {
                numberScreen.panelAddr1.Children.Add(btnCreator.btnNumber(310, 310, 50, i.ToString()));
            }
            numberScreen.ShowDialog();

            if (numberScreen.Data == null) target.Quantity = 1;
            //2011-4-2 사용자가 제시간안에 못눌렀을때


            if (numberScreen.Data != "취소")
            {
                target.Quantity = Convert.ToInt32(numberScreen.Data);
            }
            else
            {
                target.Quantity = 1;
            }
            //  return selName;
        }

        static void ProductScreen_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                (sender as ProductScreen).Data = (e.Source as Button).Content.ToString();
                (sender as ProductScreen).Close();
            }

        }

        #endregion

        #region ProductScreen

        public static int CreateNewProductScreen(OrderDetail targetSouce, bool isAdd)
        {
            //mode true = add , false = edit,remove
            ProductScreen productScreen = new ProductScreen();
            productScreen.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(productScreen_PreviewMouseDown);

            foreach (var p in DataManager.ProductListbyCategories(isAdd))
            {
                productScreen.panelAddr1.Children.Add(btnCreator.BtnAddProductInfo(110, 156, 25, p));
            }
            productScreen.ShowDialog();


            if (productScreen.Data == null) return 0;
            //사용자가 타이밍안에 음식을 고르지 못하고 메뉴화면이꺼졌을대 리턴0
            //2011-4-2

            targetSouce.ProductID = DataManager.GetProductID(productScreen.Data);
           if(targetSouce.ProductID != 63) //63 == 취소
            //if (productScreen.Data != "취소")
            {
               
                return 1;
            }
            else
            {
                
                return 0;
            }

            //  return selName;
        }

        static void productScreen_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                var temp = (e.Source as Button).Content.ToString();
                (sender as ProductScreen).Data = temp.Substring(0, temp.IndexOf("\r"));
                (sender as ProductScreen).Close();
            }
        }


        #endregion

        #region AddrFindScreen

        public static void CreateNewAddrGuScreen(OrderScreen target)
        {
            ButtonSelectScreen addrGuScr = new ButtonSelectScreen();
            addrGuScr.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(address_PreviewMouseDown);
            foreach (var p in DataManager.AddrGetGuList())
            {
                addrGuScr.panelAddr.Children.Add(btnCreator.btnAddAddrGu(250, 250, 70, p)); // false is Gu mode
            }
            addrGuScr.ShowDialog();
            if (addrGuScr.Data != "취소")
            {
                target.OrderData.Customer.AddrGugun = addrGuScr.Data;
            }

        }
        public static void CreateNewAddrGuScreen(OrderDetailScreen target)
        {
            ButtonSelectScreen addrGuScr = new ButtonSelectScreen();
            addrGuScr.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(address_PreviewMouseDown);
            foreach (var p in DataManager.AddrGetGuList())
            {
                addrGuScr.panelAddr.Children.Add(btnCreator.btnAddAddrGu(250, 250, 70, p)); // false is Gu mode
            }
            addrGuScr.ShowDialog();
            if (addrGuScr.Data != "취소")
            {
                target.OrderData.Customer.AddrGugun = addrGuScr.Data;
            }

        }
        public static void CreateNewAddrDongScreen(OrderScreen target)
        {
            ButtonSelectScreen addrDongScr = new ButtonSelectScreen();

            foreach (var p in DataManager.AddressGetListByGu(target.OrderData.Customer.AddrGugun))
            {

                addrDongScr.panelAddr.Children.Add(btnCreator.btnAddAddrDong(140, 210, 40, p));
            }
            addrDongScr.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(address_PreviewMouseDown);
            addrDongScr.ShowDialog();
            if (addrDongScr.Data != "취소")
            {
                target.OrderData.Customer.AddrDong = addrDongScr.Data;
            }

        }
        public static void CreateNewAddrDongScreen(OrderDetailScreen target)
        {
            ButtonSelectScreen addrDongScr = new ButtonSelectScreen();

            foreach (var p in DataManager.AddressGetListByGu(target.OrderData.Customer.AddrGugun))
            {

                addrDongScr.panelAddr.Children.Add(btnCreator.btnAddAddrDong(140, 210, 40, p));
            }
            addrDongScr.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(address_PreviewMouseDown);
            addrDongScr.ShowDialog();
            
            if (addrDongScr.Data != "취소")
            {
                target.OrderData.Customer.AddrDong = addrDongScr.Data;
            }

        }
        static void address_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (e.Source is Button)
            {
                (sender as ButtonSelectScreen).Data = (e.Source as Button).Content.ToString();
                
                (sender as ButtonSelectScreen).Close();
            }
        }

        #endregion

        #region StockScreen
        public static void CreateNewStockScreen()
        {

            StockScreen stockScreen = new StockScreen();
            stockScreen.Show();
        }

        #endregion

        #region OrderScreen
        public static void CreateNewOrderScreen2(Order newOrder)
        {

            OrderScreen orderScreen = new OrderScreen(newOrder);
            orderScreen.Closed += new EventHandler(orderScreen_Closed);

            SetOrderScreenTime(); 
            
            SetOrderScreenPosition(orderScreen);
            
            orderScreen.Show();
            UpdateMainUI();

           // FileManager.AddNumberToFile(newOrder.Customer.PNumberID);
        }
        public static void CreateNewOrderScreen3(Order newOrder,short phoneLineNo,bool isExistOrder)
        {
            //this version made for PhoneLineNumber
            OrderScreen orderScreen = new OrderScreen(newOrder,phoneLineNo,isExistOrder);
            orderScreen.Closed += new EventHandler(orderScreen_Closed);

            SetOrderScreenTime();
            
            SetOrderScreenPosition(orderScreen);

            orderScreen.Show();
            UpdateMainUI();

            // FileManager.AddNumberToFile(newOrder.Customer.PNumberID);
        }

        static void orderScreen_Closed(object sender, EventArgs e)
        {
          //  FileManager.RemoveNumberFromFile((sender as OrderScreen).OrderData.Customer.PNumberID);
            ListOrderScreen.Remove(sender as OrderScreen);
            RePositionOrderScreen();
            UpdateMainUI();
            
        }

        #region OrderScreenEtc

        private static void SetOrderScreenPosition(OrderScreen screen)
        {
            ListOrderScreen.Add(screen);

            int value = ListOrderScreen.Count % 2;

            screen.Top = ScreenSlot[value].top;
            screen.Left = ScreenSlot[value].left;
        }
        private static void SetOrderScreenTime()
        {
            foreach (var p in ListOrderScreen)
            {
                p.timer.Interval += Config.Envoriment.OrderScreenTimeStay;
            }
        }

        internal static void RePositionOrderScreen()
        {
            if (ListOrderScreen.Count > 0)
            {
                ListOrderScreen[0].Activate();
            }
        }

        internal static int OrderScreenCount()
        {
            return ListOrderScreen.Count;
        }
        #endregion
        #endregion

        #region OrderDetail screen

        public static void CreateNewOrderDetailScreen(int orderID)
        {

            MainWindow mainScreen = Application.Current.MainWindow as MainWindow;

            // ods.gridCustomer.ItemsSource = dataManager.CustomerGetInfoViaOrderModel();
            if (mainScreen.screenBoard.Children.Count > 0)
            {

                Order orderData = DataManager.GetOrderInfo(orderID); //데이터 매니저로 부터 order info get to ID
                if (orderData != null)
                {
                    OrderDetailScreen ods = new OrderDetailScreen(orderData);


                    ods.Show();
                }
                else
                {
                    
                    logger.Debug("Fail Get OrderData");
                }
            }
        }

        #endregion

        #region AttactStocktoProduct
        public static void CreateNewProductSourceScreen()
        {

            AttactStcokwithProduct ProductSourceScreen = new AttactStcokwithProduct();
            ProductSourceScreen.listProduct.ItemsSource = DataManager.ProductList();
            ProductSourceScreen.listStock.ItemsSource = DataManager.StockList();

            ProductSourceScreen.Show();
        }


        #endregion

        #region MainUI update
        public static void UpdateMainUI()
        {

            MainWindow mainScreen = Application.Current.MainWindow as MainWindow;
            if (mainScreen != null)
            {
                List<Order> temp = DataManager.GetExistOrder();
                
                mainScreen.btnBeforeOrderM.Content = "<";
                mainScreen.btnBeforeOrderP.Content = ">";
                mainScreen.OrderIndexTime = 1;

                mainScreen.screenBoard.Children.Clear();
                foreach (Order order in temp)
                {
                    //if (mainScreen.screenBoard.Children.Count > 4 &&  order.Date <= DateTime.Now.AddHours(-1) ) //.AddHours(-1))
                    //{
                    //    order.Finish = true;
                    //    order.ProcEmployee = 12;
                    //    DataManager.Submit(); //2010-7-17 바쁠때 밀리면 한시간지난건 임시처리
                    //}
                    mainScreen.screenBoard.Children.Add(btnCreator.btnOrderInfo(order));
                }
                //오더 정보 창
                mainScreen.orderSlotPanel.Children.Clear();
                foreach (OrderScreen ods in ListOrderScreen)
                {
                    mainScreen.orderSlotPanel.Children.Add(btnCreator.btnOrderScreenInfoInTaskBar(ods));
                }
            }
        }

        public static void UpdateMainUIBeforeData(int time)
        {
            MainWindow mainScreen = Application.Current.MainWindow as MainWindow;
            
            mainScreen.screenBoard.Children.Clear();

            var beforeDataQuery = DataManager.GetBeforeOrderTimeSlaceInfo(time);

            foreach (var p in beforeDataQuery)
            {
                ButtonInfo temp = btnCreator.btnOrderInfo(p as Order);
                if( temp != null)     mainScreen.screenBoard.Children.Add(btnCreator.btnOrderInfo(p as Order));
            }
        }


        internal static void CreateNewCalculateScreen()
        {
            CalculateScreen calScreen = new CalculateScreen();
            calScreen.Show();
        }

        #endregion

      
    }
}