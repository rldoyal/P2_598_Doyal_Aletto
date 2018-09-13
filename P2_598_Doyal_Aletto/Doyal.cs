using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


// Project 2 ASU CSE 598  Fall 2018
// Team Aletto Doyal 598
// All code in this directory was developed by Roy Doyal

namespace P2_598_Doyal_Aletto
{

    // Encoder -- turns OrderObject into a string
    public class Bookstore
    {


        private double LastPrice = 200.00;
        private double CurrentPrice = 0.00;
        private Int32 StoreNumber = 0;
        private Int32 StoreInventory = 100;
        const Int32 MAX_INVENTORY = 1000;  // max number of books the store can hold



        public Bookstore(int i)
        {
            StoreNumber = i;
        }

        // functional thread for the bookstore.  It will generate an order every 2 seconds.
        public void BookStoreFunc()
        {

            Aletto.Publisher pub1 = new Aletto.Publisher();

            while (Program.BookStoreThreadRunning)
            {

                Console.WriteLine("\n\n");

                //CurrentPrice = pub1.getPrice();
                SellBooks(); // create demand
                OrderObject newOrder = CreateOrder("1");
                if (newOrder != null)
                    Console.WriteLine("\t\t New Order Created... \n" +
                            "\t\t\t SenderID : {0}\n" +
                            "\t\t\t CardNo : {1}\n" +
                            "\t\t\t Publisher : {2}\n" +
                            "\t\t\t Amount of Books: {3}\n" +
                            "\t\t\t Unit Price : {4}\n" +
                            "\t\t\t Order Placed at : {5}\n",
                    newOrder.getSenderId(), newOrder.getCardNo(), newOrder.getReceiverId(), newOrder.getAmount(),
                    newOrder.getUnitPrice(), newOrder.getTimestamp());
                else
                    Console.WriteLine("\t\t *** No order created  for store : {0}", StoreNumber);
                //}
                LastPrice = CurrentPrice; // set the last known price
                Thread.Sleep(1000); // sleep for 2 seconds
            }

            Console.WriteLine("@@@@   BookStoreFunc threaded exited gracefully...Thread Name {0}", Thread.CurrentThread.Name);

        }

        public Int32 getStoreInventory()
        {
            return StoreInventory;
        }

        OrderObject CreateOrder(string publisherName)
        {
            string SenderID = Thread.CurrentThread.Name;
            Int32 CardNo = 5000;
            string ReceiverID = publisherName;
            Int32 amountBooks = BooksNeeded();
            // Set the last price

            DateTime timeStamp = DateTime.Now;
            if (amountBooks > 0)
            {
                OrderObject myObj = new OrderObject(SenderID, CardNo, ReceiverID, amountBooks, CurrentPrice, timeStamp);
                AddStoreInventory(amountBooks); // update the store inventory
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


        // sell books from the store -- create demand
        void SellBooks()
        {
            Random rnd = new Random();

            Int32 BooksSold = rnd.Next(0, StoreInventory); //  Might make no sales and might sell all inventory
            StoreInventory -= BooksSold; // update the inventory
            Console.WriteLine("Store {0} sold {1} books, new store Inventory {2}", StoreNumber, BooksSold, StoreInventory);

        }

        void AddStoreInventory(Int32 newBooks)
        {
            Console.WriteLine("Adding {0} new books.  Thread Name : {1}", newBooks, StoreNumber);
            StoreInventory += newBooks;
        }

        Int32 BooksNeeded()
        {
            // TODO :  Update this method to include pricing but for now, just do basic
            Int32 booksWanted = 0;

            // if we are low on books, 50 or less, then buy some books to get up back to 300 books
            Console.WriteLine("*** Checking to see if we need more books for Store {0}, Inventory {1}, last price {2}, current price{3}",
                StoreNumber, StoreInventory, LastPrice, CurrentPrice);
            if (CurrentPrice < LastPrice)
            {
                double priceDiff = ((LastPrice - CurrentPrice) / LastPrice);
                Int32 ShelfSpace = MAX_INVENTORY - StoreInventory; // how much space we have on the shelfs for new books
                if (priceDiff > .75) // fill the store to max
                    booksWanted = (ShelfSpace);
                else if (priceDiff > .50) // fill to 50% of shelf space
                    booksWanted = (ShelfSpace / 2);
                else if (priceDiff > .10) // fill to 25% capacity
                    booksWanted = (ShelfSpace / 4);
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

