using System;
using System.Collections.Generic;
using UMA24.Model;
//using UMA24.Interface;
using System.Collections.ObjectModel;

namespace UMA24.Manager
{
    static class OrderManager
    {

        //오더디테일을 갖는다
        static public OrderDetail GetNewOrderDetail(bool isAddorEdit)
        {
            OrderDetail newOrderDetails = new OrderDetail();
            if (ScreenManager.CreateNewProductScreen(newOrderDetails, isAddorEdit) == 0) return null; //사용자가마음대로끄면 널리턴

            ScreenManager.CreateNewNumberScreen(newOrderDetails);
          
            newOrderDetails.Product = DataManager.GetProduct(newOrderDetails.ProductID);

            //if (newOrderDetails.Quantity == 0 && newOrderDetails.Product.ProductName =="취소") return null;
             return newOrderDetails;
        }

      


        #region ScreenButton

        #region SaveToDB
        //public static void OrderSaveToDB(OrderModel order)
        //{
        //    int id =DataManager.CustomerGetID(order.PhoneNumber);
        //    if (id> 0) 
        //    {
        //        if (order.isFixed == true)
        //        {                   
        //            //ID 저장되어 있고 수정이 되었을때
        //            DataManager.SaveToDBCustomer( DataManager.intoOrderToCustomer(order, id));
        //            DataManager.SaveToDBOrder(DataManager.intoOrderModelToOrder(order));
        //        }
        //        else 
        //        {
        //            // ID 저장되어있고 수정안되어 있다면 고객정보는 수정안해도됨
        //            DataManager.SaveToDBOrder(DataManager.intoOrderModelToOrder(order));
        //        }
        //    }
        //    else if (id == 0)
        //    {
        //        // ID가 저장안되있을때
        //      //  int newID = dataManager.getNewId();
        //        DataManager.SaveToDBNewCustomer(DataManager.intoOrderToNewCustomer(order));
        //        DataManager.SaveToDBOrder(DataManager.intoOrderModelToOrder(order));
        //    }

        //}
        #endregion
        public static Order CreateNewOrder(string phoneNumber)
        {

            Order newOrder = new Order();
            if (phoneNumber.Length > 0)
            {
                int customerID = DataManager.CustomerGetID2(phoneNumber);

                if (customerID > 0)
                {
                    newOrder.Customer = DataManager.CustomerGetInfo(customerID);
                   
                    return newOrder;
                }
                else
                {
                    Customer newCustomer = new Customer { PNumberID = phoneNumber };
                    newOrder.Customer = newCustomer;
                    return newOrder;
                }
            }
            else
            {
                // ""상태로 들어온것 수동주문모드
                Customer newCustomer = new Customer();
                newOrder.Customer = newCustomer;
                return newOrder;
            }

        }

        #endregion


        #region Grid Updaing
 
        public static void UpdateNewOrderDetails2(System.Windows.Controls.DataGrid grid, System.Data.Linq.EntitySet<OrderDetail> targetList)
        {
            int selIndex = grid.SelectedIndex;
            int diffIndex = grid.Items.Count - selIndex;

            if (diffIndex == 1)
            {
                //add item
                OrderDetail newOrderDetail = GetNewOrderDetail(true);

                //2011-4-2 새로추가
                if (newOrderDetail != null)
                {
                    targetList.Add(newOrderDetail);
                    DataManager.Submit();
                }
                grid.SelectedIndex = -1;
            }
            else if (selIndex > -1)
            {
                OrderDetail newOrderDetail = GetNewOrderDetail(false);
                if (newOrderDetail != null)
                {

                    targetList[selIndex] = newOrderDetail;
                    DataManager.Submit();
                }
                else
                {
                    targetList.RemoveAt(selIndex);
                    DataManager.Submit();
                }
            }
         
        }

        public static void UpdateNewOrderDetails3(System.Windows.Controls.DataGrid grid, ObservableCollection<OrderDetail> targetList)
        {
            int selIndex = grid.SelectedIndex;
            int diffIndex = grid.Items.Count - selIndex;

            if (diffIndex == 1)
            {
                //add item
                OrderDetail newOrderDetail = GetNewOrderDetail(true);

                if (newOrderDetail != null )
                {
                    bool findSameProduct = false; //같은 제품있는지

                    foreach (var p in targetList)
                    {
                        if (p.Product == newOrderDetail.Product)
                        {
                            p.Quantity += newOrderDetail.Quantity;
                            findSameProduct = true;
                            //문제를 일으킬수있음
                        }
                    }

                    if (findSameProduct == false)
                    {
                        
                        targetList.Add(newOrderDetail);
                    }
                }

                grid.SelectedIndex = -1;
            }
            else if (selIndex > -1)
            {
                OrderDetail newOrderDetail = GetNewOrderDetail(false);
                if (newOrderDetail != null)
                {

                    targetList[selIndex] = newOrderDetail;
                }
                else
                {
                    targetList.RemoveAt(selIndex);
                }
            }
        }
        public static string OrderPriceUpdate(ObservableCollection<OrderDetail> targetList)
        {
            int sum = 0;
            foreach (var p in targetList)
            {
                sum = sum + (Convert.ToInt32(p.Product.Price) * p.Quantity);
            }
            return sum.ToString();

        }
        #endregion


        internal static void UpdateOrderDetails()
        {
            DataManager.Submit();
          
        }



        internal static void SaveToDBNewOrder(Order OrderData)
        {
            DataManager.SaveToDBNewOrder(OrderData);
            
        }
    }
}

