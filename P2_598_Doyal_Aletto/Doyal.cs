using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


// Project 2 ASU CSE 598  Fall 2018
// Team Aletto Doyal 598
// All code in this directory was developed by Roy Doyal

namespace P2_598_Aletto_Doyal
{

    // Encoder -- turns OrderObject into a string
    public class Bookstore
    {

        public SupplyDemand myDemand = new SupplyDemand();
        Int32 LastPrice = 0;

        public Bookstore()
        {
            // setup and start the demand thread
            Thread demandThread = new Thread(new ThreadStart(myDemand.DemandFunc));
            demandThread.Name = "1";
            demandThread.Start();
            

        }

        // functional thread for the bookstore.  It will generate an order every 2 seconds.
        public void BookStoreFunc()
        {
            while (Program.BookStoreThreadRunning)
            {
                // Determine if Ineed to buy books or not.
                // if book supply is low (below 50), I need to replenish
                // if diference between last and current price is greater than a 20% drop, I need to buy books
                // if I don't need books, don't create an order :)

                //if (myDemand.BooksNeeded()>0) // automatically buy books
                //{
                // look at the two publisher prices and pick the lowest. 
                Console.WriteLine("\n\n");
                OrderObject newOrder = CreateOrder("2");
                if (newOrder != null)
                    Console.WriteLine("\t\t\t New Order Created... {0},{1},{2},{3},{4},{5}",
                       newOrder.getSenderId(), newOrder.getCardNo(), newOrder.getReceiverId(), newOrder.getAmount(),
                       newOrder.getUnitPrice(), newOrder.getTimestamp());
                else
                    Console.WriteLine("\t\t\t\t *** No order created");
                //}
              
                Thread.Sleep(2000); // sleep for 2 seconds
            }

        }

 
        OrderObject CreateOrder( string publisherName )
        {
            string SenderID = Thread.CurrentThread.Name;
            Int32 CardNo = 5000;
            string ReceiverID = publisherName;
            Int32 amountBooks = myDemand.BooksNeeded();
            Int32 unitPrice = Program.GV.getCurrentPrice();
            // Set the last price
            LastPrice = unitPrice;
            DateTime timeStamp = DateTime.Now;
            if (amountBooks > 0)
            {
                OrderObject myObj = new OrderObject(SenderID, CardNo, ReceiverID, amountBooks, unitPrice, timeStamp);
                myDemand.addStoreInventory(amountBooks); // update the store inventory
                return myObj;
            }
            return null;
        }

        // Encoder -- turns OrderObject into a CSV string
        string Encorder(OrderObject order)
        {
            string orderStr = null;
            // build CSV String
            orderStr = order.getSenderId();
            orderStr += "," + order.getCardNo().ToString();
            orderStr += "," + order.getReceiverId();
            orderStr += "," + order.getAmount().ToString();
            orderStr += "," + order.getUnitPrice().ToString();
            orderStr += "," + order.getTimestamp();

            return orderStr; // return the encoded string just created.
        }


    }

    public class SupplyDemand
    {
        const Int32 MaxBookQuanity = 500;  // max number of books allowed in the store
        static Int32 StoreInventory = 100;  // initial book inventory set at 500.  Max Books = 500.
        bool NeedDemandThread = true; // variable to start/stop demand thread
        private Int32 LastPrice = 150; // TODO THis will turn into a function for multiple publishers.
        public SupplyDemand()
        {

        }
        // function to act as demand... 
        // it will be a thread that consumes a random number of books every 1 second
        public void DemandFunc()
        {
            Random rnd = new Random();
            Int32 RemoveBooks;
            while (NeedDemandThread)
            {
                Thread.Sleep(1000); // sleep for 1 second (1000 ms)
                RemoveBooks = rnd.Next(5, 250);
                StoreInventory -= RemoveBooks;  // allow no more than 250 max books per demand
                Console.WriteLine(" Removed {0} books...", RemoveBooks);
                if (StoreInventory < 0)
                    StoreInventory = 25; // don't let the book store get less than 25 books
            }
        }
        public void TurnDemandOff()
        {
            NeedDemandThread = false;
        }

        public Int32 getStoreInventory()
        {
            return StoreInventory;
        }

        public void addStoreInventory(Int32 newBooks)
        {
            Console.WriteLine("Adding {0} new books.", newBooks);
            StoreInventory += newBooks;
        }

        public Int32 BooksNeeded()
        {
            // TODO :  Update this method to include pricing but for now, just do basic
            Int32 booksWanted = 0;

            // if we are low on books, 50 or less, then buy some books to get up back to 300 books
            Console.WriteLine("\t\t Check to see if we need more books last price {0}, current price{1}",
                LastPrice, Program.GV.getCurrentPrice());
            if (Program.GV.getCurrentPrice() < LastPrice) 
            {
                double priceDiff = (((double)LastPrice - (double)Program.GV.getCurrentPrice()) / (double)LastPrice);
                if (priceDiff > .75 ) // fill the store to max
                    booksWanted = (500 - StoreInventory);
                else if (priceDiff > .50 ) // fill to 80%
                    booksWanted = (400 - StoreInventory);
                else if (priceDiff > .10 ) // fill to 50% capacity
                    booksWanted = (250 - StoreInventory);
            }
            // if the booksWanted is negative, then we don't need to purchase.

            // if books inventory is below 50 books, then make sure we fill up to 350.
            if ((StoreInventory <= 50) && (booksWanted >= 0))
                booksWanted = 350 - StoreInventory;

            if (booksWanted < 1)
                booksWanted = 0;

            return booksWanted;
        }

        

    }

}
