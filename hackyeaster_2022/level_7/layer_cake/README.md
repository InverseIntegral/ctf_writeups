# Layer Cake

## Description
Level: medium<br/>
Author: PS

Someone said there would be cake.

`hackyeaster/layercake:latest`

## Solution

For this challenge we are given a docker image name that we can pull from dockerhub. 
As you might know, docker images consist of several layers that store the states between the commands during the build
process.

If we look at the executed commands of the layers [on
dockerhub](https://hub.docker.com/layers/layercake/hackyeaster/layercake/latest/images/sha256-a489ea275575b28109bd54275be2ac6f59d9480f7d0eef1e2b7884a59cea68b7?context=explore)
we can see that the file `egg.png` is overwritten multiple times. The goal is now clear. We have to extract the layers
and check the image.

```
$ docker save hackyeaster/layercake -o layers.tar
$ tar -xvf layers.tar
```

Now all that's left to do is check the individual layers and find the image:

![](egg.png)

