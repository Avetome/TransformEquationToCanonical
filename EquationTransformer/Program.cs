using System;
using System.IO;
using System.Linq;

namespace EquationTransformer
{
    class MainClass
    {
        private const ConsoleColor NormalColor = ConsoleColor.White;
        private const ConsoleColor MessagesColor = ConsoleColor.Gray;
        private const ConsoleColor ResultColor = ConsoleColor.White;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                UserInputMode();
            }
            else
            {
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine($"File {args[0]} not found");
                }

                FileMode(args[0]);
            }
        }

        private static void UserInputMode()
        {
            Console.Clear();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);

            Console.Title = "Transform equation To canonical form";

            Console.ForegroundColor = MessagesColor;
            Console.WriteLine("***** Enter a double numbers with dots (\"4.5\", not \"4,5\") *****");
            Console.WriteLine("***** Divide operation (\"/\") not supported, as well as any math functions, bit-wise operations etc...*****");
            Console.WriteLine("***** Press CTRL+C to exit the program *****");
            Console.ForegroundColor = NormalColor;

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = MessagesColor;
                Console.Write("Enter the equation: ");
                Console.ForegroundColor = NormalColor;

                string equation = Console.ReadLine();

                if (string.IsNullOrEmpty(equation))
                {
                    continue;
                }

                EquationTrasformer equationTrasformer = new EquationTrasformer();
                try
                {
                    string result = equationTrasformer.Transform(equation);

                    Console.ForegroundColor = ResultColor;
                    Console.Write("Equation canonical form: ");

                    Console.Write(result);
                    Console.WriteLine();

                    Console.ForegroundColor = NormalColor;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ErrorColor;
                    Console.WriteLine("Enter a valid equation!");
                    Console.ForegroundColor = NormalColor;
                }
            }
        }

        private static void FileMode(string filename)
        {
            string line, result;

            using (var inputFile = new StreamReader(filename))
            using (var outputFile = new StreamWriter(filename + ".out"))
            {
                Console.WriteLine("Processing file...");

                while ((line = inputFile.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    {
                        // forgive them their sins...
                        continue;
                    }

                    EquationTrasformer equationTrasformer = new EquationTrasformer();
                    result = equationTrasformer.Transform(line);

                    outputFile.WriteLine(result);
                }

                Console.WriteLine("File processing finished");
            }
        }


        private static void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exit application");

            Environment.Exit(0);
        }
    }
}
