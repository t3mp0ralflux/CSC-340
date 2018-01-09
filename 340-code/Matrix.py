__author__ = 'Darryl'

'''
Class for housing, displaying and performing arithmetic on data represented as a matrix
'''


class Matrix:
    def __init__(self, data):
        self.data = data
        self.rows = len(data)
        self.cols = len(data[0])
        self.size = [self.rows, self.cols]

    def get_cols(self):
        return self.cols

    def get_rows(self):
        return self.rows

    def get_data(self):
        return self.data

    def get_size(self):
        return self.size

    def set_size(self):
        self.size = [self.rows, self.cols]

    def set_rows(self):
        self.rows = len(self.data)
        self.set_size()

    def set_cols(self):
        self.cols = len(self.data[0])
        self.set_size()

    # Prints the matrix in a easy to view format
    def print_data(self):
        for i in range(self.rows):
            print(self.data[i])

    # Returns the result matrix of two matrices added together
    def add(self, other):
        if self.size == other.size:
            res = []
            for i in range(self.rows):
                row = []
                for j in range(self.cols):
                    row.append(self.data[i][j] + other.data[i][j])
                res.append(row)
            return Matrix(res)
        else:
            return self.zero()

    # Returns the result matrix of two matrices subtracted
    def subtract(self, other):
        if self.size == other.size:
            res = []
            for i in range(self.rows):
                row = []
                for j in range(self.cols):
                    row.append(self.data[i][j] - other.data[i][j])
                res.append(row)
            return Matrix(res)
        else:
            return self.zero()

    # Returns the zero matrix for a given square matrix
    def zero(self):
        res = []
        for i in range(self.rows):
            res.append([0] * self.cols)
        return Matrix(res)

    # Returns the identity matrix for a square matrix
    def identity(self):
        res = self.zero()
        for i in range(self.cols):
            res.data[i][i] = 1
        return res

    # Returns the output of two matrices multiplied
    def multiply(self, other):
        if self.cols == other.rows:
            answer = []

            for i in range(self.rows):
                answer.append([0] * other.cols)

            for i in range(self.rows):
                for j in range(other.cols):
                    for k in range(other.rows):
                        answer[i][j] += self.data[i][k] * other.data[k][j]

            return Matrix(answer)
        else:
            return self.zero()

    # Scales each item of a matrix by a number
    def scaler(self, scale):
        for i in self.data:
            for j in range(self.cols):
                i[j] = i[j] * scale

    # Transposes a Matrix
    def transpose(self):
        self.data = [list(x) for x in zip(*self.data)]
        self.set_cols()
        self.set_rows()

    # Returns the trace of a matrix - the summation of the diagonal.
    def trace(self):
        res = 0
        for i in range(self.cols):
            res += self.data[i][i]
        return res

    # Returns the max absolute value of a matrix
    def find_max(self):
        res = abs(self.get_data()[0][0])
        for i in range(self.rows):
            for j in range(self.cols):
                if abs(self.get_data()[i][j]) > res:
                    res = abs(self.get_data()[i][j])

        return res


'''
mat = Matrix([[2, 3], [4, 5]])
mat1 = Matrix([[6, -11, 6], [1, 0, 0], [0, 1, 0]])
mat2 = Matrix([[7, 8], [9, 10], [11, 12]])
mat3 = Matrix([[-1, 0, 0.09]])
mat4 = Matrix([[-5.45, -1, 0]])
mat5 = Matrix([[0], [1], [0]])

temp = Matrix(mat1.get_data())

temp.transpose()
mat3.transpose()
a = mat4.multiply(mat3)

a.print_data()
print(a.get_data())

print(mat1.find_max())

print(mat3.get_data()[0][0])

mat1.multiply(mat5).print_data()

'''

