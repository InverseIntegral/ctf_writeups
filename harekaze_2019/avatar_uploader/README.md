# Avatar Uploader 1

Category: Misc<br/>
Author: st98

I made a web application called Avatar Uploader, which you can upload avatars. The uploader checks types of uploaded
images and only accepts PNG. However, if you could trick the check, I will give you the flag.

## Solution

The flag is printed when an avatar is uploaded that tricks the check:

```php
// check whether file is uploaded
if (!file_exists($_FILES['file']['tmp_name']) || !is_uploaded_file($_FILES['file']['tmp_name'])) {
  error('No file was uploaded.');
}

// check file size
if ($_FILES['file']['size'] > 256000) {
  error('Uploaded file is too large.');
}

// check file type
$finfo = finfo_open(FILEINFO_MIME_TYPE);
$type = finfo_file($finfo, $_FILES['file']['tmp_name']);
finfo_close($finfo);
if (!in_array($type, ['image/png'])) {
  error('Uploaded file is not PNG format.');
}

// check file width/height
$size = getimagesize($_FILES['file']['tmp_name']);
if ($size[0] > 256 || $size[1] > 256) {
  error('Uploaded image is too large.');
}
if ($size[2] !== IMAGETYPE_PNG) {
  // I hope this never happens...
  error('What happened...? OK, the flag for part 1 is: <code>' . getenv('FLAG1') . '</code>');
}
```

`finfo_file` should say that the image is a PNG and `getimagesize` should say that it's not. I constructed an image with
the PNG header but without any data:

```
00000000: 8950 4e47 0d0a 1a0a 0000 000d 4948 4452  .PNG........IHDR
00000010: 0a                                       .
```

This tricked the check and revealed the flag: `HarekazeCTF{seikai_wa_hitotsu!janai!!}`
