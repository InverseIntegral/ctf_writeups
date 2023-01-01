# 11 - Santa's Screenshot Render Function

## Description

Level: Medium<br/>
Author: Deaths Pirate

Santa has been screenshotting NFTs all year. Now that the price has dropped, he has resorted to screenshotting websites.
It's impossible that this may pose a security risk, is it?

You can find Santa's website here: https://hackvent.deathspirate.com

## Solution

We are given a website that allows us to take screenshots of websites. Moreover, the website clearly mentions the fact
that it is deployed on the AWS cloud. We can request a screenshot of the [AWS Metadata
Endpoint](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ec2-instance-metadata.html) and obtain a token to log in
with. Unfortunately, this process is quite tedious and it's easy to make a mistake by mixing up a `1` for an `l`. In the
end, the challenge author decided to make it a bit easier and obtain the token through the HTTP response directly
instead of the image. With the token, it was then possible to log in and obtain part of the flag.

There was another part of this challenge but unfortunately the website is no longer available at the moment and I can't
really reproduce the solving process anymore...

