# Satellite

## Description

Placing your ship in range of the Osmiums, you begin to receive signals. Hoping that you are not detected, because it's
too late now, you figure that it may be worth finding out what these signals mean and what information might be
"borrowed" from them. Can you hear me Captain Tim? Floating in your tin can there? Your tin can has a wire to ground
control?

Find something to do that isn't staring at the Blue Planet.

[Attachment](768be4f10429f613eb27fa3e3937fe21c7581bdca97d6909e070ab6f7dbf2fbf)

## Solution

Unpacking the zip gives a pdf and an executable. Running the executable prints:

```
Hello Operator. Ready to connect to a satellite?
Enter the name of the satellite to connect to or 'exit' to quit
```

From the task description and from the binary we can find the valid satellite name `osmium`.
Connecting to it gives a new menu:

```
Establishing secure connection to osmium satellite...
Welcome. Enter (a) to display config data, (b) to erase all data or (c) to disconnect
```

Option a prints:

```
Username: brewtoot password: ********************
166.00 IS-19 2019/05/09 00:00:00
Swath 640km
Revisit capacity twice daily, anywhere Resolution panchromatic: 30cm multispectral: 1.2m
Daily acquisition capacity: 220,000km²
Remaining config data written to: https://docs.google.com/document/d/14eYPluD_pi3824GAFanS29tWdTcKxP_XUxx7e303-3E
```

From the google docs link we get the following string:
`VXNlcm5hbWU6IHdpcmVzaGFyay1yb2NrcwpQYXNzd29yZDogc3RhcnQtc25pZmZpbmchCg==`. Base64 decoding it gives:

```
Username: wireshark-rocks
Password: start-sniffing!
```

Running wireshark when choosing option a gives the following output:

```
Username: brewtoot password: CTF{4efcc72090af28fd33a2118985541f92e793477f}
166.00 IS-19 2019/05/09 00:00:00
Swath 640km
Revisit capacity twice daily, anywhere Resolution panchromatic: 30cm multispectral: 1.2m
Daily acquisition capacity: 220,000kmÂ²
Remaining config data written to: https://docs.google.com/document/d/14eYPluD_pi3824GAFanS29tWdTcKxP_XUxx7e303-3E
```

The flag is therefore `CTF{4efcc72090af28fd33a2118985541f92e793477f}`.
