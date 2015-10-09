using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace videoConnection
{
    class Program
    {
        static void Main()
        {
            StreamReader cReader = new StreamReader("..\\..\\customer.txt");
            StreamReader mReader = new StreamReader("..\\..\\movie.txt");
            StreamReader rReader = new StreamReader("..\\..\\rental.txt");
            DateTime today = DateTime.Today;
            List<string> movie = new List<string>();
            List<string> customer = new List<string>();
            List<string> rental = new List<string>();
            string c = cReader.ReadLine(); 
            while(c != null)
            {
                customer.Add(c);
                c = cReader.ReadLine();
            }
            cReader.Close();
            string m = mReader.ReadLine();
            while(m != null)
            {
                movie.Add(m);
                m = mReader.ReadLine();
            }
            mReader.Close();
            string r = rReader.ReadLine();
            while(r != null)
            {
                rental.Add(r);
                r = rReader.ReadLine();
            }
            rReader.Close();
            Menu(customer, movie, rental);
        }

        static void Menu(List<string> customer, List<string> movie, List<string> rental)
        {
            StreamWriter cWriter = new StreamWriter("..\\..\\customer.txt");
            StreamWriter mWriter = new StreamWriter("..\\..\\movie.txt");
            StreamWriter rWriter = new StreamWriter("..\\..\\rental.txt");
            int cLen = customer.Count;
            int mLen = movie.Count;
            int rLen = rental.Count;
            for (int i = 0; i < cLen; i++)
            {
                cWriter.WriteLine(customer[i]);
            }
            for (int i = 0; i < mLen; i++)
            {
                mWriter.WriteLine(movie[i]);
            }
            for (int i = 0; i < rLen; i++)
            {
                rWriter.WriteLine(rental[i]);
            }
            cWriter.Close();
            mWriter.Close();
            rWriter.Close();
            Console.WriteLine("MAIN MENU\nWrite a number and press ENTER.\n");
            Console.WriteLine("1. Check out movie");
            Console.WriteLine("2. Return movie");
            Console.WriteLine("3. Overdue list");
            Console.WriteLine("4. Add new customer");
            Console.WriteLine("5. Add new movie");
            Console.WriteLine("6. See all rentals");
            Console.WriteLine("7. See all customers");
            Console.WriteLine("8. See all movies");
            Console.WriteLine();
            string userInput = Console.ReadLine();
            Console.WriteLine();

            if (userInput == "3")
            {
                Overdue(rental);
                Console.WriteLine("\nPress Enter to return to Main Menu");
                Console.ReadLine();
                Console.WriteLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "1")
            {
                rental.Add(CheckOut(customer, movie, rental));
                Console.WriteLine("Successfully checked out.\nPress Enter to return to Main Menu.");
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "2")
            {
                rental.RemoveAt(Returned(rental));
                Console.WriteLine("Press Enter to return to Main Menu.");
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "6")
            {
                int rentalLen = rental.Count;
                for (int i = 0; i < rentalLen; i++)
                {
                    Console.WriteLine(rental[i]);
                }
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "7")
            {
                int customerLen = customer.Count;
                for (int i = 0; i < customerLen; i++)
                {
                    Console.WriteLine(customer[i]);
                }
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "8")
            {
                int movLen = movie.Count;
                for (int i = 0; i < movLen; i++)
                {
                    Console.WriteLine(movie[i]);
                }
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "4")
            {
                customer.Add(AddCustomer());
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else if (userInput == "5")
            {
                movie.Add(AddMovie());
                Console.ReadLine();
                Menu(customer, movie, rental);
            }
            else
            {
                Console.WriteLine("Invalid Entry.\nPress Enter to return to Main Menu.");
                Console.ReadLine();
                Menu(customer, movie, rental);
            }

        }

        static void Overdue (List<string> rental)
        {
            DateTime today = DateTime.Today;
            int len = rental.Count;
            Console.WriteLine("Overdue Rentals:");
            for (int i = 0; i < len; i++)
            {
                string[] parts = rental[i].Split(',');
                string due = parts[2];
                DateTime dueDate = Convert.ToDateTime(due);
                int timeCompare = DateTime.Compare(today, dueDate);
                if(timeCompare > 0)
                {
                    Console.WriteLine(rental[i]);
                }
            }
            return;
        }

        static string CheckOut (List<string> customer, List<string> movie, List<string> rental)
        {
            DateTime today = DateTime.Today;
            int customerLen = customer.Count;
            for (int i = 0; i < customerLen; i++)
            {
                Console.WriteLine(i + 1 + ". " + customer[i]);
            }
            Console.WriteLine("\nCustomer Number:");
            string firstInput = Console.ReadLine();
            int cust = int.Parse(firstInput);
            string customerName = "";
            for (int i = 0; i < customerLen; i++)
            {
                if (cust - 1 == i)
                {
                    customerName = customer[i];
                    break;
                }
            }
            int movieLen = movie.Count;
            for (int i = 0; i < movieLen; i++)
            {
                Console.WriteLine(i + 1 + ". " + movie[i]);
            }
            Console.WriteLine("\nMovie Number:");
            string secondInput = Console.ReadLine();
            int mov = int.Parse(secondInput);
            string movieName = "";
            for (int i = 0; i < movieLen; i++)
            {
                if (mov - 1 == i)
                {
                    movieName = movie[i];
                    break;
                }
            }
            DateTime dueDate = DateTime.Today.AddDays(3);
            Console.WriteLine("Due back by: " + dueDate);
            string checkout = customerName + "," + movieName + "," + dueDate;
            Console.WriteLine(checkout);
            return checkout;
        }

        static int Returned (List<string> rental)
        {
            int len = rental.Count;
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine(i + 1 + ". " + rental[i]);
            }
            Console.WriteLine("\nWrite index number of returned item:");
            string input = Console.ReadLine();
            int mov = int.Parse(input);
            int thisRental = mov - 1;
            Console.WriteLine();
            for (int i = 0; i < len; i++)
            {
                if(thisRental == i)
                {
                    Console.WriteLine("The following rental has been returned:");
                    Console.WriteLine(rental[i]);
                    break;
                }
            }
            return thisRental;
        }

        static string AddCustomer()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Customer Full Name.");
            string name = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Enter Customer Phone Number");
            string phoneNum = Console.ReadLine();
            String addCust = (name + " " + phoneNum);
            Console.WriteLine();
            Console.WriteLine(addCust + " added to customer list.");
            return addCust;
        }

        static string AddMovie()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Movie Title.");
            string title = Console.ReadLine();
            //Console.WriteLine("Enter Customer Phone Number");
            //string phoneNum = Console.ReadLine();
            String addMov = (title);
            Console.WriteLine();
            Console.WriteLine(title + " added to movie list.");
            return title;
        }
    }
}
