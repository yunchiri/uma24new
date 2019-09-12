using System.Windows;
using System.Diagnostics;
using UMA24.Screen;
using UMA24.ModelControl;
using UMA24.Engine;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Windows.Threading;




namespace UMA24
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IUma24 uma24engine;
        //Task showTime;
        System.Timers.Timer uiTimer;
       private string timeString; 
        //EXAMPLE
        //ModelCreater EXAMPLE = new ModelCreater();
        private int orderIndexTime;
        public int OrderIndexTime 
        {
            get
            {
                return this.orderIndexTime;
            }
            set
            {              
                    orderIndexTime =  value;
                          }
        }
        
       
        public MainWindow()
        {
            InitializeComponent();
           uma24engine = new IUma24(this);
            OrderIndexTime = 1;
          
            
       
        }

        private void MainScreen_Loaded(object sender, RoutedEventArgs e)
        {

            Process[] myProc = Process.GetProcessesByName("UMA24PR");
            Process curProc = Process.GetCurrentProcess();
            foreach (var p in myProc)
            {
                if (curProc.Id != p.Id) Application.Current.Shutdown();
            }
            StartUITimer();
            uma24engine.AddOnScreenTurnOn();
        }

        #region UITimer
        private void StartUITimer()
        {
            uiTimer = new System.Timers.Timer();
            uiTimer.Interval = 3000;
            uiTimer.Elapsed += new System.Timers.ElapsedEventHandler(uiTimer_Elapsed);
            uiTimer.Start();
        }

        void uiTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timeString = System.DateTime.Now.ToShortTimeString();
            SetTimeDelegate timedelegate = new SetTimeDelegate(SetTime);
            this.txtShowTime.Dispatcher.Invoke(timedelegate, timeString);
        }

        delegate void SetTimeDelegate(string str);
        //private void TimeClock()
        //{
        //    timeString = System.DateTime.Now.ToShortTimeString();
        //    SetTimeDelegate timdd = new SetTimeDelegate(SetTime);
        // //   timdd.Invoke(timeString);
        //    this.txtShowTime.Dispatcher.Invoke(timdd, timeString);
        //}
        private void SetTime(string _setTime)
        {   
            this.txtShowTime.Content = _setTime;
        }

        #endregion

        /// <summary>
        /// 수동모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
         //   dataGrid1.ItemsSource = u24dc.Customers.ToList();
            //uma24engine.NewOrder(new Model.PhoneCall { _phoneNumber = textBox1.Text, _phoneLineNo = 0 }, false);
              uma24engine.NewOrder(textBox1.Text,0,false);  //manual mode new order
            
         
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            uma24engine.Exit();
            this.Close();
        }

      
        /// <summary>
        /// 새로운 주문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            //홀주문
            uma24engine.NewOrder("",0, false); //manual mode new order

        }

        /// <summary>
        /// 재고
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            uma24engine.QueryStock();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            //uma24engine.Status();
           // print p = new print();
          //  p.Show();
        }

        /// <summary>
        ///  정산
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
          
           uma24engine.Calculate();
        }


        private void MainScreen_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var p in Application.Current.Windows)
            {
                if ((p as Window).IsLoaded == true) (p as Window).Close();
            }
            uma24engine.Exit();
            Application.Current.Shutdown();
        }


        #region timeslide for order
        /// <summary>
        /// 이전주문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeforeOrderM_Click(object sender, RoutedEventArgs e)
        {
            OrderIndexTime--;
            if (OrderIndexTime == 0)
            {
                btnBeforeOrderM.Content = "좀전";// +"시간전";
            }
            else
            {
                btnBeforeOrderM.Content = OrderIndexTime.ToString() + "시간";// +"시간전";
            }
           
          btnBeforeOrderP.Content = ">";
          
            uma24engine.BeforeOder(OrderIndexTime);
        }

        /// <summary>
        /// 이후주문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeforeOrderP_Click(object sender, RoutedEventArgs e)
        {
          
            if (OrderIndexTime < 0)
            {
                OrderIndexTime++;

                if (OrderIndexTime == 0)
                {
                    btnBeforeOrderP.Content = "좀전";
                    btnBeforeOrderM.Content = "<";
                }
                else
                {
                    //  uma24engine.UpdateUI();
                    btnBeforeOrderP.Content = OrderIndexTime.ToString() + "시간";
                    btnBeforeOrderM.Content = "<";
                }


                uma24engine.BeforeOder(OrderIndexTime);
            }

            else if (OrderIndexTime == 0)
            {
                OrderIndexTime++;

                btnBeforeOrderM.Content = "<";
                btnBeforeOrderP.Content = ">";
                uma24engine.UpdateUI();
            }
        }

        #endregion


        /// <summary>
        /// 재고 결합
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStockMap_Click(object sender, RoutedEventArgs e)
        {
            uma24engine.ProductToSouceMaker();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            uma24engine.showPrice();
        }

        private void btnPrintMode_Click(object sender, RoutedEventArgs e)
        {
            if (uma24engine.PrintMode() == true)
            {
                this.btnPrintMode.Background = System.Windows.Media.Brushes.Green;
            }
            else
            {
                this.btnPrintMode.Background = System.Windows.Media.Brushes.Yellow;
            }
        }

    }
}
