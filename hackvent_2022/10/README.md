# 10 - Notme

## Description

Level: Medium<br/>
Author: HaCk0

Santa brings you another free gift! We are happy to announce a free note taking webapp for everybody. No account name
restriction, no filtering, no restrictions and the most important thing: no bugs! Because it cannot be hacked, Santa
decided to name it Notme = Not me you can hack!

Or can you?

## Solution

For this challenge we are given a webervice that saves notes. My first thought was that the flag would be stored in
a note of a different user. Therefore, I tried to access notes of the "admin account" by modifying requests. After
a while I stumbled accross the password reset which performed the following request:

```
POST https://5b860491-0f85-42c2-bf07-a364e00c08d2.idocker.vuln.land/api/user/1

{ "password":"test" }
```

I tried to enumerate different user ids and figured out that there was a user with id `1337` (most likely the admin). My
suspicion was confirmed when I saw the response of the password reset:
 
```
{
  "id":1337,
  "role":"user",
  "username":"Santa",
  "password":"eeb36e726e3ffec16da7798415bb4e531bf8a57fbe276fcc3fc6ea986cb02e9a",
  "createdAt":"2022-12-27T12:03:29.812Z",
  "updatedAt":"2022-12-27T12:12:17.020Z"
}
```

After this I was able to login with the username "Santa" and the newly set password. And there I got the flag:
`HV22{Sql1_is_An_0Ld_Cr4Ft}`. This was actually not the intended way of solving this challenge. There was a blind SQL
injection in the input field but this was probably much easier than the intended solution ;)

