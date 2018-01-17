/* A matrix class to use for CSC 340*/
/* Written by Phillip McLaurin */

public class Matrix {

	private double[][] matrix; 

	public Matrix (double[][] values) {
		matrix = values;
	}

	public int getRows () {
		return matrix.length;
	}

	public int getCols () {
		return matrix[0].length;
	}

	public double getVal (int i, int j) {
		return matrix[i][j];
	}

	// scalar multiplication of the matrix
	public Matrix multiply (double scalar) {
		Matrix temp = new Matrix (matrix);
		for (int i=0; i < matrix.length; i++) {
			for (int j=0; j < matrix[i].length; j++) {
				temp.matrix[i][j] *= scalar;
			}
		}
		return temp;
	}

	// matrix multiplication
	public Matrix multiply (Matrix other) {
		if (other.getCols() != getRows())
			return null;

		double[][] c = new double[getRows()][other.getCols()];
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < other.getCols(); j++) {
				for (int k=0; k < getCols(); k++) {
					c[i][j] = c[i][j] + (matrix[i][k] * other.getVal(k, j));
				}
			}
		}
		Matrix temp = new Matrix(c);
		return temp;
	}

	// adds matrix with matrix passed in
	public Matrix add (Matrix other) {
		if (getRows() != other.getRows() || getCols() != other.getCols()) {
			return null;
		}
		double[][] temp = new double[getRows()][getCols()];
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < getCols(); j++) {
				temp[i][j] = matrix[i][j] + other.matrix[i][j];
			}
		}
		return new Matrix(temp);
	}

	public Matrix subtract (Matrix other) {
		if (getRows() != other.getRows() || getCols() != other.getCols()) {
			return null;
		}
		double[][] temp = new double[getRows()][getCols()];
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < getCols(); j++) {
				temp[i][j] = matrix[i][j] - other.matrix[i][j];
			}
		}
		return new Matrix(temp);
	}

	// prints the matrix
	public void printMatrix () {
		for (int i=0; i < getRows(); i++) {
			System.out.print("[");
			for (int j=0; j < getCols(); j++) {
				System.out.print(matrix[i][j] + ", ");
			}
			System.out.println("]");
		}
	}

	// returns the transpose matrix
	public Matrix transpose () {
		double[][] temp = new double[getCols()][getRows()];
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < getCols(); j++) {
				temp[j][i] = matrix[i][j];
			}
		}
		return new Matrix(temp);
	}

	// interchanges two rows
	public void interchange (int rowA, int rowB) {
		double[] temp = new double[getCols()];
		for (int i=0; i < getCols(); i++) {
			temp[i] = getVal(rowA, i);
			matrix[rowA][i] = matrix[rowB][i];
			matrix[rowB][i] = temp[i];
		}
	}

	// multiplies a row by a constant
	public void rowMultiply (int row, double scalar) {
		for (int i=0; i < getCols(); i++) {
			matrix[row][i] *= scalar;
		}
	}

	// adds rowB to rowA
	public void rowAdd (int rowA, int rowB) {
		for (int i=0; i < getCols(); i++) {
			matrix[rowA][i] += matrix[rowB][i];
		}
	}

	// augments the matrix with nx1 2d array
	public Matrix augment (double[][] values) {
		// makes a 2d array that is the size of augmented matrix
		double[][] newMatrix = new double[getRows()][getCols() + 1];
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < getCols(); j++) {
				newMatrix[i][j] = matrix[i][j];
			}
			newMatrix[i][getCols()] = values[i][0];
		}
		return new Matrix(newMatrix);
	}

	// augments the matrix with another matrix
	public Matrix augment (Matrix mat) {
		// makes a 2d array that is the size of augmented matrix
		double[][] newMatrix = new double[getRows()][getCols() + mat.getCols()];
		for (int i=0; i < newMatrix.length; i++) {
			for (int j=0; j < newMatrix[0].length; j++) {
				if (j < getCols()) {
					newMatrix[i][j] = matrix[i][j];
				}
				else {
					newMatrix[i][j] = mat.matrix[i][j-getCols()];
				}
			}
			newMatrix[i][getCols()] = mat.getVal(i, 0);
		}
		return new Matrix(newMatrix);
	}

	// performs Gauss-Jordan elimination on the matrix
	public Matrix gaussJordan (Matrix b) {
		int e = 1;
		int p;

		// augment current matrix with the values
		Matrix C = augment(b);

		for (int j=0; j < C.getRows(); j++) {
			p = j;

			// loop through each row and find the pivot
			for (int i=0; i < C.getRows(); i++) {

				// look at the absolute value of the pivot and see if it is larger than current pivot
				// if so, make that the new pivot
				if (Math.abs(C.matrix[p][j]) < Math.abs(C.matrix[i][j])) {
					p = i;
				}
			}
			// if an entire column is 0s, there is no sln
			if (C.matrix[p][j] == 0) {
				e = 0;
			}
			// move the pivot row to row j
			if (p > j) {
				C.interchange(j, p);
			}

			// divide row j by the leading coefficient
			C.rowMultiply(j, (1.0/C.getVal(j, j)));
			// back substitution
			for (int i=0; i < C.getRows(); i++) {
				if (i != j) {
					double Cij = C.matrix[i][j];
					for (int k=0; k < C.getCols(); k++) {
						C.matrix[i][k] = C.matrix[i][k] - C.matrix[j][k] * Cij;
					}
				}
			}

		}

		// if there is no unique soln, return null
		if (e == 0)
			return null;
		else
			return C;
	}


	// returns a matrix with the solns
	public double[] gaussianElim  (Matrix b) {
		int _E = 1;
		int p;

		// augment current matrix with the values
		Matrix C = augment(b);

		for (int j=0; j < C.getRows(); j++) {
			p = j;

			// loop through each row and find the pivot
			for (int i=0; i < C.getRows(); i++) {

				// look at the absolute value of the pivot and see if it is larger than current pivot
				// if so, make that the new pivot
				if (Math.abs(C.matrix[p][j]) < Math.abs(C.matrix[i][j])) {
					p = i;
				}
			}

			// if an entire column is zeroes, return 0. no unique soln
			if (C.matrix[p][j] == 0) {
				_E = 0;
			}

			// interchange if the pivot is below row j
			if (p > j) {
				C.interchange(j, p);
			}

			// divide the row by the leading coefficient
			C.rowMultiply(j, (1.0/C.getVal(j, j)));

			for (int i=0; i < C.getRows(); i++) {
				if (i > j) {
					double Cij = C.matrix[i][j];
					double Cjj = C.matrix[j][j];
					for (int k=0; k < C.getCols(); k++) {
						C.matrix[i][k] = C.matrix[i][k] - (C.matrix[j][k] * (Cij/Cjj));
					}
				}
			}

		}
   
   		// return null if no unique soln
   		if (_E == 0)
   			return null;

   		// create partitions D and e
		double[][] D = new double[C.getRows()][C.getCols() - 1];
		double[][] e = new double[C.getRows()][1];
		
		// populate the partitions
		for (int i=0; i < C.getRows(); i++) {
			for (int j=0; j < C.getCols() - 1; j++ ) {
				D[i][j] = C.getVal(i, j);
			}
		}

		// populate the partitions
		for (int i=0; i < e.length; i++)
			e[i][0] = C.getVal(i, C.getCols() - 1);

		double[] x = new double[C.getRows()];
		double sum;

		// back substitution
		for (int j=D.length - 1; j >= 0; j--) {
			sum = 0;
			for (int i=j+1; i < D.length; i++) {
				sum = sum + (D[j][i] * x[i]);
			}
			x[j] = (e[j][0] - sum)/D[j][j];
		}

		return x;
	}

	// find the inverse matrix
	public Matrix findInverse () {
		double[][] temp = new double[getRows()][getCols()];

		// error check
		if (getRows() != getCols())
			return null;

		// augments the matrix with its corresponding indentity matrix to find inverse
		for (int i=0; i < getRows(); i++) {
			for (int j=0; j < getCols(); j++) {
				if (i == j) {
					temp[i][j] = 1;
				}
			}
		}

		// turns temp into a matrix object and then returns the inverse
		Matrix inverse = new Matrix(temp);
		inverse = gaussJordan(inverse);

		// create a new array to store the inverse matrix
		temp = new double[inverse.getRows()][inverse.getCols()/2];

		// we have to start the column index j, at the half way mark, since our matrix is still augmented
		for (int i=0; i < inverse.getRows(); i++) {
			for (int j=(inverse.getCols()/2); j < inverse.getCols(); j++) {
				temp[i][j-inverse.getCols()/2] = inverse.matrix[i][j];
			}
		}

		return new Matrix(temp);

	}

	public double findDeterminant() {
		int p;
		int r = 0;
		double determinant = 0;

		// augment current matrix with the values
		Matrix C = new Matrix(matrix);

		for (int j=0; j < C.getRows(); j++) {
			p = j;

			// loop through each row and find the pivot
			for (int i=0; i < C.getRows(); i++) {

				// look at the absolute value of the pivot and see if it is larger than current pivot
				// if so, make that the new pivot
				if (Math.abs(C.matrix[p][j]) < Math.abs(C.matrix[i][j])) {
					p = i;
				}
			}

			// interchange if the pivot is below row j
			if (p > j) {
				C.interchange(j, p);
				r++;
			}

			for (int i=0; i < C.getRows(); i++) {
				if (i > j) {
					double Cij = C.matrix[i][j];
					double Cjj = C.matrix[j][j];
					for (int k=0; k < C.getCols(); k++) {
						C.matrix[i][k] = C.matrix[i][k] - (C.matrix[j][k] * (Cij/Cjj));
					}
				}
			}

		}

		// initialize determinant to the value in C00
		determinant = C.matrix[0][0];
		// then multiply down the diagoanl
		for (int i=1; i < C.getRows(); i++) {
			determinant = determinant * C.matrix[i][i];
		}

		// put the negative/positive sign if needed
		determinant = determinant * Math.pow((-1), r);

		return determinant;
	}

	public double trace() {
		if (getRows() != getCols()) {
			System.out.println("Not a square matrix, returning 0");
			return 0;
		}
		double sum = 0;
		for (int i=0; i < getRows(); i++) {
			sum += matrix[i][i];
		}

		return sum;

	}

}