using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using UMA24.Model;

namespace UMA24.Manager
{
   static class PrintManager
    {
      
           public static bool printMode { get; set; }

          
       private static void Print(DocumentPaginator _doc,string _caption,bool isReseption)
       {

           if (isReseption == true || printMode == true)
           {
               PrintDialog diag = new PrintDialog();

               try
               {
                   diag.PrintDocument(_doc, _caption);
               }
               catch (ArgumentNullException)
               {
               }
           }
       }
      
        public static void PrintReseption(string text, string caption)
        {

            Run r = new Run(text);
            Paragraph ph = new Paragraph(r);

            FlowDocument doc = new FlowDocument(ph);
            doc.FontSize = Config.Envoriment.FontSize;
            doc.FontFamily = new System.Windows.Media.FontFamily("맑은 고딕");
           // doc.PageWidth = 105;
          //  doc.PageHeight = 148;
           
            doc.PagePadding = new System.Windows.Thickness(5);
            Print((doc as IDocumentPaginatorSource).DocumentPaginator, caption,true);

        }

        public static void PrintPreInformation(Order _newOrder, string caption)
        {

            string preInformationSring = DateTime.Now.ToShortTimeString() + "<" + _newOrder.PhoneLineNo + ">"
                       + Environment.NewLine + _newOrder.Customer.PNumberID
                       + Environment.NewLine + ">" +  _newOrder.Customer.AddrGugun + " " + _newOrder.Customer.AddrDong
                       + Environment.NewLine + ">" + _newOrder.Customer.AddrExtra;

            Run r = new Run(preInformationSring);
            Paragraph ph = new Paragraph(r);

            FlowDocument doc = new FlowDocument(ph);
            doc.FontSize = Config.Envoriment.FontSize + 5;
            doc.FontFamily = new System.Windows.Media.FontFamily("맑은 고딕");
            doc.PagePadding = new System.Windows.Thickness(5);

            Print((doc as IDocumentPaginatorSource).DocumentPaginator, caption,false);
        
        }
        public static void PrintPreInformation(string text, string caption)
        {


            Run r = new Run(text);
            Paragraph ph = new Paragraph(r);

            FlowDocument doc = new FlowDocument(ph);
            doc.FontSize = Config.Envoriment.FontSize+5;
            doc.FontFamily = new System.Windows.Media.FontFamily("맑은 고딕");
            // doc.PageWidth = 105;
             
            

            doc.PagePadding = new System.Windows.Thickness(5);

            Print((doc as IDocumentPaginatorSource).DocumentPaginator, caption,false);
         
        }

        private static string ThankYouResption(byte? lv)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("-------------당첨쿠폰!!------------");sb.Append(Environment.NewLine);
            sb.Append(UserRaing(lv)); sb.Append(Environment.NewLine);
            
            sb.Append("#다음주문에 이 영수증을 주세요"); sb.Append(Environment.NewLine);
            sb.Append("#한주문에 한개만 사용가능합니다."); sb.Append(Environment.NewLine);
            sb.Append("-----------감사합니다-----------"); sb.Append(Environment.NewLine);
            return sb.ToString();
        }
       
       private static string AppendString(string source)
       {
           return source + Environment.NewLine;
       }

        public static void PrintOrderInfo(Order _order,bool? _isMotel, bool? _isVAT)
        {
          


            int sumPrice = 0;
            int vat = 0;
            //int Count=DataManager.OrderGetCount(_order.CustomerID);
            StringBuilder printData = new StringBuilder();

            //todo 2010-12-15
            if ( DataManager.CalculateRating(_order) == true)
            {
                //printData.Append(ThankYouResption(_order.Customer.LV));
            }

           
            printData.Append("엄마밥상야식 24시간"); printData.Append(Environment.NewLine);
            
            printData.Append("본점'남중구점 : 473-2788"); printData.Append(Environment.NewLine);
            printData.Append("수성점 : 767-2788"); printData.Append(Environment.NewLine);
            //printData.Append(DataManager.CommentGetMent("인사말")); printData.Append(Environment.NewLine);
           
            printData.Append("=====================");            printData.Append(Environment.NewLine);
            printData.Append("주문전화 : "); printData.Append(_order.Customer.PNumberID);            printData.Append(Environment.NewLine);
            printData.Append("주소 : "); printData.Append(_order.Customer.AddrGugun);printData.Append(" "); printData.Append(_order.Customer.AddrDong);            printData.Append(Environment.NewLine);
            printData.Append(_order.Customer.AddrExtra); printData.Append(Environment.NewLine);
            if (_order.OrderExtend != null) { printData.Append("+"); printData.Append(_order.OrderExtend); printData.Append(Environment.NewLine); }
            printData.Append("------------------------------------");            printData.Append(Environment.NewLine);

            if (_isMotel == true)
            {
                sumPrice = PrintChangePrint(printData, _order);
            }
            else
            {
                sumPrice = PrintWithOutChangePrint(printData, _order);
            }

            if (_isVAT == true)
            {
                vat = Convert.ToInt32(sumPrice * 0.1);
                sumPrice = sumPrice + vat;
            }

            printData.Append("------------------------------------"); printData.Append(Environment.NewLine);

            if (_isVAT == true)
            {
                printData.Append("부가세 : "); printData.Append(vat.ToString());
                printData.Append(Environment.NewLine);
            }
            printData.Append("총 가격 : "); printData.Append(sumPrice.ToString());           
            //printData.Append("총 주문횟수 : "); printData.Append(Count.ToString());
            
            //별카운터
            //string userLevel =UserRaing(Count);
            //if (userLevel != null) { printData.Append("        <"); printData.Append(userLevel); printData.Append(">"); }
           
            printData.Append(Environment.NewLine);


            printData.Append("------------------------------------"); printData.Append(Environment.NewLine);
            printData.Append(DataManager.CommentGetMent("정보")); printData.Append(Environment.NewLine);
            printData.Append(DataManager.CommentGetMent("끝말"));

            //finally print data
            PrintReseption(printData.ToString(), "감사");
            //printData.Append("통합본점 : 대구남구 봉덕동 585-7"); printData.Append(Environment.NewLine);
            
            


        }

