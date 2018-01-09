__author__ = 'Darryl'

'''
Class for finding eigenvalues and eigenvectors based on data points from a file.
Also helps with finding the roots of a polynomial equation.
'''

from Matrix import Matrix


class Operations:
    def __init__(self):
        self.data = []
        self.matrices = []
        self.mean = Matrix([[0, 0]])
        self.covariance = Matrix([[0, 0], [0, 0]])
        self.setup()
        self.set_mean()
        self.set_covariance()
        print(len(self.matrices))

    def get_mean(self):
        return self.mean

    def get_covariance(self):
        return self.covariance

    # Read in and prepare the eigendata from a file
    def setup(self):
        temp = []
        filename = open('eigendata.txt', 'r')
        for line in filename:
            row = line.strip()
            temp.append(row)
        for i in range(0, len(temp)):
            self.data.append(temp[i].split())
        for i in self.data:
            matrix = Matrix([[float(i[0]), float(i[1])]])
            self.matrices.append(matrix)

    # Calculate the mean and assign it to the Class variable self.mean
    def set_mean(self):
        for i in self.matrices:
            self.mean = self.mean.add(i)

        self.mean.scaler(1 / len(self.matrices))

    # Calculate the covariance and assign it to the Class variable self.covariance
    def set_covariance(self):
        for i in self.matrices:
            a = i.subtract(self.mean)
            a_transpose = Matrix(a.get_data())
            a_transpose.transpose()
            b = a_transpose.multiply(a)
            self.covariance = self.covariance.add(b)

        self.covariance.scaler(1 / len(self.matrices))

    # Implementation of the power method for finding the dominant eigenvalue for a square matrix
    # Returns both eigenvalues for a 2x2 matrix as well as a unit length vector.
    # If the matrix is not a 2x2 matrix mu will be the dominant eigenvalue.
    # iterations: max number of iterations allowed
    # mat: the covariance matrix of the matrix being tested
    # estimate: a matrix to begin with as the estimate
    def power(self, iterations, mat, estimate):
        r = 1
        k = 0
        m = iterations
        y = estimate
        x = mat.multiply(y)
        while r > .001 and k < m:
            max_x = x.find_max()
            x.scaler(1 / max_x)
            y = Matrix(x.get_data())
            x = mat.multiply(y)
            temp = Matrix(y.get_data())
            temp.transpose()
            a = temp.multiply(x).get_data()[0][0]
            b = temp.multiply(y).get_data()[0][0]
            mu = a / b
            r = Matrix(y.get_data())
            r.scaler(mu)
            r = r.subtract(x)
            r = r.find_max()
            k += 1

        y.scaler(1 / mu)

        return mu, mat.trace() - mu, y

    # Implementation of the leverrier method for finding the characteristic equation for a polynomial
    def leverrier(self, companion):
        res = []
        b = companion
        a = -(b.trace())
        res.append(a)
        for i in range(companion.get_rows() - 1, 0, -1):
            ai = companion.identity()
            ai.scaler(a)
            b = companion.multiply(b.add(ai))
            a = -(b.trace()) / (5 - i + 1)
            res.append(a)
        return res

    # This method is used to find the roots of a polynomial equation by providing the companion matrix
    # for the polynomial and an estimate matrix for the power method.
    # Based on Deflation from this site: http://www.maths.qmul.ac.uk/~wj/MTH5110/notes/notes08.pdf
    def find_roots(self, comp, est):
        count = comp.get_rows()
        for i in range(count):
            e1, e2, vector = self.power(1000, comp, est)
            print('comp eigen:', round(e1))
            scale = vector.get_data()[i][0]
            new = vector.multiply(Matrix([comp.get_data()[i]]))
            new.scaler(1 / scale)
            comp = comp.subtract(new)


test = Operations()
print('Mean:')
test.get_mean().print_data()
print('\nCovariance:')
test.get_covariance().print_data()
print('\nTrace: ', test.get_covariance().trace())
cov = test.get_covariance()
est = Matrix([[1], [1]])
e1, e2, vector = test.power(1000, cov, est)
print('\nEigenvalues:', e1, e2)
print('\nEigenvector:', vector.get_data(), '\n')
est = Matrix([[1], [1], [1]])
e1, e2, vector = test.power(1000, Matrix([[-4, 14, 0], [-5, 13, 0], [-1, 0, 2]]), est)
print('\nEigenvalues:', e1, e2)
print('\nEigenvector:', vector.get_data(), '\n')

comp = Matrix([[-3, 182, 330, -5653, -15015], [1, 0, 0, 0, 0], [0, 1, 0, 0, 0], [0, 0, 1, 0, 0], [0, 0, 0, 1, 0]])
comp.print_data()

print('\nCoefficients:', test.leverrier(comp))

est = Matrix([[1], [1], [1], [1], [1]])
e1, e2, vector = test.power(1000, comp, est)
print('\nLargest', e1)

est = Matrix([[1], [1], [1], [1], [1]])
test.find_roots(comp, est)

