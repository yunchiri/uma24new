using System;
using System.Collections.Generic;
using System.Linq;
using UMA24.Model;
//using UMA24.Interface;
using System.Windows.Media;
using NLog;
namespace UMA24.Manager
{
    static class DataManager
    {
        static uma24DataContext dataSource = new uma24DataContext();
       static Logger logger = LogManager.GetCurrentClassLogger();
       
        //구의 리스트 뽑아오기
        #region AddressInfo


        public static List<string> AddrGetGuList()
        {
            //popular == null => deleted same same
            return dataSource.Addresses.Where(p => p.Popular != null).Select(n => n.GUGUN).Distinct().ToList();
        }
        //구의 동네 뽑아오기
        public static List<Address> AddressGetListByGu(string gu)
        {

            if (gu != null)
            {
                return dataSource.Addresses.Where(p => p.GUGUN == gu).OrderByDescending(a => a.Popular.Value).ToList();
            }
            else
            {
                return dataSource.Addresses.Where(p => p.Popular >= Config.Envoriment.AddressScopeLevel).ToList();

            }
        }

        internal static string AddrGetGubyDong(string p)
        {
            return dataSource.Addresses.Where(c => c.DONG == p).Select(c => c.GUGUN).Single();
        }

        internal static int AddrGetID(string dong)
        {
            return dataSource.Addresses.Where(p => p.DONG == dong).Select(p => p.ID).Single();
        }



        #endregion

        #region CustomerInfo

        internal static Customer CustomerGetInfo(int customerID)
        {
            return dataSource.Customers.Single(p => p.ID == customerID);
        }

        public static int CustomerGetID2(string phoneNumber)
        {
            if (dataSource.Customers.Where(p => p.PNumberID == phoneNumber).ToList().Count > 0)
            {
                return dataSource.Customers.Where(p => p.PNumberID == phoneNumber).Select(p => p.ID).Single();
            }
            else
            {
                return 0;
            }
        }


        #endregion


        #region ProductInfo

        internal static int GetProductID(string pName)
        {
            return dataSource.Products.Where(p => p.ProductName == pName).Select(p => p.ID).Single();
        }

        internal static Product GetProduct(int p)
        {
            return dataSource.Products.Single(q => q.ID == p);
        }


        public static List<Product> ProductListbyCategories(bool isAdd)
        {
            if (DateTime.Now.Hour > 10 && DateTime.Now.Hour < 15)
            {
                isAdd = false;
            }
            if (isAdd)
            {
                return dataSource.Products.Where(

p => p.Price != -1 &&
    p.ProductName.Contains("취소") != true &&
                    p.ProductName.Contains("부대") != true &&
    //p.ProductName.Contains("홀") != true &&
                    p.ProductName.Contains("점심") != true)
                    .OrderBy(p => p.Categories).ThenBy(p => p.Price).ToList();
            }
            else
            {
                return dataSource.Products.Where(p=>p.Price != -1).OrderBy(p => p.Categories).ThenBy(p => p.Price).ToList();
            }

        }
        public static List<string> ProductList()
        {
            return dataSource.Products.Select(p => p.ProductName).ToList();
        }

        public static string RandomProductForLevel(byte? lv)
        {
              decimal startPrice = 0;
            decimal endPrice = 0;

            switch (lv)
            {
                case 1: startPrice = 0; endPrice = 1000; break;
                case 2: startPrice = 2000; endPrice = 5000; break;
                case 3: startPrice = 10000; endPrice = 13000; break;
                case 4: startPrice = 13000; endPrice = 17000; break;
                case 5: startPrice = 15000; endPrice = 25000; break;
                default: break;
            }
          

            var PriceItemList= dataSource.Products.Where(p => p.Price > startPrice && p.Price <= endPrice).ToList();
            Random ranDom = new Random(DateTime.Now.Second);
            string result;

            try
            {
                result = PriceItemList[ranDom.Next(0, PriceItemList.Count)].ProductName;
            }
            catch (ArgumentOutOfRangeException)
            {
                logger.Debug("random error");
                result = PriceItemList[ranDom.Next(0, PriceItemList.Count-1)].ProductName;
            }
            return result;
        }
        #endregion