        private static string UserRaing(byte? _rating)
        {
            string Ment;
            switch (_rating)
            {
                case 5: Ment = "★★★★★ : 25000 할인권"; break;
                case 4: Ment = "★★★★ : 18000 할인권"; break;
                case 3: Ment = "★★★ : 12000 할인권"; break;
                case 2: Ment = "★★ : 5000 할인권"; break;
                case 1: Ment = "★ : 콜라 무료이용권"; break;
                default: Ment = null; break;
            }

        //    return Ment +" : "+ DataManager.RandomProductForLevel(_rating) + "서비스";
            return Ment;
        }

       
        private static int PrintChangePrint(StringBuilder printData, Order ord)
        {
            int sum = 0;
            
            System.Collections.Generic.List<int> money5 = new System.Collections.Generic.List<int>();
           System.Collections.Generic.List<int> money10 = new System.Collections.Generic.List<int>();
             System.Collections.Generic.List<PrintModel> copyOrder = new System.Collections.Generic.List<PrintModel>();
            

          for(int i=0;i<ord.OrderDetails.Count;i++)
          {
             OrderDetail ords= ord.OrderDetails[i];
             copyOrder.Add(new PrintModel
             {
                 Name = ords.Product.ProductName,
                 Quantity = ords.Quantity,
                 Price = Convert.ToInt32(ords.Product.Price)
               
             });
             int tempPrice = copyOrder[i].Price * copyOrder[i].Quantity;
             if (tempPrice >= 10000)
             {
                 money10.Add(i);
             }
             else
            
             
                if (ords.Product.Price >= 5000 && ords.Product.Price < 10000)
                {
                    money5.Add(i);
                }
          }
          if (money10.Count > 0)
          {
              if (copyOrder[money10[0]].Price >= 10000)
              {
                  copyOrder[money10[0]].Price += 1000;
              }
              else
              {
                  copyOrder[money10[0]].Price += 500;
              }
          }
          else if (money5.Count >= 2)
          {
              copyOrder[money5[0]].Price += 500;
              copyOrder[money5[1]].Price += 500;
          }
         

            foreach (var p in copyOrder)
            {
               
                printData.Append(p.Name); printData.Append(" ");
                printData.Append(p.Quantity); printData.Append(" 가격 : ");

                int priceProduct = p.Price * p.Quantity;
                printData.Append(priceProduct.ToString());

                printData.Append(Environment.NewLine);

                //printData.Append(DataManager.CommentGetProductMent(p.Product.ID));
             
                sum += priceProduct;
            }
            return sum;
        }
        private static int PrintWithOutChangePrint(StringBuilder printData, Order ord)
        {
            int sum= 0;
            foreach (var p in ord.OrderDetails)
            {
                int priceProduct = Convert.ToInt32(p.Product.Price) * p.Quantity;

                printData.Append(p.Product.ProductName); printData.Append(" ");
                printData.Append(p.Quantity); printData.Append(" > 가격 : ");
                printData.Append(priceProduct.ToString());

                printData.Append(Environment.NewLine);

                printData.Append(DataManager.CommentGetProductMent(p.Product.ID));

                int sourceCount = p.Product.ProductSourceDetails.Count;
                
                if (sourceCount > 0)
                {
                    
                    int count = 0;
                    foreach (var pDetail in p.Product.ProductSourceDetails)
                    {

                        if (pDetail.Stock.Madeby != null && pDetail.Product != null&&pDetail.Stock.Madeby.Length > 1 )
                        {
                            if (count++ == 0) printData.Append("원산지>");
            
                            printData.Append(pDetail.Stock.StockName);
                            printData.Append("(");
                            printData.Append(pDetail.Stock.Madeby);
                            printData.Append(") ");
                            printData.Append(Environment.NewLine);
                            //if (count + 1 == sourceCount)
                            //{
                            //    if (count == sourceCount) printData.Append(Environment.NewLine);
                            //}
                            //else
                            //{
                            //    printData.Append(Environment.NewLine);
                            //}
                        }
                    }
                    
                   
                    
                    
                    }
                sum += priceProduct;
            }
            return sum;
        }
    }
}
