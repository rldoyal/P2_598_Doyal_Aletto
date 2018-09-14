using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace P2_598_Doyal_Aletto
{
    public class MultiCellBuffer
    {
        public String[] buffers;
        private const int N = 3;
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
                    buffers = new String[N];


                    for (int i = 0; i < n; i++)
                    {
                        buffers[i] = String.Empty;

                    }
                }
                else
                    Console.WriteLine(" MultiCellBuffer Constructor - n > N ..");
            }
        }

        public void setOneCell(String data)
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
                    if ((String.IsNullOrEmpty(buffers[i])))// make sure empty
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

        public String getOneCell()
        {
            String outStr = String.Empty;
            read_pool.WaitOne();

            lock (this)
            {
                while (elementCount == 0)
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < n; i++)
                {

                    if (!(String.IsNullOrEmpty(buffers[i]))) // make sure cell has data
                    {
                        outStr = buffers[i];
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
