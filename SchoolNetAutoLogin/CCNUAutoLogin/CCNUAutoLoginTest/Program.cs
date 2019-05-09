using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCNUAutoLoginTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Services s = new Services();
            s.OnStart();

            Console.ReadLine();
        }
    }
}
