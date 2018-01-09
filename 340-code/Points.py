__author__ = 'Darryl'

'''
A class for testing my random search, genetic algorithm, and simulated annealing
developed to find answers to the traveling sales person problem.
'''

from Matrix import Matrix
import math
import random


class Points:
    def __init__(self):
        self.data = []
        self.points = {}
        self.set_points()
        self.temp_dist = []
        self.distance = Matrix([[0]])
        self.set_distances()
        self.random_bins = [0] * 100
        self.random_total = 0
        self.random_squares = 0
        self.random_count = 0
        self.random_best_dist = self.distance.find_max() * len(self.distance.get_data())
        self.random_best_trip = []
        self.genetic_best_dist = self.distance.find_max() * len(self.distance.get_data())
        self.genetic_best_trip = []
        self.genetic_bins = [0] * 100

    # Return Matrix that is composed of the distances between points being tested
    def get_distances(self):
        return self.distance

    # Return random search bins
    def get_random_bins(self):
        return self.random_bins

    # Return the summation of all trips tested in the random search
    def get_random_total(self):
        return self.random_total

    # Return the summation of all trips squared
    def get_random_squares(self):
        return self.random_squares

    # Return the number of trips tested
    def get_random_count(self):
        return self.random_count

    # Print the bins for the random search
    def print_random_bins(self):
        print('Random bins')
        for i in range(len(self.random_bins)):
            print(self.random_bins[i])

    # Print the bins for the genetic search
    def print_genetic_bins(self):
        print('Genetic bins')
        for i in range(len(self.genetic_bins)):
            print(self.genetic_bins[i])

    # Get points from a file
    def set_points(self):
        count = 1
        temp = []
        filename = open('points.txt', 'r')
        for line in filename:
            row = line.strip()
            temp.append(row)
        for i in range(0, len(temp)):
            self.data.append(temp[i].split())
        for i in self.data:
            mat = Matrix([[float(i[0]), float(i[1])]])
            self.points[count] = mat
            count += 1

    # Create a matrix of each points distance from the other points
    def set_distances(self):
        for i in range(1, 15):
            temp = []
            for j in range(1, 15):
                x1 = self.points[i].get_data()[0][0]
                y1 = self.points[i].get_data()[0][1]
                x2 = self.points[j].get_data()[0][0]
                y2 = self.points[j].get_data()[0][1]
                temp.append(math.hypot(x2 - x1, y2 - y1))
            self.temp_dist.append(temp)
        self.distance = Matrix(self.temp_dist)

    # Create a random trip, including all cities in TSP problem
    def random_trip(self):
        dist = random.sample(range(14), 14)
        for i in range(len(dist)):
            dist[i] += 1
        return dist

    # Performs the random search of finding a possible solution
    # Takes in the number of random trips to evaluate
    # Also returns the best trip and the cost of the best trip
    def random_search(self, iterations):
        self.random_count = iterations
        for i in range(iterations):
            trip = self.random_trip()
            dist = test.calculate_distance(trip)

            if dist < self.random_best_dist:
                self.random_best_dist = dist
                self.random_best_trip = trip
            self.random_total += dist
            self.random_squares += dist * dist
            index = int((dist - 20) / -0.2)
            self.random_bins[index] += 1

        return self.random_best_dist, self.random_best_trip

    # Returns the total cost of a trip
    def calculate_distance(self, trip):
        dist = 0
        for j in range(1, len(trip)):
            dist += self.distance.get_data()[trip[j - 1] - 1][trip[j] - 1]

        dist += self.distance.get_data()[trip[0] - 1][trip[len(trip) - 1] - 1]
        return dist

    # Creates a "child" trip based on a more fit parent (parent1)
    # and a less fit parent (parent2)
    # Used in the genetic method
    def make_child(self, parent1, parent2):

        child = parent1[:7]

        for i in range(7, 14):
            if parent2[i] in child:
                child.append(0)
            else:
                child.append(parent2[i])

        for i in range(len(child) + 1):
            if i not in child:
                for j in range(len(child)):
                    if child[j] == 0:
                        child[j] = i
                        break
        return child

    # Performs an implementation of the genetic algorithm for finding possible
    # solutions to the TSP problem.
    def genetic(self):
        population = []
        for i in range(500):
            population.append(self.random_trip())

        for i in range(100):
            sorted(population, key=self.calculate_distance)

            size = int(len(population) * .1)

            p1 = population[random.randint(0, size)]
            p2 = population[random.randint(0, size)]

            if self.calculate_distance(p1) > self.calculate_distance(p2):
                m_fit = p1
                l_fit = p2
            else:
                m_fit = p2
                l_fit = p1

            child = self.make_child(m_fit, l_fit)
            dist = self.calculate_distance(child)

            if dist < self.genetic_best_dist:
                self.genetic_best_dist = dist
                self.genetic_best_trip = child

            if dist < self.calculate_distance(m_fit):
                population.remove(l_fit)
                population.append(child)

            index = int((dist - 20) / -0.2)
            self.genetic_bins[index] += 1

        return self.genetic_best_dist, self.genetic_best_trip

    # Returns a neighbor trip to the current trip being tested.
    # Two cities of the input trip are switched and returned.
    def make_neighbor(self, trip):
        first = random.randint(0, 13)
        second = random.randint(0, 13)
        trip[second], trip[first] = trip[first], trip[second]
        return trip

    # Performs an implementation of the simulated annealing algorithm to
    # find possible solutions to the TSP problem.
    def annealing(self):
        current = self.random_trip()
        current_cost = self.calculate_distance(current)
        temperature = 1.0
        cool_rate = 0.9999
        temperature_min = .2
        while temperature > temperature_min:
            i = 0
            while i < 10:
                # print('temp', temperature, i)
                i += 1
                neighbor = self.make_neighbor(current)
                neighbor_cost = self.calculate_distance(neighbor)
                delta = neighbor_cost - current_cost
                probability = math.e ** (delta / temperature)
                if delta < 0 or (delta > 0 and probability > random.random()):
                    current = neighbor
                    current_cost = neighbor_cost


            temperature *= cool_rate

        return current, current_cost

test = Points()

test.get_distances().print_data()
a = test.get_distances().find_max()
print('max', a, a * 14)
a = test.get_distances().get_data()[2][7]
print('min', a, a * 14)

b_dist, b_trip = test.random_search(1000000)
test.print_random_bins()
print('Random totals', test.get_random_total())
print('Random squares', test.get_random_squares())
print('Random count', test.get_random_count())
standard = (test.get_random_squares() - test.get_random_total() ** 2 / test.get_random_count()) / test.get_random_count()
standard = math.sqrt(standard)
print('Standard Deviation: ', standard)
print('Mean: ', test.get_random_total() / test.get_random_count())
print('Best Trip: ', b_trip)
print('Best Dist: ', b_dist)

a = test.random_trip()
b = test.random_trip()
print('a', a)
print('c', test.make_child(a, b))
print('b', b)


g_dist, g_trip = test.genetic()
print('Best Trip: ', g_trip)
print('Best Dist: ', g_dist)

'''
trip, cost = test.annealing()
print(trip)
print(cost)
a_trip = trip
a_dist = cost
annealing_bins = [0] * 100
for i in range(150):
    trip, cost = test.annealing()
    index = int((cost - 20) / -0.2)
    annealing_bins[index] += 1
    if cost < a_dist:
        a_dist = cost
        a_trip = trip

print('dist', a_dist)
print('trip', a_trip)

for i in range(len(annealing_bins)):
        print(annealing_bins[i])

'''