        #region ColorInfo
        public static Brush AddrColor(Address address)
        {
            //return Brushes.BlanchedAlmond;
            var color = dataSource.Addresses.Where(p => p.DONG == address.DONG && p.GUGUN == address.GUGUN);
            if (color.Count() > 0)
            {

                switch (color.Single().Color)
                {
                    //1~4 south  5~8 east 9 ~ 12 north  13 ~16 west
                    case "1": return Brushes.LightCoral;
                    case "2": return Brushes.Tomato;
                    case "3": return Brushes.OrangeRed;
                    case "4": return Brushes.Crimson;
                    case "5": return Brushes.LightGreen;
                    case "6": return Brushes.LimeGreen;
                    case "7": return Brushes.MediumSeaGreen;
                    case "8": return Brushes.SeaGreen;
                    case "9": return Brushes.LightBlue;
                    case "10": return Brushes.SkyBlue;
                    case "11": return Brushes.CornflowerBlue;
                    case "12": return Brushes.RoyalBlue;
                    case "13": return Brushes.LightGoldenrodYellow;
                    case "14": return Brushes.PaleGoldenrod;
                    case "15": return Brushes.Gold;
                    case "16": return Brushes.Goldenrod;
                    case "17": return Brushes.Honeydew;
                    case "18": return Brushes.Cornsilk;
                    case "19": return Brushes.OldLace;
                    case "20": return Brushes.Beige;
                    case "21": return Brushes.Moccasin;
                    case "22": return Brushes.Peru;
                    case "23": return Brushes.SandyBrown;
                    case "24": return Brushes.Chocolate;
                    case "25": return Brushes.LavenderBlush;
                    case "26": return Brushes.Thistle;
                    case "27": return Brushes.Violet;
                    case "28": return Brushes.Orchid;
                    case "29": return Brushes.LightPink;
                    case "30": return Brushes.Pink;
                    case "31": return Brushes.HotPink;
                    case "32": return Brushes.DeepPink;

                    default: return Brushes.PowderBlue;
                }

            }
            return Brushes.AliceBlue;

        }
        public static Brush AddrColor(Customer customer)
        {
            //return Brushes.BlanchedAlmond;
            if (customer == null) return Brushes.AliceBlue;
            var color = dataSource.Addresses.Where(p => p.DONG == customer.AddrDong && p.GUGUN == customer.AddrGugun);
            if (color.Count() > 0)
            {

                switch (color.Single().Color)
                {
                    //1~4 south  5~8 east 9 ~ 12 north  13 ~16 west
                    case "1": return Brushes.LightCoral;
                    case "2": return Brushes.Tomato;
                    case "3": return Brushes.OrangeRed;
                    case "4": return Brushes.Crimson;
                    case "5": return Brushes.LightGreen;
                    case "6": return Brushes.LimeGreen;
                    case "7": return Brushes.MediumSeaGreen;
                    case "8": return Brushes.SeaGreen;
                    case "9": return Brushes.LightBlue;
                    case "10": return Brushes.SkyBlue;
                    case "11": return Brushes.CornflowerBlue;
                    case "12": return Brushes.RoyalBlue;
                    case "13": return Brushes.LightGoldenrodYellow;
                    case "14": return Brushes.PaleGoldenrod;
                    case "15": return Brushes.Gold;
                    case "16": return Brushes.Goldenrod;
                    case "17": return Brushes.Honeydew;
                    case "18": return Brushes.Cornsilk;
                    case "19": return Brushes.OldLace;
                    case "20": return Brushes.Beige;
                    case "21": return Brushes.Moccasin;
                    case "22": return Brushes.Peru;
                    case "23": return Brushes.SandyBrown;
                    case "24": return Brushes.Chocolate;
                    case "25": return Brushes.LavenderBlush;
                    case "26": return Brushes.Thistle;
                    case "27": return Brushes.Violet;
                    case "28": return Brushes.Orchid;
                    case "29": return Brushes.LightPink;
                    case "30": return Brushes.Pink;
                    case "31": return Brushes.HotPink;
                    case "32": return Brushes.DeepPink;

                    default: return Brushes.PowderBlue;
                }

            }
            return Brushes.SteelBlue;

        }
        #endregion


