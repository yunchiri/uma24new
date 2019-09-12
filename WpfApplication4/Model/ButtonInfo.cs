
namespace UMA24.Model
{
    class ButtonInfo :System.Windows.Controls.Button
    {
        public ButtonInfo() { }
        public UMA24.Screen.OrderScreen targetScreen;
        public int ID { get; set; }
    }
}
