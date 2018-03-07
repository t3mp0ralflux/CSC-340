using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class TSPAnnealing
    {
        private static Trip route = new Trip();
        private static Trip best;
        private static Trip worst;
        private static double startTemp = 100000;
        private static double finalTemp = .0001;
        private static double temp;
        private static double k = 1.38064852e-23; // boltzmann constant

        // variables relating to statistical anyalysis
        private static double longestPossible = 1.094372401818199 * 14;
        private static double shortestPossible = .05431844593233095 * 14;
        private static double dx = (longestPossible - shortestPossible) / 100;
        private static double[] bins = new double[100];
        private static double sum = 0;
        private static double stdDev = 0;
        private static double numberOfSolns = 0;

        //public static void Main(String[] args)

        //{
        //    // some initiliazion below
        //    DateTime startTime = DateTime.Now;
        //    temp = startTemp;
        //    // best and worst are set to route, which is the first trip created
        //    best = route;
        //    worst = route;
        //    // just for information it prints our starting route and its length
        //    Logger.Out("This is the initial state of the trip");
        //    best.printTrip();
        //    Logger.Out(best.getTripLength().ToString());

        //    sum += route.getTripLength();
        //    stdDev += (route.getTripLength() * route.getTripLength());
        //    int binIndex = (int)((route.getTripLength() - shortestPossible) / dx);
        //    bins[binIndex]++;
        //    numberOfSolns++;

        //    // until the temperature reaches a certain point OR a sufficient solution is found
        //    while (temp > finalTemp && best.getTripLength() > 3.3f)
        //    {
        //        Trip newRoute = route;
        //        Random random = new Random();

        //        int swap1 = random.Next(newRoute.route.Count());
        //        int swap2 = random.Next(newRoute.route.Count());

        //        // if they're the same, make a new swap1 index
        //        while (swap1 == swap2)
        //            swap1 = random.Next(newRoute.route.Count());

        //        newRoute.swapCities(swap1, swap2);

        //        double delta = newRoute.getTripLength() - route.getTripLength();
        //        double val = k * temp;

        //        sum += newRoute.getTripLength();
        //        stdDev += (newRoute.getTripLength() * newRoute.getTripLength());
        //        binIndex = (int)((newRoute.getTripLength() - shortestPossible) / dx);
        //        bins[binIndex]++;
        //        numberOfSolns++;

        //        if (delta < 0)
        //        {
        //            route = newRoute;
        //        }

        //        else if ((delta >= 0 && ((1 / val) * Math.Exp(-delta / (val))) > random.Next() / val))
        //        {
        //            route = newRoute;
        //        }

        //        if (best.getTripLength() > route.getTripLength())
        //            best = route;
        //        if (worst.getTripLength() < route.getTripLength())
        //            worst = route;

        //        TimeSpan duration = DateTime.Now - startTime;
        //        //temp = temp / (1 + (Convert.ToDouble(duration.Milliseconds) / 1000));
        //        temp *= .99999;
        //    }
        //    best.printTrip();
        //    Logger.Out(best.getTripLength().ToString());
        //    worst.printTrip();
        //    Logger.Out(worst.getTripLength().ToString());
        //    double mean = sum / numberOfSolns;
        //    stdDev = stdDev - (Math.Pow(sum, 2) / numberOfSolns);
        //    stdDev = Math.Sqrt(stdDev / (numberOfSolns - 1));
        //    Logger.Out("Mean = " + mean + "\nStandard Deviation: " + stdDev);
        //    for (int i = 0; i < bins.Length; i++)
        //    {
        //        Logger.Out(bins[i].ToString());
        //    }
        //    Console.ReadKey();
        //}
    }
}
