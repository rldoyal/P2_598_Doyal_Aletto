using System;
using System.Threading;
using static P2_598_Doyal_Aletto.Publisher;
using static P2_598_Doyal_Aletto.Bookstore;

namespace P2_598_Doyal_Aletto
{

    class GlobalVariables
    {
        const Int32 MaxBookQuanity = 500;  // max number of books allowed in the store
        //static Int32 StoreInventory = 250;  // initial book inventory set at 500.  Max Books = 500.
        //bool NeedDemandThread = true; // variable to start/stop demand thread
        static double pub1Price = 0;
        static double pub2Price = 0;
        ReaderWriterLockSlim pubLock1 = new ReaderWriterLockSlim();
        ReaderWriterLockSlim pubLock2 = new ReaderWriterLockSlim();

        //Returns price of publisher 1
        public double get_Pub1_Price()
        {
            pubLock1.EnterReadLock();
            try
            {
                return pub1Price;
            }
            finally
            {
                pubLock1.ExitReadLock();
            }
        }

        //Sets price of publisher 1
        public void set_Pub1_Price(double price)
        {
            pubLock1.EnterWriteLock();
            try
            {
                pub1Price = price;
            }
            finally
            {
                pubLock1.ExitWriteLock();
            }
        }

        //Returns price of publisher 2
        public double get_Pub2_Price()
        {
            pubLock2.EnterReadLock();
            try
            {
                return pub2Price;
            }
            finally
            {
                pubLock2.ExitReadLock();
            }
        }

        //Sets price of publisher 2
        public void set_Pub2_Price(double price)
        {
            pubLock2.EnterWriteLock();
            try
            {
                pub2Price = price;
            }
            finally
            {
                pubLock2.ExitWriteLock();
            }
        }
    }


    class Program
    {
        public static GlobalVariables GV = new GlobalVariables();
        public static bool BookStoreThreadRunning = true;
        public static MultiCellBuffer mcb;
        public static void TestBookStore()
 
        {
            Random rnd = new Random();

            

            Bookstore bs = new Bookstore(1);
            Bookstore bs1 = new Bookstore(2);

            // set initial book prices
            GV.set_Pub1_Price(rnd.NextDouble() * (200-20)+20);
            GV.set_Pub2_Price(rnd.NextDouble() * (200 - 20) + 20);


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
                GV.set_Pub1_Price(rnd.NextDouble() * (200 - 20) + 20);
                GV.set_Pub2_Price(rnd.NextDouble() * (200 - 20) + 20);
               
                System.Threading.Thread.Sleep(1500);

                String p1s = mcb.getOneCell(1);
                Console.WriteLine("\t\t\t\t\t getoneCell(1) : {0}", p1s);
                p1s = mcb.getOneCell(1);
                Console.WriteLine("\t\t\t\t\t getoneCell(1) : {0}", p1s);
                String p2s = mcb.getOneCell(2);
                Console.WriteLine("\t\t\t\t\t getoneCell(2) : {0}", p2s);
                p2s = mcb.getOneCell(2);
                Console.WriteLine("\t\t\t\t\t getoneCell(2) : {0}", p2s);


            }
            BookStoreThreadRunning = false; // shut off threads

        }

        public static void TestPricingModel()
        {
            DateTime now = new DateTime();
            Publisher p = new Publisher(1);
            Random rdm = new Random();


            for (int i = 0; i < 20; i++)
            {
                now = DateTime.Now;
                OrderObject o = new OrderObject(i, (Int32)(rdm.NextDouble() * 10000), i, (Int32)(rdm.NextDouble() * 200), 100, now, DateTime.Now.Millisecond);
                double price = Math.Round(p.getModeler().calcPrice(o),2);
                Console.WriteLine("ObjectOrder " + o.getBookStoreId() + " paid $" + price.ToString() 
                + ". The number of books in the order was " + o.getAmount() + ". The number of orders in the recent orders " +
                "queue was " + p.getModeler().getQueue().Count + ". " +
                "The Publisher has " + p.getBooks().ToString() + " books.\n");
                o.setUnitPrice(price);
                System.Threading.Thread.Sleep((Int32)(rdm.NextDouble() * 1000));
            }

            Console.WriteLine("There are " + p.getModeler().getQueue().Count.ToString() + " in the recent orders queue.\n");

            foreach (OrderObject order in p.getModeler().getQueue())
            {
                Console.WriteLine("Order " + order.getBookStoreId() + " paid $" + order.getUnitPrice().ToString() + ".\n");
            }
        }

        //Tests for the encoder and decoders
        public static void EncodeDecodeTest()
        {
            DateTime now = new DateTime();
            Publisher p = new Publisher( 1);
            Random rdm = new Random();
            Bookstore bookstore = new Bookstore(1);

            for (int i = 0; i < 20; i++)
            {
                now = DateTime.Now;
                OrderObject o = new OrderObject
                (
                i, //senderId
                (Int32)(rdm.NextDouble() * 10000), //cardNo
                i, //receiverId
                (Int32)(rdm.NextDouble() * 200), //amount
                (Int32)(rdm.NextDouble() * 100), //unitPrice
                now, //timestamp,
                DateTime.Now.Millisecond
                );
                string str = bookstore.Encoder(o);
                Console.WriteLine("The OrderObject's actual contents were: " + o.getBookStoreId()+"," + o.getCardNo().ToString() + ","
                + o.getPublisherId() + "," + o.getAmount().ToString() + "," + o.getUnitPrice().ToString() + "," + o.getTimestamp().ToString());
                Console.WriteLine("The string created by the encoder was: " + str);

                o = p.getDecoder().decode(str);
                Console.WriteLine("The Decoder created another OrderObject from the string created by the encoder whose contents were: \n" 
                + o.getBookStoreId() + "," + o.getCardNo().ToString() + ","
                + o.getPublisherId() + "," + o.getAmount().ToString() + "," 
                + o.getUnitPrice().ToString() + "," + o.getTimestamp().ToString() + "\n");

                System.Threading.Thread.Sleep(50);
            }
        }


        static void Main(string[] args)
        {

            const Int32 NumStores = 5;


            // create the multi cell buffer
             mcb = new MultiCellBuffer(3);

            Publisher publisher1 = new Publisher(1);
            Publisher publisher2 = new Publisher(2);

            
            // start the publisher threads
            Thread P1 = new Thread(new ThreadStart(publisher1.runPublisher));
            Thread P2 = new Thread(new ThreadStart(publisher2.runPublisher));
            P1.Start();
            P2.Start();

           

            Thread[] retailStores = new Thread[NumStores];
            for (int i=0; i < NumStores; i++)
            {
                // start n retail stores
                Bookstore bs = new Bookstore(i);
                retailStores[i] = new Thread(new ThreadStart(bs.BookStoreFunc));
                retailStores[i].Start();
            }


            //TestBookStore();

            //TestPricingModel();

            //EncodeDecodeTest();

            //Console.Read();
        }
        
    }
}  