        #region SAVE DB
        internal static void SaveToDBNewOrder(Order OrderData)
        {

            SaveToDBCustomer(OrderData.Customer);
            SaveToDBOrder(OrderData);
          


        }

        internal static void SaveToDBCustomer(Customer customer)
        {
            if (customer.ID < 0) dataSource.Customers.InsertOnSubmit(customer);
        }

        /// <summary>
        /// order.finish  true => succes  false => no  null => 그릇배달
        /// </summary>
        /// <param name="order"></param>
        internal static void SaveToDBOrder(Order order)
        {
            CheckOrder(order);
            
            if (order.OrderDetails.Count > 0)
            {
                order.Finish = false;
                if (order.ID > 0)
                {
                    Order tarGerOrder = dataSource.Orders.Single(p => p.ID == order.ID);
                    tarGerOrder = order;      
                }
                else
                {   
                    dataSource.Orders.InsertOnSubmit(order);                   
                }


                try
                {
                    dataSource.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Make some adjustments.
                    // ...
                    // Try again.
                    dataSource.SubmitChanges();
                }
            }
            // dataSource.SubmitChanges();
        }
        internal static void SaveToDBOrderFinish(Order order, string employee)
        {
            if (order.Finish != true)
            {
                foreach (var p in order.OrderDetails)
                {
                    foreach (var ps in p.Product.ProductSourceDetails)
                    {
                        ps.Stock.Qualtity = ps.Stock.Qualtity - (ps.Quantity * p.Quantity);
                        //재고수량 = 재고수량  - (재료양 * 주문 갯수)
                    }
                    order.Customer.UseMoney += (int)p.Product.Price;
                    //2011-01-14일 추가
                }
                order.Finish = true;
            }
            order.ProcEmployee = dataSource.Employees.Where(p => p.EmployeeName == employee).Select(p => p.ID).Single();
            SaveToDBUpdateAddress(order.CustomerID); //주소업데이트
            dataSource.SubmitChanges();
        }

        internal static void CheckOrder(Order _order)
        {
            List<OrderDetail> item = new List<OrderDetail>();
            foreach (var p in _order.OrderDetails)
            {
                if (p.Quantity == 0) { item.Add(p); }
            }

            if (item.Count > 0)
            {
                foreach (var p in item)
                {
                    _order.OrderDetails.Remove(p);
                }
            }
        }
        public static bool CalculateRating(Order order)
        {
            if (order.ProcEmployee == null || 
                order.Finish !=true ||
                order.Customer == null ||
                order.Customer.AddrExtra == null ||
               // order.Customer.AddrExtra.Contains("모텔") || 
                order.Customer.AddrExtra.Contains("그릇"))
            {
                return false;
            }
            #region initCalculate
            //초기화 다시시키기
            if (order.Customer.LV == null)
            {
                order.Customer.LV = 0;
            }

            if (order.Customer.UseMoney == null)
            {
                //이때까지 먹은 음식 다 계산한다
                decimal sum = 0;

                try
                {
                    // 모든 가격을 더하는 쿼리
                    sum = dataSource.Orders.Where(p => p.CustomerID == order.CustomerID).Sum(p2 => p2.OrderDetails.Sum(p4 => p4.Product.Price * p4.Quantity));
                    order.Customer.UseMoney = Convert.ToInt32(sum); //todo if change money type to int type 한다면 이것 고체야함
                }
                catch (ArgumentNullException)
                {
                    //주문이 없었을때 혹은 다 지워졌을때
                    logger.Debug("Faile Calculate UsedMoney...");
                }
            }
            else
            {
                //오덜 디테일을 합산
                if (order.ProcEmployee == null || order.Finish != true)
                {
                    decimal sumMoney = order.OrderDetails.Sum(p => p.Product.Price * p.Quantity);
                    order.Customer.UseMoney += Convert.ToInt32(sumMoney);
                }
            }
            #endregion
            ///레벨 5이상은 다른 방식으로 적용한다.
            if (order.Customer.LV >= 5)
            {
                return false;                //아직 뭘 줘야될지 모름
            }
            else
            {
                if (GetRatingLevelMoney(order.Customer.LV) <= order.Customer.UseMoney)
                {
                    
                        order.Customer.LV++;
                        try
                        {
                            dataSource.SubmitChanges();
                            return true;
                        }
                        catch (Exception)
                        {
                            logger.Debug("Faile submit level");
                            return false;
                        }
                    
                    //to print lv1
                }
                return false;
            }


        }

