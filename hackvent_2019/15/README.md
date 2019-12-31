# 15 - Santa's Workshop

## Description

Level: Hard<br/>
Author: inik & avarax

The Elves are working very hard.
Look at [http://whale.hacking-lab.com:2080/](http://whale.hacking-lab.com:2080/) to see how busy they are.

## Solution

For this challenge we get a webpage that connects to an MQTT broker which sends random numbers over a topic. These
numbers are then displayed on the webpage. The first thing I did was to go through the source of the webpage. There I
found the `config.js`:

```js
var mqtt;
var reconnectTimeout = 100;
var host = 'whale.hacking-lab.com';
var port = 9001;
var useTLS = false;
var username = 'workshop';
var password = '2fXc7AWINBXyruvKLiX';
var clientid = localStorage.getItem("clientid");
if (clientid == null) {
  clientid = ('' + (Math.round(Math.random() * 1000000000000000))).padStart(16, '0');
  localStorage.setItem("clientid", clientid);
}
var topic = 'HV19/gifts/'+clientid;
// var topic = 'HV19/gifts/'+clientid+'/flag-tbd';
```

I used [an MQTT web client](http://www.hivemq.com/demos/websocket-client/) to connect to the above broker and started
expirementing. After some read up about MQTTs topics I came across the special `$SYS` topics and the multi level
wildcard `#`. I combined them and connected to `$SYS/#` to get some information about the broker and got the following
response: 

```
mosquitto version 1.4.11 (We elves are super-smart and know about CVE-2017-7650 and the POC. So we made a
genious fix you never will be able to pass. Hohoho)
```

After reading about the CVE it was clear what I had to do. To bypass their fix I did some simple tests and realized that
they only checked if the first characters was `#`. Therefore, I used my previous client id and added `/#` to get access
to all other subtopics. With that I got the following message `Congrats, you got it. The elves should not overrate their
smartness!!!` on the topic named `HV19/gifts/.../HV19{N0_1nput_v4l1d4t10n_3qu4ls_d1s4st3r}` which contained the flag.
