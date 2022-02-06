#!/usr/local/bin/node
// don't mind the ugly hack to read input
console.log("What do you want to run?");
let inpBuf = Buffer.alloc(2048);
const input = inpBuf.slice(0, require("fs").readSync(0, inpBuf)).toString("utf8");
inpBuf = undefined;

Function.prototype.constructor = undefined;
(async () => {}).constructor.prototype.constructor = undefined;
(function*(){}).constructor.prototype.constructor = undefined;
(async function*(){}).constructor.prototype.constructor = undefined;

for (const key of Object.getOwnPropertyNames(global)) {
    if (["global", "console", "eval"].includes(key)) {
        continue;
    }
    global[key] = undefined;
    delete global[key];
}

delete global.global;
process = undefined;

{
    let AbortController=undefined;let AbortSignal=undefined;let AggregateError=undefined;let Array=undefined;let ArrayBuffer=undefined;let Atomics=undefined;let BigInt=undefined;let BigInt64Array=undefined;let BigUint64Array=undefined;let Boolean=undefined;let Buffer=undefined;let DOMException=undefined;let DataView=undefined;let Date=undefined;let Error=undefined;let EvalError=undefined;let Event=undefined;let EventTarget=undefined;let FinalizationRegistry=undefined;let Float32Array=undefined;let Float64Array=undefined;let Function=undefined;let Infinity=undefined;let Int16Array=undefined;let Int32Array=undefined;let __dirname=undefined;let Int8Array=undefined;let Intl=undefined;let JSON=undefined;let Map=undefined;let Math=undefined;let MessageChannel=undefined;let MessageEvent=undefined;let MessagePort=undefined;let NaN=undefined;let Number=undefined;let Object=undefined;let Promise=undefined;let Proxy=undefined;let RangeError=undefined;let ReferenceError=undefined;let Reflect=undefined;let RegExp=undefined;let Set=undefined;let SharedArrayBuffer=undefined;let String=undefined;let Symbol=undefined;let SyntaxError=undefined;let TextDecoder=undefined;let TextEncoder=undefined;let TypeError=undefined;let URIError=undefined;let URL=undefined;let URLSearchParams=undefined;let Uint16Array=undefined;let Uint32Array=undefined;let Uint8Array=undefined;let Uint8ClampedArray=undefined;let WeakMap=undefined;let WeakRef=undefined;let WeakSet=undefined;let WebAssembly=undefined;let _=undefined;let exports=undefined;let _error=undefined;let assert=undefined;let async_hooks=undefined;let atob=undefined;let btoa=undefined;let buffer=undefined;let child_process=undefined;let clearImmediate=undefined;let clearInterval=undefined;let clearTimeout=undefined;let cluster=undefined;let constants=undefined;let crypto=undefined;let decodeURI=undefined;let decodeURIComponent=undefined;let dgram=undefined;let diagnostics_channel=undefined;let dns=undefined;let domain=undefined;let encodeURI=undefined;let encodeURIComponent=undefined;let arguments=undefined;let escape=undefined;let events=undefined;let fs=undefined;let global=undefined;let globalThis=undefined;let http=undefined;let http2=undefined;let https=undefined;let inspector=undefined;let isFinite=undefined;let isNaN=undefined;let module=undefined;let net=undefined;let os=undefined;let parseFloat=undefined;let parseInt=undefined;let path=undefined;let perf_hooks=undefined;let performance=undefined;let process=undefined;let punycode=undefined;let querystring=undefined;let queueMicrotask=undefined;let readline=undefined;let repl=undefined;let require=undefined;let setImmediate=undefined;let setInterval=undefined;let __filename=undefined;let setTimeout=undefined;let stream=undefined;let string_decoder=undefined;let structuredClone=undefined;let sys=undefined;let timers=undefined;let tls=undefined;let trace_events=undefined;let tty=undefined;let unescape=undefined;let url=undefined;let util=undefined;let v8=undefined;let vm=undefined;let wasi=undefined;let worker_threads=undefined;let zlib=undefined;let __proto__=undefined;let hasOwnProperty=undefined;let isPrototypeOf=undefined;let propertyIsEnumerable=undefined;let toLocaleString=undefined;let toString=undefined;let valueOf=undefined;

    console.log(eval(input));
}