        public static int GetRatingLevelMoney(byte? lv)
        {
            switch (lv)
            {
                
                case 4: return Config.Envoriment.lv5;
                case 3: return Config.Envoriment.lv4;
                case 2: return Config.Envoriment.lv3;
                case 1: return Config.Envoriment.lv2;
                case 0: return Config.Envoriment.lv1;
                default: return 1000000;
            }
        }
        internal static int GetRating(int p)
        {

       //     int itemCount = OrderGetCount(p);
           byte? itemCount;

           try
           {
               itemCount = dataSource.Customers.Where(f => f.ID == p).Single().LV;
               return (int)itemCount;
           }
           catch (ArgumentNullException)
           {
               return 0;
           }
           catch (InvalidOperationException)
           {
               return 0;
           }
            
            //if (itemCount >= Config.Envoriment.lv5)
            //{
            //    return 5;
            //}
            //else if (itemCount >= Config.Envoriment.lv4)
            //{
            //    return 4;
            //}
            //else if (itemCount >= Config.Envoriment.lv3)
            //{
            //    return 3;
            //}
            //else if (itemCount >= Config.Envoriment.lv2)
            //{
            //    return 2;
            //}
            //else if (itemCount >= Config.Envoriment.lv1)
            //{
            //    return 1;
            //}
            //else
            //{
            //    return 0;
            //}


        }
        internal static void SaveToDBUpdateAddress(int id)
        {
            var address = dataSource.Customers.Where(p => p.ID == id).Single();
            if (address.AddrDong != null && !address.AddrGugun.Contains("가게") && !address.AddrGugun.Contains("부대"))
            {
                try
                {
                    dataSource.Addresses.Single(c => c.DONG == address.AddrDong && c.GUGUN == address.AddrGugun).Popular++;
                }
                catch (ArgumentNullException)
                {
                    logger.Debug("invalied Address : " + address.AddrGugun + " | " + address.AddrDong);
                }
                catch (InvalidOperationException)
                {
                    logger.Debug("DB address error : " + address.AddrGugun + " | " + address.AddrDong);
                }
                

            }
        }


        #endregion

        #region OrderInfo
        internal static int OrderGetCount(int p)
        {
            return dataSource.Orders.Where(q => q.CustomerID == p).Count();
        }

        internal static List<Order> GetExistOrder()
        {
            return dataSource.Orders.Where(f => f.Finish == false).OrderBy(f => f.Date).ToList();
        }
        internal static List<string> GetExistOrderPhoneNumber()
        {
            return dataSource.Orders.Where(f => f.Finish == false).Select(f => f.Customer.PNumberID).ToList();
        }


        internal static System.Collections.IEnumerable EmployeeGet()
        {
            return dataSource.Employees.Where(p => p.type =="배달").Select(p => p.EmployeeName).ToList();
        }

