using System;

namespace cp_lab_4
{
    enum Methods
    {
        Halving = 1,
        Newton = 2,
    }

    delegate double func(double x);

    internal class RootFinder
    {
        private static double h = 1e-10;
        private int max_iter = 1000;
        private double eps;
        private func f;

        public RootFinder(double eps, func f, bool tabulate = false)
        {
            this.eps = eps;
            this.f = f;
        }

        public void ChangePresicion(double eps)
        {
            this.eps = eps;
        }

        public void ChangeFunction(func f)
        {
            this.f = f;
        }

        private double df(double x)
        {
            return (f(x + h) - f(x)) / h;
        }

        private double d2f(double x)
        {
            return (df(x + h) - df(x)) / h;
        }

        public void Tabulation(double a, double b, double step, Methods methodChoice)
        {
            Console.WriteLine("Disclaimer: -1 count means solution was found during tabulation");

            double i = a;

            for (; i + step <= b; i += step)
            {
                if (f(i) * f(i+step) > 0) {
                    continue;
                }

                if (f(i) == 0) {
                    WriteRes(i, -1);
                } else if (f(i+step) == 0) {
                    WriteRes(i+step, -1);
                }

                RunMethod(i, i+step, methodChoice);
            }
        }

        public void RunMethod(double a, double b, Methods m)
        {
            switch (m)
            {
                case Methods.Halving:
                    Halving(a, b); break;
                case Methods.Newton:
                    Newton(a, b); break;
                default: throw new Exception("Unrecognized method");
            }
        }

        private bool WriteRes(double x, int count)
        {
            int precision = -(int)Math.Log10(eps);

            if (Math.Abs(f(x)) < eps)
            {
                Console.WriteLine($"Solution found: {x.ToString("F" + precision)} at iteration: {count}");
                return true;
            }
            return false;
        }

        private void Halving(double l, double r)
        {
            if (l > r) (l, r) = (r, l);

            int count = 0;

            if (f(l) * f(r) > 0) {
                Console.WriteLine("No solution to be found here");
                return; 
            }

            if (WriteRes(l, count) || WriteRes(r, count)) return;

            while (Math.Abs(l - r) > eps)
            {
                count++;
                double mid = 0.5 * (l + r);

                if (WriteRes(mid, count)) return;

                if (f(l) * f(mid) < 0) r = mid;
                else l = mid;
            }

            WriteRes(0.5 * (l + r), count);
        }

        private void Newton(double a, double b)
        {
            if (a > b) (a, b) = (b, a);

            double x = b;

            if (f(b) * d2f(b) < 0)
            {
                x = a;
                if (f(a) * d2f(a) < 0)
                {
                    Console.WriteLine("Convergence isn't guaranteed");
                }
            }

            for (int i = 0; i < max_iter; i++)
            {
                if (Math.Abs(x) > 1e6) throw new Exception("Divergence detected");

                double dx = df(x);
                if (Math.Abs(dx) < 1e-12)
                    throw new Exception($"Zero derivative exception at x={x}");

                double diff = f(x) / dx;
                x -= diff;

                if (WriteRes(x, i)) return;
            }

            Console.WriteLine("No solution found");
        }
    }

}
