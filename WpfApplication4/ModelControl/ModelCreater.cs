using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using UMA24.Model;
using System.Windows.Media;
using UMA24.Manager;
using UMA24.Screen;
using System.Collections.Generic;

namespace UMA24.ModelControl
{
    class ModelCreater
    {
        #region btnOrderInfos 

        public ButtonInfo btnOrderInfo(Order content)
        {
            if (content.Finish != null)
            {
                ButtonInfo orderInfoButton = new ButtonInfo();
                orderInfoButton.ID = content.ID;
                orderInfoButton.Name = "ID" + content.ID.ToString();
                orderInfoButton.Height = 430;
                orderInfoButton.Width = 315;
                orderInfoButton.FontSize = 30;
                orderInfoButton.Background = DataManager.AddrColor(content.Customer);
                orderInfoButton.FontWeight = System.Windows.FontWeights.Heavy;
                orderInfoButton.Content = StringOrderInfo(content);

                orderInfoButton.Click += new System.Windows.RoutedEventHandler(orderInfoButton_Click);
                return orderInfoButton;
            }
            else
            {
                return null;
            }
        }

        void orderInfoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
                 var source = sender as ButtonInfo;
               //  source.BorderBrush = Brushes.Red;
                 ScreenManager.CreateNewOrderDetailScreen(source.ID);
       //오더 디테일 생성
           
        
        }
        #endregion

        internal  Model.ButtonInfo btnOrderScreenInfoInTaskBar(OrderScreen screen)
        {
             string id = screen.OrderData.Customer.PNumberID;
             string date = screen.OrderData.Date.ToShortTimeString();
            ButtonInfo orderScreenButton = new ButtonInfo();
            orderScreenButton.Name = "ID" + id;
            orderScreenButton.Height = 60;
            orderScreenButton.Width = 315;
          orderScreenButton.Background = DataManager.AddrColor(screen.OrderData.Customer);
            if(id != null)            orderScreenButton.Content =date+"<"+screen.OrderData.PhoneLineNo.ToString() +">" + id.ToString();
            orderScreenButton.FontSize = 25;
            orderScreenButton.FontWeight = System.Windows.FontWeights.Heavy;
            orderScreenButton.targetScreen = screen;
            orderScreenButton.Click +=new System.Windows.RoutedEventHandler(orderScreenButton_Click);
            return orderScreenButton;


        }

