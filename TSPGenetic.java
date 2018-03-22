import java.util.*;

public class TSPGenetic {

	private static ArrayList<Trip> currentPopulation = new ArrayList<Trip>();
	private static int fittest;
	private static int longest;
	private static double maxTripLength;
	private static double minTripLength;

	// variables relating to the workings of the algorithm
	private static double generation = 0;
	private static int maxPopulation = 50; // the size of the population
	private static final int heatDeath = 100000; // the max number of generations
	private static final double shouldMutate = .90;

	// variables relating to statistical analysis
	private static double longestPossible = 1.094372401818199 * 14;
	private static double shortestPossible = .05431844593233095 * 14;
	private static double dx = (longestPossible - shortestPossible) / 50;
	private static double[] bins = new double[50];
	private static double sum = 0;
	private static double stdDev = 0;
	private static double numberOfSolns = 0;

	public static void main(String[] args) {
		generatePop();
		while (generation != heatDeath) {
			ageGeneration();
		}
		System.out.print("fittest: ");
		currentPopulation.get(fittest).printTrip();
		System.out.println("fit length: " + currentPopulation.get(fittest).getTripLength());
		System.out.print("least fit length: ");
		currentPopulation.get(longest).printTrip();
		System.out.println("least fit length: " + currentPopulation.get(longest).getTripLength());
		double mean = sum / numberOfSolns;
		stdDev = stdDev - (Math.pow(sum, 2) / numberOfSolns);
		stdDev = Math.sqrt(stdDev / (numberOfSolns - 1));
		System.out.println("Mean = " + mean + "\nStandard Deviation: " + stdDev);
		for (int i=0; i < bins.length; i++) {
			System.out.println(bins[i]);
		}
	}

	public static void generatePop () {
		for (int i=0; i < maxPopulation; i++) {
			Trip temp = new Trip();
			currentPopulation.add(temp);
			sum += temp.getTripLength();
			stdDev += temp.getTripLength() * temp.getTripLength();
			int binIndex = (int)((temp.getTripLength() - shortestPossible)/dx);
			bins[binIndex]++;
			numberOfSolns++;
		}

		// Set 'adam' as both our fittest and least fit
		fittest = 0;
		longest = 0;

		// find the fittest
		for (int i=1; i < currentPopulation.size(); i++) {
			Trip curr = currentPopulation.get(i);
			Trip fit = currentPopulation.get(fittest);
			Trip not = currentPopulation.get(longest);
			if (testFitness(curr) > testFitness(not))
				longest = i;
			if (testFitness(curr) < testFitness(fit))
				fittest = i;
		}
	}

	public static void mutate (Trip mutant) {
		Random random = new Random();
		int swap1 = random.nextInt(mutant.route.size());
		int swap2 = random.nextInt(mutant.route.size());
		mutant.swapCities(swap1, swap2);
	}

	public static void breed (int m, int f) {
		// moreFit will be 0 if the mother is more fit than the father
		// it will be 1 if the father is more fit than the mother
		int moreFit;
		// use the index to set the mother and father
		Trip mother = currentPopulation.get(m);
		Trip father = currentPopulation.get(f);
		Trip child = new Trip();
		//make every entry in the childs route null
		child.makeRouteNull();
		
		for (int i=0; i < 7; i++)
			child.route.set(i, mother.route.get(i));

		int fatherDNAIndex = 7;
		for (int i=0; i < father.route.size(); i++) {
			for (int j=0; j < child.route.size(); j++) {
				if (father.route.get(i).equals(child.route.get(j)))
					break;
				else if (child.route.get(j) == null) {
					child.route.set(fatherDNAIndex, father.route.get(i));
					fatherDNAIndex++;
					break;
				}
			}
		}

		// decides if a mutation should occur
		if (Math.random() > shouldMutate)
			mutate(child);

		// test to see if the mother or father is more fit
		if (testFitness(mother) < testFitness(father))
			moreFit = 0;
		else
			moreFit = 1;

		// if the mother is the least fit of the family, replace the mother with child
		if (testFitness(child) < testFitness(mother) && moreFit == 1) {
			currentPopulation.set(m, child);
			sum += child.getTripLength();
			stdDev += child.getTripLength() * child.getTripLength();
			int binIndex = (int)((child.getTripLength() - shortestPossible)/dx);
			bins[binIndex]++;
			numberOfSolns++;
		}
		// if the father is the least, replace father with child
		else if (testFitness(child) < testFitness(father) && moreFit == 0) {
			currentPopulation.set(f, child);
			sum += child.getTripLength();
			stdDev += child.getTripLength() * child.getTripLength();
			int binIndex = (int)((child.getTripLength() - shortestPossible)/dx);
			bins[binIndex]++;
			numberOfSolns++;
		}

	}

	public static void ageGeneration () {
		generation++;
		for (int i=0; i < currentPopulation.size(); i++) {
			Trip curr = currentPopulation.get(i);
			Trip fit = currentPopulation.get(fittest);
			Trip not = currentPopulation.get(longest);
			if (testFitness(curr) > testFitness(not))
				longest = i;
			if (testFitness(curr) < testFitness(fit))
				fittest = i;
		}
		int fitIndex = fittest;
		int unfitIndex = longest;

		// breed the entire population with the fittest
		for (int i=0; i < currentPopulation.size(); i++) {
			if (i != fitIndex)
				breed(fitIndex, i);
		}
		whoIsFittest();
	}

	public static void whoIsFittest () {
		for (int i=0; i < currentPopulation.size(); i++) {
			Trip curr = currentPopulation.get(i);
			Trip fit = currentPopulation.get(fittest);
			if (testFitness(curr) < testFitness(fit))
				fittest = i;
		}
	}

	public static double testFitness (Trip t) {
		t.calcTripLength();
		return t.getTripLength();
	}

}