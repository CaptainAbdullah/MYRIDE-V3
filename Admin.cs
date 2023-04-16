using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace MYRIDE
{
    internal class Admin
    {

        List<Driver> drivers;
        public Admin()
        {
            drivers = new List<Driver>();
        }

        public List<Driver> Drivers { get { return drivers; } }
        //We can still load list of Drivers from DB using DB sytax. That's why not removing List<Driver> drivers as Data Member of Admin Class


        public void addDriver(string name, int age, string gender,string address, string phoneNo, string vType, string vModel, string vLP)
        {
            Driver driver = new Driver();
            driver.Name = name;
            driver.Age = age;
            driver.Gender = gender;
            driver.Address = address;
            driver.PhoneNo = phoneNo;
            driver.Vehicle.Type = vType;
            driver.Vehicle.Model = vModel;
            driver.Vehicle.LicensePlate = vLP;
            //store data in Database
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = $"INSERT INTO driver (name,age,gender,address,phoneNo,vType, vModel,vLicensePlate) VALUES ('{driver.Name}','{driver.Age}','{driver.Gender}','{driver.Address}','{driver.PhoneNo}','{driver.Vehicle.Type}','{driver.Vehicle.Model}','{driver.Vehicle.LicensePlate}')";
            SqlCommand command = new SqlCommand(query,conn);
            int status = command.ExecuteNonQuery();
            if (status == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Driver is added successfully");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Driver is not added");
                Console.ResetColor();
            }
            conn.Close();
        }

        public bool setRating(int id, int rating)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlConnection conn2 = new SqlConnection(connectionString);
            conn.Open();
            string query = $"SELECT * FROM driver WHERE d_id = {id}";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            int ratingCount = 0;
            int prevRating = 0;
            if(reader.Read())
            {
                ratingCount = Convert.ToInt32(reader["ratingCount"]);
                prevRating = Convert.ToInt32(reader["rating"]);
                prevRating = prevRating + rating;
                ratingCount++;
                prevRating = prevRating/ratingCount;
                query = $"UPDATE driver SET ratingCount = {ratingCount}, rating = {prevRating} WHERE d_id = {id}";
                conn2.Open();
                command = new SqlCommand(query, conn2);
                int status = command.ExecuteNonQuery();
                if(status==1)
                {
                    return true;
                }
            }
            conn2.Close();
            conn.Close();
            return false;
        }


        public void updateDriverCurrLocation(int id, string currLoc)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            Location location = new Location();
            location.setLocation(currLoc);
            string query = $"UPDATE driver SET curr_latitude = {location.Latitude}, curr_longitude = {location.Longitude} WHERE d_id = {id}";
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteReader();
        }

        public void removeDriver(int id)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            string query = $"delete from driver where d_id = {id}";
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            int status = command.ExecuteNonQuery();
            if (status == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Record Deleted Successfully");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Record not found");
                Console.ResetColor();
            }
            conn.Close();
        }

        public void updateDriver(int id)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            string query = $"SELECT * FROM driver WHERE d_id = {id}";
            conn.Open() ;
            SqlCommand command = new SqlCommand (query, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int d_id = Convert.ToInt32(reader[0]);
                if(d_id == id)
                {
                    conn.Close() ;
                    Console.WriteLine("------------Driver with ID " + id + " exists------------");
                    Console.Write("Enter Name: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string name = Console.ReadLine();
                    Console.ResetColor();
                    if (name != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET name = '{name}' WHERE d_id = {id}";
                        command = new SqlCommand (query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter Age: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string age = Console.ReadLine();
                    Console.ResetColor();
                    if (age != string.Empty)
                    {
                        int driverAge = Convert.ToInt32(age);
                        conn.Open();
                        query = $"UPDATE driver SET age = '{driverAge}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter Gender: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string gender = Console.ReadLine();
                    Console.ResetColor();
                    if (gender != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET gender = '{gender}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter Address: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string address = Console.ReadLine();
                    Console.ResetColor();
                    if (address != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET address = '{address}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter PhoneNo: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string phoneNo = Console.ReadLine();
                    Console.ResetColor();
                    if (phoneNo != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET phoneNo = '{phoneNo}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter Vehicle Type (Car/Bike/Rickshaw): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleType = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleType != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET vType = '{vehicleType}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();

                    }
                    Console.Write("Enter Vehicle Model: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleModel = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleModel != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET vModel = '{vehicleModel}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.Write("Enter Vehicle License Plate: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleLP = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleLP != string.Empty)
                    {
                        conn.Open();
                        query = $"UPDATE driver SET vLicensePlate = '{vehicleLP}' WHERE d_id = {id}";
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Record Updated!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Record not found");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Record not found");
                Console.ResetColor();
            }
            conn.Close(); 
        }


        public void updateAvailability(int id, char val)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = $"UPDATE driver Set availability = '{val}' where d_id = {id}";
            SqlCommand command = new SqlCommand(query, conn);
            int count = command.ExecuteNonQuery();
            if(count == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Record Updated!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not update Availability at this time");
                Console.ResetColor();
            }    
        }

        public void searchDriver()
        {
            Driver driver = new Driver();

            Console.Write("Enter Driver ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string id = Console.ReadLine();
            Console.ResetColor();
            if (id != string.Empty)
            {
                int ID = Convert.ToInt32(id);
                driver.ID = ID;

            }
            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ResetColor();
            if (name != string.Empty)
            {
                driver.Name = name;
            }
            Console.Write("Enter Age: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string age = Console.ReadLine();
            Console.ResetColor();
            if (age != string.Empty)
            {
                int ageID = Convert.ToInt32(age);
                driver.Age = ageID;

            }
            Console.Write("Enter Gender: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string gender = Console.ReadLine();
            Console.ResetColor();
            if (gender != string.Empty)
            {
               driver.Gender = gender;
            }
            Console.Write("Enter Address: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string address = Console.ReadLine();
            Console.ResetColor();
            if (address != string.Empty)
            {
                driver.Address = address;   
            }
            Console.Write("Enter Vehicle Type (Car/Bike/Rickshaw): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleType = Console.ReadLine();
            Console.ResetColor();
            if (vehicleType != string.Empty)
            {
               driver.Vehicle.Type = vehicleType;

            }
            Console.Write("Enter Vehicle Model: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleModel = Console.ReadLine();
            Console.ResetColor();
            if (vehicleModel != string.Empty)
            {
                driver.Vehicle.Model = vehicleModel;
            }
            Console.Write("Enter Vehicle License Plate: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleLP = Console.ReadLine();
            Console.ResetColor();
            if (vehicleLP != string.Empty)
            {
               driver.Vehicle.LicensePlate = vehicleLP;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Results on Basis of Filters");
            Console.ResetColor();
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = $"SELECT * FROM driver WHERE d_id = {driver.ID} OR name = '{driver.Name}' OR age = {driver.Age} OR address = '{driver.Address}' OR vType = '{driver.Vehicle.Type}' OR vModel = '{driver.Vehicle.Model}' OR vLicensePlate = '{driver.Vehicle.LicensePlate}'";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Name           Age       Gender       V.Type        V.Model        V.License");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.ResetColor();
                Console.WriteLine(reader["name"] + "   " + reader["age"] + "   " + reader["gender"] + "   " + reader["vType"] + "   " + reader["vModel"] + "     " + reader["vLicensePlate"]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.ResetColor();
            }
            conn.Close();

        }

    }
}