        void orderScreenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //occur new problem
            var source = sender as ButtonInfo;     
            source.targetScreen.Focus();
        }

        //main ui order buttnon content
        private StringBuilder StringOrderInfo(Order content)
        {
            StringBuilder sb = new StringBuilder();
            if (content.Finish == true) { sb.Append("처리자 : "); sb.Append(DataManager.EmployeeGetName( content.ProcEmployee) ); sb.Append(Environment.NewLine); }
            sb.Append(content.Date.ToShortTimeString()); sb.Append(DataManager.GetRatingString(content.Customer.LV)); sb.Append(Environment.NewLine);//시간

            if (content.Customer.PNumberID.Length > 3) { sb.Append(content.Customer.PNumberID); sb.Append("<"); sb.Append(content.PhoneLineNo); sb.Append(">"); sb.Append(Environment.NewLine); }//홀일때는 전화번호 없음

            sb.Append(content.Customer.AddrGugun + " ");//구군

            if (content.Customer.AddrGugun != "홀") { sb.Append(content.Customer.AddrDong); sb.Append(Environment.NewLine); } //홀이 아닐때 동표시

            if (content.Customer.AddrExtra != null && content.Customer.AddrExtra.Length >0)            {                sb.Append(content.Customer.AddrExtra); sb.Append(Environment.NewLine);            } //확장주소 있을대
      
            if (content.OrderExtend != null  && content.OrderExtend.Length > 0) { sb.Append(content.OrderExtend); sb.Append(Environment.NewLine); } //추가말
            foreach (OrderDetail od in content.OrderDetails) // 오더 디테일
            {
                sb.Append(od.Product.ProductName + " ");
                sb.Append(od.Quantity);                
                sb.Append(Environment.NewLine);
            }
            sb.Append("총가격 : "); sb.Append(Convert.ToInt32(content.OrderDetails.Sum(p => p.Product.Price * p.Quantity)).ToString()); //총가격
      
            return sb;
        }


        public Button btnAddAddrDong(double height, double width, double fontsize,Address content)
        {
            Button btnSerchAddrDong = new Button();
            btnSerchAddrDong.Height = height;
            btnSerchAddrDong.Width = width;
            btnSerchAddrDong.FontSize = fontsize;
            btnSerchAddrDong.Content = content.DONG;
            btnSerchAddrDong.Background = DataManager.AddrColor(content);
            return btnSerchAddrDong;

        }
        public Button btnAddAddrGu(double height, double width, double fontsize, string content)
        {
            Button btnSerchAddrGu = new Button();
            btnSerchAddrGu.Height = height;
            btnSerchAddrGu.Width = width;
            btnSerchAddrGu.FontSize = fontsize;        
            btnSerchAddrGu.Content = content;
          
     
            btnSerchAddrGu.Background = ButtonColorSelector(content);
            return btnSerchAddrGu;
        }
        public Button btnNumber(double height, double width, double fontsize, string content)
        {
            Button btnNumberSerch = new Button();
            btnNumberSerch.Height = height;
            btnNumberSerch.Width = width;
            btnNumberSerch.FontSize = fontsize;
            btnNumberSerch.Content = content;
          //  btnNumberSerch.Click += new System.Windows.RoutedEventHandler(btnNumberSerch_Click);
            return btnNumberSerch;
        }

     

        public SolidColorBrush ButtonColorSelector(string selectData)
        {
            switch (selectData)
            {
                case "남구":                   return Brushes.Gold;
                case "수성구":                   return Brushes.CornflowerBlue; 
                case "중구":                   return Brushes.OrangeRed;
                case "달서구":                   return Brushes.Yellow;
                case "서구":                   return Brushes.OliveDrab; 
                case "북구":                   return Brushes.Pink; 
                case "동구":                   return Brushes.DodgerBlue; 
                case "홀주문":                   return Brushes.YellowGreen;

                default:                   return Brushes.Salmon;                    
            }
        }
        public Button BtnAddProductInfo(double height, double width, double fontsize,Product  content)
    {
        Button btnProductAdd = new Button();
        btnProductAdd.Height = height;
        btnProductAdd.Width = width;
        btnProductAdd.FontSize = fontsize;
        btnProductAdd.FontWeight = System.Windows.FontWeights.Heavy;
        btnProductAdd.Content = content.ProductName + Environment.NewLine +Convert.ToInt32( content.Price);
        switch (content.Categories.Substring(2))
        {
            case "식사": btnProductAdd.Background = Brushes.Gold; break;
            case "찌개": btnProductAdd.Background = Brushes.CornflowerBlue; break;
            case "탕": btnProductAdd.Background = Brushes.OrangeRed; break;
            case "덮밥": btnProductAdd.Background = Brushes.Yellow; break;
            case "죽": btnProductAdd.Background = Brushes.OliveDrab; break;
            case "찜": btnProductAdd.Background = Brushes.Pink; break;
            case "안주": btnProductAdd.Background = Brushes.DodgerBlue; break;
            case "기타": btnProductAdd.Background = Brushes.YellowGreen; break;
            case "취소": btnProductAdd.Background = Brushes.Sienna; break;
            case "구이": btnProductAdd.Background = Brushes.Tomato; break;
            case "식사탕": btnProductAdd.Background = Brushes.YellowGreen; break;
            case "생선찌개": btnProductAdd.Background = Brushes.Goldenrod; break;
            case "닭": btnProductAdd.Background = Brushes.Honeydew; break;
            case "돼지": btnProductAdd.Background = Brushes.Magenta; break;
            case "옵션": btnProductAdd.Background = Brushes.Silver; break;
            default: btnProductAdd.Background = Brushes.Salmon; break;
        }
        return btnProductAdd;
    }
        //public Button btnAddProduct(double height, double width, double fontsize, ProductCategoriesModel content)
        //{
            
        //    Button btnProductAdd = new Button();
        //    btnProductAdd.Height = height;
        //    btnProductAdd.Width = width;
        //    btnProductAdd.FontSize = fontsize;
        //    btnProductAdd.FontWeight = System.Windows.FontWeights.Heavy;
        //    btnProductAdd.Content = content.Name + Environment.NewLine + content.Price;
        //    //btnProductAdd.Click += new System.Windows.RoutedEventHandler(btnSerchAddr_Click);
        //    //btnProductAdd.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(btnSerchAddr_PreviewTouchDown);
        //    ////카테고리별 색깔
        //    switch (content.Categories.Substring(1))
        //    {
        //        case "식사":                    btnProductAdd.Background = Brushes.Gold; break;
        //        case "찌개":                    btnProductAdd.Background = Brushes.CornflowerBlue; break;
        //        case "탕":                    btnProductAdd.Background = Brushes.OrangeRed; break;
        //        case "덮밥":                    btnProductAdd.Background = Brushes.Yellow; break;
        //        case "죽":                    btnProductAdd.Background = Brushes.OliveDrab; break;
        //        case "찜":                    btnProductAdd.Background = Brushes.Pink; break;
        //        case "안주":                    btnProductAdd.Background = Brushes.DodgerBlue; break;
        //        case "기타":                    btnProductAdd.Background = Brushes.YellowGreen; break;
        //        case "취소" :                     btnProductAdd.Background = Brushes.Sienna; break;
        //        default:                    btnProductAdd.Background = Brushes.Salmon; break;
        //    }
        //    return btnProductAdd;
        //}
    }
}
