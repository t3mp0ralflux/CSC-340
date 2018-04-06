import math


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

    elif d == -1:  # find inverse FFT
        temp_fft = fft(z, 1, n)
        # temp_fft = temp_fft[::-1]
        inv_fft_list = []
        for i in temp_fft:
            inv_fft_list.append((i.real / n))
        inv_fft_list.append(inv_fft_list[0])
        inv_fft_list.pop(0)
        for k in reversed(inv_fft_list):
            print(k)


def limit_plot(samples):
    fs_10000 = []
    gs_10000 = []
    S = 10000
    for i in range(0, 512):
        param = i / samples
        fs_10000.append(commonsignal1(param, S))
        gs_10000.append(commonsignal2(param, S))

    with open("fs_10000_signal.txt", 'w') as file:
        for i in fs_10000:
            file.write(str(i) + "\n")

    with open("gs_10000_signal.txt", 'w') as file2:
        for i in gs_10000:
            file2.write(str(i) + "\n")

    fs_fft = fft(fs_10000, 1, 512)
    gs_fft = fft(gs_10000, 1, 512)

    with open("fs_10000_psd.txt", 'w') as fs_psd:
        for i in fs_fft:
            temp = i * i.conjugate()
            final = temp.real
            fs_psd.write(str(final) + "\n")

    with open("gs_10000_psd.txt", 'w') as gs_psd:
        for i in gs_fft:
            temp = i * i.conjugate()
            final = temp.real
            gs_psd.write(str(final) + "\n")


def main():
    limit_plot(512)



main()
