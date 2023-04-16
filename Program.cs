using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


namespace MYRIDE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Admin admin = new Admin();
            int selectedOpt = 0;
            while (selectedOpt != 4)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.WriteLine("                                 Main Menu                                  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("                            WELCOME TO MYRIDE                               ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.ResetColor();
                Console.WriteLine("1. Book a Ride");
                Console.WriteLine("2. Enter as Driver");
                Console.WriteLine("3. Enter as Admin");
                Console.WriteLine("4. Exit the App");
                Console.Write("\nPress 1 to 4 to select an option: ");
                Console.ForegroundColor = ConsoleColor.Green;
                selectedOpt = Convert.ToInt32(Console.ReadLine());
                Console.ResetColor();
                //Console.WriteLine(selectedOpt);

                //Input Validation Starts
                while (selectedOpt != 1 && selectedOpt != 2 && selectedOpt != 3 && selectedOpt != 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Option!");
                    Console.ResetColor();
                    Console.Write("Please Press 1 to 4 to select an option: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    selectedOpt = Convert.ToInt32(Console.ReadLine());
                    Console.ResetColor();
                }
                //Input Validation Ends

                if (selectedOpt == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Book a Ride");
                    Console.ResetColor();
                    Console.Write("Enter Name: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string name = Console.ReadLine();
                    Console.ResetColor();
                    Console.Write("Enter Phone no: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string phNo = Console.ReadLine();
                    Console.ResetColor();
                    Console.Write("Enter Start Location: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string sLoc = Console.ReadLine();
                    Console.ResetColor();
                    Console.Write("Enter End Location: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string eLoc = Console.ReadLine();
                    Console.ResetColor();
                    Console.Write("Enter Ride Type(Car/Rickshaw/Bike): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string rideType = Console.ReadLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("--------------------------------THANK YOU-----------------------------------");
                    Console.ResetColor();

                    Ride myRide = new Ride();
                    myRide.go(name, phNo, sLoc, eLoc, rideType);
                    int price = myRide.calculatePrice();
                    Console.WriteLine("Price for this ride is: " + price);

                    Console.Write("Enter ‘Y’ if you want to Book the ride, enter ‘N’ if you want to cancel operation: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string choice = Console.ReadLine();
                    Console.ResetColor();
                    if (choice == "Y" || choice == "y")
                    {
                        int id = myRide.assignDriver(sLoc, rideType);
                        if (id == -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Sorry No Driver Available!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\n                             Happy Travel...!                           ");
                            Console.ResetColor();
                            Console.Write("\nGive rating out of 5: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string r = Console.ReadLine();
                            Console.ResetColor();
                            int rating = Convert.ToInt32(r);
                            Console.WriteLine(admin.setRating(id, rating)); //We can Add Data to Rides Table While Setting 'Rating' ... bcz rating from user means that Ride is Successful!
                        }
                    }

                }
                else if (selectedOpt == 2)
                {
                    Console.Write("Enter ID: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    int id = Convert.ToInt32(Console.ReadLine());
                    Console.ResetColor();
                    Console.Write("Enter Name: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string name = Console.ReadLine();
                    Console.ResetColor();
                    string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=myRideDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    string query = $"SELECT * FROM driver WHERE d_id = {id}";
                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    int driverID = 1;
                    string driverName = "";
                    if (reader.Read())
                    {
                        driverID = Convert.ToInt32(reader["d_id"]);
                        driverName = Convert.ToString(reader["name"]);
                    }
                    if (driverID == id && driverName == name)
                    {
                        Console.WriteLine("Hello " + driverName + "!");
                        conn.Close();
                        Console.Write("\nEnter your Current Location: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        string loc = Console.ReadLine();
                        Console.ResetColor();
                        admin.updateDriverCurrLocation(id, loc);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Location updated!");
                        Console.ResetColor();
                        int driverChoice = 0;
                        while (driverChoice != 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("----------------------------------------------------------------------------");
                            Console.ResetColor();
                            Console.WriteLine("1. Change Availability");
                            Console.WriteLine("2. Change Location");
                            Console.WriteLine("3. Exit as Driver");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("----------------------------------------------------------------------------");
                            Console.ResetColor();
                            Console.Write("Press 1 to 3 to select an option: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            driverChoice = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();
                            //Input Validation Starts
                            while (driverChoice != 1 && driverChoice != 2 && driverChoice != 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid Option!");
                                Console.ResetColor();
                                Console.Write("Please Press 1 to 3 to select an option: ");
                                driverChoice = Convert.ToInt32(Console.ReadLine());
                            }
                            //Input Validation Ends
                            if (driverChoice == 1)
                            {
                                Console.Write("Are you available (Y/N): ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                string availability = Console.ReadLine();
                                Console.ResetColor();
                                if (availability == "Y" || availability == "y")
                                {
                                    admin.updateAvailability(id, 'Y');
                                }
                                else
                                    admin.updateAvailability(id, 'N');
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Availability Updated!");
                                Console.ResetColor();
                            }
                            else if (driverChoice == 2)
                            {
                                Console.Write("\nEnter your Location: ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                loc = Console.ReadLine();
                                Console.ResetColor();
                                admin.updateDriverCurrLocation(id, loc);
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("Location updated!");
                                Console.ResetColor();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Driver with ID " + id + " is not registered with us!");
                        Console.ResetColor();
                    }
                }
                else if (selectedOpt == 3)
                {
                    int choice = 0;
                    while (choice != 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("----------------------------------------------------------------------------");
                        Console.ResetColor();
                        Console.WriteLine("1. Add Driver");
                        Console.WriteLine("2. Remove Driver");
                        Console.WriteLine("3. Update Driver");
                        Console.WriteLine("4. Search Driver");
                        Console.WriteLine("5. Exit as Admin");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("----------------------------------------------------------------------------");
                        Console.ResetColor();

                        Console.Write("Press 1 to 5 to select an option: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        choice = Convert.ToInt32(Console.ReadLine());
                        Console.ResetColor();
                        //Input Validation Starts
                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Option!");
                            Console.ResetColor();
                            Console.Write("Please Press 1 to 5 to select an option: ");
                            choice = Convert.ToInt32(Console.ReadLine());
                        }
                        //Input Validation Ends

                        if (choice == 1) //AddDriver
                        {
                            Console.Write("Enter Name: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string name = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter Age: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string age = Console.ReadLine();
                            Console.ResetColor();
                            int driverAge = Convert.ToInt32(age);
                            Console.Write("Enter Gender: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string gender = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter Address: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string address = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter PhoneNo: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string phoneNo = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter Vehicle Type (Car/Bike/Rickshaw): ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string vehicleType = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter Vehicle Model: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string vehicleModel = Console.ReadLine();
                            Console.ResetColor();
                            Console.Write("Enter Vehicle License Plate: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string vehicleLP = Console.ReadLine();
                            Console.ResetColor();
                            admin.addDriver(name, driverAge, gender, address, phoneNo ,vehicleType, vehicleModel, vehicleLP);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.ResetColor();
                        }
                        else if (choice == 2) //RemoveDriver
                        {
                            Console.WriteLine("Enter the Driver ID: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string ID = Console.ReadLine();
                            Console.ResetColor();
                            int id = Convert.ToInt32(ID);
                            admin.removeDriver(id);
                        }
                        else if (choice == 3) //updateDriver
                        {
                            Console.Write("Enter Driver ID: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            string id = Console.ReadLine();
                            Console.ResetColor();
                            int ID = Convert.ToInt32(id);
                            admin.updateDriver(ID);
                        }
                        else if (choice == 4) //Search Driver
                        {
                            admin.searchDriver();
                            Console.WriteLine("\n");
                        }
                        else
                        {
                            //Display MainMenu
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------THANK YOU FOR USING MYRIDE------------------------");
                    Console.ResetColor();
                }
            }
        }










    }
}

