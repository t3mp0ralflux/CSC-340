__author__ = 'Darryl'


class Matrix:
    def __init__(self):
        self.data = []
        self.x1 = []
        self.y1 = []
        self.x2 = []
        self.y2 = []
        self.class1 = []
        self.class2 = []

    def gauss(self, matrix):

        for i in range(0, len(matrix)):
            '''
            Find the maximum value for this column
            '''
            num = abs(matrix[i][i])
            row = i

            for j in range(i + 1, len(matrix)):
                if abs(matrix[j][i]) > num:
                    num = abs(matrix[j][i])
                    row = j
            '''
            Switch the maximum row with the current row
            '''
            for j in range(i, len(matrix) + 1):
                temp = matrix[row][j]
                matrix[row][j] = matrix[i][j]
                matrix[i][j] = temp
            '''
            Make the rows below this one 0
            in the current column
            '''
            for k in range(i + 1, len(matrix)):
                c = -matrix[k][i] / matrix[i][i]
                for j in range(i, len(matrix) + 1):
                    if i == j:
                        matrix[k][j] = 0
                    else:
                        matrix[k][j] += c * matrix[i][j]

        '''
        Solve the equation matrix x = b for the matrix
        '''
        x = [0] * len(matrix)
        for i in range(len(matrix) - 1, -1, -1):
            x[i] = matrix[i][len(matrix)] / matrix[i][i]

            for j in range(i - 1, -1, -1):
                matrix[j][len(matrix)] = matrix[j][len(matrix)] - matrix[j][i] * x[i]

        return x

    def determinant(self, matrix):

        det = 1.0
        rows = len(matrix)
        cols = len(matrix[0])

        for i in range(rows):
            row = i
            '''
            If the current absolute value matrix[j][i] greater than
            absolute value matrix[row][i] then swap the current row
            with row i and flip the det sign.
            '''
            for j in range(i + 1, rows):
                if abs(matrix[j][i]) > abs(matrix[row][i]):
                    row = j
                    matrix[i], matrix[row] = matrix[row], matrix[i]
                    det *= -1

            '''
            For each item in each row divide by the diagonal of matrix.
            '''
            for j in range(i + 1, rows):
                temp = matrix[j][i] / matrix[i][i]

                '''
                For each item in a column multiply that item by the previous
                calculation.

                Computes the diagonal values for the final calculation.
                '''
                for c in range(i, cols):
                    matrix[j][c] -= matrix[i][c] * temp

        '''
        Multiply det by the diagonal of the matrix.
        '''
        print('final')

        for i in range(rows):
            print(matrix[i])
            det *= matrix[i][i]

        return det