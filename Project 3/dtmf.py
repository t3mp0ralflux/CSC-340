import math
import signal_generator as fourier


def get_tones():
    a = []
    b = []
#    c = []

    with open("tonedataA.txt", 'r') as file:
        for i in file.readlines():
            i = i.strip()
            a.append(float(i))

    with open("tonedataB.txt", 'r') as file:
        for i in file.readlines():
            i = i.strip()
            b.append(float(i))

 #   with open("tonedataC.txt", 'r') as file:
 #       for i in file.readlines():
 #           i = i.strip()
  #          c.append(float(i))

    return a, b #, c


def main():
    N = 4096
    # get frequency data for each tone
    #A_data, B_data, C_data = get_tones()
    A_data, B_data = get_tones()
    a_fft = fourier.fft(A_data, 1, N)
    b_fft = fourier.fft(B_data, 1, N)
    #c_fft = fourier.fft(C_data, 1, N)
    a_psd = fourier.psd(a_fft)
    b_psd = fourier.psd(b_fft)
    #c_psd = fourier.psd(c_fft)

    fs = 44100
    bins = []
    for k in range(0, N):
        bins.append((k * fs) / N)

    max_a = max(a_psd)
    max_b = max(b_psd)
    #max_c = max(c_psd)

   # for i in a_psd:
   #     print(i / max_a)

    for i in b_psd:
        print(i / max_b)

    """
    for i in c_psd:
        print(i / max_c)
    """


main()
