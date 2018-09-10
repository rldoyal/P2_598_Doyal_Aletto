using System;
using System.Threading;
using static P2_598_Doyal_Aletto.Aletto.Publisher;

namespace P2_598_Doyal_Aletto
{

    class GlobalVariables
    {
        const Int32 MaxBookQuanity = 500;  // max number of books allowed in the store
        //static Int32 StoreInventory = 250;  // initial book inventory set at 500.  Max Books = 500.
        //bool NeedDemandThread = true; // variable to start/stop demand thread
        static double  CurrentPrice = 200;

        public double getCurrentPrice()
        {
            return CurrentPrice;
        }
        public void setCurrentPrice( Int32 price)
        {
            CurrentPrice = price;
            Console.WriteLine("\t\t\t Reseting Price = {0}", price);
        }
    }
    class Program
    {
        public static GlobalVariables GV = new GlobalVariables();
        public static bool BookStoreThreadRunning = true;
        public static void TestBookStore()
        {
            Random rnd = new Random();

            Bookstore bs = new Bookstore();
            // Start the BookStore Thread

            Thread BookStoreThread = new Thread(new ThreadStart(bs.BookStoreFunc));
            BookStoreThread.Name = "1";
            BookStoreThread.Start();

            // test the bookstore demand thread.
            for (int i = 0; i < 10; i++)
            {
                GV.setCurrentPrice(rnd.Next(50, 200));
               // bs.myDemand.addStoreInventory(rnd.Next(100, 500));
                Console.WriteLine(" i={0} : inventory = {1}", i, bs.myDemand.getStoreInventory());
                System.Threading.Thread.Sleep(1500);
            }
            BookStoreThreadRunning = false;
            bs.myDemand.TurnDemandOff();
        }

        public static void TestPricingModel()
        {
            DateTime now = new DateTime();
            PricingModel p = new PricingModel();
            Random rdm = new Random();


            for (int i = 0; i < 5; i++)
            {
                now = DateTime.Now;
                OrderObject o = new OrderObject("sender" + i.ToString(), (Int32)rdm.NextDouble() * 10000, "receiver" + i.ToString(), 100, 100, now);
                double price = p.calcPrice(o);
                Console.WriteLine("ObjectOrder " + o.getSenderId() + "paid $" + price.ToString() + ".\n");
                o.setUnitPrice(price);
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("There are " + p.orders.Count.ToString() + " in the recent orders queue.\n");

            foreach (OrderObject order in p.orders)
            {
                Console.WriteLine("Order " + order.getSenderId() + " paid $" + order.getUnitPrice().ToString() + ".\n");
            }
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            TestBookStore();

            TestPricingModel();

            Console.Read();
        }
        
    }
}  
