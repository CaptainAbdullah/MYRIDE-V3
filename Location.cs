using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYRIDE
{
    internal class Location
    {

        float latitude;
        float longitude;

        public void setLocation(float longitude, float latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }
        public void setLocation(string loc)
        {
            string[] words = loc.Split(',');
            latitude = float.Parse(words[0]);
            longitude = float.Parse(words[1]);
        }


        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public float getLatitude()
        {
            return latitude;
        }
        public float getLongitude()
        {
            return longitude;
        }
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
    }
}
