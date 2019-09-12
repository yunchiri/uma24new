
namespace UMA24.Model
{
    struct PhoneCall
    {
        public string _phoneNumber;
       public short _phoneLineNo;

        public PhoneCall(string phoneNumber, byte phoneLineNo)
        {
            _phoneLineNo = phoneLineNo;
            _phoneNumber = phoneNumber;
        }
    }

    
}
