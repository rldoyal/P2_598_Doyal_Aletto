using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2_598_Doyal_Aletto
{
    public class OrderObject
    {
        private Int32 bookstoreId; //the identity of the bookstore
        private Int32 cardNo; //integer that represents a credit card number
        private string publisherId; //the identity of the publisher
        private Int32 amount; //represents the number of books to order
        private double unitPrice; //represents the unit price of the book received from the publisher
        private DateTime timestamp; //Timestamp the order was created

        //Constructor to be used by the encoder
        public OrderObject(Int32 sender, Int32 cNum, Int32 numBooks, double price)
        {
            bookstoreId = sender;
            cardNo = cNum;
            publisherId = String.Empty;
            amount = numBooks;
            unitPrice = price;
            timestamp = DateTime.Now;
        }

        //Constructor to be used by the decoder
        public OrderObject(Int32 sender, Int32 cNum, string recId, Int32 numBooks, double price, DateTime now)
        {
            bookstoreId = sender;
            cardNo = cNum;
            publisherId = recId;
            amount = numBooks;
            unitPrice = price;
            timestamp = now;
        }

        //Get bookstoreId
        public Int32 getBookstoreId()
        {
            return bookstoreId;
        }

        //Set bookstoreId
        public void setBookstoreId(Int32 s)
        {
            bookstoreId = s;
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

        //Get publisherId
        public string getpublisherId()
        {
            return publisherId;
        }

        //Set publisherId
        public void setpublisherId(string r)
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
    }
}
