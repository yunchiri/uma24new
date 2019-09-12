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
using UMA24.Model;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for CalculateScreen.xaml
    /// </summary>
    public partial class CalculateScreen : Window
    {
       
        uma24DataContext db;
   
        int[] time = new int[24];
        public CalculateScreen()
        {
            InitializeComponent();


            Init();
        
            
        }

        void Init()
        {
            for(int i =0 ; i<24 ;i++) time[i] = i+1;
            
            db = new uma24DataContext();
            this.CalulateGrid.DataContext = db.Orders;
            this.comboBox1.ItemsSource = db.Employees.Where(p=>p.type != null).Select(p => p.EmployeeName);
            this.cmbTimeA.ItemsSource = time;  
            this.cmbTimeA.SelectedIndex = DateTime.Now.Hour;
            
            this.cmbTimeB.ItemsSource = time;    
            this.cmbTimeB.SelectedIndex = DateTime.Now.Hour;
            this.dateBefore.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            this.dateAfter.Text = DateTime.Now.ToShortDateString();
            
        }
  

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {

            this.CalulateGrid.ItemsSource = null; //초기화

            if (comboBox1.SelectedIndex > -1)
            {
                txtTotalSum.Text = Querying(MakeQuery());
                btnCalc.Content = "계산";
            }
            else
            {
                btnCalc.Content = "이름선택";
               
            }

        }

        private double dayConvert(string target)
        {
            return  Convert.ToDouble(target);
        }

        private  CalcModel MakeQuery()
        {
            CalcModel calcQuery = new CalcModel();

            int employeeId = db.Employees
                .Where(p=>p.EmployeeName == this.comboBox1.SelectionBoxItem.ToString())
                .Select(p=>p.ID).Single();
            
            DateTime startTime = dateBefore.SelectedDate.Value.

             AddHours(dayConvert(cmbTimeB.SelectedValue.ToString())).
             AddMinutes(dayConvert(txtTimeBeforeMin.Text));

            DateTime endTime = dateAfter.SelectedDate.Value.

                     AddHours(dayConvert(cmbTimeA.SelectedValue.ToString())).
                AddMinutes(dayConvert(txtTimeAfterMin.Text));



            calcQuery.Init(employeeId ,startTime,endTime);
            return calcQuery;
        }

        private string Querying(CalcModel query)
        {
            List<OrderExtends> orders = new List<OrderExtends>();
           // this.CalulateGrid.ItemsSource =
            List<Order> source;
           
            if (this.comboBox1.SelectedValue.ToString() == "총계")
            {
                         source= db.Orders
               . Where(p=>p.Date >= query.StartDate &&p.Date <= query.EndDate&& p.Finish == true)
               .ToList();
            }
            else
            {
                         source= db.Orders
               . Where(p=>p.Date >= query.StartDate &&p.Date <= query.EndDate && p.ProcEmployee == query.EmployeeID && p.Finish == true)
               .ToList();
            }


        
            foreach( Order p in source)
            {
                int sum = 0;
                foreach(OrderDetail od in p.OrderDetails)
                {
                    sum = sum + (Convert.ToInt32( od.Product.Price) * od.Quantity);
                }
                OrderExtends stockExtends = new OrderExtends(p);
                stockExtends.Account = sum;
                orders.Add(stockExtends);
            }

            this.CalulateGrid.ItemsSource = orders;
            this.txtTotalDelivery.Text = orders.Count().ToString();
            return orders.Sum(p => p.Account).ToString();

      //      dataManager.get
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
