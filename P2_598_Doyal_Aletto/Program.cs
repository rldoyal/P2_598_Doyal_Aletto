using System;
using System.Threading;
using static P2_598_Doyal_Aletto.Aletto;
using static P2_598_Doyal_Aletto.Aletto.Publisher;
using static P2_598_Doyal_Aletto.Bookstore;

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

            Bookstore bs = new Bookstore(1);
            Bookstore bs1 = new Bookstore(2);

            // Start the BookStore Thread

            Thread BookStoreThread = new Thread(new ThreadStart(bs.BookStoreFunc));
            BookStoreThread.Name = "1";
            BookStoreThread.Start();

            Thread BookStoreThread1 = new Thread(new ThreadStart(bs1.BookStoreFunc));
            BookStoreThread1.Name = "2";
            BookStoreThread1.Start();



            // test the bookstore demand thread.
            for (int i = 0; i < 10; i++)
            {

                System.Threading.Thread.Sleep(1500);
            }
            BookStoreThreadRunning = false; // shut off threads

        }

        public static void TestPricingModel()
        {
            DateTime now = new DateTime();
            Publisher p = new Publisher();
            Random rdm = new Random();


            for (int i = 0; i < 20; i++)
            {
                now = DateTime.Now;
                OrderObject o = new OrderObject("sender" + i.ToString(), (Int32)(rdm.NextDouble() * 10000), "receiver" + i.ToString(), (Int32)(rdm.NextDouble() * 200), 100, now);
                double price = Math.Round(p.getModeler().calcPrice(o),2);
                Console.WriteLine("ObjectOrder " + o.getSenderId() + " paid $" + price.ToString() 
                + ". The number of books in the order was " + o.getAmount() + ". The number of orders in the recent orders " +
                "queue was " + p.getModeler().getQueue().Count + ". " +
                "The Publisher has " + p.getBooks().ToString() + " books.\n");
                o.setUnitPrice(price);
                System.Threading.Thread.Sleep((Int32)(rdm.NextDouble() * 1000));
            }

            Console.WriteLine("There are " + p.getModeler().getQueue().Count.ToString() + " in the recent orders queue.\n");

            foreach (OrderObject order in p.getModeler().getQueue())
            {
                Console.WriteLine("Order " + order.getSenderId() + " paid $" + order.getUnitPrice().ToString() + ".\n");
            }
        }

        //Tests for the encoder and decoders
        public static void EncodeDecodeTest()
        {
            DateTime now = new DateTime();
            Publisher p = new Publisher();
            Random rdm = new Random();
            Bookstore bookstore = new Bookstore(1);

            for (int i = 0; i < 20; i++)
            {
                now = DateTime.Now;
                OrderObject o = new OrderObject
                (
                "sender" + i.ToString(), //senderId
                (Int32)(rdm.NextDouble() * 10000), //cardNo
                "receiver" + i.ToString(), //receiverId
                (Int32)(rdm.NextDouble() * 200), //amount
                (Int32)(rdm.NextDouble() * 100), //unitPrice
                now //timestamp
                );
                string str = bookstore.Encoder(o);
                Console.WriteLine("The OrderObject's actual contents were: " + o.getSenderId() + "," + o.getCardNo().ToString() + ","
                + o.getReceiverId() + "," + o.getAmount().ToString() + "," + o.getUnitPrice().ToString() + "," + o.getTimestamp().ToString());
                Console.WriteLine("The string created by the encoder was: " + str);

                o = p.getDecoder().decode(str);
                Console.WriteLine("The Decoder created another OrderObject from the string created by the encoder whose contents were: \n" 
                + o.getSenderId() + "," + o.getCardNo().ToString() + ","
                + o.getReceiverId() + "," + o.getAmount().ToString() + "," 
                + o.getUnitPrice().ToString() + "," + o.getTimestamp().ToString() + "\n");

                System.Threading.Thread.Sleep(50);
            }
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            TestBookStore();

            //TestPricingModel();

            //EncodeDecodeTest();

            //Console.Read();
        }
        
    }
}  
