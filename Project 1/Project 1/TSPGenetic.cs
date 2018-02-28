using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    public class TSPGenetic
    {
        private static List<Trip> currentPopulation = new List<Trip>();
        private static int fittest;
        private static int longest;
        private static double maxTripLength;
        private static double minTripLength;
        public static StringBuilder LogString = new StringBuilder();

        // variables relating to the workings of the algorithm
        private static double generation = 0;
        private static int maxPopulation = 50; // the size of the population
        private static int heatDeath = 100000; // the max number of generations
        private static double shouldMutate = .90;

        // variables relating to statistical analysis
        private static double longestPossible = 1.094372401818199 * 14;
        private static double shortestPossible = .05431844593233095 * 14;
        private static double dx = (longestPossible - shortestPossible) / 50;
        private static double[] bins = new double[50];
        private static double sum = 0;
        private static double stdDev = 0;
        private static double numberOfSolns = 0;

        //for the saving of data


<<<<<<< HEAD
        public static void Main(String[] args)
        {
            Logger.LogString = null;
            generatePop();
            while (generation != heatDeath)
            {
                ageGeneration();
            }
            Logger.Out("fittest: ");
            currentPopulation[fittest].printTrip();
            Logger.Out("fit length: " + currentPopulation[fittest].getTripLength());
            Logger.Out("least fit length: ");
            currentPopulation[longest].printTrip();
            Logger.Out("least fit length: " + currentPopulation[longest].getTripLength());
            double mean = sum / numberOfSolns;
            stdDev = stdDev - (Math.Pow(sum, 2) / numberOfSolns);
            stdDev = Math.Sqrt(stdDev / (numberOfSolns - 1));
            Logger.Out("Mean = " + mean);
            Logger.Out("Standard Deviation: " + stdDev);
            for (int i = 0; i < bins.Length; i++)
            {
                Logger.Out(bins[i].ToString());
            }
            using (StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory().ToString() + "\\GeneticBins.txt", true))
            {
                file.WriteLine(Logger.LogString);
            }
            Console.ReadKey();

        }
=======
        //public static void Main(String[] args)
        //{
        //    Logger.LogString = null;
        //    generatePop();
        //    while (generation != heatDeath)
        //    {
        //        ageGeneration();
        //    }
        //    Logger.Out("fittest: ");
        //    currentPopulation[fittest].printTrip();
        //    Logger.Out("fit length: " + currentPopulation[fittest].getTripLength());
        //    Logger.Out("least fit length: ");
        //    currentPopulation[longest].printTrip();
        //    Logger.Out("least fit length: " + currentPopulation[longest].getTripLength());
        //    double mean = sum / numberOfSolns;
        //    stdDev = stdDev - (Math.Pow(sum, 2) / numberOfSolns);
        //    stdDev = Math.Sqrt(stdDev / (numberOfSolns - 1));
        //    Logger.Out("Mean = " + mean);
        //    Logger.Out("Standard Deviation: " + stdDev);
        //    for (int i = 0; i < bins.Length; i++)
        //    {
        //        Logger.Out(bins[i].ToString());  
        //    }
        //    using (StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory().ToString() + "\\GeneticBins.txt", true))
        //    {
        //        file.WriteLine(Logger.LogString);
        //    }
        //    Console.ReadKey();
        
        //}
>>>>>>> 000ffcbccd1ad8a7ac094e6475a2bc535f0074d0

        public static void generatePop()
        {
            for (int i = 0; i < maxPopulation; i++)
            {
                Trip temp = new Trip();
                currentPopulation.Add(temp);
                sum += temp.getTripLength();
                stdDev += temp.getTripLength() * temp.getTripLength();
                int binIndex = (int)((temp.getTripLength() - shortestPossible) / dx);
                bins[binIndex]++;
                numberOfSolns++;
            }

            // Set 'adam' as both our fittest and least fit
            fittest = 0;
            longest = 0;

            // find the fittest
            for (int i = 1; i < currentPopulation.Count(); i++)
            {
                Trip curr = currentPopulation[i];
                Trip fit = currentPopulation[fittest];
                Trip not = currentPopulation[longest];
                if (testFitness(curr) > testFitness(not))
                    longest = i;
                if (testFitness(curr) < testFitness(fit))
                    fittest = i;
            }
        }

        public static void mutate(Trip mutant)
        {
            Random random = new Random();
            int swap1 = random.Next(mutant.route.Count());
            int swap2 = random.Next(mutant.route.Count());
            mutant.swapCities(swap1, swap2);
        }

        public static void breed(int m, int f)
        {
            // moreFit will be 0 if the mother is more fit than the father
            // it will be 1 if the father is more fit than the mother
            int moreFit;
            // use the index to set the mother and father
            Trip mother = currentPopulation[m];
            Trip father = currentPopulation[f];
            Trip child = new Trip();
            //make every entry in the childs route null
            child.makeRouteNull();

            for (int i = 0; i < 7; i++)
                child.route[i] = mother.route[i];

            int fatherDNAIndex = 7;
            for (int i = 0; i < father.route.Count(); i++)
            {
                for (int j = 0; j < child.route.Count(); j++)
                {
                    if (father.route[i].Equals(child.route[j]))
                        break;
                    else if (child.route[j] == null)
                    {
                        child.route[fatherDNAIndex] = father.route[i];
                        fatherDNAIndex++;
                        break;
                    }
                }
            }

            // decides if a mutation should occur
            Random rnd = new Random();
            int rndnumber = rnd.Next();
            if ( rndnumber > shouldMutate)
                mutate(child);

            // test to see if the mother or father is more fit
            if (testFitness(mother) < testFitness(father))
                moreFit = 0;
            else
                moreFit = 1;

            // if the mother is the least fit of the family, replace the mother with child
            if (testFitness(child) < testFitness(mother) && moreFit == 1)
            {
                currentPopulation[m] = child;
                sum += child.getTripLength();
                stdDev += child.getTripLength() * child.getTripLength();
                int binIndex = (int)((child.getTripLength() - shortestPossible) / dx);
                bins[binIndex]++;
                numberOfSolns++;
            }
            // if the father is the least, replace father with child
            else if (testFitness(child) < testFitness(father) && moreFit == 0)
            {
                currentPopulation[f] = child;
                sum += child.getTripLength();
                stdDev += child.getTripLength() * child.getTripLength();
                int binIndex = (int)((child.getTripLength() - shortestPossible) / dx);
                bins[binIndex]++;
                numberOfSolns++;
            }

        }

        public static void ageGeneration()
        {
            generation++;
            for (int i = 0; i < currentPopulation.Count(); i++)
            {
                Trip curr = currentPopulation[i];
                Trip fit = currentPopulation[fittest];
                Trip not = currentPopulation[longest];
                if (testFitness(curr) > testFitness(not))
                    longest = i;
                if (testFitness(curr) < testFitness(fit))
                    fittest = i;
            }
            int fitIndex = fittest;
            int unfitIndex = longest;

            // breed the entire population with the fittest
            for (int i = 0; i < currentPopulation.Count(); i++)
            {
                if (i != fitIndex)
                    breed(fitIndex, i);
            }
            whoIsFittest();
        }

        public static void whoIsFittest()
        {
            for (int i = 0; i < currentPopulation.Count(); i++)
            {
                Trip curr = currentPopulation[i];
                Trip fit = currentPopulation[fittest];
                if (testFitness(curr) < testFitness(fit))
                    fittest = i;
            }
        }

        public static double testFitness(Trip t)
        {
            t.calcTripLength();
            return t.getTripLength();
        }
    }
}
