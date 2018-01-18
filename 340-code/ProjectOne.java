import java.util.*;
import java.io.*;

/* Class used for reading in data and manipulating it for project 1 */
/* phillip mclaurin */

public class ProjectOne {

	private static final String dataFile = "C:\\Users\\BelangerB\\Desktop\\melanie\\CSC-340\\Project 1\\Project 1\\bin\\Debug\\p1data.txt";
	private static final String findingsFile = "C:\\Users\\Phillip\\Desktop\\Programs\\340\\p1findings.txt";

	private static ArrayList<Matrix> class1 = new ArrayList<Matrix>();
	private static ArrayList<Matrix> class2 = new ArrayList<Matrix>();

	public static void main (String[] args) throws FileNotFoundException {

		 
		String line = "";

		// skip over the first lines that do not contain data
		for (int i=0; i < 9; i++)
			fileReader.nextLine();

		// read in all the data
		// put class 1 in class1 arraylist
		// put class 2 in class2 arraylist
		while (fileReader.hasNext()) {
			line = fileReader.nextLine();
			Scanner stringReader = new Scanner(line);
			
			double[][] matrix1 = new double[2][1];
			double[][] matrix2 = new double[2][1];
			matrix1[0][0] = stringReader.nextDouble();
			matrix1[1][0] = stringReader.nextDouble();
			class1.add(new Matrix(matrix1));
			matrix2[0][0] = stringReader.nextDouble();
			matrix2[1][0] = stringReader.nextDouble();
			class2.add(new Matrix(matrix2));

		}

		fileReader.close();

		// initialize mean1 and mean2
		Matrix m1 = class1.get(0);
		Matrix m2 = class2.get(0);

		// add every other vector to m1 and then divide
		for (int i=1; i < class1.size(); i++) {
			m1 = m1.add(class1.get(i));
		}

		m1 = m1.multiply(1.0/class1.size());

		System.out.println("Mean Vector 1: ");
		m1.printMatrix();

		// add every other vector
		for (int i=1; i < class2.size(); i++) {
			m2 = m2.add(class2.get(i));
		}

		m2 = m2.multiply(1.0/class2.size());

		System.out.println("Mean Vector 2: ");
		m2.printMatrix();

		// covariance matrices
		Matrix cov1;
		Matrix cov2;

		// initialize them with the process given
		cov1 = class1.get(0).subtract(m1);
		cov1 = cov1.multiply(cov1.transpose());

		cov2 = class2.get(0).subtract(m2);
		cov2 = cov2.multiply(cov2.transpose());

		// after initializing use the process given to sum the rest
		for (int i=1; i < class1.size(); i++) {
			Matrix temp = class1.get(i);
			temp = temp.subtract(m1);
			temp = temp.multiply(temp.transpose());
			cov1 = cov1.add(temp);
		}

		for (int i=1; i < class2.size(); i++) {
			Matrix temp = class2.get(i);
			temp = temp.subtract(m2);
			temp = temp.multiply(temp.transpose());
			cov2 = cov2.add(temp);
		}

		// finally divide the sum by 1/k
		cov1 = cov1.multiply(1.0/class1.size());
		cov2 = cov2.multiply(1.0/class2.size());

		System.out.println("");
		System.out.println("The covariance matrix for class 1:");
		cov1.printMatrix();

		System.out.println("The covariance matrix for class 2:");
		cov2.printMatrix();
		System.out.println("");

		// find the determinants using gaussian elimination
		// see the findDeterminant function in Matrix.java
		// to see the process used

		double cov1Determinant = cov1.findDeterminant();
		double cov2Determinant = cov2.findDeterminant();

		System.out.println("Determinant of Covariance Matrix 1: " + cov1Determinant);
		System.out.println("Determinant of Covariance Matrix 2: " + cov2Determinant);

		System.out.println("");

		// find the inverses of the covariance matrices
		Matrix cov1Inverse = cov1.findInverse();
		Matrix cov2Inverse = cov2.findInverse();

		//cov1Inverse.printMatrix();
		//cov2Inverse.printMatrix();

		double[][] sys1 = {{1,2,-1,2,1,1,-2,-1},{2,-1,2,2,-1,-2,2,2},{-2,0,2,2,-1,0,-1,1}, {2,2,-3,3,2,2,1,0}, {0,0,2,3,-2,2,3,4}, {1,1,2,2,0,2,0,-1}, {-3,0,3,0,1,-3,0,-2}, {2,1,1,-2,1,0,1,1}};
		double[][] sys2 = {{1}, {2},{3},{4},{-1},{-2},{-3},{-4}};

		Matrix s1 = new Matrix(sys1);
		Matrix s2 = new Matrix(sys2);

		s2=s1.gaussJordan(s2);
		System.out.println(s1.findDeterminant());
		s2 = s1.findInverse();
		s2.printMatrix();
	}
}