import math
import signal_generator as fourier


def add_sin(t, a, c):
    f1 = 13
    f2 = 31
    funct1 = a * math.sin(2*math.pi * f1 * (t - c))
    funct2 = a * math.sin(2*math.pi * f2 * (t - c))
    return funct1 + funct2


def mult_sin(t, a, c):
    f1 = 13
    f2 = 31
    funct1 = a * math.sin(2 * math.pi * f1 * (t - c))
    funct2 = a * math.sin(2 * math.pi * f2 * (t - c))
    return funct1 * funct2


def gen_signal(k):
    signal = [0] * 256
    signal[k] = 1
    return signal


def q3a():
    test_signal1 = gen_signal(0)
    test_signal2 = gen_signal(10)
    test_signal3 = gen_signal(500)

    sig1_fft = fourier.fft(test_signal1, 1, 256)
    sig2_fft = fourier.fft(test_signal2, 1, 256)
    sig3_fft = fourier.fft(test_signal3, 1, 256)

    sig1_psd = fourier.psd(sig1_fft)
    sig2_psd = fourier.psd(sig2_fft)
    sig3_psd = fourier.psd(sig3_fft)

    with open("Q3a_results.txt", 'w') as file:
        file.write("PSD Data for Signal 1 with pulse index = 0 " + "\n" + "\n")
        for i in sig1_psd:
            file.write(str(i) + "\n")
        file.write("\n" + "PSD Data for Signal 2 with pulse index = 10 " + "\n" + "\n")
        for i in sig2_psd:
            file.write(str(i) + "\n")
        file.write("\n" + "PSD Data for Signal 3 with pulse index = 500 " + "\n" + "\n")
        for i in sig3_psd:
            file.write(str(i) + "\n")


def gen_sinusoidal(c):
    samples = 512
    signal = []
    for i in range(0, samples):
        x = i/samples
        h = math.sin(20 * math.pi * (x - c))
        signal.append(h)
    return signal


def q3b():
    h_0 = gen_sinusoidal(0)
    h_1 = gen_sinusoidal(0.1)
    h_2 = gen_sinusoidal(0.25)
    h_3 = gen_sinusoidal(0.75)

    fft_0 = fourier.fft(h_0, 1, 512)
    fft_1 = fourier.fft(h_1, 1, 512)
    fft_2 = fourier.fft(h_2, 1, 512)
    fft_3 = fourier.fft(h_3, 1, 512)

    alpha = 0.9999
    #for i in fft_2:
        #print(i)

    psd_0 = fourier.psd(fft_0)
    psd_1 = fourier.psd(fft_1)
    psd_2 = fourier.psd(fft_2)
    psd_3 = fourier.psd(fft_3)

    #for i in psd_3:
        #print(i)


def main():
    signal_x = []
    signal_y = []
    samples = 512
    for i in range(0, 512):
        t = i / samples
        signal_x.append(add_sin(t, 4, 0.1))
        signal_y.append(mult_sin(t, 4, 0.1))

    x_fft = fourier.fft(signal_x, 1, 512)
    y_fft = fourier.fft(signal_y, 1, 512)

    x_psd = []
    y_psd = []

    for i in x_fft:
        temp = i * i.conjugate()
        x_psd.append(temp.real)

    for i in y_fft:
        temp = i * i.conjugate()
        y_psd.append(temp.real)

    with open("Q2_results.txt", 'w') as file:
        file.write("x(t) PSD DATA" + "\n" + "\n")
        for i in x_psd:
            file.write(str(i) + "\n")

        file.write("\n" + "y(t) PSD DATA" + "\n")
        for i in y_psd:
            file.write(str(i) + "\n")

    #q3a()
    q3b()



#main()
