using System;
using SeleniumLogger;

namespace Testing
{
    class Test
    {

        public class Example
        {
            public Example() {}
        }

        static void Main(string[] args)
        {
            try
            {
                Example eg1 = new Example();
                Example eg2 = new Example();

                SeleniumLog log = SeleniumLog.Instance();
                bool result1 = log.AreEqual(expected: eg1, actual: eg1, message: "This test should pass ", throwException: false);
                bool result2 = log.AreEqual(expected: eg1, actual: eg2, message: "This test should fail ", throwException: false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

    }
}

//
// To view result:
// Step 1: Open the SeleniumLog.config file and check the <output_file_path>
// Step 2: Launch SeleniumLog Application for Windows
// Step 3: Open the file specified in <output_file_path>
//
// Note: Results will also appear in Visual Studio Test Explorer
//
