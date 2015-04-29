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
    /// Main program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Program entry point
        /// </summary>
        /// <remarks>USAGE: DNSResolver input_file [output_file (defaut=output.txt)] [dns_server_ip (default=8.8.8.8)]</remarks>
        /// <param name="args">Console argumments</param>
        static void Main(string[] args)
        {

            try
            {
                // get arguments, this may throw exception
                Arguments a = GetArguments(args);

                // DNSResolverClass makes the work of reading 
                // input file, resolving DNS and writing output
                DNSResolverClass res = new DNSResolverClass();
                res.Go(a);
            }
            catch (Exception ex)
            {
                // if error, show reason and help.
                Console.WriteLine("Error: " + ex.Message);
                ShowHelp();
            }

            
            
        }

        /// <summary>
        /// Show help in console
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("DNSResolver");
            Console.WriteLine("Testing program for Flexiant Limited");
            Console.WriteLine("Fermín Vallecillos");
            Console.WriteLine();
            Console.WriteLine("\tUSAGE:");
            Console.WriteLine();
            Console.WriteLine("\tDNSResolver <input_file> [output_file (defaut=output.txt)] [dns_server_ip (default=8.8.8.8)]");
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Get arguments from console and return Arguments object ready to be used by DNSResolverClass.
        /// </summary>
        /// <remarks> Function GetArguments only returns Arguments object if input parameters
        /// are correct or omitted/default. Otherwise throws exception with error reason.</remarks>
        /// <param name="args">Console arguments</param>
        /// <returns>Arguments object with safe arguments </returns>
        static Arguments GetArguments(string[] args)
        {
            Arguments arguments = new Arguments();

            if (args.Length ==0)
            {
                throw new Exception("Insufficient arguments");
            }
            else if(args.Length > 3)
            {
                throw new Exception("Too many arguments");
            }
            else
            {                
                arguments.InputFile = args[0];

                if (args.Length > 1)
                {
                    arguments.OutputFile = args[1];

                    if (args.Length > 2)
                    {
                        try
                        {
                            System.Net.IPAddress.Parse(args[2]);
                            arguments.DNS = args[2];
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid DNS server");
                        }
                        
                    }
                    else
                    {
                        arguments.DNS = "8.8.8.8";
                    }
                }
                else
                {
                    arguments.OutputFile = "output.txt";
                    arguments.DNS = "8.8.8.8";

                }



            }

            return arguments;
        }
        
    }
}
