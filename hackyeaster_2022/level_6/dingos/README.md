# Dingos

## Description
Level: medium<br/>
Author: PS

If you like 🐕 Dingos, check out my new web site!

[👉 my fancy Dingo web site](https://dingos.s3.eu-west-1.amazonaws.com/index.html)

## Solution

For this challenge we are given an AWS bucket that presents us a version 2 HTML file. If we remove the `/index.html`
part of the url, we get a listing of all the ressources in the bucket:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<ListBucketResult
	<Contents>
		<Key>img/dingo1.jpg</Key>
	</Contents>
	<Contents>
		<Key>img/dingo2.jpg</Key>
	</Contents>
	<Contents>
		<Key>img/dingo3.jpg</Key>
	</Contents>
	<Contents>
		<Key>img/dingo4.jpg</Key>
	</Contents>
	<Contents>
		<Key>img/dingo_egg_ognid.png</Key>
	</Contents>
	<Contents>
		<Key>index.html</Key>
	</Contents>
</ListBucketResult>
```

Unfortunately `img/dingo_egg_ognid.png` is just a fake flag. But what if we go back to the version 1 of the bucket? [The
documentation of AWS](https://docs.aws.amazon.com/AmazonS3/latest/userguide/list-obj-version-enabled-bucket.html) states
that we can add `?versions` to the URL to get the different versions of our objects. And indeed, we find an earlier
version of the egg:

```xml
<Version>
	<Key>img/dingo_egg_ognid.png</Key>
	<VersionId>efyGzmXduxQAcaQIBgsxEj5i8xlCUdjG</VersionId>
	<IsLatest>false</IsLatest>
```

If we query the old version with `dingo_egg_ognid.png?versionId=efyGzmXduxQAcaQIBgsxEj5i8xlCUdjG` we get the flag:

![](dingo_egg_ognid.png)

