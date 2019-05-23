# Day 14: power in the shell

We get this powershell script and the encrypt flag `2A4C9AA52257B56837369D5DD7019451C0EC04427EB95EB741D0273D55`:

```powershell
. "$PSScriptRoot\flag.ps1" #thumbprint 1398ED7F59A62962D5A47DD0D32B71156DD6AF6B46BEA949976331B8E1

if ($PSVersionTable.PSVersion.Major -gt 2)
{
    $m = [System.Numerics.BigInteger]::Parse($flag, 'AllowHexSpecifier');
    $n = [System.Numerics.BigInteger]::Parse("0D8A7A45D9BE42BB3F03F710CF105628E8080F6105224612481908DC721", 'AllowHexSpecifier');
    $c = [System.Numerics.BigInteger]::ModPow($m, 1+1, $n)
    write-host "encrypted flag:" $c.ToString("X");
}
```

We can write this as an equation `encrypted_flag = m^2 % n`. Now we have to find m. Using the [quadratic modular
equation solver](https://www.alpertron.com.ar/QUADMOD.HTM) this equation can be solved easily.

![](images/powershell.png)

The third solution gives us the correct flag when decoded to ASCII.
