using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{

    public class TSPExhaust
    {
        public static void Import(List<Matrix> class1)
        {
            String currentLine = "";

            var sr = new StreamReader(dataFile, Encoding.UTF8, true, 128);

            //skips N number of lines
            for (int n = 0; n < 2; n++)
            {
                sr.ReadLine();
            }
            while ((currentLine = sr.ReadLine()) != null)
            {
                // currentLine = sr.ReadLine();
                string[] line = currentLine.Split('\t');
                double[,] matrix1 = new double[1, 2];
                double[,] matrix2 = new double[1, 2];
                matrix1[0, 0] = Double.Parse(line[0]);
                matrix1[0, 1] = Double.Parse(line[1]);
                class1.Add(new Matrix(matrix1));
            }

            sr.Close();
        }

        private static String dataFile = Directory.GetCurrentDirectory().ToString() + "\\eigendata.txt";

	    private static List<Matrix> list = new List<Matrix>();
        private static List<Trip> population = new List<Trip>();
        private static Trip shortest;
        private static Trip longest;
        private static double maxTripLength;
        private static double minTripLength;
        private static double longestPossible = 1.094372401818199 * 14;
        private static double shortestPossible = .05431844593233095 * 14;
        private static double dx = (longestPossible - shortestPossible) / 100;
        private static double[] bins = new double[100];

        // code for creating permutations
        static int[] val;
        static int now = -1;

        static int V = 14;
        static int count = 0;

        //number of trips created
        static double runs = 0;
        static double sumOfDist = 0;
        static double sumSqdDist = 0;

//        public static void Main(String[] args){

//		Matrix mean = list[0];

//		for (int i = 1; i<list.Count(); i++) {
//			mean = mean.add(list[i]);
//		}

//        mean = mean.multiply(1.0/list.Count());
//		mean.printMatrix();

//	//	Console.WriteLine("x=" + (x/count) + "y=" + (y/count));

//		// covariance matrix calculation
//		Matrix cov1;

//        cov1 = list[0].subtract(mean);
//        cov1 = cov1.multiply(cov1.transpose());

//		for (int i = 1; i<list.Count(); i++) {
//			Matrix temp = list[i];
//            temp = temp.subtract(mean);
//			temp = temp.multiply(temp.transpose());
//			cov1 = cov1.add(temp);
//		}

//		cov1 = cov1.multiply(1.0/list.Count());
//		cov1.printMatrix();

//		Console.WriteLine("\n\n");

//        Console.WriteLine("trace of covariance matrix is: " + Project2.trace(cov1));
//        Console.WriteLine("determinant of cov matrix is: " + cov1.findDeterminant());

//		Console.WriteLine("\n\n\n\n\n\n");

//		//remove any garbage from the bin counter
//		for (int i = 0; i<bins.Length; i++)
//			bins[i] = 0;


//        // used for exhaustive search
//        // long startTime = System.currentTimeMillis();
//        // getPerms();
//        // long endTime = System.currentTimeMillis();


//        /* this code was used to find the shortest and longest distance between cities
//		   no reason to run it after finding the answers
//		   double furthestDistance = 0;
//		   double shortestDistance = 1000000; //arbitrary large value */

//        // for (int i=0; i < shortest.cityList.size(); i++) {
//        // 	for (int j=0; j < shortest.cityList.size(); j++) {
//        // 		curr = shortest.cityDistance(shortest.cityList.get(i), shortest.cityList.get(j));
//        // 		if (curr > furthestDistance && i != j)
//        // 			furthestDistance = curr;
//        // 		if (curr < shortestDistance && i != j)
//        // 			shortestDistance = curr;
//        // 	}
//        // }

//        randomSearch();

//double tripMean;
//double stdDev;

//tripMean = sumOfDist / runs;

//		stdDev = sumSqdDist - Math.Pow(sumOfDist, 2) / runs;
//		stdDev = Math.Sqrt(stdDev / (double)(runs - 1));

//		Console.WriteLine("mean: " + tripMean + "\nstdDev: " + stdDev);
//        Console.WriteLine("longest trip route: ");
//        longest.printTrip();
//		Console.WriteLine("longest trip length: " + longest.getTripLength());
//		Console.WriteLine("shortest trip: ");
//        shortest.printTrip();
//		Console.WriteLine("shortest trip length: " + shortest.getTripLength());

//		//System.out.println("\n\nduration: " + (endTime - startTime)/1000f + " seconds\n\n");

//		for (int i = 0; i<bins.Length; i++)
//			Console.WriteLine(bins[i]);

//	}

	public static void getPerms()
{
    val = new int[V + 1];
    for (int i = 0; i <= V; i++)
        val[i] = 0;
    p(0);
}

public static void p(int k)
{
    now++;
    val[k] = now;
    if (now == V) handleP();
    for (int i = 1; i <= V; i++)
        if (val[i] == 0) p(i);
    now--;
    val[k] = 0;
}

public static void handleP()
{
    count++;
    Trip temp = new Trip(val, V);
    if (runs == 0)
    {
        shortest = temp;
        longest = temp;
    }

    if (temp.getTripLength() > longest.getTripLength())
        longest = temp;

    else if (temp.getTripLength() < shortest.getTripLength())
        shortest = temp;

    sumOfDist += temp.getTripLength();
    sumSqdDist += (temp.getTripLength() * temp.getTripLength());
    int binIndex = (int)((temp.getTripLength() - shortestPossible) / dx);
    bins[binIndex] += 1;
    runs++;

}

// used to make a million random trips
public static void randomSearch()
{
    for (int i = 0; i < 1000000; i++)
    {
        Trip temp = new Trip();
        if (runs == 0)
        {
            shortest = temp;
            longest = temp;
        }

        if (temp.getTripLength() > longest.getTripLength())
            longest = temp;

        if (temp.getTripLength() < shortest.getTripLength())
            shortest = temp;

        sumOfDist += temp.getTripLength();
        sumSqdDist += (temp.getTripLength() * temp.getTripLength());
        int binIndex = (int)((temp.getTripLength() - shortestPossible) / dx);
        bins[binIndex] += 1;
        runs++;
    }
}
    }
}
