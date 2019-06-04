function hash(s) {
    return s.split("").reduce(function (a, b) {
        return ((a << 5) - a) + b.charCodeAt(0);
    }, 0);
}

function calculateFlag(str, amount) {
    if (Number(amount) < 0) {
        return calculateFlag(str, Number(amount) + 26);
    }

    let output = '';

    for (let i = 0; i < str.length; i++) {
        let c = str[i];

        if (c.match(/[a-z]/i)) {
            let code = str.charCodeAt(i);

            if ((code >= 65) && (code <= 90)) {
                c = String.fromCharCode(((code - 65 + Number(amount)) % 26) + 65);
            } else if ((code >= 97) && (code <= 122)) {
                c = String.fromCharCode(((code - 97 + Number(amount)) % 26) + 97);
            }
        }

        output += c;
    }

    return output;
}

function decrypt(n1, n2, n3, n4, n5, n6, n7, n8) {
    let pin = [n1, n2, n3, n4, n5, n6, n7, n8].join('');

    if (hash(pin) === -502491864) {
        console.log(pin);

        const flag = 'he19-' +
            calculateFlag('iT', n1) +
            calculateFlag('Xp', n2) + '-' +
            calculateFlag('Um', n3) +
            calculateFlag('BG', n4) + '-' +
            calculateFlag('4I', n5) +
            calculateFlag('Qv', n6) + '-' +
            calculateFlag('xr', n7) +
            calculateFlag('rr', n8);

        console.log(flag);
    }
}

for (let i = -9; i <= 9; i++) {
    for (let j = -9 + 11; j <= 9; j++) {
        for (let k = -9; k <= 9; k++) {
            for (let l = -9; l <= 9; l++) {
                for (let m = -9; m <= 9; m++) {
                    for (let n = -9; n <= 9; n++) {
                        for (let o = -9; o <= 9; o++) {
                            for (let p = -9; p <= 9; p++) {
                                decrypt(i, j, k, l, m, n, o, p);
                            }
                        }
                    }
                }
            }
        }
    }
}
