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
    /// Contains safe arguments for DNSResolverClass
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Input file path, which DNSResolverClass will read
        /// </summary>
        public string InputFile;

        /// <summary>
        /// Output file path in which DSNResolverClass will dump results.
        /// </summary>
        public string OutputFile;

        /// <summary>
        /// DNS server IP address
        /// </summary>
        public string DNS;
    }
}
