import math
import signal_generator as fourier


def low_pass(fs_fft, samples):
    """
    :param x:
    :param S:
    :return:
    """
    filter_sig = [0] * samples
    lp_filter = []
    for i in range(0, 5):
        filter_sig[i] = 1
    for i in range(0, samples):
        lp_filter.append(fs_fft[i] * filter_sig[i])
    final = fourier.fft(lp_filter, -1, samples)
    with open("low_pass.txt", 'w') as file:
        for i in final:
            file.write(str(i) + "\n")


def high_pass(fs_fft, samples):
    """
    :param x:
    :param S:
    :return:
    """
    filter_sig = [0] * samples
    hp_filter = []
    for i in range(5, 50):
        filter_sig[i] = 1
    for i in range(0, samples):
        hp_filter.append(fs_fft[i] * filter_sig[i])
    final = fourier.fft(hp_filter, -1, samples)
    with open("high_pass.txt", 'w') as file:
        for i in final:
            file.write(str(i) + "\n")


def band_pass(fs_fft, samples):
    """
    :param x:
    :param S:
    :return:
    """
    filter_sig = [0] * samples
    bp_filter = []
    for i in range(3, 8):
        filter_sig[i] = 1
    for i in range(0, samples):
        bp_filter.append(fs_fft[i] * filter_sig[i])
    final = fourier.fft(bp_filter, -1, samples)
    with open("band_pass.txt", 'w') as file:
        for i in final:
            file.write(str(i) + "\n")


def notch_filter(fs_fft, samples):
    """
    :param x:
    :param S:
    :return:
    """
    filter_sig = [1] * samples
    nf_filter = []
    for i in range(3, 8):
        filter_sig[i] = 0
    for i in range(0, samples):
        nf_filter.append(fs_fft[i] * filter_sig[i])
    final = fourier.fft(nf_filter, -1, samples)
    with open("notch_filter.txt", 'w') as file:
        for i in final:
            file.write(str(i) + "\n")


def generate_fs():
    S = 50
    fs_signal = []
    for i in range(0, 512):
        x = i / 512
        temp_sum = 0.0
        temp_list = []
        for k in range(1, S + 1):
            numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
            ans = numerator / ((2 * k) - 1)
            temp_list.append(ans)
        for i in temp_list:
            temp_sum += i
        fs_signal.append(temp_sum)
    return fs_signal


def main():
    fs_signal = generate_fs()
    fs_fft = fourier.fft(fs_signal, 1, 512)
    samples = 512
    S = 50
    low_pass(fs_fft, samples)
    high_pass(fs_fft, samples)
    band_pass(fs_fft, samples)
    notch_filter(fs_fft, samples)
    """
    for i in range(0, samples):
        x = i / samples
        lp_filter.append(low_pass(x, S))
        hp_filter.append(high_pass(x, S))
        bp_filter.append(band_pass(x, S))
        nf_filter.append(notch_filter(x, S))

    lp_fft = fourier.fft(lp_filter, 1, 512)
    hp_fft = fourier.fft(hp_filter, 1, 512)
    bp_fft = fourier.fft(bp_filter, 1, 512)
    nf_fft = fourier.fft(nf_filter, 1, 512)

    lp_mult = []
    hp_mult = []
    bp_mult = []
    nf_mult = []
    for i in range(0, 512):
        lp_mult.append(fs_fft[i] * lp_fft[i])
        hp_mult.append(fs_fft[i] * hp_fft[i])
        bp_mult.append(fs_fft[i] * bp_fft[i])
        nf_mult.append(fs_fft[i] * nf_fft[i])

    lp_final = fourier.fft(lp_mult, -1, 512)
    hp_final = fourier.fft(hp_mult, -1, 512)
    bp_final = fourier.fft(bp_mult, -1, 512)
    nf_final = fourier.fft(nf_mult, -1, 512)

    lp_max = max(lp_final)

    for i in range(0, 512):
        lp_final[i] /= lp_max

    hp_max = max(hp_final)
    for i in range(0, 512):
        hp_final[i] /= hp_max

    bp_max = max(bp_final)
    for i in range(0, 512):
        bp_final[i] /= bp_max

    nf_max = max(nf_final)
    for i in range(0, 512):
        nf_final[i] /= nf_max
    """




main()
