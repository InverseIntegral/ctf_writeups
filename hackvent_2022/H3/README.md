# H3 - Ruprecht's Secret

## Description

Level: Hard<br/>

## Solution

This one was very well hidden and took me quite some time to find. The flag of challenge 19
`HV22{__N1c3__You__ARe__Ind33d__}` had an interesting length and could be used as a private key for a blockchain wallet.
We can convert the flag to hex `485632327b5f5f4e3163335f5f596f755f5f4152655f5f496e643333645f5f7d` and then import it in
MetaMask to obtain the public address `0x65cCa9C197f6cF1e38628E4dA7305D924466e4fc`. [Searching for this address on
Etherscan](https://goerli.etherscan.io/address/0x65cCa9C197f6cF1e38628E4dA7305D924466e4fc) shows us one interesting
transaction that contains the hiden flag:
```
Weihnachtsmann
Sag mir, wieso bist du so schlau?
Woher kennst du ganz genau
Den Weihnachtswunsch von jedem Kind? (Weihnachtsmann)
Dass ich so gern ein Fahrrad möcht
Gelbe Streifen wär'n mir recht
Und Speichen, die wie Silber sind
Kein Weg ist dir im Schlitten je zu weit
Ob nah, ob fern, beschenkst du alle Kinder gern
Jetzt zur Weihnachtszeit (Der Weihnachtsmann), wenn es schneit (Kommt bald an)
Und alles weiß ist weit und breit!
Weihnachtsmann, ich hab' dir einen langen Brief geschrieben
Dass dich alle Kinder lieben, und ich hoff', du liebst auch mich 

Ich weiß, es ist soweit schon bald
Dass in der Winternacht, so kalt
Kinderlachen laut erschallt

FLAG: HV22{W31hN4Cht5m4Nn_&C0._KG}
```
