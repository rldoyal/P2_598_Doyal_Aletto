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
            private const Int32 RESTOCK_AMT = 2000; //The number of books ordered when Publisher restocks books
            Decoder decoder; //Object used to transform strings into OrderObjects


            //Publisher Constructor
            public Publisher()
            {
                p = 0;
                modeler = new PricingModel();
                books = RESTOCK_AMT;
                decoder = new Decoder();
                //runPublisher();
            }


            //Publisher Thread entry
            public static void runPublisher()
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
            public static void incrementCounter()
            {
                p++;
            }

            //Returns the instance of pricing model class
            public PricingModel getModeler()
            {
                return modeler;
            }

            //Class used to calculate unit price of a book
            public class PricingModel
            {
                private Int32 numOrders; //Numbers of past orders relevant in pricing model calculation
                public Queue<OrderObject> orders; //Keeps recent orders in a queue to use in pricing model
                private TimeSpan reference; //Time object used to determine removal of order objects from queue
                private const double DEMAND = 4; //Sets demand for how many recent orders have been processed
                private const double AVG_ORDER_SIZE = 50; //Reference for the average order size

                //Constructor for the pricing model
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
                    if (o.getAmount() > books)
                    {
                        setNumBooks(RESTOCK_AMT); //Restocks the publisher to the max capacity
                        incrementCounter(); //Advances the publisher's counter that counts towards thread termination
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
                    unitPrice +=

                        //Represents demand due to product availability, is bulk of pricing, cannot be more than 150, or less than 40
                        Math.Max(((1 - ((double)getNumBooks()) / ((double)RESTOCK_AMT)) * 150), 40)

                        //Number of recent orders compared to a demand benchmark, cannot be more than 40 extra
                        + ((Math.Min((double)orders.Count, DEMAND * 2) / (double)DEMAND) * 20) 

                        //Wholesale determination, cannot be more than 40 extra      
                        + (((double)AVG_ORDER_SIZE / Math.Max((double)o.getAmount(), AVG_ORDER_SIZE/2)) * 20); 

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

                //Returns the queue
                public Queue<OrderObject> getQueue()
                {
                    return orders;
                }

            }

            //Returns number of books stocked by publisher
            public static Int32 getNumBooks()
            {
                return books;
            }

            //Sets the number of books
            public static void setNumBooks(Int32 b)
            {
                books = b;
            }

            //Returns the decoder object
            public Decoder getDecoder()
            {
                return decoder;
            }

            //Class used to instantiate threads to process orders
            private class OrderProcessing
            {
                 
                public OrderProcessing()
                {

                }
            }

            //Class used to take strings of comma separated values and generate OrderObjects
            public class Decoder
            {
                public Decoder()
                {
                    
                }

                //Turns strings handed to the publisher into OrderObjects
                public OrderObject decode(string s)
                {
                    //string array holds the values of the OrderObject
                    string[] str = s.Split(s);

                    //Try to parse the int32 for the cardId, otherwise exit
                    Int32 cardId = 0;
                    if(!Int32.TryParse(str[1], out cardId))
                    {
                        Console.WriteLine("Decoder was unable to decode a message for cardId! Program will exit.");
                        Console.Read();
                        Environment.Exit(-1);
                    }

                    //Try to parse the int32 for the number of books, otherwise exit
                    Int32 numBooks = 0;
                    if (!Int32.TryParse(str[3], out numBooks))
                    {
                        Console.WriteLine("Decoder was unable to decode a message for numBooks! Program will exit.");
                        Console.Read();
                        Environment.Exit(-1);
                    }

                    //Try to parse the double for the price of each book, otherwise exit
                    double price = 0;
                    if (!double.TryParse(str[4], out price))
                    {
                        Console.WriteLine("Decoder was unable to decode a message for price! Program will exit.");
                        Console.Read();
                        Environment.Exit(-1);
                    }

                    //Try to parse the double for the price of each book, otherwise exit
                    DateTime timestamp = new DateTime();
                    if (!DateTime.TryParse(str[5], out timestamp))
                    {
                        Console.WriteLine("Decoder was unable to decode a message for DateTime! Program will exit.");
                        Console.Read();
                        Environment.Exit(-1);
                    }

                    //Construct all the pieces into an OrderObject
                    OrderObject o = new OrderObject(str[0], cardId, str[2], numBooks, price, timestamp);

                    return o;
                }
            }

        }
    }
}
