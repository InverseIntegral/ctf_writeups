# 02 - No source, No luck!

## Description

Now they're just trolling you, aren't they? They said there would be a flag, but now they're not even talking to us for real, just shoving us along 😤 No manners, they got!

## Solution

We are presented with a website that does not seem to have any source code. The whole site only uses css and redirects
us. If we use curl to request the site, we get the following response:

```
< HTTP/2 200
< content-type: text/html; charset=utf-8
< date: Mon, 27 Dec 2021 14:40:31 GMT
< link: <style.css>; rel=stylesheet;
< refresh: 5; url=https://www.youtube.com/watch?v=dQw4w9WgXcQ
< server: Werkzeug/2.0.2 Python/3.10.1
< content-length: 0
```

Seems like we have to rquest the stylesheet to find the flag:

```
html {
  display: flex;
  height:100vh;
  overflow: hidden;
  justify-content: center;
  align-items: center;
  flex-direction:column;
  background: #222;
}

body::before, body::after {
  font-weight: bold;
  font-family: 'SF Mono', 'Courier New', Courier, monospace;
  font-size: 42px;
  color: #ff4473;
}

head {
  display: block;
  background-image: url(https://media.giphy.com/media/Ju7l5y9osyymQ/giphy.gif);
  height:20rem;
  width:20rem;
  background-repeat: no-repeat;
  background-size: cover;
  border: 5px solid #fff;
  border-radius: 10px;
  border-style: dashed;
}

body::before {
  display: inline-block;
  padding-top: 3rem;
  content: "Never gonna give you up...";
}

body::after {
  margin-left: 16px;
  display: inline;
  content: "HV21{h1dd3n_1n_css}";
  background: #ff4473;
  animation: blink 1s infinite;
}

@keyframes blink {
  from {
    opacity: 1;
  }

  to {
    opacity: 0;
  }
}
```

And there it is: `HV21{h1dd3n_1n_css}`.

