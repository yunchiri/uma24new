using System;
using UMA24.Manager;
//using UMA24.Interface;
using iCaller;
using System.Collections.Generic;
using NLog;
using UMA24.Model;
using System.Threading.Tasks;

namespace UMA24.Engine
{
    class IUma24
    {
        iCallerContol icaller;
        MainWindow maunUi;
        private short phoneLineNo;
        // private bool phoneHookState = true;
        Logger logger;
        //List<UMA24.Model.PhoneCall> CallInfoList;
        //Task aa = new Task();
        Action<PhoneCall, bool> NewOrderWindow;

        public IUma24() { }
        public IUma24(MainWindow mainUI)
        {
            this.maunUi = mainUI;
            logger = LogManager.GetCurrentClassLogger();
            InitiCaller();

            NewOrderWindow = new Action<PhoneCall, bool>(NewOrder);
            ScreenManager.UpdateMainUI();
            PrintManager.printMode = true;
            //CallInfoList = new List<Model.PhoneCall>();
        }


        internal void InitiCaller()
        {
            icaller = new iCallerContol();
            icaller.iCallerOpen();
            icaller.PhoneNoReceived += new __iCallerContol_PhoneNoReceivedEventHandler(icaller_PhoneNoReceived);
            icaller.LineNoReceived += new __iCallerContol_LineNoReceivedEventHandler(icaller_LineNoReceived);

        }

        void icaller_LineNoReceived(short LineNo)
        {
            this.phoneLineNo = LineNo;
        }


        void icaller_PhoneNoReceived(string PhoneNo)
        {
            NewOrder(PhoneNo,phoneLineNo, true);
            //CallInfoList.Add(new Model.PhoneCall { _phoneNumber = PhoneNo, _phoneLineNo = phoneLineNo });
            //NewOrderWindow(new Model.PhoneCall { _phoneNumber = PhoneNo, _phoneLineNo = phoneLineNo }, true);
        }
        public void NewOrder(PhoneCall callItem, bool isAutoMode)
        {
            if (DataManager.GetExistOrderPhoneNumber(callItem._phoneNumber) == false)
            {

                //Customer tempCustomer = DataManager.GetCustomerEmergenyCallerInfor(_phoneNo);
                //if (tempCustomer != null)
                //{
                //    ScreenManager.callerInfoScr.ShowUp(tempCustomer);
                //}

                //else
                //{

                Order newOder = OrderManager.CreateNewOrder(callItem._phoneNumber);
                newOder.Date = DateTime.Now;
                newOder.PhoneLineNo = callItem._phoneLineNo;
                // ScreenManager.CreateNewOrderScreen2(newOder);

                ScreenManager.CreateNewOrderScreen3(newOder,callItem._phoneLineNo, DataManager.isExistOrder(callItem._phoneNumber));
                if (isAutoMode) PrintManager.PrintPreInformation(newOder, "preInfo");    //전화왔을때만 프린트
                logger.Trace("NewOrder : " +callItem._phoneNumber + " LineNo : " +callItem._phoneLineNo);//매뉴얼 모드에서는 프린트가 필요없다
                //}
            }
            else
            {
                Employee tempEmployee = DataManager.GetEmployeeCallerInfor(callItem._phoneNumber);

                if (tempEmployee != null)
                {
                  //  ScreenManager.callerInfoScr.ShowUp(tempEmployee);
                }
                //직원의 간단한 소개 담긴 페이지 출력 필요하면 만들고 일단 보류

            }
        }

        public void NewOrder(string _phoneNo, short _phoneLineNo, bool isAutoMode)
        {
            if (DataManager.GetExistOrderPhoneNumber(_phoneNo) == false)
            {

                //Customer tempCustomer = DataManager.GetCustomerEmergenyCallerInfor(_phoneNo);
                //if (tempCustomer != null)
                //{
                //    ScreenManager.callerInfoScr.ShowUp(tempCustomer);
                //}

                //else
                //{

                    Order newOder = OrderManager.CreateNewOrder(_phoneNo);
                    newOder.Date = DateTime.Now;
                    newOder.PhoneLineNo = _phoneLineNo;
                    // ScreenManager.CreateNewOrderScreen2(newOder);

                    ScreenManager.CreateNewOrderScreen3(newOder, _phoneLineNo, DataManager.isExistOrder(_phoneNo));
                    if (isAutoMode) PrintManager.PrintPreInformation(newOder, "preInfo");    //전화왔을때만 프린트
                    logger.Trace("NewOrder : " + _phoneNo + " LineNo : " + _phoneLineNo);//매뉴얼 모드에서는 프린트가 필요없다
                //}
            }
            else
            {

              
                //Employee tempEmployee = DataManager.GetEmployeeCallerInfor(_phoneNo);
               
                //if(tempEmployee != null)
                //{
                //    ScreenManager.callerInfoScr.ShowUp(tempEmployee);
                //}
                //직원의 간단한 소개 담긴 페이지 출력 필요하면 만들고 일단 보류

            }
        }
      
        /// <summary>
        /// mode = auto mode & manual mode
        /// no use any more  2010-12-14
        /// </summary>
        /// <param name="pNumber"></param>
        /// <param name="isautoMode"></param>
        //public void NewOrder(string pNumber, bool isAutoMode)
        //{
          
        //    if ( DataManager.GetExistOrderPhoneNumber(pNumber) == false)
        //    {

        //        Order newOder = OrderManager.CreateNewOrder(pNumber);
        //        newOder.Date = DateTime.Now;
              
        //        // ScreenManager.CreateNewOrderScreen2(newOder);
             
        //        ScreenManager.CreateNewOrderScreen3(newOder, phoneLineNo,   DataManager.isExistOrder(pNumber));
        //        if (isAutoMode) PrintManager.PrintPreInformation(newOder, "preInfo");    //전화왔을때만 프린트
        //        logger.Trace("NewOrder : " + pNumber + " LineNo : " + phoneLineNo);
        //    }
        //    else
        //    {
        //        //매뉴얼 모드에서는 프린트가 필요없다
        //        //직원의 간단한 소개 담긴 페이지 출력 필요하면 만들고 일단 보류
        //    }
           
        //}

        public void QueryStock()
        {
            ScreenManager.CreateNewStockScreen();
        }
        public void Status()
        {

        }
        public void Calculate()
        {
            ScreenManager.CreateNewCalculateScreen();
        }



        public void ProductToSouceMaker()
        {
            ScreenManager.CreateNewProductSourceScreen();
        }

        public void AddOnScreenTurnOn()
        {
            ScreenManager.AddonScr.Show();
        }

        internal void Exit()
        {
            //icaller.iCallerClose();
            

        }

        public void showPrice()
        {
            ScreenManager.ShowPriceScreen();
        }

        public void UpdateUI()
        {
            ScreenManager.UpdateMainUI();
        }

        internal void BeforeOder(int time)
        {
            ScreenManager.UpdateMainUIBeforeData(time);
        }



        internal bool PrintMode()
        {

            PrintManager.printMode = !PrintManager.printMode;
            return PrintManager.printMode;
        }
    }
}