        internal static List<string> EmployeePhonNumber()
        {
            return dataSource.Employees.Select(p => p.PhoneNumber).ToList();
        }

        internal static Order GetOrderInfo(int id)
        {
            Order returnData;
            try
            {
                 returnData = dataSource.Orders.Single(p => p.ID == id);
            }
            catch (ArgumentNullException)
            {
                returnData = null;
                logger.Debug("wrong getOrderInfo ID : " + id);
            }
            catch (InvalidOperationException)
            {
                returnData = null;
                logger.Debug("wrong getOrderInfo ID : " + id);
            }
            return returnData;
        }

        internal static System.Collections.IEnumerable GetBeforeOrderTimeSlaceInfo(int hour)
        {
            return dataSource.Orders.Where(p => p.Date >= DateTime.Now.AddHours(hour - 1) && p.Date <= DateTime.Now.AddHours(hour));
        }

        internal static void DeleteOrder(Order order)
        {
           
                foreach (var p in order.OrderDetails)
                {
                    foreach (var ps in p.Product.ProductSourceDetails)
                    {
                        ps.Stock.Qualtity = ps.Stock.Qualtity + (ps.Quantity * p.Quantity);
                        //재고수량 = 재고수량  - (재료양 * 주문 갯수)
                    }
                }
                order.Finish = null;
           
            // order.OrderDetails.cou
            dataSource.OrderDetails.DeleteAllOnSubmit(order.OrderDetails);
            dataSource.Orders.DeleteOnSubmit(order);
            dataSource.SubmitChanges();
        }

        internal static void DeleteOrderFromOrderScreen(Order order)
        {
            if (order.ID > 0)
            {
                dataSource.Orders.DeleteOnSubmit(order);
                
                try
                {
                    dataSource.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Make some adjustments.
                    // ...
                    // Try again.
                    dataSource.SubmitChanges();
                }
                
            }
        }
        internal static System.Collections.IEnumerable OrderGetFomerData(int customerID)
        {

            return dataSource.OrderDetails.Where(p => p.Order.CustomerID == customerID && p.Product.Price >= 5000 && p.Order.Finish == true).OrderByDescending(p => p.Order.Date).ToList();
        }

        #endregion

        #region Stock & ProductSources
        internal static List<String> StockList()
        {
            return dataSource.Stocks.Select(p => p.StockName).ToList();
        }

        internal static List<ProductSourceDetail> ProductSourceGet(string stockName)
        {

            return dataSource.ProductSourceDetails.Where(p => p.Stock.StockName == stockName).ToList();

        }

        internal static int GetStockID(string sName)
        {
            return dataSource.Stocks.Where(p => p.StockName == sName).Select(p => p.ID).Single();
        }
        #endregion



        #region Comment
        internal static string CommentGetMent(string _type)
        {
            var headMent = dataSource.Comments.Where(p => p.Type == _type).ToList();
            Random random = new Random(DateTime.Now.Second);
            if (headMent != null)
            {
                int selectHeadMent = random.Next(headMent.Count - 1);

                int select = random.Next(1, 3);

                switch (select)
                {
                    case 1: return headMent[selectHeadMent].Comment1;
                    case 2: return headMent[selectHeadMent].Comment2;
                    case 3: return headMent[selectHeadMent].Comment3;
                    default: return headMent[selectHeadMent].Comment1;

                }
            }
            return "감사합니다";

        }
        internal static string CommentGetProductMent(int id)
        {
            var headMent = dataSource.Comments.Where(p => p.ProductID == id).ToList();
            Random random = new Random();
            if (headMent.Count > 0)
            {
                int selectHeadMent = random.Next(headMent.Count - 1);
                int select = random.Next(1, 3);

                switch (select)
                {
                    case 1: return ">" + headMent[selectHeadMent].Comment1 + Environment.NewLine;
                    case 2: return ">" + headMent[selectHeadMent].Comment2 + Environment.NewLine;
                    case 3: return ">" + headMent[selectHeadMent].Comment3 + Environment.NewLine;
                    default: return ">" + headMent[selectHeadMent].Comment1 + Environment.NewLine;

                }
            }
            return null;

        }

