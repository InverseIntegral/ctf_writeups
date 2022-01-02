# 03 - Too Much GlItTer!

## Description

To celebrate Christmas even more the elves have setup a small website to help promote christmas on the internet. It is
currently under heavy development but they wanted to show it off anyhow.

Unfortunately they made a pretty silly error which threatens the future of their project.

## Solution

From the challenge title it was clear to me that we would be dealing with git. I navigated to `/.git/` and found a git
folder structure exposed. I then used [this git downloader](https://github.com/arthaud/git-dumper) to fetch the
repository. Getting the flag from there was simple:

```
> git log

commit 0bd2f175eb525057f6f306d7b420e24807beb9f2 (HEAD -> master)
Author: Mathias Scherer <scherer.mat@gmail.com>
Date:   Wed Dec 1 16:29:24 2021 +0100

    Adds flag placeholder

commit 9189c31b7f1c3f2e40133851ba3b6c39ffb704bd
Author: Mathias Scherer <scherer.mat@gmail.com>
Date:   Wed Dec 1 16:27:07 2021 +0100

    Initial commit

> git branch

  feature/flag
* master


> git checkout feature/flag
> git log

commit b009ea9155d990aa9185e1157aaf583a636e93fd (HEAD -> feature/flag)
Author: Mathias Scherer <scherer.mat@gmail.com>
Date:   Wed Dec 1 16:28:33 2021 +0100

    Adds flag to flag.html

commit 9189c31b7f1c3f2e40133851ba3b6c39ffb704bd
Author: Mathias Scherer <scherer.mat@gmail.com>
Date:   Wed Dec 1 16:27:07 2021 +0100

    Initial commit

> git diff b009ea9155d990aa9185e1157aaf583a636e93fd^!
```

From this I got the flag `HV{n3V3r_Sh0w_Y0uR_.git}`.

