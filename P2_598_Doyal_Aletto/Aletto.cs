using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace P2_598_Aletto_Doyal
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


            public double getPrice( )
            {

            }

            //Class used to calculate unit price of a book
    public class PricingModel
    {
        private Int32 numOrders; //Numbers of past orders relevant in pricing model calculation
        private Queue<OrderObject> orders; //Keeps recent orders in a queue to use in pricing model
        private TimeSpan reference; //Time object used to determine removal of order objects from queue
        private const double RESTOCK_AMT = 2000; //The number of books ordered when Publisher restocks books
        private const double DEMAND = 3; //Sets demand for how many recent orders have been processed
        private const double AVG_ORDER_SIZE = 100; //Reference for the average order size


        public PricingModel()
        {
            numOrders = 0;
            orders = new Queue<OrderObject>();
            reference = new TimeSpan(0, 0, 3);
        }

        public double calcPrice(OrderObject o)
        {
            double unitPrice = 25;

            //Check to see if any orders are older than the reference timespan
            foreach (OrderObject order in orders)
            {
                if (o.getTimestamp() - order.getTimestamp() > reference)
                {
                    orders.Dequeue();

                }
            }

            orders.Enqueue(o); //Put new order in the queue
            setNumOrders(); //Set the appropriate number of recent orders

            //Actual price calculation: combination of number ordered versus number left, 
            //demand created by recent orders, and the size of the order
            unitPrice += (1 - ((double)o.getAmount()) / RESTOCK_AMT * 50) + (50 * ((double)orders.Count / DEMAND)) + (50 * (2 - ((double)o.getAmount() / AVG_ORDER_SIZE)));

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

        public static void Main(string[] args)
        {
            DateTime now = new DateTime();
            now = DateTime.Now;
            PricingModel p = new PricingModel();


            for (int i = 0; i < 5; i++)
            {
                OrderObject o = new OrderObject("sender" + i.ToString(), Math.Random(), "receiver" + i.ToString(), 100, 100, now);
            }

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
