using System.Windows;
using System;

namespace UMA24.Screen
{
    /// <summary>
    /// Interaction logic for CallerInformation.xaml
    /// </summary>
    public partial class CallerInformation : Window
    {
        System.Timers.Timer closeTImer;
        Action closeAction;

        public CallerInformation()
        {
            InitializeComponent();
            Init();
            
        }

        private void Init()
        {
            closeTImer = new System.Timers.Timer();
            closeTImer.Interval = 4000;
            closeTImer.AutoReset = true;
            closeTImer.Elapsed += new System.Timers.ElapsedEventHandler(closeTImer_Elapsed);

            closeAction = new Action(AutoHide);
        }

        private void closeTImer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(closeAction, null);
        }

        private void AutoHide()
        {
            this.Hide();
            //lb_CallNumber = null;
            //lb_Content = null;
           // lb_userName.Content = null;
             //Action showMethod = () => testName.DisplayToWindow();
            closeTImer.Stop();
        }
        


        internal void ShowUp(Customer tempCustomer)
        {
            lb_CallNumber.Content = tempCustomer.PNumberID;
            lb_Content.Content = "빨리오라는 전화";
            lb_userName.Content = tempCustomer.AddrDong + " " + tempCustomer.AddrExtra;
            closeTImer.Start();
            this.ShowDialog();
            this.Activate();
        }

        internal void ShowUp(Employee tempEmployee)
        {

            lb_CallNumber.Content = tempEmployee.PhoneNumber;
            lb_Content.Content = "아마 영수증 잃어버린듯";
            lb_userName.Content = tempEmployee.EmployeeName;

            closeTImer.Start();
            this.ShowDialog();
            this.Activate();
        }

        private void UserInfo_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AutoHide();
        }
    }
}
