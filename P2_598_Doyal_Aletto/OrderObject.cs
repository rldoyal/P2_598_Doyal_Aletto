﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2_598_Doyal_Aletto
{
    /// <summary>
    /// Order Object -- This object contains one data for one order.  
    /// The publisher and Retail threads will use this class for order information.
    /// One object will be created for each order.
    /// </summary>
    public class OrderObject
    {
        private Int32 bookStoreId; //the identity of the sender
        private Int32 cardNo; //integer that represents a credit card number
        private Int32 publisherId; //the identity of the receiver
        private Int32 amount; //represents the number of books to order
        private double unitPrice; //represents the unit price of the book received from the publisher
        private DateTime timestamp; //Timestamp the order was created
        private long timeTicks;  // timestamp  in milliseconds

        //Constructor to be used by the encoder
        public OrderObject(Int32 bsNum, Int32 cNum, Int32 pubid, Int32 numBooks, double price)
        {
            bookStoreId = bsNum;
            cardNo = cNum;
            publisherId = pubid;
            amount = numBooks;
            unitPrice = price;
            timestamp = DateTime.Now;
            timeTicks = DateTime.Now.Ticks;;
        }

        //Constructor to be used by the decoder
        public OrderObject(Int32 bsNum, Int32 cNum, Int32 recId, Int32 numBooks, double price, DateTime now, long ticks)
        {
            bookStoreId = bsNum;
            cardNo = cNum;
            publisherId = recId;
            amount = numBooks;
            unitPrice = price;
            timestamp = now;
            timeTicks = ticks;

        }

        //Get senderId
        public Int32 getBookStoreId()
        {
            return bookStoreId;
        }

        //Set senderId
        public void setBookStoreId(Int32 s)
        {
            bookStoreId = s;
        }

        //Get cardNo
        public Int32 getCardNo()
        {
            return cardNo;
        }

        //Set cardNo
        public void setCardNo(Int32 c)
        {
            cardNo = c;
        }

        //Get receiverId
        public Int32 getPublisherId()
        {
            return publisherId;
        }

        //Set receiverId
        public void setPublisherId(Int32 r)
        {
            publisherId = r;
        }

        //Returns number of books in the order
        public Int32 getAmount()
        {
            return amount;
        }

        //Set amount
        public void setAmount(Int32 a)
        {
            amount = a;
        }

        //Get unitPrice
        public double getUnitPrice()
        {
            return unitPrice;
        }

        //Set unitPrice
        public void setUnitPrice(double u)
        {
            unitPrice = u;
        }

        //Get timestamp
        public DateTime getTimestamp()
        {
            return timestamp;
        }

        // set milliseconds
        public void setTicks( long ticks)
        {
            timeTicks = ticks;
        }

        // get for milliseconds
        public long getTicks()
        {
            return timeTicks;
        }
    }
}