        #endregion



        internal static void Submit()
        {
            dataSource.SubmitChanges();
        }

        //internal static void RemoveOrderDetails(Order orderData)
        //{
        //    dataSource.OrderDetails.DeleteAllOnSubmit(orderData.OrderDetails);
        //    DataManager.Submit();
        //}

        //internal static void SaveToDBOrderDetails(OrderDetail orderDetail)
        //{
        //    dataSource.OrderDetails.InsertOnSubmit(orderDetail);
        //}

        internal static string EmployeeGetName(int? empID)
        {
            return dataSource.Employees.Where(p => p.ID == empID).Select(p => p.EmployeeName).Single();
        }

        internal static bool isExistOrder(string pNumber)
        {
          
            try
            {
             dataSource.Orders.Where(f => f.Finish == false && f.Customer.PNumberID == pNumber).Single();
                return true;
            }
            catch(ArgumentNullException	)//source is null.
            {
                return false;
            }
            catch(InvalidOperationException) //source is over than one
            {
                return false;
            }
        }

        /// <summary>
        /// 전화왔을때 직원이거나 독촉전화
        /// </summary>
        /// <param name="pNumber"></param>
        /// <returns></returns>
        internal static Employee GetEmployeeCallerInfor(string pNumber)
        {
            try
            {
                return dataSource.Employees.Where(p => p.PhoneNumber == pNumber).Single();
            }
            catch (ArgumentNullException)//source is null.
            {
                
                return dataSource.Employees.Where(p => p.PhoneNumber == pNumber).First();
            }
            catch (InvalidOperationException) //source is over than one
            {
                return null;
            }
        }
        internal static Customer GetCustomerEmergenyCallerInfor(string pNumber)
        {
            try
            {
                return dataSource.Orders.Where(f => f.Finish == true
                    && f.Date > DateTime.Now.AddMinutes(-35)
                && f.Customer.PNumberID == pNumber).Select(p=>p.Customer).Single();
                
            }
            catch (ArgumentNullException)//source is null.
            {
                return null;
            }
            catch (InvalidOperationException) //source is over than one
            {
                return null;
            }
        }


        internal static bool GetExistOrderPhoneNumber(string pNumber)
        {
            bool emp, ord;
            try
            {
               dataSource.Employees.Where(p=>p.PhoneNumber == pNumber ).Single();
               //emp = true;
               return true; //2011-2-14
            }
            catch (ArgumentNullException)//source is null.
            {
                emp= false;
            }
            catch (InvalidOperationException) //source is over than one
            {
                emp= false;
            }

            try
            {
                if (dataSource.Orders.Where(f => f.Finish == false && f.Customer.PNumberID == pNumber).ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false; //같으넌화번호과 있다
                }
            }
            catch (ArgumentNullException)//source is null.
            {
                ord= false;
            }
            catch (InvalidOperationException) //source is over than one
            {
                ord= false;
            }


            if (emp == ord) { return false; } else { return true; }
        }



        internal static string GetRatingString(byte? _rating)
        {
            switch (_rating)
            {
                case 5: return " ★★★★★";
                case 4: return " ★★★★";
                case 3: return " ★★★";
                case 2: return " ★★";
                case 1: return " ★";
                default: return null;
            }
        }

        internal static int OrderGetFomerDataCount(int customerID)
        {
            return dataSource.Orders.Where(p => p.CustomerID == customerID && p.Finish == true).Count();
            //return dataSource.OrderDetails.Where(p => p.Order.CustomerID == customerID && p.Product.Price >= 5000 && p.Order.Finish == true).Count();
        }
    }
}
