using System;
using cp_lab_4;

namespace cp_lab_4
{
    class Program
    {
        static void Main()
        {
            double a = -100;
            double b = 100;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Root Finding Program");
                Console.WriteLine("---------------------");

                Console.WriteLine("Choose a function to solve:");
                Console.WriteLine("1) f(x) = x^2 - 10");
                Console.WriteLine("2) f(x) = cos(4x) - x");
                Console.WriteLine("3) f(x) = x^3 - 2x");
                Console.WriteLine("Print anything else to exit");
                Console.Write("Your choice: ");
                int funcChoice;
                try { funcChoice = int.Parse(Console.ReadLine() ?? "1"); } catch { return; }

                func f = funcChoice switch
                {
                    1 => (x) => x * x - 10,
                    2 => (x) => Math.Cos(4 * x) - x,
                    3 => (x) => x * x * x - 2 * x,
                };

                Console.Write("Enter epsilon (default 1e-6): ");
                double eps;
                try { eps = double.Parse(Console.ReadLine() ?? "1e-6"); } catch { eps = 1e-6; }

                Console.Write("Use tabulation (y/n) (defaults to n):");
                string tab;
                try { tab = Console.ReadLine() ?? "n"; } catch { tab = "n"; }

                RootFinder rf = new RootFinder(eps, f);

                if (tab != "y")
                {
                    Console.Write("Enter interval start a: ");
                    a = double.Parse(Console.ReadLine() ?? "-10");

                    Console.Write("Enter interval end b: ");
                    b = double.Parse(Console.ReadLine() ?? "10");
                }

                Console.WriteLine("Choose method:");
                Console.WriteLine("1) Halving");
                Console.WriteLine("2) Newton");
                Console.Write("Your choice: ");

                if (Enum.TryParse(Console.ReadLine() ?? "1", out Methods method))
                {
                    Console.WriteLine($"You chose {method}");
                }
                else
                {
                    Console.WriteLine("Invalid choice, defaulting to Halving");
                    method = Methods.Halving;
                }

                try
                {
                    if (tab == "y")
                    {
                        rf.Tabulation(a, b, 1, method);
                        return;
                    }

                    rf.RunMethod(a, b, method);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during root finding: {ex.Message}");
                }
            }
        }
    }
}
