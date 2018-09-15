using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace P2_598_Doyal_Aletto
{

    public class BufferString
    {
        private string bufferStr;
        private Int32 publisherNumber;

        public BufferString()
        {
            publisherNumber = 0;
            bufferStr = String.Empty;
        }
        public void setBufferString(string str, Int32 pubNum)
        {

            bufferStr = str;
            publisherNumber = pubNum;

        }

        public string getBufferString()
        {

            publisherNumber = 0; // null out the publisher
            return bufferStr;
        }

        public Int32 getPublisherNum()
        {
            return publisherNumber;
        }

        public void Reset()  // reset the values
        {
            publisherNumber = 0;
            bufferStr = String.Empty;
        }
    }

    public class MultiCellBuffer
    {
        public BufferString[] buffers;
        private const int N = 5;
        private int n; // number of cells
        private int elementCount;
        private static Semaphore write_pool;
        private static Semaphore read_pool;

        // constructor for class
        public MultiCellBuffer(int n)
        {
            lock (this) // we want no interruptiongs
            {
                elementCount = 0;

                if (n <= N)
                {
                    this.n = n;
                    write_pool = new Semaphore(n, n);
                    read_pool = new Semaphore(n, n);
                    buffers = new BufferString[3];


                    for (int i = 0; i < n; i++)
                    {
                        buffers[i] = new BufferString();

                    }
                }
                else
                    Console.WriteLine(" MultiCellBuffer Constructor - n > N ..");
            }
        }

        public void setOneCell(BufferString data)
        {
            write_pool.WaitOne();

            lock (this)
            {
                while (elementCount == n)
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < n; i++)
                {
                    if (buffers[i].getPublisherNum() == 0) // make sure empty
                    {
                        buffers[i] = data;
                        elementCount++;
                        i = n;
                    }
                }
                write_pool.Release();
                Monitor.Pulse(this);
            }
        }

        public String getOneCell( Int32 pubNum)
        {
            String outStr = null;
            read_pool.WaitOne();

            lock (this)
            {
                while (elementCount == 0)
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < n; i++)
                {

                    if (buffers[i].getPublisherNum() == pubNum) // make sure the data cell has a value for that publsiher
                    {
                        outStr = buffers[i].getBufferString();
                        buffers[i].Reset();
                        elementCount--;
                        i = n;
                    }

                }
                read_pool.Release();
                Monitor.Pulse(this);
            }
            return outStr;
        }

    }
}
