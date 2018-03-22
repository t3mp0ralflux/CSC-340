import java.util.*;

public class Trip {

	private double tripLength = 0;

	// initialize all of the cities
	private City dummy = new City ("ERROR", 0, 0);
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
	public ArrayList<City> cityList = new ArrayList<City>();
	{
		cityList.add(dummy);
		cityList.add(A);
		cityList.add(B);
		cityList.add(C);
		cityList.add(D);
		cityList.add(E);
		cityList.add(F);
		cityList.add(G);
		cityList.add(H);
		cityList.add(I);
		cityList.add(J);
		cityList.add(K);
		cityList.add(L);
		cityList.add(M);
		cityList.add(N);
	}

	// a nested class that holds a city
	class City {
		private String name;
		private double x;
		private double y;

		public City (String name, double x, double y) {
			this.name = name;
			this.x = x;
			this.y = y;
		}

		public double getX () {
			return x;
		}

		public double getY () {
			return y;
		}

		public String getName () {
			return name;
		}

	}

	public ArrayList<City> route = new ArrayList<City>();

	// constructor for creating a trip
	public Trip (int[] val, int V) {
		for (int i=1; i <= V; i++) {
			route.add(cityList.get(val[i]));
		}
		calcTripLength();
	}

	// another constructor, less parameters.  used to create a random trip
	public Trip () {
		cityList.remove(dummy);
		cityList.trimToSize();
		Random random = new Random();
		int val = 0;
		int run = 14;
		for (int i=0; i < run; i++) {
			val = random.nextInt(cityList.size());
			route.add(cityList.get(val));
			cityList.remove(val);
		}
		calcTripLength();
	}

	// prints the trip
	public void printTrip () {
		for (int i=0; i < route.size(); i++) {
			if (route.get(i) != null)
				System.out.print(route.get(i).getName() + " ");
		}
		if (route.get(0) != null)
			System.out.print(route.get(0).getName());
		System.out.println();
	}

	// calculates the length of the trip and stores in tripLength
	public void calcTripLength () {
		tripLength = 0;
		for (int i=0; i < route.size() - 1; i++) {
			// if we are not on the last city we can use the first part of the if
			tripLength += cityDistance(route.get(i), route.get(i+1));

		}
		tripLength += cityDistance(route.get(route.size()-1), route.get(0));
	}

	// calculate the distance between two citie using distance formula
	public double cityDistance (City one, City two) {
		return Math.sqrt(Math.pow((one.getX() - two.getX()), 2) + Math.pow((one.getY() - two.getY()), 2));

	}

	public double getTripLength () {
		return tripLength;
	}

	public void makeRouteNull () {
		for (int i=0; i < route.size(); i++) {
			route.set(i, null);
		}
	}

	public void swapCities (int one, int two) {
		City temp = route.get(one);
		route.set(one, route.get(two));
		route.set(two, temp);
	}

}