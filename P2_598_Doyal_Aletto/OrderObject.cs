using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2_598_Aletto_Doyal
{
    public class OrderObject
    {
        private string senderId; //the identity of the sender
        private Int32 cardNo; //integer that represents a credit card number
        private string receiverId; //the identity of the receiver
        private Int32 amount; //represents the number of books to order
        private double unitPrice; //represents the unit price of the book received from the publisher
        private DateTime timestamp; //Timestamp the order was created

        //Constructor to be used by the encoder
        public OrderObject(string sender, Int32 cNum, Int32 numBooks, double price)
        {
            senderId = sender;
            cardNo = cNum;
            receiverId = String.Empty;
            amount = numBooks;
            unitPrice = price;
            timestamp = DateTime.Now;
        }

        //Constructor to be used by the decoder
        public OrderObject(string sender, Int32 cNum, string recId, Int32 numBooks, double price, DateTime now)
        {
            senderId = sender;
            cardNo = cNum;
            receiverId = recId;
            amount = numBooks;
            unitPrice = price;
            timestamp = now;
        }

        //Get senderId
        public string getSenderId()
        {
            return senderId;
        }

        //Set senderId
        public void setSenderId(string s)
        {
            senderId = s;
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
        public string getReceiverId()
        {
            return receiverId;
        }

        //Set receiverId
        public void setReceiverId(string r)
        {
            receiverId = r;
        }

        //Get amount
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
    }
}
