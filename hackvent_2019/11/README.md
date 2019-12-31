# 11 - Santas Quick Response 3.0

## Description

Level: Medium<br/>
Author: inik

The elves created an API where you get random jokes about santa.

Go and try it here: [http://whale.hacking-lab.com:10101](http://whale.hacking-lab.com:10101)

## Solution

This challenge was straightforward. There is an API where we can create new users and request random jokes. Each joke
looked like this:

```json
{
   "joke": "Three phrases that sum up Christmas are: Peace on Earth, Goodwill to Men, and Batteries not Included.",
   "author": "Author Unknown",
   "platinum": false
}
```

It was clear that the goal was to get a platinum joke which would then contain the flag. Whilst trying to register a new
user, I received the following error message when trying to add new properties to the JSON object:

```json
Unrecognized field "test" (class ch.dkuhn.hv19.fsja.model.User), not marked as ignorable (3 known properties: "password", "platinum", "username"])
 at [Source: org.glassfish.jersey.message.internal.ReaderInterceptorExecutor$UnCloseableInputStream@28f46ce4; line: 1, column: 68] (through reference chain: ch.dkuhn.hv19.fsja.model.User["test"])
```

Now that I knew that the `User` class has an attribute called `platinum` I simply created a new user where `platinum` was
set to `true`:

```json
{
   "username": "some_username",
   "password": "some_password",
   "platinum": true
}
```

Accessing the joke endpoint then returned the flag:

```json
{
   "joke": "Congratulation! Sometimes bugs are rather stupid. But that's how it happens, sometimes. Doing all the crypto stuff right and forgetting the trivial stuff like input validation, Hohoho! Here's your flag: HV19{th3_cha1n_1s_0nly_as_str0ng_as_th3_w3ak3st_l1nk}",
   "author": "Santa",
   "platinum": true
}
```
