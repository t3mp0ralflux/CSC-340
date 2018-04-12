import math
from cmath import exp, pi


lp_list = []
hp_list = []
bp_list = []
nf_list = []


def low_pass(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    temp_list = []
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
        ans = numerator / ((2 * k) - 1)
        temp_list.append(ans)
    temp_list.sort()
    min_list = []
    """low_fft = fft(temp_list, 1, 50)
    for i in range(0, 5):
        min_list.append(low_fft[i])

    freq_list = fft(min_list, -1, 50)
    for i in freq_list:
        temp_sum += i"""

    return temp_sum




def high_pass(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    temp_list = []
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
        ans = numerator / ((2 * k) - 1)
        temp_list.append(ans)
    #temp_list.sort()

    max_list = []
    for i in range(5, len(temp_list) - 1):
        max_list.append(temp_list[i])
    for i in max_list:
        temp_sum += i

    return temp_sum


def band_pass(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    temp_list = []
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
        ans = numerator / ((2 * k) - 1)
        temp_list.append(ans)
    band_list = []

    for i in range(3, 7):
        band_list.append(temp_list[i])
    for i in band_list:
        temp_sum += i

    return temp_sum


def notch_filter(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    temp_list = []
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
        ans = numerator / ((2 * k) - 1)
        temp_list.append(ans)
    notch_list = []

    for i in range(0, 3):
        notch_list.append(temp_list[i])
    for i in range(7, len(temp_list) - 1):
        notch_list.append(temp_list[i])
    for i in notch_list:
        temp_sum += i

    return temp_sum


def commonsignal1(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * ((2 * k) - 1) * x)
        ans = numerator / ((2 * k) - 1)
        temp_sum += ans

    return temp_sum


def commonsignal2(x, S):
    """
    :param x:
    :param S:
    :return:
    """
    temp_sum = 0.0
    for k in range(1, S + 1):
        numerator = math.sin(2 * math.pi * (2 * k) * x)
        ans = numerator / (2 * k)
        temp_sum += ans
    return temp_sum


def org_fft(z, n):
    """
    :param z:
    :param n:
    :return:
    """
    i = 0
    while i < n:
        r = i
        k = 0
        m = 1
        while m < n:
            k = 2 * k + (r % 2)
            k = int(k)
            r /= 2
            m *= 2
        if k > i:
            t = z[i]
            z[i] = z[k]
            z[k] = t
        i += 1

    return z


def fft(z, d, n):
    """
    :param z:
    :param d:
    :return:
    """
    if d == 1:
        r = n / 2
        r = int(r)
        i = 1
        theta = (-2 * math.pi * d) / n
        while i < n:
            w = math.cos(i * theta) + math.sin(i * theta)*1j
            k = 0
            while k < n:
                u = 1
                m = 0
                while m < r:
                    t = z[k + m] - z[k + m + r]
                    z[k + m] = z[k + m] + z[k + m + r]
                    z[k + m + r] = t * u
                    u *= w
                    m += 1
                k += 2 * r
            i *= 2
            r /= 2
            r = int(r)
        return org_fft(z, n)

    elif d == -1: # find inverse FFT
        temp_fft = fft(z, 1, n)
        #temp_fft = temp_fft[::-1]
        inv_fft_list = []
        for i in temp_fft:
            inv_fft_list.append((i.real / n))
        inv_fft_list.append(inv_fft_list[0])
        inv_fft_list.pop(0)

        return inv_fft_list[::-1]

        #for k in reversed(inv_fft_list):
         #   print(k)


def psd(x):
    """
    :param x:
    :return:
    """
    psd_list = []
    for i in x:
        temp = i * i.conjugate()
        psd_list.append(temp.real)

    return psd_list


def call_signals():
    """
    :return:
    """
    fs_samples1 = []
    fs_samples2 = []
    fs_samples3 = []
    gs_samples1 = []
    gs_samples2 = []
    gs_samples3 = []

    samples = 512
   # S = .5
     # S = 3
    # for k in range(0, 3):
       # # S = S * 10
        # for i in range(0, 512):
            # param = i / samples
            # # when S = 3
            # if S < 4:
                # fs_samples1.append(commonsignal1(param, int(S)))
                # gs_samples1.append(commonsignal2(param, int(S)))
            # # when S = 10
            # elif S < 11:
                # fs_samples2.append(commonsignal1(param, int(S)))
                # gs_samples2.append(commonsignal2(param, int(S)))
            # # when S = 50
            # else:
                # fs_samples3.append(commonsignal1(param, int(S)))
                # gs_samples3.append(commonsignal2(param, int(S)))
    for i in range (0,512):
        param = i/ samples
        fs_samples1.append(commonsignal1(param, int(3)))
        gs_samples1.append(commonsignal2(param, int(3)))
        fs_samples2.append(commonsignal1(param, int(10)))
        gs_samples2.append(commonsignal2(param, int(10)))
        fs_samples3.append(commonsignal1(param, int(50)))
        gs_samples3.append(commonsignal2(param, int(50)))

    # Write results of signal generation for Fs(t) function
    with open("fs_signal_results.txt", 'w') as fs_results:
        fs_results.write("S = 3: " + "\n\n")
        for i in fs_samples1:
            fs_results.write(str(i) + "\n")
        fs_results.write("\n" + "S = 10: " + "\n\n")
        for i in fs_samples2:
            fs_results.write(str(i) + "\n")
        fs_results.write("\n" + "S = 50: " + "\n\n")
        for i in fs_samples3:
            fs_results.write(str(i) + "\n")

    # Write results of signal generation for Gs(t) function
    with open("gs_signal_results.txt", 'w') as gs_results:
        gs_results.write("S = 3: " + "\n\n")
        for i in gs_samples1:
            gs_results.write(str(i) + "\n")
        gs_results.write("\n" + "S = 10: " + "\n\n")
        for i in gs_samples2:
            gs_results.write(str(i) + "\n")
        gs_results.write("\n" + "S = 50: " + "\n\n")
        for i in gs_samples3:
            gs_results.write(str(i) + "\n")

    # Write results of Low-Pass filtering to text file
    with open("low_pass.txt", 'w') as lp_results:
        for i in lp_list:
            lp_results.write(str(i) + "\n")

    # Write results of High-Pass filtering to text file
    with open("high_pass.txt", 'w') as hp_results:
        for i in hp_list:
            hp_results.write(str(i) + "\n")

    # Write results of Band-Pass filtering to text file
    with open("band_pass.txt", 'w') as bp_results:
        for i in bp_list:
            bp_results.write(str(i) + "\n")

    # Write results of Band-Pass filtering to text file
    with open("notch_filter.txt", 'w') as nf_results:
        for i in nf_list:
            nf_results.write(str(i) + "\n")

    fs_fft = fft(fs_samples3, 1, 512)
    gs_fft = fft(gs_samples3, 1, 512)

    with open("fs500_psd.txt", 'w') as fs_psd:
        for i in fs_fft:
            temp = i * i.conjugate()
            final = temp.real
            fs_psd.write(str(final) + "\n")

    with open("gs500_psd.txt", 'w') as gs_psd:
        for i in gs_fft:
            temp2 = i * i.conjugate()
            final2 = temp2.real
            gs_psd.write(str(final2) + "\n")


def fft_test():
    test_list = []
    with open("fft_test.txt", 'r') as file:
        for i in file.readlines():
            test_list.append(float(i))

    t_fft = fft(test_list, 1, 16)
    for i in t_fft:
        print(i)

    print("\n" + "TESTING INVERSE FFT: " + "\n")

    fft(t_fft, -1, 16)


def main():
    """
    :return:
    """
    fs_fft = []
    gs_fft = []
    call_signals()
    fft_test()


#main()
