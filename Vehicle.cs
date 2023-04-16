using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYRIDE
{
    internal class Vehicle
    {
        string type;
        string model;
        string licensePlate;

        public Vehicle()
        {
            type = "unknown";
            model = "unknown";
            licensePlate = "unknown";
        }
        public Vehicle(string type, string model, string licensePlate)
        {
            Type = type;
            Model = model;
            LicensePlate = licensePlate;
        }


        ///////////////////////////////////////////Properties/////////////////////////////////////////
        public virtual string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public string LicensePlate
        {
            get { return licensePlate; }
            set { licensePlate = value; }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////
    }
}
