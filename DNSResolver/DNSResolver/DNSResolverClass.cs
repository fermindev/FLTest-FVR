// --------------------------------------------------------------------------------------------
// DNSResolver - Test for Flexian Limited
// Fermín Vallecillos <ferminvallecillos@gmail.com>
// --------------------------------------------------------------------------------------------
// A program for resolving DNS entries using multiple threads. I should mention that this
// code is strongly based on the work of Alphons van der Heijden, published in CodeProject
// as 'DNS.NET Resolver (C#)' (http://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C).
// .NET seems not to provide a method for specific DNS queries (QType). Alhpons's code worked
// nicely for the purpose, helping to be focused on parsing input, managing threads and
// writing results.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNSResolver
{
    /// <summary>
    /// This class is responsible for reading the file, resolve DNS queries and writing output
    /// </summary>
    public class DNSResolverClass
    {
        List<string> results;               // stores results from DNS queries (one DNS record per line)
        object synLock = new object();      // lock for threads writting on results
        Arguments a;                        // input arguments
        int threads;                        // number of running threads
        

        /// <summary>
        /// Perform entire task (read, resolve, write)
        /// </summary>
        /// <param name="a">Input safe arguments</param>
        public void Go(Arguments a)
        {
            string[] lines;     // lines in input file
            int lineIdx;        // input file line index (base 1, basically for debugging wrong lines)
            DateTime start;      // datetime for mark process start (benchmark)

            
            this.a = a;
            start = DateTime.Now;

            #region Read input file

            Console.WriteLine("Reading input file");

            try
            {   
                lines = System.IO.File.ReadAllLines(a.InputFile);
            }
            catch (Exception ex)
            {

                throw new Exception("Cannot read input file (please check file exists and you have read permissions). ");
            }

            #endregion

            #region Launch DNS queries on separated threads

            Console.WriteLine("Resolving, please wait...");
            
            results = new List<string>();
            lineIdx = 0;
            threads = lines.Length;

            // launch a thread for every query
            foreach (string line in lines)
            {
                lineIdx += 1;

                // split tabs and analyze line
                string[] sep = { "\t" };
                string[] items = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);


                if (items.Length == 2)
                {
                    DNSQuery dnsq = new DNSQuery();

                    try
                    {
                        dnsq.Host = items[0];
                        dnsq.QType = (Heijden.DNS.QType)Enum.Parse(typeof(Heijden.DNS.QType), items[1]); 

                        System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Worker));
                        t.Start(dnsq);

                    }
                    catch (Exception)
                    {
                        // if error, it should be wrong qtype
                        AddResult("[ERROR - Invalid QType (line " + lineIdx.ToString() + ")[: " + items[1]);
                    }


                }
                else
                {
                    // missing host or qtype
                    AddResult("[ERROR - Invalid line (" + lineIdx.ToString() + ")[: " + line);
                }
            }

            
            do
            {
                // wait while running threads are alive

            } while (threads > 0);

            Console.WriteLine("All queries collected");

            #endregion

            #region write output file

            Console.WriteLine("Writing results on " + a.OutputFile);

            try
            {
                System.IO.File.WriteAllLines(a.OutputFile, results.ToArray<string>());

                // get elapsed time
                TimeSpan time = DateTime.Now.Subtract(start);

                Console.WriteLine("Task completed in " + time.TotalSeconds.ToString("0.000") + " seconds");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                throw new Exception("Cannot write output file (please check you have provided a valid path and you have write permissions on destination folder)");
            }

            
            #endregion
        }


        /// <summary>
        /// DNS query thread
        /// </summary>
        /// <param name="obj">Input query as DNSQuery object</param>
        void Worker(object obj)
        {
            DNSQuery q = (DNSQuery)obj;                 // incoming query parameters (host, qtype)
            List<string> data = new List<string>();     // results for this single query (may be multiple)

            try
            {
                // using Alhpons's class from here...
                Heijden.DNS.Resolver resolver = new Heijden.DNS.Resolver(a.DNS);
                resolver.Recursion = true;
                
                // get results
                Heijden.DNS.Response response= resolver.Query(q.Host, q.QType);              

                
                if (response.Answers.Count > 0)
                {
                    // for every record found, add result line
                    foreach (Heijden.DNS.AnswerRR answer in response.Answers)
                    {
                        data.Add(q.Host + " " + q.QType.ToString() + " ---> " + answer.RECORD.ToString());
                    }

                    AddResults(data);
                }
                else
                {
                    // if no record found, show a nice message for alleviating disgruntled user...
                    AddResult(q.Host + " " + q.QType.ToString() + " ---> SORRY, NO RECORDS FOUND :(");
                }
                
                
            }
            catch (Exception ex)
            {
                // unmanaged errors are just dumped as another result providing original query and error
                // information
                AddResult("[ERROR] " + q.Host + " " + q.QType.ToString() + ":" + ex.Message);

            }

            // any case (success/fail), mark this thread as finished
            threads -= 1;
        }

        /// <summary>
        /// Add several lines at once to results variable
        /// </summary>
        /// <param name="data">Lis of string containing lines</param>
        void AddResults(List<string> data)
        {
            // ensure we write all lines together for this result
            lock (synLock)
            {
                foreach (string dat in data)
                {
                    results.Add(dat);

                    // uncomment for debug
                    // Console.WriteLine(dat);   
                }
            }

            
        }

        /// <summary>
        /// Add line to results variable
        /// </summary>
        /// <param name="data">New line</param>
        void AddResult(string data)
        {
            
            lock (synLock)
            {
                results.Add(data);
            }

            // uncomment for debug
            //Console.WriteLine(data);
        }
    }
}
