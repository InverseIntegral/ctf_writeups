import requests
import string

URL = "https://030029e6-ad43-4951-9442-33c64b7ac112.idocker.vuln.land/"
data = ""

for char in string.ascii_lowercase:
    params = {
        "alphabet_select": char,
        "user_input": "XXX"
    }
    response = requests.post(URL, data=params)
    data += response.text
    data += "<br/>"

with open("output.html", "w") as f:
    f.write(data)