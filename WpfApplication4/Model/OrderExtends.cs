
namespace UMA24.Model
{
    class OrderExtends : Order
    {
       
        int account;

        public OrderExtends(Order p)
        {
            this.Customer = p.Customer;
            this.CustomerID = p.CustomerID;
            this.Date = p.Date;
           // this.Employee = p.Employee;
            this.Finish = p.Finish;
            this.ID = p.ID;
            this.OrderDetails = p.OrderDetails;
            this.OrderExtend = p.OrderExtend;
            this.ProcEmployee = p.ProcEmployee;
            
        }

public int Account
{
  get { return account; }
  set { account = value; }
}}
}
