using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace P2_598_Doyal_Aletto
{
    class Aletto
    {
        public class Publisher
        {
            private static Int32 books; //Static variable for number of books in the publisher
            private static Int32 p; //Counter used to to determine number of price cuts before terminating
            PricingModel modeler; //To be used to calculate per unit cost of a book
            private static Boolean readable; //Flag used to signal 


            //Constructor
            public Publisher(Int32 initCounter, Int32 bks)
            {
                p = initCounter;
                modeler = new PricingModel();
                books = bks;

            }


            //Thread entry
            public void runPublisher()
            {
                Int32 counter = 0;
                while (!readable)
                {
                    Thread.Sleep(50);
                    counter++;
                    if(counter == 10)
                    {

                    }
                }
            }

            //Increments the counter p
            public void incrementCounter()
            {
                p++;
            }

            //Class used to calculate unit price of a book
    public class PricingModel
    {
        private Int32 numOrders; //Numbers of past orders relevant in pricing model calculation
        public Queue<OrderObject> orders; //Keeps recent orders in a queue to use in pricing model
        private TimeSpan reference; //Time object used to determine removal of order objects from queue
        private const Int32 RESTOCK_AMT = 2000; //The number of books ordered when Publisher restocks books
        private const double DEMAND = 5; //Sets demand for how many recent orders have been processed
        private const double AVG_ORDER_SIZE = 100; //Reference for the average order size


        public PricingModel()
        {
            numOrders = 0;
            orders = new Queue<OrderObject>();
            reference = new TimeSpan(0, 0, 3);
        }

        //Calculates the unit price for a book with current market conditions
        public double calcPrice(OrderObject o)
        {
            //Base setting for output to keep final price calculation within 50 to 200
            double unitPrice = 0;
            
            //Determine if a new book order is required based on the current order
            if(o.getAmount() > books)
            {
                setNumBooks(RESTOCK_AMT);
                Console.WriteLine("The Publisher just restocked books.\n");
                //************************  TODO: Callback event, lets bookstores know there is a price drop*********************
            }

            //Check to see if any orders are older than the reference timespan
            Int32 count = 0; //Counts number of OrderObjects that are relatively too old to include in price calculation
            foreach (OrderObject order in orders)
            {
                if (o.getTimestamp() - order.getTimestamp() > reference)
                {
                    
                    count++;
 
                }
            }

            //Deqeue the older OrderObjects
            while (count != 0)
                {
                        orders.Dequeue();
                        count--;
                }

            orders.Enqueue(o); //Put new order in the queue
            setNumOrders(); //Set the appropriate number of recent orders

            //Actual price calculation: combination of number ordered versus number left, 
            //demand created by recent orders, and the size of the order
            unitPrice += (1 - ((double)getNumBooks()) / ((double)RESTOCK_AMT)) * 150 //Number ordered versus number left
                        + (((double)orders.Count / (double)DEMAND) * 25) //Number of recent orders compared to a demand benchmark
                        + (((double)o.getAmount() / (double)AVG_ORDER_SIZE) * 25); //Wholesale determination
            
            //Subtract the number of books purchased from the supply of books
            books = books - o.getAmount();

            //Make sure price is below 200 and above 50
            if (unitPrice > 200)
            {
                unitPrice = 200;
            }
            if (unitPrice < 50)
            {
                unitPrice = 50;
            }

            return unitPrice;
        }

        //Sets the number of orders
        public void setNumOrders()
        {
            numOrders = orders.Count;
        }

        //Returns number of books stocked by publisher
        public Int32 getNumBooks(){
            return books;
        }

        //Sets the number of books
        public void setNumBooks(Int32 b){
            books = b;
        }
    }
            
        //Class used to instantiate threads to process orders
        private class OrderProcessing
        {
                 
            public OrderProcessing()
            {

            }
        }

        }
    }
}
