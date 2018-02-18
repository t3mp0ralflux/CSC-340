using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    public class Trip
    {
        private double tripLength = 0;

        // initialize all of the cities
        private City dummy = new City("ERROR", 0, 0);
        private City A = new City("A", 0.725228831, 0.028301616);
        private City B = new City("B", 0.632613331, .879012261);
        private City C = new City("C", .085878084, .352754499);
        private City D = new City("D", .880437853, .852414005);
        private City E = new City("E", .725231388, .382031121);
        private City F = new City("F", .74550796, .32391051);
        private City G = new City("G", .166612034, .10383117);
        private City H = new City("H", .637364579, .962848092);
        private City I = new City("I", .136222778, .908730558);
        private City J = new City("J", .078296453, .248218346);
        private City K = new City("K", .396853572, .962644387);
        private City L = new City("L", .390728772, .703886694);
        private City M = new City("M", .276887516, .354859612);
        private City N = new City("N", .974681576, .344309411);

        // store them in the cityList with an initialization block
        public List<City> cityList = new List<City>();

        public void init()
        {
            cityList.Add(dummy);
            cityList.Add(A);
            cityList.Add(B);
            cityList.Add(C);
            cityList.Add(D);
            cityList.Add(E);
            cityList.Add(F);
            cityList.Add(G);
            cityList.Add(H);
            cityList.Add(I);
            cityList.Add(J);
            cityList.Add(K);
            cityList.Add(L);
            cityList.Add(M);
            cityList.Add(N);
        }
	

    // a nested class that holds a city
    public class City
    {
        private String name;
        private double x;
        private double y;

        public City(String name, double x, double y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }

        public String getName()
        {
            return name;
        }

    }

    public List<City> route = new List<City>();

    // constructor for creating a trip
    public Trip(int[] val, int V)
    {
            init();
        for (int i = 1; i <= V; i++)
        {
            route.Add(cityList[val[i]]);
        }
        calcTripLength();
    }

    // another constructor, less parameters.  used to create a random trip
    public Trip()
    {
            init();
            cityList.Remove(dummy);
        cityList.TrimExcess();
        Random random = new Random();
        int val = 0;
        int run = 14;                        //change to 14 after initial run
        for (int i = 0; i < run; i++)
        {
            val = random.Next(cityList.Count());
            route.Add(cityList[val]);
            cityList.Remove(cityList[val]);
        }
        calcTripLength();
    }

    // prints the trip
    public void printTrip()
    {
        for (int i = 0; i < route.Count(); i++)
        {
            if (route[i] != null)
                Logger.Out(route[i].getName() + " ");
        }
        if (route[0] != null)
            Logger.Out(route[0].getName());
        Console.WriteLine();
    }

    // calculates the length of the trip and stores in tripLength
    public void calcTripLength()
    {
        tripLength = 0;
        for (int i = 0; i < route.Count() - 1; i++)
        {
            // if we are not on the last city we can use the first part of the if
            tripLength += cityDistance(route[i], route[i + 1]);

        }
        tripLength += cityDistance(route[route.Count() - 1], route[0]);
    }

    // calculate the distance between two citie using distance formula
    public double cityDistance(City one, City two)
    {
        return Math.Sqrt(Math.Pow((one.getX() - two.getX()), 2) + Math.Pow((one.getY() - two.getY()), 2));

    }

    public double getTripLength()
    {
        return tripLength;
    }

    public void makeRouteNull()
    {
        for (int i = 0; i < route.Count(); i++)
        {
            route[i] = null;
        }
    }

    public void swapCities(int one, int two)
    {
        City temp = route[one];
        route[one] =  route[two];
        route[two] = temp;
    }
}
}
