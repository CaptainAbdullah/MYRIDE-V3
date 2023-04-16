using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MYRIDE
{
    internal class Ride : Vehicle
    {
        Location start_location;
        Location end_location;
        int price;
        Driver driver;
        Passenger passenger;
        float fuel_price = 272;

        public Ride() : base()
        {
            start_location = new Location();
            end_location = new Location();
            driver = new Driver();
            passenger = new Passenger();
        }


        ///////////////////////////////////////Properties//////////////////////////////////////////
        public Location StartLocation
        {
            get { return start_location; }
            //set { start_location = value; }
        }
        public Location EndLocation
        {
            get { return end_location; }
            //set { end_location = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public Driver Driver
        {
            get { return driver; }
        }
        public Passenger Passenger
        {
            get { return passenger; }
        }

        public override string Type
        {
            get => base.Type;
            set => base.Type = value;
        }

        public float FuelPrice
        {
            get { return fuel_price; }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////



        public void assignPassenger(Passenger passenger)
        { this.passenger = passenger; }
        public int assignDriver(string sLoc,string rideType)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            string query = $"SELECT * FROM driver WHERE vType = '{rideType}' AND availability = 'Y'";
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            int driverID = -1;
            float distance = 0;
            int counter = 0;
            Location dLoc = new Location();
            while(reader.Read())
            {
                Console.Write(reader["name"]);
                dLoc.Latitude = (float)Convert.ToDouble(reader["curr_latitude"]);
                dLoc.Longitude = (float)Convert.ToDouble(reader["curr_longitude"]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" curr_Location: " + dLoc.Latitude + "," + dLoc.Longitude + "\n");
                Console.ResetColor();
                if (counter == 0)
                {
                    distance = calculateDistance(dLoc, sLoc);
                    driverID = Convert.ToInt32(reader["d_id"]);
                }
                else
                {
                    float tempDistance = calculateDistance(dLoc, sLoc);
                    if(tempDistance < distance)
                    {
                        distance = tempDistance;
                        driverID = Convert.ToInt32(reader["d_id"]);
                    }
                }
                counter++;
            }
            conn.Close();
            query = $"SELECT name FROM driver WHERE d_id = {driverID}";
            conn.Open();
            command = new SqlCommand(query, conn);
            SqlDataReader r =  command.ExecuteReader();
            if (r.Read())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulations your driver " + r["name"]  +" is on its way :)");
                Console.ResetColor();
            }
            conn.Close();
            return driverID;
        }

        public int calculatePrice()
        {
            float distance = calculateDistance();
            if (Type == "Car" || Type == "car")
            {
                price = (int)((distance * fuel_price) / 15);
                float commission = (float)(price * 0.2);
                price += (int)commission;

            }
            else if (Type == "Bike" || Type == "bike")
            {
                price = (int)((distance * fuel_price) / 50);
                float commission = (float)(price * 0.05);
                price += (int)commission;
            }
            else if (Type == "Rickshaw" || Type == "rickshaw")
            {
                price = (int)((distance * fuel_price) / 35);
                float commission = (float)(price * 0.1);
                price += (int)commission;
            }
            return price;

        }

        public void setLocations(string sloc, string eloc)
        {
            start_location.setLocation(sloc);
            end_location.setLocation(eloc);
        }

        /////////////////////////////////////////////////HELPER FUNCTIONS/////////////////////////////////////////////////////////
        public void go(string name, string phNo, string sLoc, string eLoc, string rideType)
        {

            passenger.Name = name;
            passenger.PhoneNo = phNo;
            start_location.setLocation(sLoc);
            end_location.setLocation(eLoc);
            Type = rideType;
        }

        float calculateDistance()
        {
            float lat1 = start_location.getLatitude();
            float lon1 = start_location.getLongitude();

            float lat2 = end_location.getLatitude();
            float lon2 = end_location.getLongitude();

            float a = lon2 - lon1;
            float b = lat2 - lat1;
            a = a * a;
            b = b * b;

            float val = a + b;
            float distance = (float)Math.Sqrt(val);
            return distance;
        }
        float calculateDistance(Location dLoc, string sLoc) //Overloading
        {
            float lat1 = dLoc.getLatitude();
            float lon1 = dLoc.getLongitude();

            Location sLocOriginal = new Location();
            sLocOriginal.setLocation(sLoc);
            float lat2 = sLocOriginal.getLatitude();
            float lon2 = sLocOriginal.getLongitude();

            float a = lon2 - lon1;
            float b = lat2 - lat1;
            a = a * a;
            b = b * b;

            float val = a + b;
            float distance = (float)Math.Sqrt(val);
            return distance;
        }
    }
}
