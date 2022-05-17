# Bucket Egg

## Description
Level: easy<br/>
Author: PS

My Irish friend told me about his new web site. He told me it was in a bucket named `egg-in-a-bucket`. No clue what that is...

## Solution

For this challenge we are supposed to find an AWS bucket named `egg-in-a-bucket`. Furthermore, the challenge description
hints at the concrete location. Checking [the AWS
documentation](https://docs.aws.amazon.com/general/latest/gr/s3.html#s3_website_region_endpoints) we can see that there
exists a region Europe (Ireland). All we have to do now is use the bucket name and access it:
`egg-in-a-bucket.s3-website-eu-west-1.amazonaws.com`. We are greeted with a QR code that contains the flag:

![](qr.png)

