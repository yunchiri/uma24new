
namespace UMA24.Model
{
    class ScreenPositionModel
    {
      //  public int no { get; private set; }
       public  double left { get;private           set; }
        public double top{ get;private set;}
        public bool canUse = true;
      
        public ScreenPositionModel() { }

        public ScreenPositionModel(double left, double top)
        {
            this.left = left;
            this.top = top;
        }
    }
}
