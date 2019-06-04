let a = ['abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'];
let s = 1;

for (let j = 0; j < 7332; j++) {
    s += 2;

    theBoxOfCarrots.forEach((element, index) => {
        s = element[0] + Math.abs(Math.floor(Math.sin(s) * 20));
        element[0] = s;
        element[1] += (index + ".");
    });

    s++;

    theBoxOfCarrots.sort((a, b) => {
        return a[0] - b[0]
    });
}

