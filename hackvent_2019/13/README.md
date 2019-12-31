# 13 - TrieMe

## Description

Level: Medium<br/>
Author: kiwi

Switzerland's national security is at risk. As you try to infiltrate a secret spy facility to save the nation you
stumble upon an interesting looking login portal.

Can you break it and retrieve the critical information?

- Facility: [http://whale.hacking-lab.com:8888/trieme/](http://whale.hacking-lab.com:8888/trieme/)
- [HV19.13-NotesBean.java.zip](34913db9-fd2a-43c8-b563-55a1d10ee4cb.zip)

## Solution

The zip file contained a simple Java program with the following important parts:

```java
private PatriciaTrie<Integer> trie = init();
private static final String securitytoken = "auth_token_4835989";

public String getTrie() throws IOException {
    if(isAdmin(trie)) {
        InputStream in = getStreamFromResourcesFolder("data/flag.txt");
        StringWriter writer = new StringWriter();
        IOUtils.copy(in, writer, "UTF-8");
        String flag = writer.toString();
        return flag;
    }
    return "INTRUSION WILL BE REPORTED!";
}

public void setTrie(String note) {
    trie.put(unescapeJava(note), 0);
}
    	
private static PatriciaTrie<Integer> init(){
    PatriciaTrie<Integer> trie = new PatriciaTrie<Integer>();
    trie.put(securitytoken,0);

    return trie;
}

private static boolean isAdmin(PatriciaTrie<Integer> trie){
    return !trie.containsKey(securitytoken);
}
```

The goal of this challenge was to manipulte the trie such that it does not longer contain a particular key. At first I
thought that there was some kind of Java (de)serialization vulnerability but it turns out that there was a bug in the Apache
Commons Collections library. A search on their issue tracker revealed [the following
bug](https://issues.apache.org/jira/browse/COLLECTIONS-714). By adding null characters at the end of a key the element
withou the null character gets removed. I then simply entered `auth_token_4835989\u0000` into the input field and got
the flag `HV19{get_th3_chocolateZ}`.
