using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYRIDE
{
    internal class Passenger
    {
        string name;
        string phoneNo;

        public Passenger()
        {
            name = string.Empty;
            phoneNo = string.Empty;
        }
        public Passenger(string n, string phNo)
        {
            name = n;
            phoneNo = phNo;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PhoneNo
        {
            get { return phoneNo; }
            set { phoneNo = value; }
        }
    }
}
