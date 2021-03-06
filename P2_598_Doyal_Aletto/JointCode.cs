﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


// All code in this file was written jointly by Roy Doyal and Anthony Aletto

/// <summary>
/// Joint Code written by Roy Doyal and Anthony Aletto
/// 
/// Thi file contains the BufferString class and the MultiCell buffer
/// The bufferString class use used as the array element type for the Multicell buffer.
/// 
/// The multicell buffer is the conduit between the retail threads and the publisher threads.  
/// The multicell buffer uses Semiphores, Mutexs, and read/write locks to protect the data.
/// </summary>

namespace P2_598_Doyal_Aletto
{

    // buffer class that is being used in the multicell buffer.  We stripped out the publisher number so when the 
    // publisher threads are looking for orders, they can quickly look instead of parsing a string.
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

    // Mulit cell buffer -- link between retail stores and publishers.
    // BufferString objects are the actual stored elements.
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

        /// <summary>
        /// Sets one open buffer cell with data.  
        /// If cells are all full, the calling thread is blokced.
        /// </summary>
        /// <param name="data"></param>
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
        /// <summary>
        /// Gets one cell of data -- to be read by the publisher.
        /// The data is only read if the publisher number in the data cell matches the publisher number
        /// </summary>
        /// <param name="pubNum"></param>
        /// <returns></returns>
        public String getOneCell(Int32 pubNum)
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
