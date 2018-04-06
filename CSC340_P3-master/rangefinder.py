import math
import signal_generator as fourier


def get_data():
    """
    :return:
    """
    pulse = []
    response = []
    temp = []
    with open("rangeData.txt", 'r') as file:
        for i in file.readlines():
            line = i.strip()
            temp.append(line)
    for i in range(1, 146):
        pulse.append(float(temp[i]))
    for i in range(147, 1171):
        response.append(float(temp[i]))

    return pulse, response


def fast_correlation(x, y):
    """
    :param x:
    :param y:
    :return:
    """
    corr_list = []
    for i in range(0, len(x)):
        temp = y[i] * x[i].conjugate()
        corr_list.append(temp)
    return corr_list


def find_distance(fcc_final):
    """
    :param fcc_final:
    :return:
    """
    fcc_norm = []
    max_fcc = max(fcc_final)
    for i in fcc_final:
        fcc_norm.append(i / max_fcc)
    for i in range(0, len(fcc_norm)):
        if fcc_norm[i] == 1:
            d = i + 1

    # print the index at which the pulse signal was detected in the response signal
    #print(d)

    T = 1 / 200000
    T *= d
    T += 0.01
    r = 1500
    d_dist = r * T
    d_dist /= 2

    # print distance to target in meters
    print("The detection of the pulse signal in the response occurs at sample number " + str(d) + "\n")
    print("The distance to the target is approximately " + str(d_dist) + " meters")


def create_filter(u, p):
    y = [0] * 1024
    for k in range(0, len(y)):
        if k > p:
            temp_sum = 0.0
            for i in range(0, p):
                temp_sum += u[k - i]
            final = (1/p) * temp_sum
            y[k] = final
        else:
            temp_sum = 0.0
            for i in range(0, k):
                temp_sum += u[k - i]
            final = (1/p) * temp_sum
            y[k] = final
    return y


def smooth_filter(og_signal, filter_signal):
    conv_fft = fourier.fft(og_signal, 1, 1024)
    filtered_fft = fourier.fft(filter_signal, 1, 1024)

    temp_list = []
    for i in range(0, 1024):
        t = conv_fft[i] * filtered_fft[i]
        temp_list.append(t)
    return fourier.fft(temp_list, -1, 1024)


def main():
    pulse, response = get_data()
    for i in range(0, 879):
        pulse.append(0)

    pulse_fft = fourier.fft(pulse, 1, 1024)
    resp_fft = fourier.fft(response, 1, 1024)

    corr_list = fast_correlation(pulse_fft, resp_fft)

    fcc_final = fourier.fft(corr_list, -1, 1024)

    find_distance(fcc_final)
    filtered_response = create_filter(response, 8)
    final = smooth_filter(fcc_final, filtered_response)
    max_final = max(final)
    #for i in final:
     #   print(i / max_final)


main()
