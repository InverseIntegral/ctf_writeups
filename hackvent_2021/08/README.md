# 08 - Flag Service

## Description

Santa has setup a web service for you to receive your flag for today. Unfortunately, the flag doesn't seem to reach you.

## Solution

For this challenge we are given a website that shows the text "Thanks for visiting the Flag service. Your Flag is: ". At
that point the text cuts off. Lookin at the response in the network tab we can see something interesting:

```html
<!DOCTYPE html>
    <html>
    <head>
        <meta charset="utf-8" />
        <link rel="preconnect" href="https://fonts.googleapis.com">
        <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
        <link href="https://fonts.googleapis.com/css2?family=IBM+Plex+Mono&display=swap" rel="stylesheet">

        <style>
        body{font-family: 'IBM Plex Mono', monospace;height: 100vh !important;background-image: url("https://source.unsplash.com/random");-webkit-background-size: cover;-moz-background-size: cover;-o-background-size: cover;background-size: cover;background-color:#131627;color:#fff;overflow:hidden;}
        ::selection{background-color:rgba(0, 0, 0, 0);}
        #flex-wrapper{position:absolute;top:0;bottom:0;right:0;left:0;-ms-flex-direction:row;-ms-flex-align:center;display:-webkit-flex;display:flex}#container{margin:auto; z-index: 10; padding:25px;}#container *{margin:0}h1{text-align:center;font-size:60px;color:#131627;text-shadow:0 0 5px #fff;opacity:0;-webkit-animation:fade-in 3s ease-in 0s forwards;-moz-animation:fade-in 3s ease-in 0s forwards;-o-animation:fade-in 3s ease-in 0s forwards;animation:fade-in 3s ease-in 0s forwards}h2{font-size:50px;text-shadow:0 0 5px orange;text-align:center;opacity:0;-webkit-animation:fade-in 3s ease-in .5s forwards;-moz-animation:fade-in 3s ease-in .5s forwards;-o-animation:fade-in 3s ease-in .5s forwards;animation:fade-in 3s ease-in .5s forwards}@-webkit-keyframes fade-in{from{opacity:0}to{opacity:1}}@-moz-keyframes fade-in{from{opacity:0}to{opacity:1}}@-o-keyframes fade-in{from{opacity:0}to{opacity:1}}@keyframes fade-in{from{opacity:0}to{opacity:1}}
        </style>
        <title>Flag Service</title>
    </head>
    <body>
        <div id="flex-wrapper">
        <div id="container">
            <h1>Thanks for using the Flag service.<br/> Your Flag is:</h1>
            <h2>
```

The tags aren't closed which means that part of the response is being cut off. After trying around with curl a bit I
found the option `--ignore-content-length`. This prevents the response to stop once the length of the `Content-Lenght`
response header has been reached. Using said option, we get the response:

```html
<div id="container">
    <h1>Thanks for using the Flag service.<br/> Your Flag is:</h1>
    <h2>HV21{4lw4y5_c0un7_y0ur53lf_d0n7_7ru57_7h3_53rv3r}</h2>
</div>
```

And the flag is `HV21{4lw4y5_c0un7_y0ur53lf_d0n7_7ru57_7h3_53rv3r}`.

