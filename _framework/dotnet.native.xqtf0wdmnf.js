// This code implements the `-sMODULARIZE` settings by taking the generated
// JS program code (INNER_JS_CODE) and wrapping it in a factory function.

// When targeting node and ES6 we use `await import ..` in the generated code
// so the outer function needs to be marked as async.
async function createDotnetRuntime(moduleArg = {}) {
  var Module = moduleArg;
// include: shell.js
// include: minimum_runtime_check.js
// end include: minimum_runtime_check.js
// The Module object: Our interface to the outside world. We import
// and export values on it. There are various ways Module can be used:
// 1. Not defined. We create it here
// 2. A function parameter, function(moduleArg) => Promise<Module>
// 3. pre-run appended it, var Module = {}; ..generated code..
// 4. External script tag defines var Module.
// We need to check if Module already exists (e.g. case 3 above).
// Substitution will be replaced with actual code on later stage of the build,
// this way Closure Compiler will not mangle it (e.g. case 4. above).
// Note that if you want to run closure, and also to use Module
// after the generated code, you will need to define   var Module = {};
// before the code. Then that object will be used in the code, and you
// can continue to use Module afterwards as well.

// Determine the runtime environment we are in. You can customize this by
// setting the ENVIRONMENT setting at compile time (see settings.js).

// Attempt to auto-detect the environment
var ENVIRONMENT_IS_WEB = !!globalThis.window;
var ENVIRONMENT_IS_WORKER = !!globalThis.WorkerGlobalScope;
// N.b. Electron.js environment is simultaneously a NODE-environment, but
// also a web environment.
var ENVIRONMENT_IS_NODE = globalThis.process?.versions?.node && globalThis.process?.type != 'renderer';
var ENVIRONMENT_IS_SHELL = !ENVIRONMENT_IS_WEB && !ENVIRONMENT_IS_NODE && !ENVIRONMENT_IS_WORKER;

if (ENVIRONMENT_IS_NODE) {
  // When building an ES module `require` is not normally available.
  // We need to use `createRequire()` to construct the require()` function.
  const { createRequire } = await import('node:module');
  /** @suppress{duplicate} */
  var require = createRequire(import.meta.url);

}

// --pre-jses are emitted after the Module integration code, so that they can
// refer to Module (if they choose; they can also define Module)
// include: /usr/share/dotnet/packs/Microsoft.NETCore.App.Runtime.Mono.browser-wasm/11.0.0/runtimes/browser-wasm/native/src/es6/dotnet.es6.pre.js
if (_nativeModuleLoaded) throw new Error("Native module already loaded");
_nativeModuleLoaded = true;
createDotnetRuntime = Module = moduleArg(Module);
// end include: /usr/share/dotnet/packs/Microsoft.NETCore.App.Runtime.Mono.browser-wasm/11.0.0/runtimes/browser-wasm/native/src/es6/dotnet.es6.pre.js


var programArgs = [];
var thisProgram = './this.program';
var quit_ = (status, toThrow) => {
  throw toThrow;
};

var _scriptName = import.meta.url;

// `/` should be present at the end if `scriptDirectory` is not empty
var scriptDirectory = '';
function locateFile(path) {
  if (Module['locateFile']) {
    return Module['locateFile'](path, scriptDirectory);
  }
  return scriptDirectory + path;
}

// Hooks that are implemented differently in different runtime environments.
var readAsync, readBinary;

if (ENVIRONMENT_IS_NODE) {

  // These modules will usually be used on Node.js. Load them eagerly to avoid
  // the complexity of lazy-loading.
  var fs = require('node:fs');

  if (_scriptName.startsWith('file:')) {
    scriptDirectory = require('node:path').dirname(require('node:url').fileURLToPath(_scriptName)) + '/';
  }

// include: node_shell_read.js
readBinary = (filename) => {
  // We need to re-wrap `file://` strings to URLs.
  filename = isFileURI(filename) ? new URL(filename) : filename;
  var ret = fs.readFileSync(filename);
  return ret;
};

readAsync = async (filename, binary = true) => {
  // See the comment in the `readBinary` function.
  filename = isFileURI(filename) ? new URL(filename) : filename;
  var ret = fs.readFileSync(filename, binary ? undefined : 'utf8');
  return ret;
};
// end include: node_shell_read.js
  if (process.argv.length > 1) {
    thisProgram = process.argv[1].replace(/\\/g, '/');
  }

  programArgs = process.argv.slice(2);

  quit_ = (status, toThrow) => {
    process.exitCode = status;
    throw toThrow;
  };

} else
if (ENVIRONMENT_IS_SHELL) {

  readBinary = (f) => {
    if (globalThis.readbuffer) {
      return new Uint8Array(readbuffer(f));
    }
    let data = read(f, 'binary');
    assert(typeof data == 'object');
    return data;
  };

  readAsync = async (f) => readBinary(f);

  globalThis.clearTimeout ??= (id) => {};

  // v8 and jsc both use `arguments`. spidermonkey uses `scriptArgs`
  programArgs = globalThis.arguments ?? globalThis.scriptArgs;

  if (globalThis.quit) {
    quit_ = (status, toThrow) => {
      // Unlike node which has process.exitCode, d8 has no such mechanism. So we
      // have no way to set the exit code and then let the program exit with
      // that code when it naturally stops running (say, when all setTimeouts
      // have completed). For that reason, we must call `quit` - the only way to
      // set the exit code - but quit also halts immediately.  To increase
      // consistency with node (and the web) we schedule the actual quit call
      // using a setTimeout to give the current stack and any exception handlers
      // a chance to run.  This enables features such as addOnPostRun (which
      // expected to be able to run code after main returns).
      setTimeout(() => {
        if (!(toThrow instanceof ExitStatus)) {
          let toLog = toThrow;
          if (toThrow && typeof toThrow == 'object' && toThrow.stack) {
            toLog = [toThrow, toThrow.stack];
          }
          err(`exiting due to exception: ${toLog}`);
        }
        quit(status);
      });
      throw toThrow;
    };
  }

  if (globalThis.print) {
    // Use `print` to implement console.log/error/warn as needed.
    globalThis.console ??= /** @type{!Console} */({});
    console.log ??= /** @type{!function(this:Console, ...*): undefined} */ (print);
    console.warn ??= console.error ??= /** @type{!function(this:Console, ...*): undefined} */ (globalThis.printErr ?? print);
  }

} else

// Note that this includes Node.js workers when relevant (pthreads is enabled).
// Node.js workers are detected as a combination of ENVIRONMENT_IS_WORKER and
// ENVIRONMENT_IS_NODE.
if (ENVIRONMENT_IS_WEB || ENVIRONMENT_IS_WORKER) {
  try {
    scriptDirectory = new URL('.', _scriptName).href; // includes trailing slash
  } catch {
    // Must be a `blob:` or `data:` URL (e.g. `blob:http://site.com/etc/etc`), we cannot
    // infer anything from them.
  }

  {
// include: web_or_worker_shell_read.js
if (ENVIRONMENT_IS_WORKER) {
    readBinary = (url) => {
      var xhr = new XMLHttpRequest();
      xhr.open('GET', url, false);
      xhr.responseType = 'arraybuffer';
      xhr.send(null);
      return new Uint8Array(/** @type{!ArrayBuffer} */(xhr.response));
    };
  }

  readAsync = async (url) => {
    // Fetch has some additional restrictions over XHR, like it can't be used on a file:// url.
    // See https://github.com/github/fetch/pull/92#issuecomment-140665932
    // Cordova or Electron apps are typically loaded from a file:// url.
    // So use XHR on webview if URL is a file URL.
    if (isFileURI(url)) {
      return new Promise((resolve, reject) => {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url, true);
        xhr.responseType = 'arraybuffer';
        xhr.onload = () => {
          if (xhr.status == 200 || (xhr.status == 0 && xhr.response)) { // file URLs can return 0
            resolve(xhr.response);
            return;
          }
          reject(xhr.status);
        };
        xhr.onerror = reject;
        xhr.send(null);
      });
    }
    var response = await fetch(url, { credentials: 'same-origin' });
    if (response.ok) {
      return response.arrayBuffer();
    }
    throw new Error(response.status + ' : ' + response.url);
  };
// end include: web_or_worker_shell_read.js
  }
} else
{
}

var out = console.log.bind(console);
var err = console.error.bind(console);

// end include: shell.js

// include: preamble.js
// === Preamble library stuff ===

// Documentation for the public APIs defined in this file must be updated in:
//    site/source/docs/api_reference/preamble.js.rst
// A prebuilt local version of the documentation is available at:
//    site/build/text/docs/api_reference/preamble.js.txt
// You can also build docs locally as HTML or other formats in site/
// An online HTML version (which may be of a different version of Emscripten)
//    is up at http://kripken.github.io/emscripten-site/docs/api_reference/preamble.js.html

var wasmBinary;

// Wasm globals

//========================================
// Runtime essentials
//========================================

// whether we are quitting the application. no code should run after this.
// set in exit() and abort()
var ABORT = false;

// set by exit() and abort().  Passed to 'onExit' handler.
// NOTE: This is also used as the process return code in shell environments
// but only when noExitRuntime is false.
var EXITSTATUS;

// In STRICT mode, we only define assert() when ASSERTIONS is set.  i.e. we
// don't define it at all in release modes.  This matches the behaviour of
// MINIMAL_RUNTIME.
// TODO(sbc): Make this the default even without STRICT enabled.
/** @type {function(*, string=)} */
function assert(condition, text) {
  if (!condition) {
    // This build was created without ASSERTIONS defined.  `assert()` should not
    // ever be called in this configuration but in case there are callers in
    // the wild leave this simple abort() implementation here for now.
    abort(text);
  }
}

/**
 * Indicates whether filename is delivered via file protocol (as opposed to http/https)
 * @noinline
 */
var isFileURI = (filename) => filename.startsWith('file://');

// include: runtime_common.js
// include: runtime_stack_check.js
// end include: runtime_stack_check.js
// include: runtime_exceptions.js
// end include: runtime_exceptions.js
// include: runtime_debug.js
// end include: runtime_debug.js
// Memory management

var runtimeInitialized = false;

var runtimeExited = false;



// When ALLOW_MEMORY_GROWTH is enabled, the conversion from Wasm
// memory to ArrayBuffer requires some additional logic.
function getMemoryBuffer() {
  return wasmMemory.buffer;
}

function updateMemoryViews() {
  // If we already have a heap that is resizeable/growable buffer we don't
  // need to do anything in updateMemoryViews.
  if (HEAP8?.buffer?.resizable) return;
  var b = getMemoryBuffer();
  Module['HEAP8'] = HEAP8 = new Int8Array(b);
  Module['HEAP16'] = HEAP16 = new Int16Array(b);
  Module['HEAPU8'] = HEAPU8 = new Uint8Array(b);
  Module['HEAPU16'] = HEAPU16 = new Uint16Array(b);
  Module['HEAP32'] = HEAP32 = new Int32Array(b);
  Module['HEAPU32'] = HEAPU32 = new Uint32Array(b);
  Module['HEAPF32'] = HEAPF32 = new Float32Array(b);
  Module['HEAPF64'] = HEAPF64 = new Float64Array(b);
  Module['HEAP64'] = HEAP64 = new BigInt64Array(b);
  Module['HEAPU64'] = HEAPU64 = new BigUint64Array(b);
}

// include: memoryprofiler.js
// end include: memoryprofiler.js
// end include: runtime_common.js
function preRun() {
  var preRun = Module['preRun'];
  if (preRun) {
    if (typeof preRun == 'function') preRun = [preRun];
    onPreRuns.push(...preRun);
  }
  // Begin ATPRERUNS hooks
  callRuntimeCallbacks(onPreRuns);
  // End ATPRERUNS hooks
}

function initRuntime() {
  runtimeInitialized = true;

  // Begin ATINITS hooks
  if (!Module['noFSInit'] && !FS.initialized) FS.init();
TTY.init();
  // End ATINITS hooks

  wasmExports['__wasm_call_ctors']();

  // Begin ATPOSTCTORS hooks
  FS.ignorePermissions = false;
  // End ATPOSTCTORS hooks

}

function exitRuntime() {
  ___funcs_on_exit(); // Native atexit() functions
  // Begin ATEXITS hooks
  FS.quit();
TTY.shutdown();
  // End ATEXITS hooks
  runtimeExited = true;
}

function postRun() {

  var postRun = Module['postRun'];
  if (postRun) {
    if (typeof postRun == 'function') postRun = [postRun];
    onPostRuns.push(...postRun);
  }

  // Begin ATPOSTRUNS hooks
  callRuntimeCallbacks(onPostRuns);
  // End ATPOSTRUNS hooks
}

/**
 * @param {string|number=} what
 */
function abort(what) {
  Module['onAbort']?.(what);

  what = `Aborted(${what})`;
  // TODO(sbc): Should we remove printing and leave it up to whoever
  // catches the exception?
  err(what);

  ABORT = true;

  what += '. Build with -sASSERTIONS for more info.';

  // Use a wasm runtime error, because a JS error might be seen as a foreign
  // exception, which means we'd run destructors on it. We need the error to
  // simply make the program stop.
  // FIXME This approach does not work in Wasm EH because it currently does not assume
  // all RuntimeErrors are from traps; it decides whether a RuntimeError is from
  // a trap or not based on a hidden field within the object. So at the moment
  // we don't have a way of throwing a wasm trap from JS. TODO Make a JS API that
  // allows this in the wasm spec.

  // Suppress closure compiler warning here. Closure compiler's builtin extern
  // definition for WebAssembly.RuntimeError claims it takes no arguments even
  // though it can.
  // TODO(https://github.com/google/closure-compiler/pull/3913): Remove if/when upstream closure gets fixed.
  // See above, in the meantime, we resort to wasm code for trapping.
  //
  // In case abort() is called before the module is initialized, wasmExports
  // and its exported '__trap' function is not available, in which case we throw
  // a RuntimeError.
  //
  // We trap instead of throwing RuntimeError to prevent infinite-looping in
  // Wasm EH code (because RuntimeError is considered as a foreign exception and
  // caught by 'catch_all'), but in case throwing RuntimeError is fine because
  // the module has not even been instantiated, even less running.
  if (runtimeInitialized) {
    ___trap();
  }
  /** @suppress {checkTypes} */
  var e = new WebAssembly.RuntimeError(what);

  // Throw the error whether or not MODULARIZE is set because abort is used
  // in code paths apart from instantiation where an exception is expected
  // to be thrown when abort is called.
  throw e;
}

var wasmBinaryFile;

function findWasmBinary() {

  if (ENVIRONMENT_IS_SHELL) {
    return 'dotnet.native.wasm';
  }

  if (Module['locateFile']) {
    return locateFile('dotnet.native.wasm');
  }

  // Use bundler-friendly `new URL(..., import.meta.url)` pattern; works in browsers too.
  return new URL('dotnet.native.wasm', import.meta.url).href;

}

function getBinarySync(file) {
  if (readBinary) {
    return readBinary(file);
  }
  // Throwing a plain string here, even though it not normally advisable since
  // this gets turning into an `abort` in instantiateArrayBuffer.
  throw 'both async and sync fetching of the wasm failed';
}

async function getWasmBinary(binaryFile) {
  // If we don't have the binary yet, load it asynchronously using readAsync.
  if (!wasmBinary) {
    // Fetch the binary using readAsync
    try {
      var response = await readAsync(binaryFile);
      return new Uint8Array(response);
    } catch {
      // Fall back to getBinarySync below;
    }
  }

  // Otherwise, getBinarySync should be able to get it synchronously
  return getBinarySync(binaryFile);
}

async function instantiateArrayBuffer(binaryFile, imports) {
  try {
    var binary = await getWasmBinary(binaryFile);
    var instance = await WebAssembly.instantiate(binary, imports);
    return instance;
  } catch (reason) {
    err(`failed to asynchronously prepare wasm: ${reason}`);

    abort(reason);
  }
}

async function instantiateAsync(binary, binaryFile, imports) {
  if (!binary
      // Don't use streaming for file:// delivered objects in a webview, fetch them synchronously.
      && !isFileURI(binaryFile)
      // Avoid instantiateStreaming() on Node.js environment for now, as while
      // Node.js v18.1.0 implements it, it does not have a full fetch()
      // implementation yet.
      //
      // Reference:
      //   https://github.com/emscripten-core/emscripten/pull/16917
      && !ENVIRONMENT_IS_NODE
      // Shell environments don't have fetch.
      && !ENVIRONMENT_IS_SHELL
     ) {
    try {
      var response = fetch(binaryFile, { credentials: 'same-origin' });
      var instantiationResult = await WebAssembly.instantiateStreaming(response, imports);
      return instantiationResult;
    } catch (reason) {
      // We expect the most common failure cause to be a bad MIME type for the binary,
      // in which case falling back to ArrayBuffer instantiation should work.
      err(`wasm streaming compile failed: ${reason}`);
      err('falling back to ArrayBuffer instantiation');
      // fall back of instantiateArrayBuffer below
    };
  }
  return instantiateArrayBuffer(binaryFile, imports);
}

function getWasmImports() {
  // prepare imports
  var imports = {
    'env': wasmImports,
    'wasi_snapshot_preview1': wasmImports,
  };
  return imports;
}

// Create the wasm instance.
// Receives the wasm imports, returns the exports.
async function createWasm() {
  // Load the wasm module and create an instance of using native support in the JS engine.
  // handle a generated wasm instance, receiving its exports and
  // performing other necessary setup
  function receiveInstance(instance) {
    wasmExports = instance.exports;

    assignWasmExports(wasmExports);

    Module['wasmExports'] = wasmExports;

    updateMemoryViews();

    return wasmExports;
  }

  // Prefer streaming instantiation if available.
  function receiveInstantiationResult(result) {
    // 'result' is a ResultObject object which has both the module and instance.
    // receiveInstance() will swap in the exports (to Module.asm) so they can be called
    // TODO: Due to Closure regression https://github.com/google/closure-compiler/issues/3193, the above line no longer optimizes out down to the following line.
    // When the regression is fixed, can restore the above PTHREADS-enabled path.
    return receiveInstance(result['instance']);
  }

  var info = getWasmImports();

  // User shell pages can write their own Module.instantiateWasm = function(imports, successCallback) callback
  // to manually instantiate the Wasm module themselves. This allows pages to
  // run the instantiation parallel to any other async startup actions they are
  // performing.
  // Also pthreads and wasm workers initialize the wasm instance through this
  // path.
  var instantiateWasm = Module['instantiateWasm'];
  if (instantiateWasm) {
    return new Promise((resolve) => {
        instantiateWasm(info, (inst) => resolve(receiveInstance(inst)));
    });
  }

  wasmBinaryFile ??= findWasmBinary();
  var result = await instantiateAsync(wasmBinary, wasmBinaryFile, info);
  var exports = receiveInstantiationResult(result);
  return exports;
}

// end include: preamble.js

// Begin JS library code


  class ExitStatus {
      name = 'ExitStatus';
      constructor(status) {
        this.message = `Program terminated with exit(${status})`;
        this.status = status;
      }
    }

  /** @type {!Int16Array} */
  var HEAP16;

  /** @type {!Int32Array} */
  var HEAP32;

  /** not-@type {!BigInt64Array} */
  var HEAP64;

  /** @type {!Int8Array} */
  var HEAP8;

  /** @type {!Float32Array} */
  var HEAPF32;

  /** @type {!Float64Array} */
  var HEAPF64;

  /** @type {!Uint16Array} */
  var HEAPU16;

  /** @type {!Uint32Array} */
  var HEAPU32;

  /** not-@type {!BigUint64Array} */
  var HEAPU64;

  /** @type {!Uint8Array} */
  var HEAPU8;

  var callRuntimeCallbacks = (callbacks) => {
      while (callbacks.length > 0) {
        // Pass the module as the first argument.
        callbacks.shift()(Module);
      }
    };
  var onPostRuns = [];
  var addOnPostRun = (cb) => onPostRuns.push(cb);

  var onPreRuns = [];
  var addOnPreRun = (cb) => onPreRuns.push(cb);


  
    /**
   * @param {number} ptr
   * @param {string} type
   */
  function getValue(ptr, type = 'i8') {
    if (type.endsWith('*')) type = '*';
    switch (type) {
      case 'i1': return HEAP8[ptr];
      case 'i8': return HEAP8[ptr];
      case 'i16': return HEAP16[((ptr)>>1)];
      case 'i32': return HEAP32[((ptr)>>2)];
      case 'i64': return HEAP64[((ptr)>>3)];
      case 'float': return HEAPF32[((ptr)>>2)];
      case 'double': return HEAPF64[((ptr)>>3)];
      case '*': return HEAPU32[((ptr)>>2)];
      default: abort(`invalid type for getValue: ${type}`);
    }
  }

  var noExitRuntime = false;

  
    /**
   * @param {number} ptr
   * @param {number} value
   * @param {string} type
   */
  function setValue(ptr, value, type = 'i8') {
    if (type.endsWith('*')) type = '*';
    switch (type) {
      case 'i1': HEAP8[ptr] = value; break;
      case 'i8': HEAP8[ptr] = value; break;
      case 'i16': HEAP16[((ptr)>>1)] = value; break;
      case 'i32': HEAP32[((ptr)>>2)] = value; break;
      case 'i64': HEAP64[((ptr)>>3)] = BigInt(value); break;
      case 'float': HEAPF32[((ptr)>>2)] = value; break;
      case 'double': HEAPF64[((ptr)>>3)] = value; break;
      case '*': HEAPU32[((ptr)>>2)] = value; break;
      default: abort(`invalid type for setValue: ${type}`);
    }
  }

  

  var DOTNET = {
  setup:function setup(emscriptenBuildOptions) {
      // PTHREADS is emscripten's define symbol, which is passed to acorn optimizer, so we could use it here
      const modulePThread = {};
      const ENVIRONMENT_IS_PTHREAD = false;
      const dotnet_replacements = {
          fetch: globalThis.fetch,
          ENVIRONMENT_IS_WORKER,
          modulePThread,
          scriptDirectory,
      };
  
      ENVIRONMENT_IS_WORKER = dotnet_replacements.ENVIRONMENT_IS_WORKER;
      Module.__dotnet_runtime.initializeReplacements(dotnet_replacements);
      noExitRuntime = dotnet_replacements.noExitRuntime;
      fetch = dotnet_replacements.fetch;
      scriptDirectory = dotnet_replacements.scriptDirectory;
      Module.__dotnet_runtime.passEmscriptenInternals({
          isPThread: ENVIRONMENT_IS_PTHREAD,
          quit_, ExitStatus,
          updateMemoryViews,
          getMemory: () => { return wasmMemory; },
          getWasmIndirectFunctionTable: () => { return wasmTable; },
      }, emscriptenBuildOptions);
  
          Module.__dotnet_runtime.configureEmscriptenStartup(Module);
  },
  };
  function _SystemInteropJS_BindJSImportST(
  ) {
  return {runtime_idx:22};//SystemInteropJS_BindJSImportST
  }

  function _SystemInteropJS_CancelPromise(
  ) {
  return {runtime_idx:26};//SystemInteropJS_CancelPromise
  }

  function _SystemInteropJS_InvokeJSFunction(
  ) {
  return {runtime_idx:23};//SystemInteropJS_InvokeJSFunction
  }

  function _SystemInteropJS_InvokeJSImportST(
  ) {
  return {runtime_idx:24};//SystemInteropJS_InvokeJSImportST
  }

  function _SystemInteropJS_ReleaseCSOwnedObject(
  ) {
  return {runtime_idx:21};//SystemInteropJS_ReleaseCSOwnedObject
  }

  function _SystemInteropJS_ResolveOrRejectPromise(
  ) {
  return {runtime_idx:25};//SystemInteropJS_ResolveOrRejectPromise
  }

  function _SystemJS_ConsoleClear(
  ) {
  return {runtime_idx:20};//SystemJS_ConsoleClear
  }

  function _SystemJS_GetCurrentProcessId(
  ) {
  return {runtime_idx:19};//SystemJS_GetCurrentProcessId
  }

  function _SystemJS_GetLocaleInfo(
  ) {
  return {runtime_idx:27};//SystemJS_GetLocaleInfo
  }

  function _SystemJS_RandomBytes(
  ) {
  return {runtime_idx:18};//SystemJS_RandomBytes
  }

  function _SystemJS_ScheduleBackgroundJobImpl(
  ) {
  return {runtime_idx:6};//SystemJS_ScheduleBackgroundJobImpl
  }

  function _SystemJS_ScheduleTimerImpl(
  ) {
  return {runtime_idx:0};//SystemJS_ScheduleTimerImpl
  }

  var UTF8Decoder = globalThis.TextDecoder && new TextDecoder();
  
  
    /**
   * heapOrArray is either a regular array, or a JavaScript typed array view.
   * @param {number} idx
   * @param {number=} maxBytesToRead
   * @param {boolean=} ignoreNul
   * @return {number}
   */
  var findStringEnd = (heapOrArray, idx, maxBytesToRead, ignoreNul) => {
      var maxIdx = idx + maxBytesToRead;
      if (ignoreNul) return maxIdx;
      // TextDecoder needs to know the byte length in advance, it doesn't stop on
      // null terminator by itself.
      // As a tiny code save trick, compare idx against maxIdx using a negation,
      // so that maxBytesToRead=undefined/NaN means Infinity.
      while (heapOrArray[idx] && !(idx >= maxIdx)) ++idx;
      return idx;
    };
  
    /**
   * Given a pointer 'idx' to a null-terminated UTF8-encoded string in the given
   * array that contains uint8 values, returns a copy of that string as a
   * Javascript String object.
   * heapOrArray is either a regular array, or a JavaScript typed array view.
   * @param {number=} idx
   * @param {number=} maxBytesToRead
   * @param {boolean=} ignoreNul - If true, the function will not stop on a NUL character.
   * @return {string}
   */
  var UTF8ArrayToString = (heapOrArray, idx = 0, maxBytesToRead, ignoreNul) => {
  
      var endPtr = findStringEnd(heapOrArray, idx, maxBytesToRead, ignoreNul);
  
      // When using conditional TextDecoder, skip it for short strings as the overhead of the native call is not worth it.
      if (endPtr - idx > 16 && heapOrArray.buffer && UTF8Decoder) {
        return UTF8Decoder.decode(heapOrArray.subarray(idx, endPtr));
      }
      var str = '';
      while (idx < endPtr) {
        // For UTF8 byte structure, see:
        // http://en.wikipedia.org/wiki/UTF-8#Description
        // https://www.ietf.org/rfc/rfc2279.txt
        // https://tools.ietf.org/html/rfc3629
        var u0 = heapOrArray[idx++];
        if (!(u0 & 0x80)) { str += String.fromCharCode(u0); continue; }
        var u1 = heapOrArray[idx++] & 63;
        if ((u0 & 0xE0) == 0xC0) { str += String.fromCharCode(((u0 & 31) << 6) | u1); continue; }
        var u2 = heapOrArray[idx++] & 63;
        if ((u0 & 0xF0) == 0xE0) {
          u0 = ((u0 & 15) << 12) | (u1 << 6) | u2;
        } else {
          u0 = ((u0 & 7) << 18) | (u1 << 12) | (u2 << 6) | (heapOrArray[idx++] & 63);
        }
  
        if (u0 < 0x10000) {
          str += String.fromCharCode(u0);
        } else {
          var ch = u0 - 0x10000;
          str += String.fromCharCode(0xD800 | (ch >> 10), 0xDC00 | (ch & 0x3FF));
        }
      }
      return str;
    };
  
    /**
   * Given a pointer 'ptr' to a null-terminated UTF8-encoded string in the
   * emscripten HEAP, returns a copy of that string as a Javascript String object.
   *
   * @param {number} ptr
   * @param {number=} maxBytesToRead - An optional length that specifies the
   *   maximum number of bytes to read. You can omit this parameter to scan the
   *   string until the first 0 byte. If maxBytesToRead is passed, and the string
   *   at [ptr, ptr+maxBytesToReadr[ contains a null byte in the middle, then the
   *   string will cut short at that byte index.
   * @param {boolean=} ignoreNul - If true, the function will not stop on a NUL character.
   * @return {string}
   */
  var UTF8ToString = (ptr, maxBytesToRead, ignoreNul) => {
      return ptr ? UTF8ArrayToString(HEAPU8, ptr, maxBytesToRead, ignoreNul) : '';
    };
  var ___assert_fail = (condition, filename, line, func) =>
      abort(`Assertion failed: ${UTF8ToString(condition)}, at: ` + [filename ? UTF8ToString(filename) : 'unknown filename', line, func ? UTF8ToString(func) : 'unknown function']);

  var PATH = {
  isAbs:(path) => path.charAt(0) === '/',
  splitPath:(filename) => {
        var splitPathRe = /^(\/?|)([\s\S]*?)((?:\.{1,2}|[^\/]+?|)(\.[^.\/]*|))(?:[\/]*)$/;
        return splitPathRe.exec(filename).slice(1);
      },
  normalizeArray:(parts, allowAboveRoot) => {
        // if the path tries to go above the root, `up` ends up > 0
        var up = 0;
        for (var i = parts.length - 1; i >= 0; i--) {
          var last = parts[i];
          if (last === '.') {
            parts.splice(i, 1);
          } else if (last === '..') {
            parts.splice(i, 1);
            up++;
          } else if (up) {
            parts.splice(i, 1);
            up--;
          }
        }
        // if the path is allowed to go above the root, restore leading ..s
        if (allowAboveRoot) {
          for (; up; up--) {
            parts.unshift('..');
          }
        }
        return parts;
      },
  normalize:(path) => {
        var isAbsolute = PATH.isAbs(path),
            trailingSlash = path.slice(-1) === '/';
        // Normalize the path
        path = PATH.normalizeArray(path.split('/').filter((p) => !!p), !isAbsolute).join('/');
        if (!path && !isAbsolute) {
          path = '.';
        }
        if (path && trailingSlash) {
          path += '/';
        }
        return (isAbsolute ? '/' : '') + path;
      },
  dirname:(path) => {
        var result = PATH.splitPath(path),
            root = result[0],
            dir = result[1];
        if (!root && !dir) {
          // No dirname whatsoever
          return '.';
        }
        if (dir) {
          // It has a dirname, strip trailing slash
          dir = dir.slice(0, -1);
        }
        return root + dir;
      },
  basename:(path) => path && path.match(/([^\/]+|\/)\/*$/)[1],
join:(...paths) => PATH.normalize(paths.join('/')),
join2:(l, r) => PATH.normalize(l + '/' + r),
};

/** @noinline */
  var base64Decode = (b64) => {
      if (ENVIRONMENT_IS_NODE) {
        var buf = Buffer.from(b64, 'base64');
        return new Uint8Array(buf.buffer, buf.byteOffset, buf.length);
      }
  
      var b1, b2, i = 0, j = 0, bLength = b64.length;
      var output = new Uint8Array((bLength*3>>2) - (b64[bLength-2] == '=') - (b64[bLength-1] == '='));
      for (; i < bLength; i += 4, j += 3) {
        b1 = base64ReverseLookup[b64.charCodeAt(i+1)];
        b2 = base64ReverseLookup[b64.charCodeAt(i+2)];
        output[j] = base64ReverseLookup[b64.charCodeAt(i)] << 2 | b1 >> 4;
        output[j+1] = b1 << 4 | b2 >> 2;
        output[j+2] = b2 << 6 | base64ReverseLookup[b64.charCodeAt(i+3)];
      }
      return output;
    };
  var initRandomFill = () => {
  
      if (ENVIRONMENT_IS_SHELL) {
        return (view) => {
          if (!os.system) {
            throw new Error('randomFill not supported on d8 unless --enable-os-system is passed');
          }
          const b64 = os.system('sh', ['-c', `head -c${view.byteLength} /dev/urandom | base64 --wrap=0`]);
          view.set(base64Decode(b64));
          return 0;
        };
      }
  
      return (view) => (crypto.getRandomValues(view), 0);
    };
  var randomFill = (view) => (randomFill = initRandomFill())(view);
  
  
  
  var PATH_FS = {
  resolve:(...args) => {
        var resolvedPath = '',
          resolvedAbsolute = false;
        for (var i = args.length - 1; i >= -1 && !resolvedAbsolute; i--) {
          var path = (i >= 0) ? args[i] : FS.cwd();
          // Skip empty and invalid entries
          if (typeof path != 'string') {
            throw new TypeError('Arguments to path.resolve must be strings');
          } else if (!path) {
            return ''; // an invalid portion invalidates the whole thing
          }
          resolvedPath = path + '/' + resolvedPath;
          resolvedAbsolute = PATH.isAbs(path);
        }
        // At this point the path should be resolved to a full absolute path, but
        // handle relative paths to be safe (might happen when process.cwd() fails)
        resolvedPath = PATH.normalizeArray(resolvedPath.split('/').filter((p) => !!p), !resolvedAbsolute).join('/');
        return ((resolvedAbsolute ? '/' : '') + resolvedPath) || '.';
      },
  relative:(from, to) => {
        from = PATH_FS.resolve(from).slice(1);
        to = PATH_FS.resolve(to).slice(1);
        function trim(arr) {
          var start = 0;
          for (; start < arr.length; start++) {
            if (arr[start] !== '') break;
          }
          var end = arr.length - 1;
          for (; end >= 0; end--) {
            if (arr[end] !== '') break;
          }
          if (start > end) return [];
          return arr.slice(start, end - start + 1);
        }
        var fromParts = trim(from.split('/'));
        var toParts = trim(to.split('/'));
        var length = Math.min(fromParts.length, toParts.length);
        var samePartsLength = length;
        for (var i = 0; i < length; i++) {
          if (fromParts[i] !== toParts[i]) {
            samePartsLength = i;
            break;
          }
        }
        var outputParts = [];
        for (var i = samePartsLength; i < fromParts.length; i++) {
          outputParts.push('..');
        }
        outputParts = outputParts.concat(toParts.slice(samePartsLength));
        return outputParts.join('/');
      },
  };
  
  
  
  var FS_stdin_getChar_buffer = [];
  
  var lengthBytesUTF8 = (str) => {
      var len = 0;
      for (var i = 0; i < str.length; ++i) {
        // Gotcha: charCodeAt returns a 16-bit word that is a UTF-16 encoded code
        // unit, not a Unicode code point of the character! So decode
        // UTF16->UTF32->UTF8.
        // See http://unicode.org/faq/utf_bom.html#utf16-3
        var c = str.charCodeAt(i); // possibly a lead surrogate
        if (c <= 0x7F) {
          len++;
        } else if (c <= 0x7FF) {
          len += 2;
        } else if (c >= 0xD800 && c <= 0xDFFF) {
          len += 4; ++i;
        } else {
          len += 3;
        }
      }
      return len;
    };
  
  var stringToUTF8Array = (str, heap, outIdx, maxBytesToWrite) => {
      // Parameter maxBytesToWrite is not optional. Negative values, 0, null,
      // undefined and false each don't write out any bytes.
      if (!(maxBytesToWrite > 0))
        return 0;
  
      var startIdx = outIdx;
      var endIdx = outIdx + maxBytesToWrite - 1; // -1 for string null terminator.
      for (var i = 0; i < str.length; ++i) {
        // For UTF8 byte structure, see http://en.wikipedia.org/wiki/UTF-8#Description
        // and https://www.ietf.org/rfc/rfc2279.txt
        // and https://tools.ietf.org/html/rfc3629
        var u = str.codePointAt(i);
        if (u <= 0x7F) {
          if (outIdx >= endIdx) break;
          heap[outIdx++] = u;
        } else if (u <= 0x7FF) {
          if (outIdx + 1 >= endIdx) break;
          heap[outIdx++] = 0xC0 | (u >> 6);
          heap[outIdx++] = 0x80 | (u & 63);
        } else if (u <= 0xFFFF) {
          if (outIdx + 2 >= endIdx) break;
          heap[outIdx++] = 0xE0 | (u >> 12);
          heap[outIdx++] = 0x80 | ((u >> 6) & 63);
          heap[outIdx++] = 0x80 | (u & 63);
        } else {
          if (outIdx + 3 >= endIdx) break;
          heap[outIdx++] = 0xF0 | (u >> 18);
          heap[outIdx++] = 0x80 | ((u >> 12) & 63);
          heap[outIdx++] = 0x80 | ((u >> 6) & 63);
          heap[outIdx++] = 0x80 | (u & 63);
          // Gotcha: if codePoint is over 0xFFFF, it is represented as a surrogate pair in UTF-16.
          // We need to manually skip over the second code unit for correct iteration.
          i++;
        }
      }
      // Null-terminate the pointer to the buffer.
      heap[outIdx] = 0;
      return outIdx - startIdx;
    };
  /** @type {function(string, boolean=, number=)} */
  var intArrayFromString = (stringy, dontAddNull, length) => {
      var len = length > 0 ? length : lengthBytesUTF8(stringy)+1;
      var u8array = new Array(len);
      var numBytesWritten = stringToUTF8Array(stringy, u8array, 0, u8array.length);
      if (dontAddNull) u8array.length = numBytesWritten;
      return u8array;
    };
  var FS_stdin_getChar = () => {
      if (!FS_stdin_getChar_buffer.length) {
        var result = null;
        if (ENVIRONMENT_IS_NODE) {
          // we will read data by chunks of BUFSIZE
          var BUFSIZE = 256;
          var buf = Buffer.alloc(BUFSIZE);
          var bytesRead = 0;
  
          // For some reason we must suppress a closure warning here, even though
          // fd definitely exists on process.stdin, and is even the proper way to
          // get the fd of stdin,
          // https://github.com/nodejs/help/issues/2136#issuecomment-523649904
          // This started to happen after moving this logic out of library_tty.js,
          // so it is related to the surrounding code in some unclear manner.
          /** @suppress {missingProperties} */
          var fd = process.stdin.fd;
  
          try {
            bytesRead = fs.readSync(fd, buf, 0, BUFSIZE);
          } catch(e) {
            // Cross-platform differences: on Windows, reading EOF throws an
            // exception, but on other OSes, reading EOF returns 0. Uniformize
            // behavior by treating the EOF exception to return 0.
            if (e.toString().includes('EOF')) bytesRead = 0;
            else throw e;
          }
  
          if (bytesRead > 0) {
            result = buf.slice(0, bytesRead).toString('utf-8');
          }
        } else
        if (globalThis.window?.prompt) {
          // Browser.
          result = window.prompt('Input: ');  // returns null on cancel
          if (result !== null) {
            result += '\n';
          }
        } else
        if (globalThis.readline) {
          /** @suppress{checkTypes, undefinedVars} */
          result = readline();
          if (result) {
            result += '\n';
          }
        } else
        {}
        if (!result) {
          return null;
        }
        FS_stdin_getChar_buffer = intArrayFromString(result, true);
      }
      return FS_stdin_getChar_buffer.shift();
    };
  var TTY = {
  ttys:[],
  init() {
        // https://github.com/emscripten-core/emscripten/pull/1555
        // if (ENVIRONMENT_IS_NODE) {
        //   // currently, FS.init does not distinguish if process.stdin is a file or TTY
        //   // device, it always assumes it's a TTY device. because of this, we're forcing
        //   // process.stdin to UTF8 encoding to at least make stdin reading compatible
        //   // with text files until FS.init can be refactored.
        //   process.stdin.setEncoding('utf8');
        // }
      },
  shutdown() {
        // https://github.com/emscripten-core/emscripten/pull/1555
        // if (ENVIRONMENT_IS_NODE) {
        //   // inolen: any idea as to why node -e 'process.stdin.read()' wouldn't exit immediately (with process.stdin being a tty)?
        //   // isaacs: because now it's reading from the stream, you've expressed interest in it, so that read() kicks off a _read() which creates a ReadReq operation
        //   // inolen: I thought read() in that case was a synchronous operation that just grabbed some amount of buffered data if it exists?
        //   // isaacs: it is. but it also triggers a _read() call, which calls readStart() on the handle
        //   // isaacs: do process.stdin.pause() and i'd think it'd probably close the pending call
        //   process.stdin.pause();
        // }
      },
  register(dev, ops) {
        TTY.ttys[dev] = { input: [], output: [], ops: ops };
        FS.registerDevice(dev, TTY.stream_ops);
      },
  stream_ops:{
  open(stream) {
          var tty = TTY.ttys[stream.node.rdev];
          if (!tty) {
            throw new FS.ErrnoError(43);
          }
          stream.tty = tty;
          stream.seekable = false;
        },
  close(stream) {
          // flush any pending line data
          stream.tty.ops.fsync(stream.tty);
        },
  fsync(stream) {
          stream.tty.ops.fsync(stream.tty);
        },
  read(stream, buffer, offset, length, pos /* ignored */) {
          if (!stream.tty || !stream.tty.ops.get_char) {
            throw new FS.ErrnoError(60);
          }
          var bytesRead = 0;
          for (var i = 0; i < length; i++) {
            var result;
            try {
              result = stream.tty.ops.get_char(stream.tty);
            } catch (e) {
              throw new FS.ErrnoError(29);
            }
            if (result === undefined && bytesRead === 0) {
              throw new FS.ErrnoError(6);
            }
            if (result === null || result === undefined) break;
            bytesRead++;
            buffer[offset+i] = result;
          }
          if (bytesRead) {
            stream.node.atime = Date.now();
          }
          return bytesRead;
        },
  write(stream, buffer, offset, length, pos) {
          if (!stream.tty || !stream.tty.ops.put_char) {
            throw new FS.ErrnoError(60);
          }
          try {
            for (var i = 0; i < length; i++) {
              stream.tty.ops.put_char(stream.tty, buffer[offset+i]);
            }
          } catch (e) {
            throw new FS.ErrnoError(29);
          }
          if (length) {
            stream.node.mtime = stream.node.ctime = Date.now();
          }
          return i;
        },
  },
  default_tty_ops:{
  get_char(tty) {
          return FS_stdin_getChar();
        },
  put_char(tty, val) {
          if (val === null || val === 10) {
            out(UTF8ArrayToString(tty.output));
            tty.output = [];
          } else {
            if (val != 0) tty.output.push(val); // val == 0 would cut text output off in the middle.
          }
        },
  fsync(tty) {
          if (tty.output?.length > 0) {
            out(UTF8ArrayToString(tty.output));
            tty.output = [];
          }
        },
  ioctl_tcgets(tty) {
          // typical setting
          return {
            c_iflag: 25856,
            c_oflag: 5,
            c_cflag: 191,
            c_lflag: 35387,
            c_cc: [
              0x03, 0x1c, 0x7f, 0x15, 0x04, 0x00, 0x01, 0x00, 0x11, 0x13, 0x1a, 0x00,
              0x12, 0x0f, 0x17, 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            ]
          };
        },
  ioctl_tcsets(tty, optional_actions, data) {
          // currently just ignore
          return 0;
        },
  ioctl_tiocgwinsz(tty) {
          return [24, 80];
        },
  },
  default_tty1_ops:{
  put_char(tty, val) {
          if (val === null || val === 10) {
            err(UTF8ArrayToString(tty.output));
            tty.output = [];
          } else {
            if (val != 0) tty.output.push(val);
          }
        },
  fsync(tty) {
          if (tty.output?.length > 0) {
            err(UTF8ArrayToString(tty.output));
            tty.output = [];
          }
        },
  },
  };
  
  
  var zeroMemory = (ptr, size) => HEAPU8.fill(0, ptr, ptr + size);
  
  var alignMemory = (size, alignment) => {
      return Math.ceil(size / alignment) * alignment;
    };
  var mmapAlloc = (size) => {
      size = alignMemory(size, 65536);
      var ptr = _emscripten_builtin_memalign(65536, size);
      if (ptr) zeroMemory(ptr, size);
      return ptr;
    };
  var MEMFS = {
  ops_table:null,
  mount(mount) {
        return MEMFS.createNode(null, '/', 16895, 0);
      },
  createNode(parent, name, mode, dev) {
        if (FS.isBlkdev(mode) || FS.isFIFO(mode)) {
          // not supported
          throw new FS.ErrnoError(63);
        }
        MEMFS.ops_table ||= {
          dir: {
            node: {
              getattr: MEMFS.node_ops.getattr,
              setattr: MEMFS.node_ops.setattr,
              lookup: MEMFS.node_ops.lookup,
              mknod: MEMFS.node_ops.mknod,
              rename: MEMFS.node_ops.rename,
              unlink: MEMFS.node_ops.unlink,
              rmdir: MEMFS.node_ops.rmdir,
              readdir: MEMFS.node_ops.readdir,
              symlink: MEMFS.node_ops.symlink
            },
            stream: {
              llseek: MEMFS.stream_ops.llseek
            }
          },
          file: {
            node: {
              getattr: MEMFS.node_ops.getattr,
              setattr: MEMFS.node_ops.setattr
            },
            stream: {
              llseek: MEMFS.stream_ops.llseek,
              read: MEMFS.stream_ops.read,
              write: MEMFS.stream_ops.write,
              mmap: MEMFS.stream_ops.mmap,
              msync: MEMFS.stream_ops.msync
            }
          },
          link: {
            node: {
              getattr: MEMFS.node_ops.getattr,
              setattr: MEMFS.node_ops.setattr,
              readlink: MEMFS.node_ops.readlink
            },
            stream: {}
          },
          chrdev: {
            node: {
              getattr: MEMFS.node_ops.getattr,
              setattr: MEMFS.node_ops.setattr
            },
            stream: FS.chrdev_stream_ops
          }
        };
        var node = FS.createNode(parent, name, mode, dev);
        if (FS.isDir(node.mode)) {
          node.node_ops = MEMFS.ops_table.dir.node;
          node.stream_ops = MEMFS.ops_table.dir.stream;
          node.contents = {};
        } else if (FS.isFile(node.mode)) {
          node.node_ops = MEMFS.ops_table.file.node;
          node.stream_ops = MEMFS.ops_table.file.stream;
          // The actual number of bytes used in the typed array, as opposed to
          // contents.length which gives the whole capacity.
          node.usedBytes = 0;
          // The byte data of the file is stored in a typed array.
          // Note: typed arrays are not resizable like normal JS arrays are, so
          // there is a small penalty involved for appending file writes that
          // continuously grow a file similar to std::vector capacity vs used.
          node.contents = MEMFS.emptyFileContents ??= new Uint8Array(0);
        } else if (FS.isLink(node.mode)) {
          node.node_ops = MEMFS.ops_table.link.node;
          node.stream_ops = MEMFS.ops_table.link.stream;
        } else if (FS.isChrdev(node.mode)) {
          node.node_ops = MEMFS.ops_table.chrdev.node;
          node.stream_ops = MEMFS.ops_table.chrdev.stream;
        }
        node.atime = node.mtime = node.ctime = Date.now();
        // add the new node to the parent
        if (parent) {
          parent.contents[name] = node;
          parent.atime = parent.mtime = parent.ctime = node.atime;
        }
        return node;
      },
  getFileDataAsTypedArray(node) {
        return node.contents.subarray(0, node.usedBytes); // Make sure to not return excess unused bytes.
      },
  expandFileStorage(node, newCapacity) {
        var prevCapacity = node.contents.length;
        if (prevCapacity >= newCapacity) return; // No need to expand, the storage was already large enough.
        // Don't expand strictly to the given requested limit if it's only a very
        // small increase, but instead geometrically grow capacity.
        // For small filesizes (<1MB), perform size*2 geometric increase, but for
        // large sizes, do a much more conservative size*1.125 increase to avoid
        // overshooting the allocation cap by a very large margin.
        var CAPACITY_DOUBLING_MAX = 1024 * 1024;
        newCapacity = Math.max(newCapacity, (prevCapacity * (prevCapacity < CAPACITY_DOUBLING_MAX ? 2.0 : 1.125)) >>> 0);
        if (prevCapacity) newCapacity = Math.max(newCapacity, 256); // At minimum allocate 256b for each file when expanding.
        var oldContents = MEMFS.getFileDataAsTypedArray(node);
        node.contents = new Uint8Array(newCapacity); // Allocate new storage.
        node.contents.set(oldContents);
      },
  resizeFileStorage(node, newSize) {
        if (node.usedBytes == newSize) return;
        var oldContents = node.contents;
        node.contents = new Uint8Array(newSize); // Allocate new storage.
        node.contents.set(oldContents.subarray(0, Math.min(newSize, node.usedBytes))); // Copy old data over to the new storage.
        node.usedBytes = newSize;
      },
  node_ops:{
  getattr(node) {
          var attr = {};
          // device numbers reuse inode numbers.
          attr.dev = FS.isChrdev(node.mode) ? node.id : 1;
          attr.ino = node.id;
          attr.mode = node.mode;
          attr.nlink = 1;
          attr.uid = 0;
          attr.gid = 0;
          attr.rdev = node.rdev;
          if (FS.isDir(node.mode)) {
            attr.size = 4096;
          } else if (FS.isFile(node.mode)) {
            attr.size = node.usedBytes;
          } else if (FS.isLink(node.mode)) {
            attr.size = node.link.length;
          } else {
            attr.size = 0;
          }
          attr.atime = new Date(node.atime);
          attr.mtime = new Date(node.mtime);
          attr.ctime = new Date(node.ctime);
          // NOTE: In our implementation, st_blocks = Math.ceil(st_size/st_blksize),
          //       but this is not required by the standard.
          attr.blksize = 4096;
          attr.blocks = Math.ceil(attr.size / attr.blksize);
          return attr;
        },
  setattr(node, attr) {
          for (const key of ["mode", "atime", "mtime", "ctime"]) {
            if (attr[key] != null) {
              node[key] = attr[key];
            }
          }
          if (attr.size !== undefined) {
            MEMFS.resizeFileStorage(node, attr.size);
          }
        },
  lookup(parent, name) {
          // This error may happen quite a bit. To avoid overhead we reuse it (and
          // suffer a lack of stack info).
          if (!MEMFS.doesNotExistError) {
            MEMFS.doesNotExistError = new FS.ErrnoError(44);
            /** @suppress {checkTypes} */
            MEMFS.doesNotExistError.stack = '<generic error, no stack>';
          }
          throw MEMFS.doesNotExistError;
        },
  mknod(parent, name, mode, dev) {
          return MEMFS.createNode(parent, name, mode, dev);
        },
  rename(old_node, new_dir, new_name) {
          var new_node;
          try {
            new_node = FS.lookupNode(new_dir, new_name);
          } catch (e) {}
          if (new_node) {
            if (FS.isDir(old_node.mode)) {
              // if we're overwriting a directory at new_name, make sure it's empty.
              for (var i in new_node.contents) {
                throw new FS.ErrnoError(55);
              }
            }
            FS.hashRemoveNode(new_node);
          }
          // do the internal rewiring
          delete old_node.parent.contents[old_node.name];
          new_dir.contents[new_name] = old_node;
          old_node.name = new_name;
          new_dir.ctime = new_dir.mtime = old_node.parent.ctime = old_node.parent.mtime = Date.now();
        },
  unlink(parent, name) {
          delete parent.contents[name];
          parent.ctime = parent.mtime = Date.now();
        },
  rmdir(parent, name) {
          var node = FS.lookupNode(parent, name);
          for (var i in node.contents) {
            throw new FS.ErrnoError(55);
          }
          delete parent.contents[name];
          parent.ctime = parent.mtime = Date.now();
        },
  readdir(node) {
          return ['.', '..', ...Object.keys(node.contents)];
        },
  symlink(parent, newname, oldpath) {
          var node = MEMFS.createNode(parent, newname, 0o777 | 40960, 0);
          node.link = oldpath;
          return node;
        },
  readlink(node) {
          if (!FS.isLink(node.mode)) {
            throw new FS.ErrnoError(28);
          }
          return node.link;
        },
  },
  stream_ops:{
  read(stream, buffer, offset, length, position) {
          var contents = stream.node.contents;
          if (position >= stream.node.usedBytes) return 0;
          var size = Math.min(stream.node.usedBytes - position, length);
          buffer.set(contents.subarray(position, position + size), offset);
          return size;
        },
  write(stream, buffer, offset, length, position, canOwn) {
          // If the buffer is located in main memory (HEAP), and if
          // memory can grow, we can't hold on to references of the
          // memory buffer, as they may get invalidated. That means we
          // need to copy its contents.
          if (buffer.buffer === HEAP8.buffer) {
            canOwn = false;
          }
  
          if (!length) return 0;
          var node = stream.node;
          node.mtime = node.ctime = Date.now();
  
          if (canOwn) {
            node.contents = buffer.subarray(offset, offset + length);
            node.usedBytes = length;
          } else if (node.usedBytes === 0 && position === 0) { // If this is a simple first write to an empty file, do a fast set since we don't need to care about old data.
            node.contents = buffer.slice(offset, offset + length);
            node.usedBytes = length;
          } else {
            MEMFS.expandFileStorage(node, position+length);
            // Use typed array write which is available.
            node.contents.set(buffer.subarray(offset, offset + length), position);
            node.usedBytes = Math.max(node.usedBytes, position + length);
          }
          return length;
        },
  llseek(stream, offset, whence) {
          var position = offset;
          if (whence === 1) {
            position += stream.position;
          } else if (whence === 2) {
            if (FS.isFile(stream.node.mode)) {
              position += stream.node.usedBytes;
            }
          }
          if (position < 0) {
            throw new FS.ErrnoError(28);
          }
          return position;
        },
  mmap(stream, length, position, prot, flags) {
          if (!FS.isFile(stream.node.mode)) {
            throw new FS.ErrnoError(43);
          }
          var ptr;
          var allocated;
          var contents = stream.node.contents;
          // Only make a new copy when MAP_PRIVATE is specified.
          if (!(flags & 2) && contents.buffer === HEAP8.buffer) {
            // We can't emulate MAP_SHARED when the file is not backed by the
            // buffer we're mapping to (e.g. the HEAP buffer).
            allocated = false;
            ptr = contents.byteOffset;
          } else {
            allocated = true;
            ptr = mmapAlloc(length);
            if (!ptr) {
              throw new FS.ErrnoError(48);
            }
            if (contents) {
              // Try to avoid unnecessary slices.
              if (position > 0 || position + length < contents.length) {
                if (contents.subarray) {
                  contents = contents.subarray(position, position + length);
                } else {
                  contents = Array.prototype.slice.call(contents, position, position + length);
                }
              }
              HEAP8.set(contents, ptr);
            }
          }
          return { ptr, allocated };
        },
  msync(stream, buffer, offset, length, mmapFlags) {
          MEMFS.stream_ops.write(stream, buffer, 0, length, offset, false);
          // should we check if bytesWritten and length are the same?
          return 0;
        },
  },
  };
  
  var FS_modeStringToFlags = (str) => {
      if (typeof str != 'string') return str;
      var flagModes = {
        'r': 0,
        'r+': 2,
        'w': 512 | 64 | 1,
        'w+': 512 | 64 | 2,
        'a': 1024 | 64 | 1,
        'a+': 1024 | 64 | 2,
      };
      var flags = flagModes[str];
      if (typeof flags == 'undefined') {
        throw new Error(`Unknown file open mode: ${str}`);
      }
      return flags;
    };
  
  var FS_fileDataToTypedArray = (data) => {
      if (typeof data == 'string') {
        data = intArrayFromString(data, true);
      }
      if (!data.subarray) {
        data = new Uint8Array(data);
      }
      return data;
    };
  
  var FS_getMode = (canRead, canWrite) => {
      var mode = 0;
      if (canRead) mode |= 292 | 73;
      if (canWrite) mode |= 146;
      return mode;
    };
  
  
  var asyncLoad = async (url) => {
      var arrayBuffer = await readAsync(url);
      return new Uint8Array(arrayBuffer);
    };
  
  
  var FS_createDataFile = (...args) => FS.createDataFile(...args);
  
  var getUniqueRunDependency = (id) => {
      return id;
    };
  
  var dependenciesPromise = null;
  var resolveRunDependencies = async () => dependenciesPromise;
  var runDependencies = 0;
  
  
  var dependenciesPromiseResolve = null;
  var removeRunDependency = (id) => {
      runDependencies--;
  
      Module['monitorRunDependencies']?.(runDependencies);
  
      if (!runDependencies) {
        dependenciesPromiseResolve();
      }
    };
  
  
  var addRunDependency = (id) => {
      if (!runDependencies) {
        dependenciesPromise = new Promise((resolve) => dependenciesPromiseResolve = resolve);
      }
      runDependencies++;
  
      Module['monitorRunDependencies']?.(runDependencies);
  
    };
  
  
  var preloadPlugins = [];
  var FS_handledByPreloadPlugin = async (byteArray, fullname) => {
      // Ensure plugins are ready.
      if (typeof Browser != 'undefined') Browser.init();
  
      for (var plugin of preloadPlugins) {
        if (plugin['canHandle'](fullname)) {
          return plugin['handle'](byteArray, fullname);
        }
      }
      // If no plugin handled this file then return the original/unmodified
      // byteArray.
      return byteArray;
    };
  var FS_preloadFile = async (parent, name, url, canRead, canWrite, dontCreateFile, canOwn, preFinish) => {
      // TODO we should allow people to just pass in a complete filename instead
      // of parent and name being that we just join them anyways
      var fullname = name ? PATH_FS.resolve(PATH.join2(parent, name)) : parent;
      var dep = getUniqueRunDependency(`cp ${fullname}`); // might have several active requests for the same fullname
      addRunDependency(dep);
  
      try {
        var byteArray = url;
        if (typeof url == 'string') {
          byteArray = await asyncLoad(url);
        }
  
        byteArray = await FS_handledByPreloadPlugin(byteArray, fullname);
        preFinish?.();
        if (!dontCreateFile) {
          FS_createDataFile(parent, name, byteArray, canRead, canWrite, canOwn);
        }
      } finally {
        removeRunDependency(dep);
      }
    };
  var FS_createPreloadedFile = (parent, name, url, canRead, canWrite, onload, onerror, dontCreateFile, canOwn, preFinish) => {
      FS_preloadFile(parent, name, url, canRead, canWrite, dontCreateFile, canOwn, preFinish).then(onload).catch(onerror);
    };
  var FS = {
  root:null,
  mounts:[],
  devices:{
  },
  streams:[],
  nextInode:1,
  nameTable:null,
  currentPath:"/",
  initialized:false,
  ignorePermissions:true,
  filesystems:null,
  syncFSRequests:0,
  ErrnoError:class {
        name = 'ErrnoError';
        // We set the `name` property to be able to identify `FS.ErrnoError`
        // - the `name` is a standard ECMA-262 property of error objects. Kind of good to have it anyway.
        // - when using PROXYFS, an error can come from an underlying FS
        // as different FS objects have their own FS.ErrnoError each,
        // the test `err instanceof FS.ErrnoError` won't detect an error coming from another filesystem, causing bugs.
        // we'll use the reliable test `err.name == "ErrnoError"` instead
        constructor(errno) {
          this.errno = errno;
        }
      },
  FSStream:class {
        shared = {};
        get object() {
          return this.node;
        }
        set object(val) {
          this.node = val;
        }
        get isRead() {
          return (this.flags & 2097155) !== 1;
        }
        get isWrite() {
          return (this.flags & 2097155) !== 0;
        }
        get isAppend() {
          return (this.flags & 1024);
        }
        get flags() {
          return this.shared.flags;
        }
        set flags(val) {
          this.shared.flags = val;
        }
        get position() {
          return this.shared.position;
        }
        set position(val) {
          this.shared.position = val;
        }
      },
  FSNode:class {
        node_ops = {};
        stream_ops = {};
        readMode = 292 | 73;
        writeMode = 146;
        mounted = null;
        constructor(parent, name, mode, rdev) {
          if (!parent) {
            parent = this;  // root node sets parent to itself
          }
          this.parent = parent;
          this.mount = parent.mount;
          this.id = FS.nextInode++;
          this.name = name;
          this.mode = mode;
          this.rdev = rdev;
          this.atime = this.mtime = this.ctime = Date.now();
        }
        get read() {
          return (this.mode & this.readMode) === this.readMode;
        }
        set read(val) {
          val ? this.mode |= this.readMode : this.mode &= ~this.readMode;
        }
        get write() {
          return (this.mode & this.writeMode) === this.writeMode;
        }
        set write(val) {
          val ? this.mode |= this.writeMode : this.mode &= ~this.writeMode;
        }
        get isFolder() {
          return FS.isDir(this.mode);
        }
        get isDevice() {
          return FS.isChrdev(this.mode);
        }
        // The per-inode readiness wait-queue. The node carries a Set of listener
        // entries {cb}; producers (SOCKFS, PIPEFS) call notifyListeners on a
        // readiness transition, and poll()/epoll consume it. It lives on the node
        // (not the fd) so dup'd fds share one queue. Only nodes that derive real
        // readiness (sockets, pipes, and an epoll's own node) ever use this -
        // always-ready types (regular files, ttys) never register or notify.
        addListener(cb, exclusive = false) {
          var entry = {cb, exclusive};
          var listeners = (this.listeners ??= new Set());
          listeners.add(entry);
          return {listeners, entry};
        }
        notifyListeners(flags) {
          // Iterates the set without copying, which is safe ONLY under a
          // load-bearing contract that every internal listener must honour:
          //   1. A listener must not run user code synchronously (a poll waiter only
          //      resolves a Promise; an epoll registration only re-lists +
          //      re-notifies; the epoll callback only schedules a tick). User code
          //      runs on a later tick, never inside this loop.
          //   2. A listener may delete entries only from ITS OWN waiter, never from
          //      a sibling node's set that may be mid-iteration. (Deleting an entry
          //      of the set being iterated here is fine - a Set tolerates removal of
          //      a not-yet-visited entry mid-iteration; mutating a *different* node's
          //      set is fine because that set is not being iterated.)
          // Violating either gives silently skipped wakeups that are near-impossible
          // to reproduce. Any new producer/listener must preserve it.
          if (!this.listeners) return;
          // Fire every non-exclusive listener. Among EPOLLEXCLUSIVE registrations
          // (one fd watched by several epolls) wake only one, rotating round-robin
          // per node, to avoid a thundering herd. (Only epoll registrations are ever
          // exclusive; poll waiters and a node's own consumers are not.)
          var excl;
          for (var entry of this.listeners) {
            if (entry.exclusive) (excl ||= []).push(entry);
            else entry.cb(flags);
          }
          if (excl) {
            var i = (this.exclTurn || 0) % excl.length;
            this.exclTurn = i + 1;
            excl[i].cb(flags);
          }
        }
      },
  lookupPath(path, opts = {}) {
        if (!path) {
          throw new FS.ErrnoError(44);
        }
        opts.follow_mount ??= true
  
        if (!PATH.isAbs(path)) {
          path = FS.cwd() + '/' + path;
        }
  
        // limit max consecutive symlinks to SYMLOOP_MAX.
        linkloop: for (var nlinks = 0; nlinks < 40; nlinks++) {
          // split the absolute path
          var parts = path.split('/').filter((p) => !!p);
  
          // start at the root
          var current = FS.root;
          var current_path = '/';
  
          for (var i = 0; i < parts.length; i++) {
            var islast = (i === parts.length-1);
            if (islast && opts.parent) {
              // stop resolving
              break;
            }
  
            if (parts[i] === '.') {
              continue;
            }
  
            if (parts[i] === '..') {
              current_path = PATH.dirname(current_path);
              if (FS.isRoot(current)) {
                path = current_path + '/' + parts.slice(i + 1).join('/');
                // We're making progress here, don't let many consecutive ..'s
                // lead to ELOOP
                nlinks--;
                continue linkloop;
              } else {
                current = current.parent;
              }
              continue;
            }
  
            current_path = PATH.join2(current_path, parts[i]);
            try {
              current = FS.lookupNode(current, parts[i]);
            } catch (e) {
              // if noent_okay is true, suppress a ENOENT in the last component
              // and return an object with an undefined node. This is needed for
              // resolving symlinks in the path when creating a file.
              if ((e?.errno === 44) && islast && opts.noent_okay) {
                return { path: current_path };
              }
              throw e;
            }
  
            // jump to the mount's root node if this is a mountpoint
            if (FS.isMountpoint(current) && (!islast || opts.follow_mount)) {
              current = current.mounted.root;
            }
  
            // by default, lookupPath will not follow a symlink if it is the final path component.
            // setting opts.follow = true will override this behavior.
            if (FS.isLink(current.mode) && (!islast || opts.follow)) {
              if (!current.node_ops.readlink) {
                throw new FS.ErrnoError(52);
              }
              var link = current.node_ops.readlink(current);
              if (!PATH.isAbs(link)) {
                link = PATH.dirname(current_path) + '/' + link;
              }
              path = link + '/' + parts.slice(i + 1).join('/');
              continue linkloop;
            }
          }
          return { path: current_path, node: current };
        }
        throw new FS.ErrnoError(32);
      },
  getPath(node) {
        var path;
        while (true) {
          if (FS.isRoot(node)) {
            var mount = node.mount.mountpoint;
            if (!path) return mount;
            return mount[mount.length-1] !== '/' ? `${mount}/${path}` : mount + path;
          }
          path = path ? `${node.name}/${path}` : node.name;
          node = node.parent;
        }
      },
  hashName(parentid, name) {
        var hash = 0;
  
        for (var i = 0; i < name.length; i++) {
          hash = ((hash << 5) - hash + name.charCodeAt(i)) | 0;
        }
        return ((parentid + hash) >>> 0) % FS.nameTable.length;
      },
  hashAddNode(node) {
        var hash = FS.hashName(node.parent.id, node.name);
        node.name_next = FS.nameTable[hash];
        FS.nameTable[hash] = node;
      },
  hashRemoveNode(node) {
        var hash = FS.hashName(node.parent.id, node.name);
        if (FS.nameTable[hash] === node) {
          FS.nameTable[hash] = node.name_next;
        } else {
          var current = FS.nameTable[hash];
          while (current) {
            if (current.name_next === node) {
              current.name_next = node.name_next;
              break;
            }
            current = current.name_next;
          }
        }
      },
  lookupNode(parent, name) {
        var errCode = FS.mayLookup(parent);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        var hash = FS.hashName(parent.id, name);
        for (var node = FS.nameTable[hash]; node; node = node.name_next) {
          var nodeName = node.name;
          if (node.parent.id === parent.id && nodeName === name) {
            return node;
          }
        }
        // if we failed to find it in the cache, call into the VFS
        return FS.lookup(parent, name);
      },
  createNode(parent, name, mode, rdev) {
        var node = new FS.FSNode(parent, name, mode, rdev);
  
        FS.hashAddNode(node);
  
        return node;
      },
  destroyNode(node) {
        FS.hashRemoveNode(node);
      },
  isRoot(node) {
        return node === node.parent;
      },
  isMountpoint(node) {
        return !!node.mounted;
      },
  isFile(mode) {
        return (mode & 61440) === 32768;
      },
  isDir(mode) {
        return (mode & 61440) === 16384;
      },
  isLink(mode) {
        return (mode & 61440) === 40960;
      },
  isChrdev(mode) {
        return (mode & 61440) === 8192;
      },
  isBlkdev(mode) {
        return (mode & 61440) === 24576;
      },
  isFIFO(mode) {
        return (mode & 61440) === 4096;
      },
  isSocket(mode) {
        return (mode & 49152) === 49152;
      },
  flagsToPermissionString(flag) {
        var perms = ['r', 'w', 'rw'][flag & 3];
        if ((flag & 512)) {
          perms += 'w';
        }
        return perms;
      },
  nodePermissions(node, perms) {
        if (FS.ignorePermissions) {
          return 0;
        }
        // return 0 if any user, group or owner bits are set.
        if (perms.includes('r') && !(node.mode & 292)) {
          return 2;
        }
        if (perms.includes('w') && !(node.mode & 146)) {
          return 2;
        }
        if (perms.includes('x') && !(node.mode & 73)) {
          return 2;
        }
        return 0;
      },
  mayLookup(dir) {
        if (!FS.isDir(dir.mode)) return 54;
        var errCode = FS.nodePermissions(dir, 'x');
        if (errCode) return errCode;
        if (!dir.node_ops.lookup) return 2;
        return 0;
      },
  mayCreate(dir, name) {
        if (!FS.isDir(dir.mode)) {
          return 54;
        }
        try {
          var node = FS.lookupNode(dir, name);
          return 20;
        } catch (e) {
        }
        return FS.nodePermissions(dir, 'wx');
      },
  mayDelete(dir, name, isdir) {
        var node;
        try {
          node = FS.lookupNode(dir, name);
        } catch (e) {
          return e.errno;
        }
        var errCode = FS.nodePermissions(dir, 'wx');
        if (errCode) {
          return errCode;
        }
        if (isdir) {
          if (!FS.isDir(node.mode)) {
            return 54;
          }
          if (FS.isRoot(node) || FS.getPath(node) === FS.cwd()) {
            return 10;
          }
        } else if (FS.isDir(node.mode)) {
          return 31;
        }
        return 0;
      },
  mayOpen(node, flags) {
        if (!node) {
          return 44;
        }
        if (FS.isLink(node.mode)) {
          return 32;
        }
        var mode = FS.flagsToPermissionString(flags);
        if (FS.isDir(node.mode)) {
          // opening for write
          // TODO: check for O_SEARCH? (== search for dir only)
          if (mode !== 'r' || (flags & (512 | 64))) {
            return 31;
          }
        }
        return FS.nodePermissions(node, mode);
      },
  checkOpExists(op, err) {
        if (!op) {
          throw new FS.ErrnoError(err);
        }
        return op;
      },
  MAX_OPEN_FDS:4096,
  nextfd() {
        for (var fd = 0; fd <= FS.MAX_OPEN_FDS; fd++) {
          if (!FS.streams[fd]) {
            return fd;
          }
        }
        throw new FS.ErrnoError(33);
      },
  getStreamChecked(fd) {
        var stream = FS.getStream(fd);
        if (!stream) {
          throw new FS.ErrnoError(8);
        }
        return stream;
      },
  getStream:(fd) => FS.streams[fd],
  createStream(stream, fd = -1) {
  
        // clone it, so we can return an instance of FSStream
        stream = Object.assign(new FS.FSStream(), stream);
        if (fd == -1) {
          fd = FS.nextfd();
        }
        stream.fd = fd;
        FS.streams[fd] = stream;
        return stream;
      },
  closeStream(fd) {
        FS.streams[fd] = null;
      },
  dupStream(origStream, fd = -1) {
        var stream = FS.createStream(origStream, fd);
        stream.stream_ops?.dup?.(stream);
        return stream;
      },
  doSetAttr(stream, node, attr) {
        var setattr = stream?.stream_ops.setattr;
        var arg = setattr ? stream : node;
        setattr ??= node.node_ops.setattr;
        FS.checkOpExists(setattr, 63)
        try {
          setattr(arg, attr);
        } catch (e) {
          if (e instanceof RangeError) {
            throw new FS.ErrnoError(22);
          }
          throw e;
        }
      },
  chrdev_stream_ops:{
  open(stream) {
          var device = FS.getDevice(stream.node.rdev);
          // override node's stream ops with the device's
          stream.stream_ops = device.stream_ops;
          // forward the open call
          stream.stream_ops.open?.(stream);
        },
  llseek() {
          throw new FS.ErrnoError(70);
        },
  },
  major:(dev) => ((dev) >> 8),
  minor:(dev) => ((dev) & 0xff),
  makedev:(ma, mi) => ((ma) << 8 | (mi)),
  registerDevice(dev, ops) {
        FS.devices[dev] = { stream_ops: ops };
      },
  getDevice:(dev) => FS.devices[dev],
  getMounts(mount) {
        var mounts = [];
        var check = [mount];
  
        while (check.length) {
          var m = check.pop();
  
          mounts.push(m);
  
          check.push(...m.mounts);
        }
  
        return mounts;
      },
  syncfs(populate, callback) {
        if (typeof populate == 'function') {
          callback = populate;
          populate = false;
        }
  
        FS.syncFSRequests++;
  
        if (FS.syncFSRequests > 1) {
          err(`warning: ${FS.syncFSRequests} FS.syncfs operations in flight at once, probably just doing extra work`);
        }
  
        var mounts = FS.getMounts(FS.root.mount);
        var completed = 0;
  
        function doCallback(errCode) {
          FS.syncFSRequests--;
          return callback(errCode);
        }
  
        function done(errCode) {
          if (errCode) {
            if (!done.errored) {
              done.errored = true;
              return doCallback(errCode);
            }
            return;
          }
          if (++completed >= mounts.length) {
            doCallback(null);
          }
        };
  
        // sync all mounts
        for (var mount of mounts) {
          if (mount.type.syncfs) {
            mount.type.syncfs(mount, populate, done);
          } else {
            done(null);
          }
        }
      },
  mount(type, opts, mountpoint) {
        var root = mountpoint === '/';
        var pseudo = !mountpoint;
        var node;
  
        if (root && FS.root) {
          throw new FS.ErrnoError(10);
        } else if (!root && !pseudo) {
          var lookup = FS.lookupPath(mountpoint, { follow_mount: false });
  
          mountpoint = lookup.path;  // use the absolute path
          node = lookup.node;
  
          if (FS.isMountpoint(node)) {
            throw new FS.ErrnoError(10);
          }
  
          if (!FS.isDir(node.mode)) {
            throw new FS.ErrnoError(54);
          }
        }
  
        var mount = {
          type,
          opts,
          mountpoint,
          mounts: []
        };
  
        // create a root node for the fs
        var mountRoot = type.mount(mount);
        mountRoot.mount = mount;
        mount.root = mountRoot;
  
        if (root) {
          FS.root = mountRoot;
        } else if (node) {
          // set as a mountpoint
          node.mounted = mount;
  
          // add the new mount to the current mount's children
          if (node.mount) {
            node.mount.mounts.push(mount);
          }
        }
  
        return mountRoot;
      },
  unmount(mountpoint) {
        var lookup = FS.lookupPath(mountpoint, { follow_mount: false });
  
        if (!FS.isMountpoint(lookup.node)) {
          throw new FS.ErrnoError(28);
        }
  
        // destroy the nodes for this mount, and all its child mounts
        var node = lookup.node;
        var mount = node.mounted;
        var mounts = FS.getMounts(mount);
  
        for (var [hash, current] of Object.entries(FS.nameTable)) {
          while (current) {
            var next = current.name_next;
  
            if (mounts.includes(current.mount)) {
              FS.destroyNode(current);
            }
  
            current = next;
          }
        }
  
        // no longer a mountpoint
        node.mounted = null;
  
        // remove this mount from the child mounts
        var idx = node.mount.mounts.indexOf(mount);
        node.mount.mounts.splice(idx, 1);
      },
  lookup(parent, name) {
        return parent.node_ops.lookup(parent, name);
      },
  mknod(path, mode, dev) {
        var lookup = FS.lookupPath(path, { parent: true });
        var parent = lookup.node;
        var name = PATH.basename(path);
        if (!name) {
          throw new FS.ErrnoError(28);
        }
        if (name === '.' || name === '..') {
          throw new FS.ErrnoError(20);
        }
        var errCode = FS.mayCreate(parent, name);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        if (!parent.node_ops.mknod) {
          throw new FS.ErrnoError(63);
        }
        return parent.node_ops.mknod(parent, name, mode, dev);
      },
  statfs(path) {
        return FS.statfsNode(FS.lookupPath(path, {follow: true}).node);
      },
  statfsStream(stream) {
        // We keep a separate statfsStream function because noderawfs overrides
        // it. In noderawfs, stream.node is sometimes null. Instead, we need to
        // look at stream.path.
        return FS.statfsNode(stream.node);
      },
  statfsNode(node) {
        // NOTE: None of the defaults here are true. We're just returning safe and
        //       sane values. Currently nodefs and rawfs replace these defaults,
        //       other file systems leave them alone.
        var rtn = {
          bsize: 4096,
          frsize: 4096,
          blocks: 1e6,
          bfree: 5e5,
          bavail: 5e5,
          files: FS.nextInode,
          ffree: FS.nextInode - 1,
          fsid: 42,
          flags: 2,
          namelen: 255,
        };
  
        if (node.node_ops.statfs) {
          Object.assign(rtn, node.node_ops.statfs(node.mount.opts.root));
        }
        return rtn;
      },
  create(path, mode = 0o666) {
        mode &= 4095;
        mode |= 32768;
        return FS.mknod(path, mode, 0);
      },
  mkdir(path, mode = 0o777) {
        mode &= 511 | 512;
        mode |= 16384;
        return FS.mknod(path, mode, 0);
      },
  mkdirTree(path, mode) {
        var dirs = path.split('/');
        var d = '';
        for (var dir of dirs) {
          if (!dir) continue;
          if (d || PATH.isAbs(path)) d += '/';
          d += dir;
          try {
            FS.mkdir(d, mode);
          } catch(e) {
            if (e.errno != 20) throw e;
          }
        }
      },
  mkdev(path, mode, dev) {
        if (typeof dev == 'undefined') {
          dev = mode;
          mode = 0o666;
        }
        mode |= 8192;
        return FS.mknod(path, mode, dev);
      },
  symlink(oldpath, newpath) {
        if (!PATH_FS.resolve(oldpath)) {
          throw new FS.ErrnoError(44);
        }
        var lookup = FS.lookupPath(newpath, { parent: true });
        var parent = lookup.node;
        if (!parent) {
          throw new FS.ErrnoError(44);
        }
        var newname = PATH.basename(newpath);
        var errCode = FS.mayCreate(parent, newname);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        if (!parent.node_ops.symlink) {
          throw new FS.ErrnoError(63);
        }
        return parent.node_ops.symlink(parent, newname, oldpath);
      },
  rename(old_path, new_path) {
        var old_dirname = PATH.dirname(old_path);
        var new_dirname = PATH.dirname(new_path);
        var old_name = PATH.basename(old_path);
        var new_name = PATH.basename(new_path);
        // parents must exist
        var lookup, old_dir, new_dir;
  
        // let the errors from non existent directories percolate up
        lookup = FS.lookupPath(old_path, { parent: true });
        old_dir = lookup.node;
        lookup = FS.lookupPath(new_path, { parent: true });
        new_dir = lookup.node;
  
        if (!old_dir || !new_dir) throw new FS.ErrnoError(44);
        // need to be part of the same mount
        if (old_dir.mount !== new_dir.mount) {
          throw new FS.ErrnoError(75);
        }
        // source must exist
        var old_node = FS.lookupNode(old_dir, old_name);
        // old path should not be an ancestor of the new path
        var relative = PATH_FS.relative(old_path, new_dirname);
        if (relative.charAt(0) !== '.') {
          throw new FS.ErrnoError(28);
        }
        // new path should not be an ancestor of the old path
        relative = PATH_FS.relative(new_path, old_dirname);
        if (relative.charAt(0) !== '.') {
          throw new FS.ErrnoError(55);
        }
        // see if the new path already exists
        var new_node;
        try {
          new_node = FS.lookupNode(new_dir, new_name);
        } catch (e) {
          // not fatal
        }
        // early out if nothing needs to change
        if (old_node === new_node) {
          return;
        }
        // we'll need to delete the old entry
        var isdir = FS.isDir(old_node.mode);
        var errCode = FS.mayDelete(old_dir, old_name, isdir);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        // need delete permissions if we'll be overwriting.
        // need create permissions if new doesn't already exist.
        errCode = new_node ?
          FS.mayDelete(new_dir, new_name, isdir) :
          FS.mayCreate(new_dir, new_name);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        if (!old_dir.node_ops.rename) {
          throw new FS.ErrnoError(63);
        }
        if (FS.isMountpoint(old_node) || (new_node && FS.isMountpoint(new_node))) {
          throw new FS.ErrnoError(10);
        }
        // if we are going to change the parent, check write permissions
        if (new_dir !== old_dir) {
          errCode = FS.nodePermissions(old_dir, 'w');
          if (errCode) {
            throw new FS.ErrnoError(errCode);
          }
        }
        // remove the node from the lookup hash
        FS.hashRemoveNode(old_node);
        // do the underlying fs rename
        try {
          old_dir.node_ops.rename(old_node, new_dir, new_name);
          // update old node (we do this here to avoid each backend
          // needing to)
          old_node.parent = new_dir;
        } catch (e) {
          throw e;
        } finally {
          // add the node back to the hash (in case node_ops.rename
          // changed its name)
          FS.hashAddNode(old_node);
        }
      },
  rmdir(path) {
        var lookup = FS.lookupPath(path, { parent: true });
        var parent = lookup.node;
        var name = PATH.basename(path);
        var node = FS.lookupNode(parent, name);
        var errCode = FS.mayDelete(parent, name, true);
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        if (!parent.node_ops.rmdir) {
          throw new FS.ErrnoError(63);
        }
        if (FS.isMountpoint(node)) {
          throw new FS.ErrnoError(10);
        }
        parent.node_ops.rmdir(parent, name);
        FS.destroyNode(node);
      },
  readdir(path) {
        var lookup = FS.lookupPath(path, { follow: true });
        var node = lookup.node;
        var readdir = FS.checkOpExists(node.node_ops.readdir, 54);
        return readdir(node);
      },
  unlink(path) {
        var lookup = FS.lookupPath(path, { parent: true });
        var parent = lookup.node;
        if (!parent) {
          throw new FS.ErrnoError(44);
        }
        var name = PATH.basename(path);
        var node = FS.lookupNode(parent, name);
        var errCode = FS.mayDelete(parent, name, false);
        if (errCode) {
          // According to POSIX, we should map EISDIR to EPERM, but
          // we instead do what Linux does (and we must, as we use
          // the musl linux libc).
          throw new FS.ErrnoError(errCode);
        }
        if (!parent.node_ops.unlink) {
          throw new FS.ErrnoError(63);
        }
        if (FS.isMountpoint(node)) {
          throw new FS.ErrnoError(10);
        }
        parent.node_ops.unlink(parent, name);
        FS.destroyNode(node);
      },
  readlink(path) {
        var lookup = FS.lookupPath(path);
        var link = lookup.node;
        if (!link) {
          throw new FS.ErrnoError(44);
        }
        if (!link.node_ops.readlink) {
          throw new FS.ErrnoError(28);
        }
        return link.node_ops.readlink(link);
      },
  stat(path, dontFollow) {
        var lookup = FS.lookupPath(path, { follow: !dontFollow });
        var node = lookup.node;
        var getattr = FS.checkOpExists(node.node_ops.getattr, 63);
        return getattr(node);
      },
  fstat(fd) {
        var stream = FS.getStreamChecked(fd);
        var node = stream.node;
        var getattr = stream.stream_ops.getattr;
        var arg = getattr ? stream : node;
        getattr ??= node.node_ops.getattr;
        FS.checkOpExists(getattr, 63)
        return getattr(arg);
      },
  lstat(path) {
        return FS.stat(path, true);
      },
  doChmod(stream, node, mode, dontFollow) {
        FS.doSetAttr(stream, node, {
          mode: (mode & 4095) | (node.mode & ~4095),
          ctime: Date.now(),
          dontFollow
        });
      },
  chmod(path, mode, dontFollow) {
        var node;
        if (typeof path == 'string') {
          var lookup = FS.lookupPath(path, { follow: !dontFollow });
          node = lookup.node;
        } else {
          node = path;
        }
        FS.doChmod(null, node, mode, dontFollow);
      },
  lchmod(path, mode) {
        FS.chmod(path, mode, true);
      },
  fchmod(fd, mode) {
        var stream = FS.getStreamChecked(fd);
        FS.doChmod(stream, stream.node, mode, false);
      },
  doChown(stream, node, dontFollow) {
        FS.doSetAttr(stream, node, {
          timestamp: Date.now(),
          dontFollow
          // we ignore the uid / gid for now
        });
      },
  chown(path, uid, gid, dontFollow) {
        var node;
        if (typeof path == 'string') {
          var lookup = FS.lookupPath(path, { follow: !dontFollow });
          node = lookup.node;
        } else {
          node = path;
        }
        FS.doChown(null, node, dontFollow);
      },
  lchown(path, uid, gid) {
        FS.chown(path, uid, gid, true);
      },
  fchown(fd, uid, gid) {
        var stream = FS.getStreamChecked(fd);
        FS.doChown(stream, stream.node, false);
      },
  doTruncate(stream, node, len) {
        if (FS.isDir(node.mode)) {
          throw new FS.ErrnoError(31);
        }
        if (!FS.isFile(node.mode)) {
          throw new FS.ErrnoError(28);
        }
        var errCode = FS.nodePermissions(node, 'w');
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        FS.doSetAttr(stream, node, {
          size: len,
          timestamp: Date.now()
        });
      },
  truncate(path, len) {
        if (len < 0) {
          throw new FS.ErrnoError(28);
        }
        var node;
        if (typeof path == 'string') {
          var lookup = FS.lookupPath(path, { follow: true });
          node = lookup.node;
        } else {
          node = path;
        }
        FS.doTruncate(null, node, len);
      },
  ftruncate(fd, len) {
        var stream = FS.getStreamChecked(fd);
        if (len < 0 || (stream.flags & 2097155) === 0) {
          throw new FS.ErrnoError(28);
        }
        FS.doTruncate(stream, stream.node, len);
      },
  utime(path, atime, mtime) {
        var lookup = FS.lookupPath(path, { follow: true });
        var node = lookup.node;
        var setattr = FS.checkOpExists(node.node_ops.setattr, 63);
        setattr(node, {
          atime: atime,
          mtime: mtime
        });
      },
  open(path, flags, mode = 0o666) {
        if (path === "") {
          throw new FS.ErrnoError(44);
        }
        flags = FS_modeStringToFlags(flags);
        if ((flags & 64)) {
          mode = (mode & 4095) | 32768;
        } else {
          mode = 0;
        }
        var node;
        var isDirPath;
        if (typeof path == 'object') {
          node = path;
        } else {
          isDirPath = path.endsWith("/");
          // noent_okay makes it so that if the final component of the path
          // doesn't exist, lookupPath returns `node: undefined`. `path` will be
          // updated to point to the target of all symlinks.
          var lookup = FS.lookupPath(path, {
            follow: !(flags & 131072),
            noent_okay: true
          });
          node = lookup.node;
          path = lookup.path;
        }
        // perhaps we need to create the node
        var created = false;
        if ((flags & 64)) {
          if (node) {
            // if O_CREAT and O_EXCL are set, error out if the node already exists
            if ((flags & 128)) {
              throw new FS.ErrnoError(20);
            }
          } else if (isDirPath) {
            throw new FS.ErrnoError(31);
          } else {
            // node doesn't exist, try to create it
            // Ignore the permission bits here to ensure we can `open` this new
            // file below. We use chmod below to apply the permissions once the
            // file is open.
            node = FS.mknod(path, mode | 0o777, 0);
            created = true;
          }
        }
        if (!node) {
          throw new FS.ErrnoError(44);
        }
        // can't truncate a device
        if (FS.isChrdev(node.mode)) {
          flags &= ~512;
        }
        // if asked only for a directory, then this must be one
        if ((flags & 65536) && !FS.isDir(node.mode)) {
          throw new FS.ErrnoError(54);
        }
        // check permissions, if this is not a file we just created now (it is ok to
        // create and write to a file with read-only permissions; it is read-only
        // for later use)
        if (!created) {
          var errCode = FS.mayOpen(node, flags);
          if (errCode) {
            throw new FS.ErrnoError(errCode);
          }
        }
        // do truncation if necessary
        if ((flags & 512) && !created) {
          FS.truncate(node, 0);
        }
        // we've already handled these, don't pass down to the underlying vfs
        flags &= ~(128 | 512 | 131072);
  
        // register the stream with the filesystem
        var stream = FS.createStream({
          node,
          path: FS.getPath(node),  // we want the absolute path to the node
          flags,
          seekable: true,
          position: 0,
          stream_ops: node.stream_ops,
          // used by the file family libc calls (fopen, fwrite, ferror, etc.)
          ungotten: [],
          error: false
        });
        // call the new stream's open function
        if (stream.stream_ops.open) {
          stream.stream_ops.open(stream);
        }
        if (created) {
          FS.chmod(node, mode & 0o777);
        }
        return stream;
      },
  close(stream) {
        if (FS.isClosed(stream)) {
          throw new FS.ErrnoError(8);
        }
        if (stream.getdents) stream.getdents = null; // free readdir state
        // The fd is going away: wake anything waiting on it (poll/epoll) with
        // POLLNVAL so a blocking wait unblocks and an epoll registration is evicted
        // on its next derive. Only sockets/pipes/epoll ever carry a wait-queue, so
        // for every other stream (incl. nodeless noderawfs stdio) this is a no-op.
        stream.node?.notifyListeners(32);
        try {
          if (stream.stream_ops.close) {
            stream.stream_ops.close(stream);
          }
        } catch (e) {
          throw e;
        } finally {
          FS.closeStream(stream.fd);
        }
        stream.fd = null;
      },
  isClosed(stream) {
        return stream.fd === null;
      },
  llseek(stream, offset, whence) {
        if (FS.isClosed(stream)) {
          throw new FS.ErrnoError(8);
        }
        if (!stream.seekable || !stream.stream_ops.llseek) {
          throw new FS.ErrnoError(70);
        }
        if (whence != 0 && whence != 1 && whence != 2) {
          throw new FS.ErrnoError(28);
        }
        stream.position = stream.stream_ops.llseek(stream, offset, whence);
        stream.ungotten = [];
        return stream.position;
      },
  read(stream, buffer, offset, length, position) {
        if (length < 0 || position < 0) {
          throw new FS.ErrnoError(28);
        }
        if (FS.isClosed(stream)) {
          throw new FS.ErrnoError(8);
        }
        if ((stream.flags & 2097155) === 1) {
          throw new FS.ErrnoError(8);
        }
        if (FS.isDir(stream.node.mode)) {
          throw new FS.ErrnoError(31);
        }
        if (!stream.stream_ops.read) {
          throw new FS.ErrnoError(28);
        }
        var seeking = typeof position != 'undefined';
        if (!seeking) {
          position = stream.position;
        } else if (!stream.seekable) {
          throw new FS.ErrnoError(70);
        }
        var bytesRead = stream.stream_ops.read(stream, buffer, offset, length, position);
        if (!seeking) stream.position += bytesRead;
        return bytesRead;
      },
  write(stream, buffer, offset, length, position, canOwn) {
        if (length < 0 || position < 0) {
          throw new FS.ErrnoError(28);
        }
        if (FS.isClosed(stream)) {
          throw new FS.ErrnoError(8);
        }
        if ((stream.flags & 2097155) === 0) {
          throw new FS.ErrnoError(8);
        }
        if (FS.isDir(stream.node.mode)) {
          throw new FS.ErrnoError(31);
        }
        if (!stream.stream_ops.write) {
          throw new FS.ErrnoError(28);
        }
        if (stream.seekable && stream.flags & 1024) {
          // seek to the end before writing in append mode
          FS.llseek(stream, 0, 2);
        }
        var seeking = typeof position != 'undefined';
        if (!seeking) {
          position = stream.position;
        } else if (!stream.seekable) {
          throw new FS.ErrnoError(70);
        }
        var bytesWritten = stream.stream_ops.write(stream, buffer, offset, length, position, canOwn);
        if (!seeking) stream.position += bytesWritten;
        return bytesWritten;
      },
  mmap(stream, length, position, prot, flags) {
        // User requests writing to file (prot & PROT_WRITE != 0).
        // Checking if we have permissions to write to the file unless
        // MAP_PRIVATE flag is set. According to POSIX spec it is possible
        // to write to file opened in read-only mode with MAP_PRIVATE flag,
        // as all modifications will be visible only in the memory of
        // the current process.
        if ((prot & 2) !== 0
            && (flags & 2) === 0
            && (stream.flags & 2097155) !== 2) {
          throw new FS.ErrnoError(2);
        }
        if ((stream.flags & 2097155) === 1) {
          throw new FS.ErrnoError(2);
        }
        if (!stream.stream_ops.mmap) {
          throw new FS.ErrnoError(43);
        }
        if (!length) {
          throw new FS.ErrnoError(28);
        }
        return stream.stream_ops.mmap(stream, length, position, prot, flags);
      },
  msync(stream, buffer, offset, length, mmapFlags) {
        if (!stream.stream_ops.msync) {
          return 0;
        }
        return stream.stream_ops.msync(stream, buffer, offset, length, mmapFlags);
      },
  ioctl(stream, cmd, arg) {
        if (!stream.stream_ops.ioctl) {
          throw new FS.ErrnoError(59);
        }
        return stream.stream_ops.ioctl(stream, cmd, arg);
      },
  readFile(path, opts = {}) {
        opts.flags = opts.flags ?? 0;
        opts.encoding = opts.encoding ?? 'binary';
        if (opts.encoding !== 'utf8' && opts.encoding !== 'binary') {
          abort(`Invalid encoding type "${opts.encoding}"`);
        }
        var stream = FS.open(path, opts.flags);
        var stat = FS.stat(path);
        var length = stat.size;
        var buf = new Uint8Array(length);
        FS.read(stream, buf, 0, length, 0);
        if (opts.encoding === 'utf8') {
          buf = UTF8ArrayToString(buf);
        }
        FS.close(stream);
        return buf;
      },
  writeFile(path, data, opts = {}) {
        opts.flags = opts.flags ?? 577;
        var stream = FS.open(path, opts.flags, opts.mode);
        data = FS_fileDataToTypedArray(data);
        FS.write(stream, data, 0, data.byteLength, undefined, opts.canOwn);
        FS.close(stream);
      },
  cwd:() => FS.currentPath,
  chdir(path) {
        var lookup = FS.lookupPath(path, { follow: true });
        if (lookup.node === null) {
          throw new FS.ErrnoError(44);
        }
        if (!FS.isDir(lookup.node.mode)) {
          throw new FS.ErrnoError(54);
        }
        var errCode = FS.nodePermissions(lookup.node, 'x');
        if (errCode) {
          throw new FS.ErrnoError(errCode);
        }
        FS.currentPath = lookup.path;
      },
  createDefaultDirectories() {
        FS.mkdir('/tmp');
        FS.mkdir('/home');
        FS.mkdir('/home/web_user');
      },
  createDefaultDevices() {
        // create /dev
        FS.mkdir('/dev');
        // setup /dev/null
        FS.registerDevice(FS.makedev(1, 3), {
          read: () => 0,
          write: (stream, buffer, offset, length, pos) => length,
          llseek: () => 0,
        });
        FS.mkdev('/dev/null', FS.makedev(1, 3));
        // setup /dev/tty and /dev/tty1
        // stderr needs to print output using err() rather than out()
        // so we register a second tty just for it.
        TTY.register(FS.makedev(5, 0), TTY.default_tty_ops);
        TTY.register(FS.makedev(6, 0), TTY.default_tty1_ops);
        FS.mkdev('/dev/tty', FS.makedev(5, 0));
        FS.mkdev('/dev/tty1', FS.makedev(6, 0));
        // setup /dev/[u]random
        // use a buffer to avoid overhead of individual crypto calls per byte
        var randomBuffer = new Uint8Array(1024), randomLeft = 0;
        var randomByte = () => {
          if (randomLeft === 0) {
            randomFill(randomBuffer);
            randomLeft = randomBuffer.byteLength;
          }
          return randomBuffer[--randomLeft];
        };
        FS.createDevice('/dev', 'random', randomByte);
        FS.createDevice('/dev', 'urandom', randomByte);
        // we're not going to emulate the actual shm device,
        // just create the tmp dirs that reside in it commonly
        FS.mkdir('/dev/shm');
        FS.mkdir('/dev/shm/tmp');
      },
  createSpecialDirectories() {
        // create /proc/self/fd which allows /proc/self/fd/6 => readlink gives the
        // name of the stream for fd 6 (see test_unistd_ttyname)
        FS.mkdir('/proc');
        var proc_self = FS.mkdir('/proc/self');
        FS.mkdir('/proc/self/fd');
        FS.mount({
          mount() {
            var node = FS.createNode(proc_self, 'fd', 16895, 73);
            node.stream_ops = {
              llseek: MEMFS.stream_ops.llseek,
            };
            node.node_ops = {
              lookup(parent, name) {
                var fd = +name;
                var stream = FS.getStreamChecked(fd);
                var ret = {
                  parent: null,
                  mount: { mountpoint: 'fake' },
                  node_ops: { readlink: () => stream.path },
                  id: fd + 1,
                };
                ret.parent = ret; // make it look like a simple root node
                return ret;
              },
              readdir() {
                return Array.from(FS.streams.entries())
                  .filter(([k, v]) => v)
                  .map(([k, v]) => k.toString());
              }
            };
            return node;
          }
        }, {}, '/proc/self/fd');
      },
  createStandardStreams(input, output, error) {
        // TODO deprecate the old functionality of a single
        // input / output callback and that utilizes FS.createDevice
        // and instead require a unique set of stream ops
  
        // by default, we symlink the standard streams to the
        // default tty devices. however, if the standard streams
        // have been overwritten we create a unique device for
        // them instead.
        if (input) {
          FS.createDevice('/dev', 'stdin', input);
        } else {
          FS.symlink('/dev/tty', '/dev/stdin');
        }
        if (output) {
          FS.createDevice('/dev', 'stdout', null, output);
        } else {
          FS.symlink('/dev/tty', '/dev/stdout');
        }
        if (error) {
          FS.createDevice('/dev', 'stderr', null, error);
        } else {
          FS.symlink('/dev/tty1', '/dev/stderr');
        }
  
        // open default streams for the stdin, stdout and stderr devices
        var stdin = FS.open('/dev/stdin', 0);
        var stdout = FS.open('/dev/stdout', 1);
        var stderr = FS.open('/dev/stderr', 1);
      },
  staticInit() {
        FS.nameTable = new Array(4096);
  
        FS.mount(MEMFS, {}, '/');
  
        FS.createDefaultDirectories();
        FS.createDefaultDevices();
        FS.createSpecialDirectories();
  
        FS.filesystems = {
          'MEMFS': MEMFS,
        };
      },
  init(input, output, error) {
        FS.initialized = true;
  
        // Allow Module.stdin etc. to provide defaults, if none explicitly passed to us here
        input ??= Module['stdin'];
        output ??= Module['stdout'];
        error ??= Module['stderr'];
  
        FS.createStandardStreams(input, output, error);
      },
  quit() {
        FS.initialized = false;
        // force-flush all streams, so we get musl std streams printed out
        _fflush(0);
        // close all of our streams
        for (var stream of FS.streams) {
          if (stream) {
            FS.close(stream);
          }
        }
      },
  findObject(path, dontResolveLastLink) {
        var ret = FS.analyzePath(path, dontResolveLastLink);
        if (!ret.exists) {
          return null;
        }
        return ret.object;
      },
  analyzePath(path, dontResolveLastLink) {
        // operate from within the context of the symlink's target
        try {
          var lookup = FS.lookupPath(path, { follow: !dontResolveLastLink });
          path = lookup.path;
        } catch (e) {
        }
        var ret = {
          isRoot: false, exists: false, error: 0, name: null, path: null, object: null,
          parentExists: false, parentPath: null, parentObject: null
        };
        try {
          var lookup = FS.lookupPath(path, { parent: true });
          ret.parentExists = true;
          ret.parentPath = lookup.path;
          ret.parentObject = lookup.node;
          ret.name = PATH.basename(path);
          lookup = FS.lookupPath(path, { follow: !dontResolveLastLink });
          ret.exists = true;
          ret.path = lookup.path;
          ret.object = lookup.node;
          ret.name = lookup.node.name;
          ret.isRoot = lookup.path === '/';
        } catch (e) {
          ret.error = e.errno;
        };
        return ret;
      },
  createPath(parent, path, canRead, canWrite) {
        parent = typeof parent == 'string' ? parent : FS.getPath(parent);
        var parts = path.split('/').reverse();
        while (parts.length) {
          var part = parts.pop();
          if (!part) continue;
          var current = PATH.join2(parent, part);
          try {
            FS.mkdir(current);
          } catch (e) {
            if (e.errno != 20) throw e;
          }
          parent = current;
        }
        return current;
      },
  createFile(parent, name, properties, canRead, canWrite) {
        var path = PATH.join2(typeof parent == 'string' ? parent : FS.getPath(parent), name);
        var mode = FS_getMode(canRead, canWrite);
        return FS.create(path, mode);
      },
  createDataFile(parent, name, data, canRead, canWrite, canOwn) {
        var path = name;
        if (parent) {
          parent = typeof parent == 'string' ? parent : FS.getPath(parent);
          path = name ? PATH.join2(parent, name) : parent;
        }
        var mode = FS_getMode(canRead, canWrite);
        var node = FS.create(path, mode);
        if (data) {
          data = FS_fileDataToTypedArray(data);
          // make sure we can write to the file
          FS.chmod(node, mode | 146);
          var stream = FS.open(node, 577);
          FS.write(stream, data, 0, data.length, 0, canOwn);
          FS.close(stream);
          FS.chmod(node, mode);
        }
      },
  createDevice(parent, name, input, output) {
        var path = PATH.join2(typeof parent == 'string' ? parent : FS.getPath(parent), name);
        var mode = FS_getMode(!!input, !!output);
        FS.createDevice.major ??= 64;
        var dev = FS.makedev(FS.createDevice.major++, 0);
        // Create a fake device that a set of stream ops to emulate
        // the old behavior.
        FS.registerDevice(dev, {
          open(stream) {
            stream.seekable = false;
          },
          close(stream) {
            // flush any pending line data
            if (output?.buffer?.length) {
              output(10);
            }
          },
          read(stream, buffer, offset, length, pos /* ignored */) {
            var bytesRead = 0;
            for (var i = 0; i < length; i++) {
              var result;
              try {
                result = input();
              } catch (e) {
                throw new FS.ErrnoError(29);
              }
              if (result === undefined && bytesRead === 0) {
                throw new FS.ErrnoError(6);
              }
              if (result === null || result === undefined) break;
              bytesRead++;
              buffer[offset+i] = result;
            }
            if (bytesRead) {
              stream.node.atime = Date.now();
            }
            return bytesRead;
          },
          write(stream, buffer, offset, length, pos) {
            for (var i = 0; i < length; i++) {
              try {
                output(buffer[offset+i]);
              } catch (e) {
                throw new FS.ErrnoError(29);
              }
            }
            if (length) {
              stream.node.mtime = stream.node.ctime = Date.now();
            }
            return i;
          }
        });
        return FS.mkdev(path, mode, dev);
      },
  forceLoadFile(obj) {
        if (obj.isDevice || obj.isFolder || obj.link || obj.contents) return true;
        if (globalThis.XMLHttpRequest) {
          abort("Lazy loading should have been performed (contents set) in createLazyFile, but it was not. Lazy loading only works in web workers. Use --embed-file or --preload-file in emcc on the main thread.");
        } else { // Command-line.
          try {
            obj.contents = readBinary(obj.url);
          } catch (e) {
            throw new FS.ErrnoError(29);
          }
        }
      },
  createLazyFile(parent, name, url, canRead, canWrite) {
        // Lazy chunked Uint8Array (implements get and length from Uint8Array).
        // Actual getting is abstracted away for eventual reuse.
        class LazyUint8Array {
          lengthKnown = false;
          chunks = []; // Loaded chunks. Index is the chunk number
          get(idx) {
            if (idx > this.length-1 || idx < 0) {
              return undefined;
            }
            var chunkOffset = idx % this.chunkSize;
            var chunkNum = (idx / this.chunkSize)|0;
            return this.getter(chunkNum)[chunkOffset];
          }
          setDataGetter(getter) {
            this.getter = getter;
          }
          cacheLength() {
            // Find length
            var xhr = new XMLHttpRequest();
            xhr.open('HEAD', url, false);
            xhr.send(null);
            if (!(xhr.status >= 200 && xhr.status < 300 || xhr.status === 304)) abort("Couldn't load " + url + ". Status: " + xhr.status);
            var datalength = Number(xhr.getResponseHeader("Content-length"));
            var header;
            var hasByteServing = (header = xhr.getResponseHeader("Accept-Ranges")) && header === "bytes";
            var usesGzip = (header = xhr.getResponseHeader("Content-Encoding")) && header === "gzip";
  
            var chunkSize = 1024*1024; // Chunk size in bytes
  
            if (!hasByteServing) chunkSize = datalength;
  
            // Function to get a range from the remote URL.
            var doXHR = (from, to) => {
              if (from > to) abort(`invalid range (${from}, ${to}) or no bytes requested!`);
              if (to > datalength-1) abort(`only ${datalength} bytes available! programmer error!`);
  
              // TODO: Use mozResponseArrayBuffer, responseStream, etc. if available.
              var xhr = new XMLHttpRequest();
              xhr.open('GET', url, false);
              if (datalength !== chunkSize) xhr.setRequestHeader("Range", "bytes=" + from + "-" + to);
  
              // Some hints to the browser that we want binary data.
              xhr.responseType = 'arraybuffer';
              if (xhr.overrideMimeType) {
                xhr.overrideMimeType('text/plain; charset=x-user-defined');
              }
  
              xhr.send(null);
              if (!(xhr.status >= 200 && xhr.status < 300 || xhr.status === 304)) abort("Couldn't load " + url + ". Status: " + xhr.status);
              if (xhr.response !== undefined) {
                return new Uint8Array(/** @type{Array<number>} */(xhr.response || []));
              }
              return intArrayFromString(xhr.responseText ?? '', true);
            };
            var lazyArray = this;
            lazyArray.setDataGetter((chunkNum) => {
              var start = chunkNum * chunkSize;
              var end = (chunkNum+1) * chunkSize - 1; // including this byte
              end = Math.min(end, datalength-1); // if datalength-1 is selected, this is the last block
              if (typeof lazyArray.chunks[chunkNum] == 'undefined') {
                lazyArray.chunks[chunkNum] = doXHR(start, end);
              }
              if (typeof lazyArray.chunks[chunkNum] == 'undefined') abort('doXHR failed!');
              return lazyArray.chunks[chunkNum];
            });
  
            if (usesGzip || !datalength) {
              // if the server uses gzip or doesn't supply the length, we have to download the whole file to get the (uncompressed) length
              chunkSize = datalength = 1; // this will force getter(0)/doXHR do download the whole file
              datalength = this.getter(0).length;
              chunkSize = datalength;
              out("LazyFiles on gzip forces download of the whole file when length is accessed");
            }
  
            this._length = datalength;
            this._chunkSize = chunkSize;
            this.lengthKnown = true;
          }
          get length() {
            if (!this.lengthKnown) {
              this.cacheLength();
            }
            return this._length;
          }
          get chunkSize() {
            if (!this.lengthKnown) {
              this.cacheLength();
            }
            return this._chunkSize;
          }
        }
  
        if (globalThis.XMLHttpRequest) {
          if (!ENVIRONMENT_IS_WORKER) abort('Cannot do synchronous binary XHRs outside webworkers in modern browsers. Use --embed-file or --preload-file in emcc');
          var lazyArray = new LazyUint8Array();
          var properties = { isDevice: false, contents: lazyArray };
        } else {
          var properties = { isDevice: false, url: url };
        }
  
        var node = FS.createFile(parent, name, properties, canRead, canWrite);
        // This is a total hack, but I want to get this lazy file code out of the
        // core of MEMFS. If we want to keep this lazy file concept I feel it should
        // be its own thin LAZYFS proxying calls to MEMFS.
        if (properties.contents) {
          node.contents = properties.contents;
        } else if (properties.url) {
          node.contents = null;
          node.url = properties.url;
        }
        // Add a function that defers querying the file size until it is asked the first time.
        Object.defineProperties(node, {
          usedBytes: {
            get: function() { return this.contents.length; }
          }
        });
        // override each stream op with one that tries to force load the lazy file first
        var stream_ops = {};
        for (const [key, fn] of Object.entries(node.stream_ops)) {
          stream_ops[key] = (...args) => {
            FS.forceLoadFile(node);
            return fn(...args);
          };
        }
        function writeChunks(stream, buffer, offset, length, position) {
          var contents = stream.node.contents;
          if (position >= contents.length)
            return 0;
          var size = Math.min(contents.length - position, length);
          if (contents.slice) { // normal array
            for (var i = 0; i < size; i++) {
              buffer[offset + i] = contents[position + i];
            }
          } else {
            for (var i = 0; i < size; i++) { // LazyUint8Array from sync binary XHR
              buffer[offset + i] = contents.get(position + i);
            }
          }
          return size;
        }
        // use a custom read function
        stream_ops.read = (stream, buffer, offset, length, position) => {
          FS.forceLoadFile(node);
          return writeChunks(stream, buffer, offset, length, position)
        };
        // use a custom mmap function
        stream_ops.mmap = (stream, length, position, prot, flags) => {
          FS.forceLoadFile(node);
          var ptr = mmapAlloc(length);
          if (!ptr) {
            throw new FS.ErrnoError(48);
          }
          writeChunks(stream, HEAP8, ptr, length, position);
          return { ptr, allocated: true };
        };
        node.stream_ops = stream_ops;
        return node;
      },
  };
  
  var SYSCALLS = {
  currentUmask:18,
  calculateAt(dirfd, path, allowEmpty) {
        if (PATH.isAbs(path)) {
          return path;
        }
        // relative path
        var dir;
        if (dirfd === -100) {
          dir = FS.cwd();
        } else {
          var dirstream = SYSCALLS.getStreamFromFD(dirfd);
          dir = dirstream.path;
        }
        if (path.length == 0) {
          if (!allowEmpty) {
            throw new FS.ErrnoError(44);;
          }
          return dir;
        }
        return dir + '/' + path;
      },
  writeStat(buf, stat) {
        HEAPU32[((buf)>>2)] = stat.dev;
        HEAPU32[(((buf)+(4))>>2)] = stat.mode;
        HEAPU32[(((buf)+(8))>>2)] = stat.nlink;
        HEAPU32[(((buf)+(12))>>2)] = stat.uid;
        HEAPU32[(((buf)+(16))>>2)] = stat.gid;
        HEAPU32[(((buf)+(20))>>2)] = stat.rdev;
        HEAP64[(((buf)+(24))>>3)] = BigInt(stat.size);
        HEAP32[(((buf)+(32))>>2)] = 4096;
        HEAP32[(((buf)+(36))>>2)] = stat.blocks;
        var atime = stat.atime.getTime();
        var mtime = stat.mtime.getTime();
        var ctime = stat.ctime.getTime();
        HEAP64[(((buf)+(40))>>3)] = BigInt(Math.floor(atime / 1000));
        HEAPU32[(((buf)+(48))>>2)] = (atime % 1000) * 1000 * 1000;
        HEAP64[(((buf)+(56))>>3)] = BigInt(Math.floor(mtime / 1000));
        HEAPU32[(((buf)+(64))>>2)] = (mtime % 1000) * 1000 * 1000;
        HEAP64[(((buf)+(72))>>3)] = BigInt(Math.floor(ctime / 1000));
        HEAPU32[(((buf)+(80))>>2)] = (ctime % 1000) * 1000 * 1000;
        HEAP64[(((buf)+(88))>>3)] = BigInt(stat.ino);
        return 0;
      },
  writeStatFs(buf, stats) {
        HEAPU32[(((buf)+(4))>>2)] = stats.bsize;
        HEAPU32[(((buf)+(60))>>2)] = stats.bsize;
        HEAP64[(((buf)+(8))>>3)] = BigInt(stats.blocks);
        HEAP64[(((buf)+(16))>>3)] = BigInt(stats.bfree);
        HEAP64[(((buf)+(24))>>3)] = BigInt(stats.bavail);
        HEAP64[(((buf)+(32))>>3)] = BigInt(stats.files);
        HEAP64[(((buf)+(40))>>3)] = BigInt(stats.ffree);
        HEAPU32[(((buf)+(48))>>2)] = stats.fsid;
        HEAPU32[(((buf)+(64))>>2)] = stats.flags;  // ST_NOSUID
        HEAPU32[(((buf)+(56))>>2)] = stats.namelen;
      },
  doMsync(addr, stream, len, flags, offset) {
        if (!FS.isFile(stream.node.mode)) {
          throw new FS.ErrnoError(43);
        }
        if (flags & 2) {
          // MAP_PRIVATE calls need not to be synced back to underlying fs
          return 0;
        }
        var buffer = HEAPU8.subarray(addr, addr + len);
        FS.msync(stream, buffer, offset, len, flags);
      },
  getStreamFromFD(fd) {
        var stream = FS.getStreamChecked(fd);
        return stream;
      },
  varargs:undefined,
  getStr(ptr) {
        var ret = UTF8ToString(ptr);
        return ret;
      },
  };
  function ___syscall_chdir(path) {
  try {
  
      path = SYSCALLS.getStr(path);
      FS.chdir(path);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_chmod(path, mode) {
  try {
  
      path = SYSCALLS.getStr(path);
      FS.chmod(path, mode);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_faccessat(dirfd, path, amode, flags) {
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path);
      if (amode & ~7) {
        // need a valid mode
        return -28;
      }
      var lookup = FS.lookupPath(path, { follow: true });
      var node = lookup.node;
      if (!node) {
        return -44;
      }
      var perms = '';
      if (amode & 4) perms += 'r';
      if (amode & 2) perms += 'w';
      if (amode & 1) perms += 'x';
      if (perms /* otherwise, they've just passed F_OK */ && FS.nodePermissions(node, perms)) {
        return -2;
      }
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_fchmod(fd, mode) {
  try {
  
      FS.fchmod(fd, mode);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  var syscallGetVarargI = () => {
      // the `+` prepended here is necessary to convince the JSCompiler that varargs is indeed a number.
      var ret = HEAP32[((+SYSCALLS.varargs)>>2)];
      SYSCALLS.varargs += 4;
      return ret;
    };
  var syscallGetVarargP = syscallGetVarargI;
  
  
  function ___syscall_fcntl64(fd, cmd, varargs) {
  SYSCALLS.varargs = varargs;
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      switch (cmd) {
        case 0: {
          var arg = syscallGetVarargI();
          if (arg < 0) {
            return -28;
          }
          while (FS.streams[arg]) {
            arg++;
          }
          var newStream;
          newStream = FS.dupStream(stream, arg);
          return newStream.fd;
        }
        case 1:
        case 2:
          return 0;  // FD_CLOEXEC makes no sense for a single process.
        case 3:
          return stream.flags;
        case 4: {
          var arg = syscallGetVarargI();
          var mask = 289792;
          stream.flags = (stream.flags & ~mask) | (arg & mask);
          return 0;
        }
        case 12: {
          var arg = syscallGetVarargP();
          var offset = 0;
          // We're always unlocked.
          HEAP16[(((arg)+(offset))>>1)] = 2;
          return 0;
        }
        case 13:
        case 14:
          // Pretend that the locking is successful. These are process-level locks,
          // and Emscripten programs are a single process. If we supported linking a
          // filesystem between programs, we'd need to do more here.
          // See https://github.com/emscripten-core/emscripten/issues/23697
          return 0;
      }
      return -28;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_fstat64(fd, buf) {
  try {
  
      return SYSCALLS.writeStat(buf, FS.fstat(fd));
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  var INT53_MAX = 9007199254740992;
  
  var INT53_MIN = -9007199254740992;
  var bigintToI53Checked = (num) => (num < INT53_MIN || num > INT53_MAX) ? NaN : Number(num);
  function ___syscall_ftruncate64(fd, length) {
    length = bigintToI53Checked(length);
  
  
  try {
  
      if (isNaN(length)) return -22;
      FS.ftruncate(fd, length);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  ;
  }

  
  var stringToUTF8 = (str, outPtr, maxBytesToWrite) => {
      return stringToUTF8Array(str, HEAPU8, outPtr, maxBytesToWrite);
    };
  function ___syscall_getcwd(buf, size) {
  try {
  
      if (size === 0) return -28;
      var cwd = FS.cwd();
      var cwdLengthInBytes = lengthBytesUTF8(cwd) + 1;
      if (size < cwdLengthInBytes) return -68;
      stringToUTF8(cwd, buf, size);
      return cwdLengthInBytes;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  
  function ___syscall_getdents64(fd, dirp, count) {
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd)
      stream.getdents ||= FS.readdir(stream.path);
  
      var struct_size = 280;
      var pos = 0;
      var off = FS.llseek(stream, 0, 1);
  
      var startIdx = Math.floor(off / struct_size);
      var endIdx = Math.min(stream.getdents.length, startIdx + Math.floor(count/struct_size))
      for (var idx = startIdx; idx < endIdx; idx++) {
        var id;
        var type;
        var name = stream.getdents[idx];
        if (name === '.') {
          id = stream.node.id;
          type = 4;
        }
        else if (name === '..') {
          var lookup = FS.lookupPath(stream.path, { parent: true });
          id = lookup.node.id;
          type = 4;
        }
        else {
          var child;
          try {
            child = FS.lookupNode(stream.node, name);
          } catch (e) {
            // If the entry is not a directory, file, or symlink, nodefs
            // lookupNode will raise EINVAL. Skip these and continue.
            if (e?.errno === 28) {
              continue;
            }
            throw e;
          }
          id = child.id;
          type = FS.isChrdev(child.mode) ? 2 : // character device.
                 FS.isDir(child.mode) ? 4 :    // directory
                 FS.isLink(child.mode) ? 10 :   // symbolic link.
                 8;                            // regular file.
        }
        HEAP64[((dirp + pos)>>3)] = BigInt(id);
        HEAP64[(((dirp + pos)+(8))>>3)] = BigInt((idx + 1) * struct_size);
        HEAP16[(((dirp + pos)+(16))>>1)] = 280;
        HEAP8[(dirp + pos)+(18)] = type;
        stringToUTF8(name, dirp + pos + 19, 256);
        pos += struct_size;
      }
      FS.llseek(stream, idx * struct_size, 0);
      return pos;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  
  function ___syscall_ioctl(fd, op, varargs) {
  SYSCALLS.varargs = varargs;
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      switch (op) {
        case 21509: {
          if (!stream.tty) return -59;
          return 0;
        }
        case 21505: {
          if (!stream.tty) return -59;
          if (stream.tty.ops.ioctl_tcgets) {
            var termios = stream.tty.ops.ioctl_tcgets(stream);
            var argp = syscallGetVarargP();
            HEAP32[((argp)>>2)] = termios.c_iflag || 0;
            HEAP32[(((argp)+(4))>>2)] = termios.c_oflag || 0;
            HEAP32[(((argp)+(8))>>2)] = termios.c_cflag || 0;
            HEAP32[(((argp)+(12))>>2)] = termios.c_lflag || 0;
            for (var i = 0; i < 32; i++) {
              HEAP8[(argp + i)+(17)] = termios.c_cc[i] || 0;
            }
            return 0;
          }
          return 0;
        }
        case 21510:
        case 21511:
        case 21512: {
          if (!stream.tty) return -59;
          return 0; // no-op, not actually adjusting terminal settings
        }
        case 21506:
        case 21507:
        case 21508: {
          if (!stream.tty) return -59;
          if (stream.tty.ops.ioctl_tcsets) {
            var argp = syscallGetVarargP();
            var c_iflag = HEAP32[((argp)>>2)];
            var c_oflag = HEAP32[(((argp)+(4))>>2)];
            var c_cflag = HEAP32[(((argp)+(8))>>2)];
            var c_lflag = HEAP32[(((argp)+(12))>>2)];
            var c_cc = []
            for (var i = 0; i < 32; i++) {
              c_cc.push(HEAP8[(argp + i)+(17)]);
            }
            return stream.tty.ops.ioctl_tcsets(stream.tty, op, { c_iflag, c_oflag, c_cflag, c_lflag, c_cc });
          }
          return 0; // no-op, not actually adjusting terminal settings
        }
        case 21519: {
          if (!stream.tty) return -59;
          var argp = syscallGetVarargP();
          HEAP32[((argp)>>2)] = 0;
          return 0;
        }
        case 21520: {
          if (!stream.tty) return -59;
          return -28; // not supported
        }
        case 21537:
        case 21531: {
          var argp = syscallGetVarargP();
          return FS.ioctl(stream, op, argp);
        }
        case 21523: {
          // TODO: in theory we should write to the winsize struct that gets
          // passed in, but for now musl doesn't read anything on it
          if (!stream.tty) return -59;
          if (stream.tty.ops.ioctl_tiocgwinsz) {
            var winsize = stream.tty.ops.ioctl_tiocgwinsz(stream.tty);
            var argp = syscallGetVarargP();
            HEAP16[((argp)>>1)] = winsize[0];
            HEAP16[(((argp)+(2))>>1)] = winsize[1];
          }
          return 0;
        }
        case 21524: {
          // TODO: technically, this ioctl call should change the window size.
          // but, since emscripten doesn't have any concept of a terminal window
          // yet, we'll just silently throw it away as we do TIOCGWINSZ
          if (!stream.tty) return -59;
          return 0;
        }
        case 21515: {
          if (!stream.tty) return -59;
          return 0;
        }
        default: return -28; // not supported
      }
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_lstat64(path, buf) {
  try {
  
      path = SYSCALLS.getStr(path);
      return SYSCALLS.writeStat(buf, FS.lstat(path));
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_mkdirat(dirfd, path, mode) {
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path);
      mode &= ~SYSCALLS.currentUmask;
      FS.mkdir(path, mode, 0);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_newfstatat(dirfd, path, buf, flags) {
  try {
  
      path = SYSCALLS.getStr(path);
      var nofollow = flags & 256;
      var allowEmpty = flags & 4096;
      flags = flags & (~6400);
      path = SYSCALLS.calculateAt(dirfd, path, allowEmpty);
      return SYSCALLS.writeStat(buf, nofollow ? FS.lstat(path) : FS.stat(path));
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  
  function ___syscall_openat(dirfd, path, flags, varargs) {
  SYSCALLS.varargs = varargs;
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path);
      var mode = varargs ? syscallGetVarargI() : 0;
      if (flags & 64) {
        mode &= ~SYSCALLS.currentUmask;
      }
      return FS.open(path, flags, mode).fd;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  var pollOne = (fd, events) => {
      var stream = FS.getStream(fd);
      if (!stream) return 32;
      // Streams without a poll handler (regular files, incl. NODERAWFS/NODEFS
      // which leave stream_ops unset) are treated as always readable+writable.
      var flags = stream.stream_ops?.poll
        ? stream.stream_ops.poll(stream)
        : 5;
      return flags & (events | 8 | 16 | 32);
    };
  var doPollSync = (fds, nfds) => {
      var count = 0;
      for (var i = 0, pollfd = fds; i < nfds; i++, pollfd += 8) {
        var revents = pollOne(
          HEAP32[((pollfd)>>2)],
          HEAP16[(((pollfd)+(4))>>1)]);
        if (revents) count++;
        HEAP16[(((pollfd)+(6))>>1)] = revents;
      }
      return count;
    };
  function ___syscall_poll(fds, nfds, timeout) {
  try {
  
      var count = doPollSync(fds, nfds);
      return count;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_poll_nonblocking(fds, nfds) {
  try {
  
      return doPollSync(fds, nfds);
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  
  
  function ___syscall_readlinkat(dirfd, path, buf, bufsize) {
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path);
      if (bufsize <= 0) return -28;
      var ret = FS.readlink(path);
  
      var len = Math.min(bufsize, lengthBytesUTF8(ret));
      var endChar = HEAP8[buf+len];
      stringToUTF8(ret, buf, bufsize+1);
      // readlink is one of the rare functions that write out a C string, but does never append a null to the output buffer(!)
      // stringToUTF8() always appends a null byte, so restore the character under the null byte after the write.
      HEAP8[buf+len] = endChar;
      return len;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_renameat(olddirfd, oldpath, newdirfd, newpath) {
  try {
  
      oldpath = SYSCALLS.getStr(oldpath);
      newpath = SYSCALLS.getStr(newpath);
      oldpath = SYSCALLS.calculateAt(olddirfd, oldpath);
      newpath = SYSCALLS.calculateAt(newdirfd, newpath);
      FS.rename(oldpath, newpath);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_rmdir(path) {
  try {
  
      path = SYSCALLS.getStr(path);
      FS.rmdir(path);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_stat64(path, buf) {
  try {
  
      path = SYSCALLS.getStr(path);
      return SYSCALLS.writeStat(buf, FS.stat(path));
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  function ___syscall_unlinkat(dirfd, path, flags) {
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path);
      if (!flags) {
        FS.unlink(path);
      } else if (flags === 512) {
        FS.rmdir(path);
      } else {
        return -28;
      }
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  var readI53FromI64 = (ptr) => {
      return HEAPU32[((ptr)>>2)] + HEAP32[(((ptr)+(4))>>2)] * 4294967296;
    };
  
  function ___syscall_utimensat(dirfd, path, times, flags) {
  try {
  
      path = SYSCALLS.getStr(path);
      path = SYSCALLS.calculateAt(dirfd, path, true);
      var now = Date.now(), atime, mtime;
      if (!times) {
        atime = now;
        mtime = now;
      } else {
        var seconds = readI53FromI64(times);
        var nanoseconds = HEAP32[(((times)+(8))>>2)];
        if (nanoseconds == 1073741823) {
          atime = now;
        } else if (nanoseconds == 1073741822) {
          atime = null;
        } else {
          atime = (seconds*1000) + (nanoseconds/(1000*1000));
        }
        times += 16;
        seconds = readI53FromI64(times);
        nanoseconds = HEAP32[(((times)+(8))>>2)];
        if (nanoseconds == 1073741823) {
          mtime = now;
        } else if (nanoseconds == 1073741822) {
          mtime = null;
        } else {
          mtime = (seconds*1000) + (nanoseconds/(1000*1000));
        }
      }
      // null here means UTIME_OMIT was passed. If both were set to UTIME_OMIT then
      // we can skip the call completely.
      if ((mtime ?? atime) !== null) {
        FS.utime(path, atime, mtime);
      }
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  }
  

  var __abort_js = () =>
      abort('');

  function __gmtime_js(time, tmPtr) {
    time = bigintToI53Checked(time);
  
  
      var date = new Date(time * 1000);
      if (isNaN(date.getTime())) {
        return 1;
      }
      HEAP32[((tmPtr)>>2)] = date.getUTCSeconds();
      HEAP32[(((tmPtr)+(4))>>2)] = date.getUTCMinutes();
      HEAP32[(((tmPtr)+(8))>>2)] = date.getUTCHours();
      HEAP32[(((tmPtr)+(12))>>2)] = date.getUTCDate();
      HEAP32[(((tmPtr)+(16))>>2)] = date.getUTCMonth();
      HEAP32[(((tmPtr)+(20))>>2)] = date.getUTCFullYear()-1900;
      HEAP32[(((tmPtr)+(24))>>2)] = date.getUTCDay();
      var start = Date.UTC(date.getUTCFullYear(), 0, 1, 0, 0, 0, 0);
      var yday = ((date.getTime() - start) / (1000 * 60 * 60 * 24))|0;
      HEAP32[(((tmPtr)+(28))>>2)] = yday;
      return 0;
    ;
  }

  var isLeapYear = (year) => year%4 === 0 && (year%100 !== 0 || year%400 === 0);
  
  var MONTH_DAYS_LEAP_CUMULATIVE = [0,31,60,91,121,152,182,213,244,274,305,335];
  
  var MONTH_DAYS_REGULAR_CUMULATIVE = [0,31,59,90,120,151,181,212,243,273,304,334];
  var ydayFromDate = (date) => {
      var leap = isLeapYear(date.getFullYear());
      var monthDaysCumulative = (leap ? MONTH_DAYS_LEAP_CUMULATIVE : MONTH_DAYS_REGULAR_CUMULATIVE);
      var yday = monthDaysCumulative[date.getMonth()] + date.getDate() - 1; // -1 since it's days since Jan 1
  
      return yday;
    };
  
  function __localtime_js(time, tmPtr) {
    time = bigintToI53Checked(time);
  
  
      var date = new Date(time*1000);
      if (isNaN(date.getTime())) {
        return 1;
      }
      HEAP32[((tmPtr)>>2)] = date.getSeconds();
      HEAP32[(((tmPtr)+(4))>>2)] = date.getMinutes();
      HEAP32[(((tmPtr)+(8))>>2)] = date.getHours();
      HEAP32[(((tmPtr)+(12))>>2)] = date.getDate();
      HEAP32[(((tmPtr)+(16))>>2)] = date.getMonth();
      HEAP32[(((tmPtr)+(20))>>2)] = date.getFullYear()-1900;
      HEAP32[(((tmPtr)+(24))>>2)] = date.getDay();
  
      var yday = ydayFromDate(date)|0;
      HEAP32[(((tmPtr)+(28))>>2)] = yday;
      HEAP32[(((tmPtr)+(36))>>2)] = -(date.getTimezoneOffset() * 60);
  
      // Attention: DST is in December in South, and some regions don't have DST at all.
      var start = new Date(date.getFullYear(), 0, 1);
      var summerOffset = new Date(date.getFullYear(), 6, 1).getTimezoneOffset();
      var winterOffset = start.getTimezoneOffset();
      var dst = (summerOffset != winterOffset && date.getTimezoneOffset() == Math.min(winterOffset, summerOffset))|0;
      HEAP32[(((tmPtr)+(32))>>2)] = dst;
      return 0;
    ;
  }

  
  
  
  
  
  function __mmap_js(len, prot, flags, fd, offset, allocated, addr) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      var res = FS.mmap(stream, len, offset, prot, flags);
      var ptr = res.ptr;
      HEAP32[((allocated)>>2)] = res.allocated;
      HEAPU32[((addr)>>2)] = ptr;
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  ;
  }

  
  function __msync_js(addr, len, prot, flags, fd, offset) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      if (isNaN(offset)) return -22;
      SYSCALLS.doMsync(addr, SYSCALLS.getStreamFromFD(fd), len, flags, offset);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  ;
  }

  
  function __munmap_js(addr, len, prot, flags, fd, offset) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      if (prot & 2) {
        SYSCALLS.doMsync(addr, stream, len, flags, offset);
      }
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return -e.errno;
  }
  ;
  }

  var __tzset_js = (timezone, daylight, std_name, dst_name) => {
      // TODO: Use (malleable) environment variables instead of system settings.
      var currentYear = new Date().getFullYear();
      var winter = new Date(currentYear, 0, 1);
      var summer = new Date(currentYear, 6, 1);
      var winterOffset = winter.getTimezoneOffset();
      var summerOffset = summer.getTimezoneOffset();
  
      // Local standard timezone offset. Local standard time is not adjusted for
      // daylight savings.  This code uses the fact that getTimezoneOffset returns
      // a greater value during Standard Time versus Daylight Saving Time (DST).
      // Thus it determines the expected output during Standard Time, and it
      // compares whether the output of the given date the same (Standard) or less
      // (DST).
      var stdTimezoneOffset = Math.max(winterOffset, summerOffset);
  
      // timezone is specified as seconds west of UTC ("The external variable
      // `timezone` shall be set to the difference, in seconds, between
      // Coordinated Universal Time (UTC) and local standard time."), the same
      // as returned by stdTimezoneOffset.
      // See http://pubs.opengroup.org/onlinepubs/009695399/functions/tzset.html
      HEAPU32[((timezone)>>2)] = stdTimezoneOffset * 60;
  
      HEAP32[((daylight)>>2)] = Number(winterOffset != summerOffset);
  
      var extractZone = (timezoneOffset) => {
        // Why inverse sign?
        // Read here https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/getTimezoneOffset
        var sign = timezoneOffset >= 0 ? "-" : "+";
  
        var absOffset = Math.abs(timezoneOffset)
        var hours = String(Math.floor(absOffset / 60)).padStart(2, "0");
        var minutes = String(absOffset % 60).padStart(2, "0");
  
        return `UTC${sign}${hours}${minutes}`;
      }
  
      var winterName = extractZone(winterOffset);
      var summerName = extractZone(summerOffset);
      if (summerOffset < winterOffset) {
        // Northern hemisphere
        stringToUTF8(winterName, std_name, 17);
        stringToUTF8(summerName, dst_name, 17);
      } else {
        stringToUTF8(winterName, dst_name, 17);
        stringToUTF8(summerName, std_name, 17);
      }
    };

  var _emscripten_get_now = () => performance.now();
  
  var _emscripten_get_now_res = () => { // return resolution of get_now, in nanoseconds
      if (ENVIRONMENT_IS_NODE) {
        return 1; // nanoseconds
      }
      // Modern environment where performance.now() is supported:
      return 1000; // microseconds (1/1000 of a millisecond)
    };
  
  var nowIsMonotonic = 1;
  
  var checkWasiClock = (clock_id) => clock_id >= 0 && clock_id <= 3;
  var _clock_res_get = (clk_id, pres) => {
      if (!checkWasiClock(clk_id)) {
        return 28;
      }
      var nsec;
      // all wasi clocks but realtime are monotonic
      if (clk_id === 0) {
        nsec = 1000 * 1000; // educated guess that it's milliseconds
      } else if (nowIsMonotonic) {
        nsec = _emscripten_get_now_res();
      } else {
        return 52;
      }
      HEAP64[((pres)>>3)] = BigInt(nsec);
      return 0;
    };

  
  var _emscripten_date_now = () => Date.now();
  
  
  
  function _clock_time_get(clk_id, ignored_precision, ptime) {
    ignored_precision = bigintToI53Checked(ignored_precision);
  
  
      if (!checkWasiClock(clk_id)) {
        return 28;
      }
      var now;
      // all wasi clocks but realtime are monotonic
      if (clk_id === 0) {
        now = _emscripten_date_now();
      } else if (nowIsMonotonic) {
        now = _emscripten_get_now();
      } else {
        return 52;
      }
      // "now" is in ms, and wasi times are in ns.
      var nsec = Math.round(now * 1000 * 1000);
      HEAP64[((ptime)>>3)] = BigInt(nsec);
      return 0;
    ;
  }


  
  var runtimeKeepaliveCounter = 0;
  var keepRuntimeAlive = () => noExitRuntime || runtimeKeepaliveCounter > 0;
  var _proc_exit = (code) => {
      EXITSTATUS = code;
      if (!keepRuntimeAlive()) {
        Module['onExit']?.(code);
        ABORT = true;
      }
      quit_(code, new ExitStatus(code));
    };
  
  /** @param {boolean|number=} implicit */
  var exitJS = (status, implicit) => {
      EXITSTATUS = status;
  
      if (!keepRuntimeAlive()) {
        exitRuntime();
      }
  
      _proc_exit(status);
    };
  var _exit = exitJS;
  
  var __emscripten_runtime_keepalive_clear = () => {
      noExitRuntime = false;
      runtimeKeepaliveCounter = 0;
    };
  var _emscripten_force_exit = (status) => {
      __emscripten_runtime_keepalive_clear();
      _exit(status);
    };

  var getHeapMax = () =>
      // Stay one Wasm page short of 4GB: while e.g. Chrome is able to allocate
      // full 4GB Wasm memories, the size will wrap back to 0 bytes in Wasm side
      // for any code that deals with heap sizes, which would require special
      // casing all heap size related code to treat 0 specially.
      2147483648;
  var _emscripten_get_heap_max = () => getHeapMax();


  
  
  var growMemory = (size) => {
      var oldHeapSize = wasmMemory.buffer.byteLength;
      var pages = ((size - oldHeapSize + 65535) / 65536) | 0;
      try {
        // round size grow request up to wasm page size (fixed 64KB per spec)
        wasmMemory.grow(pages); // .grow() takes a delta compared to the previous size
        updateMemoryViews();
        return 1 /*success*/;
      } catch(e) {
      }
      // implicit 0 return to save code size (caller will cast "undefined" into 0
      // anyhow)
    };
  var _emscripten_resize_heap = (requestedSize) => {
      var oldSize = HEAPU8.length;
      // With CAN_ADDRESS_2GB or MEMORY64, pointers are already unsigned.
      requestedSize >>>= 0;
      // With multithreaded builds, races can happen (another thread might increase the size
      // in between), so return a failure, and let the caller retry.
  
      // Memory resize rules:
      // 1.  Always increase heap size to at least the requested size, rounded up
      //     to next page multiple.
      // 2a. If MEMORY_GROWTH_LINEAR_STEP == -1, excessively resize the heap
      //     geometrically: increase the heap size according to
      //     MEMORY_GROWTH_GEOMETRIC_STEP factor (default +20%), At most
      //     overreserve by MEMORY_GROWTH_GEOMETRIC_CAP bytes (default 96MB).
      // 2b. If MEMORY_GROWTH_LINEAR_STEP != -1, excessively resize the heap
      //     linearly: increase the heap size by at least
      //     MEMORY_GROWTH_LINEAR_STEP bytes.
      // 3.  Max size for the heap is capped at 2048MB-WASM_PAGE_SIZE, or by
      //     MAXIMUM_MEMORY, or by ASAN limit, depending on which is smallest
      // 4.  If we were unable to allocate as much memory, it may be due to
      //     over-eager decision to excessively reserve due to (3) above.
      //     Hence if an allocation fails, cut down on the amount of excess
      //     growth, in an attempt to succeed to perform a smaller allocation.
  
      // A limit is set for how much we can grow. We should not exceed that
      // (the wasm binary specifies it, so if we tried, we'd fail anyhow).
      var maxHeapSize = getHeapMax();
      if (requestedSize > maxHeapSize) {
        return false;
      }
  
      // Loop through potential heap size increases. If we attempt a too eager
      // reservation that fails, cut down on the attempted size and reserve a
      // smaller bump instead. (max 3 times, chosen somewhat arbitrarily)
      for (var cutDown = 1; cutDown <= 4; cutDown *= 2) {
        var overGrownHeapSize = oldSize * (1 + 0.2 / cutDown); // ensure geometric growth
        // but limit overreserving (default to capping at +96MB overgrowth at most)
        overGrownHeapSize = Math.min(overGrownHeapSize, requestedSize + 100663296 );
  
        var newSize = Math.min(maxHeapSize, alignMemory(Math.max(requestedSize, overGrownHeapSize), 65536));
  
        var replacement = growMemory(newSize);
        if (replacement) {
  
          return true;
        }
      }
      return false;
    };

  var ENV = {
  };
  
  var getExecutableName = () => thisProgram;
  var getEnvStrings = () => {
      if (!getEnvStrings.strings) {
        // Default values.
        var lang = (globalThis.navigator?.language ?? 'C').replace('-', '_') + '.UTF-8';
        var env = {
          'USER': 'web_user',
          'LOGNAME': 'web_user',
          'PATH': '/',
          'PWD': '/',
          'HOME': '/home/web_user',
          'LANG': lang,
          '_': getExecutableName()
        };
        // Apply the user-provided values, if any.
        for (var x in ENV) {
          // x is a key in ENV; if ENV[x] is undefined, that means it was
          // explicitly set to be so. We allow user code to do that to
          // force variables with default values to remain unset.
          if (ENV[x] === undefined) delete env[x];
          else env[x] = ENV[x];
        }
        var strings = [];
        for (var x in env) {
          strings.push(`${x}=${env[x]}`);
        }
        getEnvStrings.strings = strings;
      }
      return getEnvStrings.strings;
    };
  
  var _environ_get = (__environ, environ_buf) => {
      var bufSize = 0;
      var envp = 0;
      for (var string of getEnvStrings()) {
        var ptr = environ_buf + bufSize;
        HEAPU32[(((__environ)+(envp))>>2)] = ptr;
        bufSize += stringToUTF8(string, ptr, Infinity) + 1;
        envp += 4;
      }
      return 0;
    };

  
  var _environ_sizes_get = (penviron_count, penviron_buf_size) => {
      var strings = getEnvStrings();
      HEAPU32[((penviron_count)>>2)] = strings.length;
      var bufSize = 0;
      for (var string of strings) {
        bufSize += lengthBytesUTF8(string) + 1;
      }
      HEAPU32[((penviron_buf_size)>>2)] = bufSize;
      return 0;
    };


  function _fd_close(fd) {
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      FS.close(stream);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  }
  

  function _fd_fdstat_get(fd, pbuf) {
  try {
  
      var rightsBase = 0;
      var rightsInheriting = 0;
      var flags = 0;
      {
        var stream = SYSCALLS.getStreamFromFD(fd);
        // All character devices are terminals (other things a Linux system would
        // assume is a character device, like the mouse, we have special APIs for).
        var type = stream.tty ? 2 :
                   FS.isDir(stream.mode) ? 3 :
                   FS.isLink(stream.mode) ? 7 :
                   4;
      }
      HEAP8[pbuf] = type;
      HEAP16[(((pbuf)+(2))>>1)] = flags;
      HEAP64[(((pbuf)+(8))>>3)] = BigInt(rightsBase);
      HEAP64[(((pbuf)+(16))>>3)] = BigInt(rightsInheriting);
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  }
  

  /** @param {number=} offset */
  var doReadv = (stream, iov, iovcnt, offset) => {
      var ret = 0;
      for (var i = 0; i < iovcnt; i++) {
        var ptr = HEAPU32[((iov)>>2)];
        var len = HEAPU32[(((iov)+(4))>>2)];
        iov += 8;
        var curr = FS.read(stream, HEAP8, ptr, len, offset);
        if (curr < 0) return -1;
        ret += curr;
        if (curr < len) break; // nothing more to read
        if (typeof offset != 'undefined') {
          offset += curr;
        }
      }
      return ret;
    };
  
  
  function _fd_pread(fd, iov, iovcnt, offset, pnum) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      if (isNaN(offset)) return 22;
      var stream = SYSCALLS.getStreamFromFD(fd)
      var num = doReadv(stream, iov, iovcnt, offset);
      HEAPU32[((pnum)>>2)] = num;
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  ;
  }

  /** @param {number=} offset */
  var doWritev = (stream, iov, iovcnt, offset) => {
      var ret = 0;
      for (var i = 0; i < iovcnt; i++) {
        var ptr = HEAPU32[((iov)>>2)];
        var len = HEAPU32[(((iov)+(4))>>2)];
        iov += 8;
        var curr = FS.write(stream, HEAP8, ptr, len, offset);
        if (curr < 0) return -1;
        ret += curr;
        if (curr < len) {
          // No more space to write.
          break;
        }
        if (typeof offset != 'undefined') {
          offset += curr;
        }
      }
      return ret;
    };
  
  
  function _fd_pwrite(fd, iov, iovcnt, offset, pnum) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      if (isNaN(offset)) return 22;
      var stream = SYSCALLS.getStreamFromFD(fd)
      var num = doWritev(stream, iov, iovcnt, offset);
      HEAPU32[((pnum)>>2)] = num;
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  ;
  }

  
  function _fd_read(fd, iov, iovcnt, pnum) {
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      var num = doReadv(stream, iov, iovcnt);
      HEAPU32[((pnum)>>2)] = num;
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  }
  

  
  function _fd_seek(fd, offset, whence, newOffset) {
    offset = bigintToI53Checked(offset);
  
  
  try {
  
      if (isNaN(offset)) return 22;
      var stream = SYSCALLS.getStreamFromFD(fd);
      FS.llseek(stream, offset, whence);
      HEAP64[((newOffset)>>3)] = BigInt(stream.position);
      if (stream.getdents && offset === 0 && whence === 0) stream.getdents = null; // reset readdir state
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  ;
  }

  function _fd_sync(fd) {
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      var rtn = stream.stream_ops?.fsync?.(stream);
      return rtn;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  }
  

  
  function _fd_write(fd, iov, iovcnt, pnum) {
  try {
  
      var stream = SYSCALLS.getStreamFromFD(fd);
      var num = doWritev(stream, iov, iovcnt);
      HEAPU32[((pnum)>>2)] = num;
      return 0;
    } catch (e) {
    if (typeof FS == 'undefined' || !(e.name === 'ErrnoError')) throw e;
    return e.errno;
  }
  }
  

  function _mono_interp_flush_jitcall_queue(
  ) {
  return {runtime_idx:12};//mono_interp_flush_jitcall_queue
  }

  function _mono_interp_invoke_wasm_jit_call_trampoline(
  ) {
  return {runtime_idx:11};//mono_interp_invoke_wasm_jit_call_trampoline
  }

  function _mono_interp_jit_wasm_entry_trampoline(
  ) {
  return {runtime_idx:9};//mono_interp_jit_wasm_entry_trampoline
  }

  function _mono_interp_jit_wasm_jit_call_trampoline(
  ) {
  return {runtime_idx:10};//mono_interp_jit_wasm_jit_call_trampoline
  }

  function _mono_interp_record_interp_entry(
  ) {
  return {runtime_idx:8};//mono_interp_record_interp_entry
  }

  function _mono_interp_tier_prepare_jiterpreter(
  ) {
  return {runtime_idx:7};//mono_interp_tier_prepare_jiterpreter
  }

  function _mono_wasm_free_method_data(
  ) {
  return {runtime_idx:13};//mono_wasm_free_method_data
  }

  function _mono_wasm_set_entrypoint_breakpoint(
  ) {
  return {runtime_idx:17};//mono_wasm_set_entrypoint_breakpoint
  }

  function _mono_wasm_trace_logger(
  ) {
  return {runtime_idx:16};//mono_wasm_trace_logger
  }



  var getCFunc = (ident) => {
      var func = Module['_' + ident]; // closure exported function
      return func;
    };
  
  var writeArrayToMemory = (array, buffer) => {
      HEAP8.set(array, buffer);
    };
  
  
  
  var stackAlloc = (sz) => __emscripten_stack_alloc(sz);
  var stringToUTF8OnStack = (str) => {
      var size = lengthBytesUTF8(str) + 1;
      var ret = stackAlloc(size);
      stringToUTF8(str, ret, size);
      return ret;
    };
  
  var stackSave = () => _emscripten_stack_get_current();
  
  var stackRestore = (val) => __emscripten_stack_restore(val);
  
  
  
    /**
   * @param {string|null=} returnType
   * @param {Array=} argTypes
   * @param {Array=} args
   * @param {Object=} opts
   */
  var ccall = (ident, returnType, argTypes, args, opts) => {
      // For fast lookup of conversion functions
      var toC = {
        'string': (str) => {
          var ret = 0;
          if (str !== null && str !== undefined && str !== 0) { // null string
            ret = stringToUTF8OnStack(str);
          }
          return ret;
        },
        'array': (arr) => {
          var ret = stackAlloc(arr.length);
          writeArrayToMemory(arr, ret);
          return ret;
        }
      };
  
      function convertReturnValue(ret) {
        if (returnType === 'string') {
          return UTF8ToString(ret);
        }
        if (returnType === 'boolean') return Boolean(ret);
        return ret;
      }
  
      var func = getCFunc(ident);
      var cArgs = [];
      var stack = 0;
      if (args) {
        for (var i = 0; i < args.length; i++) {
          var converter = toC[argTypes[i]];
          if (converter) {
            if (stack === 0) stack = stackSave();
            cArgs[i] = converter(args[i]);
          } else {
            cArgs[i] = args[i];
          }
        }
      }
      var ret = func(...cArgs);
      function onDone(ret) {
        if (stack !== 0) stackRestore(stack);
        return convertReturnValue(ret);
      }
  
      ret = onDone(ret);
      return ret;
    };

  
  
    /**
   * @param {string=} returnType
   * @param {Array=} argTypes
   * @param {Object=} opts
   */
  var cwrap = (ident, returnType, argTypes, opts) => {
      // When the function takes numbers and returns a number, we can just return
      // the original function
      var numericArgs = !argTypes || argTypes.every((type) => type === 'number' || type === 'boolean');
      var numericRet = returnType !== 'string';
      if (numericRet && numericArgs && !opts) {
        return getCFunc(ident);
      }
      return (...args) => ccall(ident, returnType, argTypes, args, opts);
    };







  var FS_createPath = (...args) => FS.createPath(...args);




  var wasmTableMirror = [];
  
  
  var getWasmTableEntry = (funcPtr) => {
      var func = wasmTableMirror[funcPtr];
      if (!func) {
        /** @suppress {checkTypes} */
        wasmTableMirror[funcPtr] = func = wasmTable.get(funcPtr);
      }
      return func;
    };
  
  var updateTableMap = (offset, count) => {
      if (functionsInTableMap) {
        for (var i = offset; i < offset + count; i++) {
          var item = getWasmTableEntry(i);
          // Ignore null values.
          if (item) {
            functionsInTableMap.set(item, i);
          }
        }
      }
    };
  
  var functionsInTableMap;
  
  var getFunctionAddress = (func) => {
      // First, create the map if this is the first use.
      if (!functionsInTableMap) {
        functionsInTableMap = new WeakMap();
        updateTableMap(0, wasmTable.length);
      }
      return functionsInTableMap.get(func) || 0;
    };
  
  
  var freeTableIndexes = [];
  
  var getEmptyTableSlot = () => {
      // Reuse a free index if there is one, otherwise grow.
      if (freeTableIndexes.length) {
        return freeTableIndexes.pop();
      }
        // Grow the table
        return wasmTable['grow'](1);
    };
  
  
  var setWasmTableEntry = (idx, func) => {
      /** @suppress {checkTypes} */
      wasmTable.set(idx, func);
      // With ABORT_ON_WASM_EXCEPTIONS wasmTable.get is overridden to return wrapped
      // functions so we need to call it here to retrieve the potential wrapper correctly
      // instead of just storing 'func' directly into wasmTableMirror
      /** @suppress {checkTypes} */
      wasmTableMirror[idx] = wasmTable.get(idx);
    };
  
  var uleb128EncodeWithLen = (arr) => {
      const n = arr.length;
      // Note: this LEB128 length encoding produces extra byte for n < 128,
      // but we don't care as it's only used in a temporary representation.
      return [(n % 128) | 128, n >> 7, ...arr];
    };
  
  
  var wasmTypeCodes = {
      'i': 0x7f, // i32
      'p': 0x7f, // i32
      'j': 0x7e, // i64
      'f': 0x7d, // f32
      'd': 0x7c, // f64
      'e': 0x6f, // externref
    };
  var generateTypePack = (types) => uleb128EncodeWithLen(Array.from(types, (type) => {
      var code = wasmTypeCodes[type];
      return code;
    }));
  var convertJsFunctionToWasm = (func, sig) => {
      // TODO: If the type reflection proposal ever makes progress we can use
      // it here instead of creatign a new module.
      var bytes = Uint8Array.of(
        0x00, 0x61, 0x73, 0x6d, // magic ("\0asm")
        0x01, 0x00, 0x00, 0x00, // version: 1
        0x01, // Type section code
          // The module is static, with the exception of the type section, which is
          // generated based on the signature passed in.
          ...uleb128EncodeWithLen([
            0x01, // count: 1
            0x60 /* form: func */,
            // param types
            ...generateTypePack(sig.slice(1)),
            // return types (for now only supporting [] if `void` and single [T] otherwise)
            ...generateTypePack(sig[0] === 'v' ? '' : sig[0])
          ]),
        // The rest of the module is static
        0x02, 0x07, // import section
          // (import "e" "f" (func 0 (type 0)))
          0x01, 0x01, 0x65, 0x01, 0x66, 0x00, 0x00,
        0x07, 0x05, // export section
          // (export "f" (func 0 (type 0)))
          0x01, 0x01, 0x66, 0x00, 0x00,
      );
  
      // We can compile this wasm module synchronously because it is very small.
      // This accepts an import (at "e.f"), that it reroutes to an export (at "f")
      var module = new WebAssembly.Module(bytes);
      var instance = new WebAssembly.Instance(module, { 'e': { 'f': func } });
      var wrappedFunc = instance.exports['f'];
      return wrappedFunc;
    };
  /** @param {string=} sig */
  var addFunction = (func, sig) => {
      // Check if the function is already in the table, to ensure each function
      // gets a unique index.
      var rtn = getFunctionAddress(func);
      if (rtn) {
        return rtn;
      }
  
      // It's not in the table, add it now.
  
      var ret = getEmptyTableSlot();
  
      // Set the new value.
      try {
        // Attempting to call this with JS function will cause table.set() to fail
        setWasmTableEntry(ret, func);
      } catch (err) {
        if (!(err instanceof TypeError)) {
          throw err;
        }
        var wrapped = convertJsFunctionToWasm(func, sig);
        setWasmTableEntry(ret, wrapped);
      }
  
      functionsInTableMap.set(func, ret);
  
      return ret;
    };

  var handleException = (e) => {
      // Certain exception types we do not treat as errors since they are used for
      // internal control flow.
      // 1. ExitStatus, which is thrown by exit()
      // 2. "unwind", which is thrown by emscripten_unwind_to_js_event_loop() and others
      //    that wish to return to JS event loop.
      if (e instanceof ExitStatus || e == 'unwind') {
        return EXITSTATUS;
      }
      quit_(1, e);
    };
  
  
  
  var maybeExit = () => {
      if (runtimeExited) {
        return;
      }
      if (!keepRuntimeAlive()) {
        try {
          _exit(EXITSTATUS);
        } catch (e) {
          handleException(e);
        }
      }
    };
  var callUserCallback = (func) => {
      if (runtimeExited || ABORT) {
        return;
      }
      try {
        return func();
      } catch (e) {
        handleException(e);
      } finally {
        maybeExit();
      }
    };
  
  var runtimeKeepalivePush = () => {
      runtimeKeepaliveCounter += 1;
    };
  
  var runtimeKeepalivePop = () => {
      runtimeKeepaliveCounter -= 1;
    };
  /** @param {number=} timeout */
  var safeSetTimeout = (func, timeout) => {
      runtimeKeepalivePush();
      return setTimeout(() => {
        runtimeKeepalivePop();
        callUserCallback(func);
      }, timeout);
    };















  var FS_unlink = (...args) => FS.unlink(...args);

  var FS_createLazyFile = (...args) => FS.createLazyFile(...args);

  var FS_createDevice = (...args) => FS.createDevice(...args);



DOTNET.setup({ wasmEnableSIMD: true,wasmEnableEH: true,enableAotProfiler: false, enableDevToolsProfiler: false, enableLogProfiler: false, enableEventPipe: false, runAOTCompilation: true, wasmEnableThreads: false, gitHash: "297acf930fb0f5c07fd4e51d78e521b4ad2e5d1f", });;

  FS.createPreloadedFile = FS_createPreloadedFile;
  FS.preloadFile = FS_preloadFile;
  FS.staticInit();;

    // Precreate a reverse lookup table from chars
    // "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/" back to
    // bytes to make decoding fast.
    for (var base64ReverseLookup = new Uint8Array(123/*'z'+1*/), __b64i = 25; __b64i >= 0; --__b64i) {
      base64ReverseLookup[48+__b64i] = 52+__b64i; // '0-9'
      base64ReverseLookup[65+__b64i] = __b64i; // 'A-Z'
      base64ReverseLookup[97+__b64i] = 26+__b64i; // 'a-z'
    }
    base64ReverseLookup[43] = 62; // '+'
    base64ReverseLookup[47] = 63; // '/'
  ;
// End JS library code

// include: postlibrary.js
// This file is included after the automatically-generated JS library code
// but before the wasm module is created.

{

  // Begin ATMODULES hooks
  if (Module['noExitRuntime']) noExitRuntime = Module['noExitRuntime'];

if (Module['print']) out = Module['print'];
if (Module['printErr']) err = Module['printErr'];
  // End ATMODULES hooks

  if (Module['arguments']) programArgs = Module['arguments'];
  if (Module['thisProgram']) thisProgram = Module['thisProgram'];

  var preInit = Module['preInit'];
  if (preInit) {
    if (typeof preInit == 'function') Module['preInit'] = preInit = [preInit];
    // Written as a loop so that preInit functions that themselves add more
    // preInit functions.  Is this actually needed?
    while (preInit.length > 0) {
      preInit.shift()();
    }
  }
}

// Begin runtime exports
  Module['out'] = out;
  Module['err'] = err;
  Module['abort'] = abort;
  Module['wasmExports'] = wasmExports;
  Module['runtimeKeepalivePush'] = runtimeKeepalivePush;
  Module['runtimeKeepalivePop'] = runtimeKeepalivePop;
  Module['maybeExit'] = maybeExit;
  Module['addRunDependency'] = addRunDependency;
  Module['removeRunDependency'] = removeRunDependency;
  Module['ccall'] = ccall;
  Module['cwrap'] = cwrap;
  Module['addFunction'] = addFunction;
  Module['setValue'] = setValue;
  Module['getValue'] = getValue;
  Module['UTF8ArrayToString'] = UTF8ArrayToString;
  Module['UTF8ToString'] = UTF8ToString;
  Module['stringToUTF8Array'] = stringToUTF8Array;
  Module['lengthBytesUTF8'] = lengthBytesUTF8;
  Module['safeSetTimeout'] = safeSetTimeout;
  Module['FS_preloadFile'] = FS_preloadFile;
  Module['FS_unlink'] = FS_unlink;
  Module['FS_createPath'] = FS_createPath;
  Module['FS_createDevice'] = FS_createDevice;
  Module['FS'] = FS;
  Module['FS_createDataFile'] = FS_createDataFile;
  Module['FS_createLazyFile'] = FS_createLazyFile;
  // End runtime exports
  // Begin JS library exports
  Module['stackAlloc'] = stackAlloc;
  Module['stackSave'] = stackSave;
  Module['stackRestore'] = stackRestore;
  // End JS library exports

// end include: postlibrary.js


// Imports from the Wasm binary.
var _SystemJS_ExecuteBackgroundJobCallback,
  _SystemJS_ExecuteTimerCallback,
  _SystemInteropJS_RegisterGCRoot,
  _SystemInteropJS_UnregisterGCRoot,
  _mono_wasm_add_assembly,
  _mono_wasm_add_satellite_assembly,
  _malloc,
  _mono_wasm_setenv,
  _mono_wasm_getenv,
  _free,
  _mono_wasm_load_runtime,
  _mono_wasm_invoke_jsexport,
  _mono_wasm_string_from_utf16_ref,
  _mono_wasm_exec_regression,
  _mono_wasm_exit,
  _fflush,
  _mono_wasm_set_main_args,
  _mono_wasm_strdup,
  _mono_wasm_parse_runtime_options,
  _mono_wasm_intern_string_ref,
  _mono_wasm_string_get_data_ref,
  _mono_wasm_write_managed_pointer_unsafe,
  _mono_wasm_copy_managed_pointer,
  _mono_wasm_init_finalizer_thread,
  _mono_wasm_i52_to_f64,
  _mono_wasm_u52_to_f64,
  _mono_wasm_f64_to_u52,
  _mono_wasm_f64_to_i52,
  _mono_wasm_method_get_full_name,
  _mono_wasm_method_get_name,
  _mono_wasm_method_get_name_ex,
  _mono_wasm_get_f32_unaligned,
  _mono_wasm_get_f64_unaligned,
  _mono_wasm_get_i32_unaligned,
  _mono_wasm_is_zero_page_reserved,
  _mono_wasm_read_as_bool_or_null_unsafe,
  _mono_wasm_assembly_load,
  _mono_wasm_assembly_find_class,
  _mono_wasm_assembly_find_method,
  _mono_aot_AntDesign_Charts_get_method,
  _pow,
  _mono_aot_Microsoft_AspNetCore_Components_get_method,
  _mono_aot_Microsoft_AspNetCore_Components_Forms_get_method,
  _mono_aot_Microsoft_AspNetCore_Components_Web_get_method,
  _mono_aot_Microsoft_AspNetCore_Components_WebAssembly_get_method,
  _mono_aot_Microsoft_AspNetCore_Razor_Language_get_method,
  _mono_aot_Microsoft_CodeAnalysis_get_method,
  _mono_aot_Microsoft_CodeAnalysis_CSharp_get_method,
  _mono_aot_Microsoft_CodeAnalysis_Razor_get_method,
  _mono_aot_Microsoft_Extensions_Configuration_get_method,
  _mono_aot_Microsoft_Extensions_Configuration_EnvironmentVariables_get_method,
  _mono_aot_Microsoft_Extensions_Configuration_Json_get_method,
  _mono_aot_Microsoft_Extensions_DependencyInjection_get_method,
  _mono_aot_Microsoft_Extensions_Diagnostics_get_method,
  _mono_aot_Microsoft_Extensions_Localization_get_method,
  _mono_aot_Microsoft_Extensions_Localization_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_Logging_get_method,
  _mono_aot_Microsoft_Extensions_Validation_get_method,
  _mono_aot_Microsoft_JSInterop_get_method,
  _mono_aot_OneOf_get_method,
  _log10,
  _mono_aot_AntDesign_get_method,
  _mono_aot_AntDesign_Docs_get_method,
  _mono_aot_AntDesign_Extensions_Localization_get_method,
  _mono_aot_Microsoft_Extensions_Configuration_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_DependencyInjection_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_Diagnostics_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_Hosting_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_Logging_Abstractions_get_method,
  _mono_aot_Microsoft_Extensions_Options_get_method,
  _mono_aot_Microsoft_Extensions_Primitives_get_method,
  _mono_aot_System_Collections_Concurrent_get_method,
  _mono_aot_System_Collections_Immutable_get_method,
  _mono_aot_System_Collections_NonGeneric_get_method,
  _mono_aot_System_Collections_Specialized_get_method,
  _mono_aot_System_Collections_get_method,
  _mono_aot_System_ComponentModel_Annotations_get_method,
  _mono_aot_System_ComponentModel_Primitives_get_method,
  _mono_aot_System_ComponentModel_TypeConverter_get_method,
  _mono_aot_System_ComponentModel_get_method,
  _mono_aot_System_Console_get_method,
  _mono_aot_System_Data_Common_get_method,
  _log,
  _mono_aot_System_Diagnostics_DiagnosticSource_get_method,
  _mono_aot_System_Diagnostics_StackTrace_get_method,
  _mono_aot_System_Diagnostics_TraceSource_get_method,
  _mono_aot_System_Diagnostics_Tracing_get_method,
  _mono_aot_System_Drawing_Primitives_get_method,
  _mono_aot_System_Drawing_get_method,
  _mono_aot_System_Globalization_get_method,
  _mono_aot_System_IO_Compression_get_method,
  _mono_aot_System_IO_MemoryMappedFiles_get_method,
  _mono_aot_System_IO_Pipelines_get_method,
  _mono_aot_System_Linq_Expressions_get_method,
  _mono_aot_System_Linq_Queryable_get_method,
  _mono_aot_System_Linq_get_method,
  _mono_aot_System_Memory_get_method,
  _mono_aot_System_Net_Http_Json_get_method,
  _mono_aot_System_Net_Http_get_method,
  _mono_aot_System_Net_Primitives_get_method,
  _mono_aot_System_ObjectModel_get_method,
  _mono_aot_System_Private_Uri_get_method,
  _mono_aot_System_Private_Xml_Linq_get_method,
  _mono_aot_System_Private_Xml_get_method,
  _mono_aot_System_Reflection_DispatchProxy_get_method,
  _mono_aot_System_Reflection_Metadata_get_method,
  _mono_aot_System_Reflection_Primitives_get_method,
  _mono_aot_System_Runtime_InteropServices_JavaScript_get_method,
  _mono_aot_System_Runtime_InteropServices_get_method,
  _mono_aot_System_Runtime_Intrinsics_get_method,
  _mono_aot_System_Runtime_Loader_get_method,
  _mono_aot_System_Runtime_Numerics_get_method,
  _mono_aot_System_Runtime_Serialization_Formatters_get_method,
  _mono_aot_System_Runtime_Serialization_Primitives_get_method,
  _mono_aot_System_Runtime_get_method,
  _mono_aot_System_Security_Claims_get_method,
  _mono_aot_System_Security_Cryptography_get_method,
  _mono_aot_System_Text_Encoding_CodePages_get_method,
  _mono_aot_System_Text_Encoding_Extensions_get_method,
  _mono_aot_System_Text_Encodings_Web_get_method,
  _mono_aot_System_Text_Json_get_method,
  _mono_aot_System_Text_RegularExpressions_get_method,
  _mono_aot_System_Threading_Tasks_Parallel_get_method,
  _mono_aot_System_Threading_ThreadPool_get_method,
  _mono_aot_System_Threading_get_method,
  _mono_aot_System_Web_HttpUtility_get_method,
  _mono_aot_System_Xml_Linq_get_method,
  _mono_aot_System_Xml_ReaderWriter_get_method,
  _mono_aot_System_Xml_XDocument_get_method,
  _mono_aot_System_Xml_XPath_XDocument_get_method,
  _mono_aot_System_get_method,
  _mono_aot_netstandard_get_method,
  _cos,
  _sin,
  _cosf,
  _sinf,
  _logf,
  _log2,
  _exp,
  _fma,
  _log2f,
  _expf,
  _powf,
  _fmaf,
  _log10f,
  _fmodf,
  _mono_aot_corlib_get_method,
  _mono_aot_AntDesign_Docs_Wasm_get_method,
  _mono_aot_aot_instances_get_method,
  _mono_wasm_send_dbg_command_with_parms,
  _mono_wasm_send_dbg_command,
  _mono_jiterp_register_jit_call_thunk,
  _mono_jiterp_stackval_to_data,
  _mono_jiterp_stackval_from_data,
  _mono_jiterp_get_arg_offset,
  _mono_jiterp_overflow_check_i4,
  _mono_jiterp_overflow_check_u4,
  _mono_jiterp_ld_delegate_method_ptr,
  _mono_jiterp_interp_entry,
  _fmod,
  _asin,
  _asinh,
  _acos,
  _acosh,
  _atan,
  _atanh,
  _cbrt,
  _cosh,
  _sinh,
  _tan,
  _tanh,
  _atan2,
  _scalbn,
  _asinf,
  _asinhf,
  _acosf,
  _acoshf,
  _atanf,
  _atanhf,
  _cbrtf,
  _coshf,
  _sinhf,
  _tanf,
  _tanhf,
  _atan2f,
  _scalbnf,
  _mono_jiterp_get_polling_required_address,
  _mono_jiterp_prof_enter,
  _mono_jiterp_prof_samplepoint,
  _mono_jiterp_prof_leave,
  _mono_jiterp_do_safepoint,
  _mono_jiterp_imethod_to_ftnptr,
  _mono_jiterp_enum_hasflag,
  _mono_jiterp_get_simd_intrinsic,
  _mono_jiterp_get_simd_opcode,
  _mono_jiterp_get_opcode_info,
  _mono_jiterp_placeholder_trace,
  _mono_jiterp_placeholder_jit_call,
  _mono_jiterp_get_interp_entry_func,
  _mono_jiterp_is_enabled,
  _mono_jiterp_encode_leb64_ref,
  _mono_jiterp_encode_leb52,
  _mono_jiterp_encode_leb_signed_boundary,
  _mono_jiterp_increase_entry_count,
  _mono_jiterp_object_unbox,
  _mono_jiterp_type_is_byref,
  _mono_jiterp_value_copy,
  _mono_jiterp_try_newobj_inlined,
  _mono_jiterp_try_newstr,
  _mono_jiterp_try_newarr,
  _mono_jiterp_gettype_ref,
  _mono_jiterp_has_parent_fast,
  _mono_jiterp_implements_interface,
  _mono_jiterp_is_special_interface,
  _mono_jiterp_implements_special_interface,
  _mono_jiterp_cast_v2,
  _mono_jiterp_localloc,
  _mono_jiterp_ldtsflda,
  _mono_jiterp_box_ref,
  _mono_jiterp_conv,
  _mono_jiterp_relop_fp,
  _mono_jiterp_get_size_of_stackval,
  _mono_jiterp_type_get_raw_value_size,
  _mono_jiterp_trace_bailout,
  _mono_jiterp_get_trace_bailout_count,
  _mono_jiterp_adjust_abort_count,
  _mono_jiterp_interp_entry_prologue,
  _mono_jiterp_get_opcode_value_table_entry,
  _mono_jiterp_get_trace_hit_count,
  _mono_jiterp_parse_option,
  _mono_jiterp_get_options_version,
  _mono_jiterp_get_options_as_json,
  _mono_jiterp_get_option_as_int,
  _mono_jiterp_object_has_component_size,
  _mono_jiterp_get_hashcode,
  _mono_jiterp_try_get_hashcode,
  _mono_jiterp_get_signature_has_this,
  _mono_jiterp_get_signature_return_type,
  _mono_jiterp_get_signature_param_count,
  _mono_jiterp_get_signature_params,
  _mono_jiterp_type_to_ldind,
  _mono_jiterp_type_to_stind,
  _mono_jiterp_get_array_rank,
  _mono_jiterp_get_array_element_size,
  _mono_jiterp_set_object_field,
  _mono_jiterp_debug_count,
  _mono_jiterp_stelem_ref,
  _mono_jiterp_get_member_offset,
  _mono_jiterp_get_counter,
  _mono_jiterp_modify_counter,
  _mono_jiterp_write_number_unaligned,
  _mono_jiterp_get_rejected_trace_count,
  _mono_jiterp_boost_back_branch_target,
  _mono_jiterp_is_imethod_var_address_taken,
  _mono_jiterp_initialize_table,
  _mono_jiterp_allocate_table_entry,
  _mono_jiterp_tlqueue_next,
  _mono_jiterp_tlqueue_add,
  _mono_jiterp_tlqueue_clear,
  _mono_interp_pgo_load_table,
  _mono_interp_pgo_save_table,
  _mono_llvm_cpp_catch_exception,
  _mono_jiterp_begin_catch,
  _mono_jiterp_end_catch,
  _sbrk,
  _posix_memalign,
  _mono_background_exec,
  _SystemJS_ExecuteDiagnosticServerCallback,
  _mono_wasm_gc_lock,
  _mono_wasm_gc_unlock,
  _mono_print_method_from_ip,
  _mono_wasm_execute_timer,
  _wasm_load_icu_data,
  ___funcs_on_exit,
  _memset,
  _htons,
  _emscripten_builtin_memalign,
  _ntohs,
  _memalign,
  ___trap,
  __emscripten_stack_restore,
  __emscripten_stack_alloc,
  _emscripten_stack_get_current,
  memory,
  __indirect_function_table,
  ___cpp_exception,
  wasmMemory,
  wasmTable;


function assignWasmExports(wasmExports) {
  _SystemJS_ExecuteBackgroundJobCallback = Module['_SystemJS_ExecuteBackgroundJobCallback'] = wasmExports['SystemJS_ExecuteBackgroundJobCallback'];
  _SystemJS_ExecuteTimerCallback = Module['_SystemJS_ExecuteTimerCallback'] = wasmExports['SystemJS_ExecuteTimerCallback'];
  _SystemInteropJS_RegisterGCRoot = Module['_SystemInteropJS_RegisterGCRoot'] = wasmExports['SystemInteropJS_RegisterGCRoot'];
  _SystemInteropJS_UnregisterGCRoot = Module['_SystemInteropJS_UnregisterGCRoot'] = wasmExports['SystemInteropJS_UnregisterGCRoot'];
  _mono_wasm_add_assembly = Module['_mono_wasm_add_assembly'] = wasmExports['mono_wasm_add_assembly'];
  _mono_wasm_add_satellite_assembly = Module['_mono_wasm_add_satellite_assembly'] = wasmExports['mono_wasm_add_satellite_assembly'];
  _malloc = Module['_malloc'] = wasmExports['malloc'];
  _mono_wasm_setenv = Module['_mono_wasm_setenv'] = wasmExports['mono_wasm_setenv'];
  _mono_wasm_getenv = Module['_mono_wasm_getenv'] = wasmExports['mono_wasm_getenv'];
  _free = Module['_free'] = wasmExports['free'];
  _mono_wasm_load_runtime = Module['_mono_wasm_load_runtime'] = wasmExports['mono_wasm_load_runtime'];
  _mono_wasm_invoke_jsexport = Module['_mono_wasm_invoke_jsexport'] = wasmExports['mono_wasm_invoke_jsexport'];
  _mono_wasm_string_from_utf16_ref = Module['_mono_wasm_string_from_utf16_ref'] = wasmExports['mono_wasm_string_from_utf16_ref'];
  _mono_wasm_exec_regression = Module['_mono_wasm_exec_regression'] = wasmExports['mono_wasm_exec_regression'];
  _mono_wasm_exit = Module['_mono_wasm_exit'] = wasmExports['mono_wasm_exit'];
  _fflush = wasmExports['fflush'];
  _mono_wasm_set_main_args = Module['_mono_wasm_set_main_args'] = wasmExports['mono_wasm_set_main_args'];
  _mono_wasm_strdup = Module['_mono_wasm_strdup'] = wasmExports['mono_wasm_strdup'];
  _mono_wasm_parse_runtime_options = Module['_mono_wasm_parse_runtime_options'] = wasmExports['mono_wasm_parse_runtime_options'];
  _mono_wasm_intern_string_ref = Module['_mono_wasm_intern_string_ref'] = wasmExports['mono_wasm_intern_string_ref'];
  _mono_wasm_string_get_data_ref = Module['_mono_wasm_string_get_data_ref'] = wasmExports['mono_wasm_string_get_data_ref'];
  _mono_wasm_write_managed_pointer_unsafe = Module['_mono_wasm_write_managed_pointer_unsafe'] = wasmExports['mono_wasm_write_managed_pointer_unsafe'];
  _mono_wasm_copy_managed_pointer = Module['_mono_wasm_copy_managed_pointer'] = wasmExports['mono_wasm_copy_managed_pointer'];
  _mono_wasm_init_finalizer_thread = Module['_mono_wasm_init_finalizer_thread'] = wasmExports['mono_wasm_init_finalizer_thread'];
  _mono_wasm_i52_to_f64 = Module['_mono_wasm_i52_to_f64'] = wasmExports['mono_wasm_i52_to_f64'];
  _mono_wasm_u52_to_f64 = Module['_mono_wasm_u52_to_f64'] = wasmExports['mono_wasm_u52_to_f64'];
  _mono_wasm_f64_to_u52 = Module['_mono_wasm_f64_to_u52'] = wasmExports['mono_wasm_f64_to_u52'];
  _mono_wasm_f64_to_i52 = Module['_mono_wasm_f64_to_i52'] = wasmExports['mono_wasm_f64_to_i52'];
  _mono_wasm_method_get_full_name = Module['_mono_wasm_method_get_full_name'] = wasmExports['mono_wasm_method_get_full_name'];
  _mono_wasm_method_get_name = Module['_mono_wasm_method_get_name'] = wasmExports['mono_wasm_method_get_name'];
  _mono_wasm_method_get_name_ex = Module['_mono_wasm_method_get_name_ex'] = wasmExports['mono_wasm_method_get_name_ex'];
  _mono_wasm_get_f32_unaligned = Module['_mono_wasm_get_f32_unaligned'] = wasmExports['mono_wasm_get_f32_unaligned'];
  _mono_wasm_get_f64_unaligned = Module['_mono_wasm_get_f64_unaligned'] = wasmExports['mono_wasm_get_f64_unaligned'];
  _mono_wasm_get_i32_unaligned = Module['_mono_wasm_get_i32_unaligned'] = wasmExports['mono_wasm_get_i32_unaligned'];
  _mono_wasm_is_zero_page_reserved = Module['_mono_wasm_is_zero_page_reserved'] = wasmExports['mono_wasm_is_zero_page_reserved'];
  _mono_wasm_read_as_bool_or_null_unsafe = Module['_mono_wasm_read_as_bool_or_null_unsafe'] = wasmExports['mono_wasm_read_as_bool_or_null_unsafe'];
  _mono_wasm_assembly_load = Module['_mono_wasm_assembly_load'] = wasmExports['mono_wasm_assembly_load'];
  _mono_wasm_assembly_find_class = Module['_mono_wasm_assembly_find_class'] = wasmExports['mono_wasm_assembly_find_class'];
  _mono_wasm_assembly_find_method = Module['_mono_wasm_assembly_find_method'] = wasmExports['mono_wasm_assembly_find_method'];
  _mono_aot_AntDesign_Charts_get_method = Module['_mono_aot_AntDesign_Charts_get_method'] = wasmExports['mono_aot_AntDesign_Charts_get_method'];
  _pow = Module['_pow'] = wasmExports['pow'];
  _mono_aot_Microsoft_AspNetCore_Components_get_method = Module['_mono_aot_Microsoft_AspNetCore_Components_get_method'] = wasmExports['mono_aot_Microsoft_AspNetCore_Components_get_method'];
  _mono_aot_Microsoft_AspNetCore_Components_Forms_get_method = Module['_mono_aot_Microsoft_AspNetCore_Components_Forms_get_method'] = wasmExports['mono_aot_Microsoft_AspNetCore_Components_Forms_get_method'];
  _mono_aot_Microsoft_AspNetCore_Components_Web_get_method = Module['_mono_aot_Microsoft_AspNetCore_Components_Web_get_method'] = wasmExports['mono_aot_Microsoft_AspNetCore_Components_Web_get_method'];
  _mono_aot_Microsoft_AspNetCore_Components_WebAssembly_get_method = Module['_mono_aot_Microsoft_AspNetCore_Components_WebAssembly_get_method'] = wasmExports['mono_aot_Microsoft_AspNetCore_Components_WebAssembly_get_method'];
  _mono_aot_Microsoft_AspNetCore_Razor_Language_get_method = Module['_mono_aot_Microsoft_AspNetCore_Razor_Language_get_method'] = wasmExports['mono_aot_Microsoft_AspNetCore_Razor_Language_get_method'];
  _mono_aot_Microsoft_CodeAnalysis_get_method = Module['_mono_aot_Microsoft_CodeAnalysis_get_method'] = wasmExports['mono_aot_Microsoft_CodeAnalysis_get_method'];
  _mono_aot_Microsoft_CodeAnalysis_CSharp_get_method = Module['_mono_aot_Microsoft_CodeAnalysis_CSharp_get_method'] = wasmExports['mono_aot_Microsoft_CodeAnalysis_CSharp_get_method'];
  _mono_aot_Microsoft_CodeAnalysis_Razor_get_method = Module['_mono_aot_Microsoft_CodeAnalysis_Razor_get_method'] = wasmExports['mono_aot_Microsoft_CodeAnalysis_Razor_get_method'];
  _mono_aot_Microsoft_Extensions_Configuration_get_method = Module['_mono_aot_Microsoft_Extensions_Configuration_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Configuration_get_method'];
  _mono_aot_Microsoft_Extensions_Configuration_EnvironmentVariables_get_method = Module['_mono_aot_Microsoft_Extensions_Configuration_EnvironmentVariables_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Configuration_EnvironmentVariables_get_method'];
  _mono_aot_Microsoft_Extensions_Configuration_Json_get_method = Module['_mono_aot_Microsoft_Extensions_Configuration_Json_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Configuration_Json_get_method'];
  _mono_aot_Microsoft_Extensions_DependencyInjection_get_method = Module['_mono_aot_Microsoft_Extensions_DependencyInjection_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_DependencyInjection_get_method'];
  _mono_aot_Microsoft_Extensions_Diagnostics_get_method = Module['_mono_aot_Microsoft_Extensions_Diagnostics_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Diagnostics_get_method'];
  _mono_aot_Microsoft_Extensions_Localization_get_method = Module['_mono_aot_Microsoft_Extensions_Localization_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Localization_get_method'];
  _mono_aot_Microsoft_Extensions_Localization_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_Localization_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Localization_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_Logging_get_method = Module['_mono_aot_Microsoft_Extensions_Logging_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Logging_get_method'];
  _mono_aot_Microsoft_Extensions_Validation_get_method = Module['_mono_aot_Microsoft_Extensions_Validation_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Validation_get_method'];
  _mono_aot_Microsoft_JSInterop_get_method = Module['_mono_aot_Microsoft_JSInterop_get_method'] = wasmExports['mono_aot_Microsoft_JSInterop_get_method'];
  _mono_aot_OneOf_get_method = Module['_mono_aot_OneOf_get_method'] = wasmExports['mono_aot_OneOf_get_method'];
  _log10 = Module['_log10'] = wasmExports['log10'];
  _mono_aot_AntDesign_get_method = Module['_mono_aot_AntDesign_get_method'] = wasmExports['mono_aot_AntDesign_get_method'];
  _mono_aot_AntDesign_Docs_get_method = Module['_mono_aot_AntDesign_Docs_get_method'] = wasmExports['mono_aot_AntDesign_Docs_get_method'];
  _mono_aot_AntDesign_Extensions_Localization_get_method = Module['_mono_aot_AntDesign_Extensions_Localization_get_method'] = wasmExports['mono_aot_AntDesign_Extensions_Localization_get_method'];
  _mono_aot_Microsoft_Extensions_Configuration_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_Configuration_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Configuration_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_DependencyInjection_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_DependencyInjection_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_DependencyInjection_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_Diagnostics_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_Diagnostics_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Diagnostics_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_Hosting_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_Hosting_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Hosting_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_Logging_Abstractions_get_method = Module['_mono_aot_Microsoft_Extensions_Logging_Abstractions_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Logging_Abstractions_get_method'];
  _mono_aot_Microsoft_Extensions_Options_get_method = Module['_mono_aot_Microsoft_Extensions_Options_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Options_get_method'];
  _mono_aot_Microsoft_Extensions_Primitives_get_method = Module['_mono_aot_Microsoft_Extensions_Primitives_get_method'] = wasmExports['mono_aot_Microsoft_Extensions_Primitives_get_method'];
  _mono_aot_System_Collections_Concurrent_get_method = Module['_mono_aot_System_Collections_Concurrent_get_method'] = wasmExports['mono_aot_System_Collections_Concurrent_get_method'];
  _mono_aot_System_Collections_Immutable_get_method = Module['_mono_aot_System_Collections_Immutable_get_method'] = wasmExports['mono_aot_System_Collections_Immutable_get_method'];
  _mono_aot_System_Collections_NonGeneric_get_method = Module['_mono_aot_System_Collections_NonGeneric_get_method'] = wasmExports['mono_aot_System_Collections_NonGeneric_get_method'];
  _mono_aot_System_Collections_Specialized_get_method = Module['_mono_aot_System_Collections_Specialized_get_method'] = wasmExports['mono_aot_System_Collections_Specialized_get_method'];
  _mono_aot_System_Collections_get_method = Module['_mono_aot_System_Collections_get_method'] = wasmExports['mono_aot_System_Collections_get_method'];
  _mono_aot_System_ComponentModel_Annotations_get_method = Module['_mono_aot_System_ComponentModel_Annotations_get_method'] = wasmExports['mono_aot_System_ComponentModel_Annotations_get_method'];
  _mono_aot_System_ComponentModel_Primitives_get_method = Module['_mono_aot_System_ComponentModel_Primitives_get_method'] = wasmExports['mono_aot_System_ComponentModel_Primitives_get_method'];
  _mono_aot_System_ComponentModel_TypeConverter_get_method = Module['_mono_aot_System_ComponentModel_TypeConverter_get_method'] = wasmExports['mono_aot_System_ComponentModel_TypeConverter_get_method'];
  _mono_aot_System_ComponentModel_get_method = Module['_mono_aot_System_ComponentModel_get_method'] = wasmExports['mono_aot_System_ComponentModel_get_method'];
  _mono_aot_System_Console_get_method = Module['_mono_aot_System_Console_get_method'] = wasmExports['mono_aot_System_Console_get_method'];
  _mono_aot_System_Data_Common_get_method = Module['_mono_aot_System_Data_Common_get_method'] = wasmExports['mono_aot_System_Data_Common_get_method'];
  _log = Module['_log'] = wasmExports['log'];
  _mono_aot_System_Diagnostics_DiagnosticSource_get_method = Module['_mono_aot_System_Diagnostics_DiagnosticSource_get_method'] = wasmExports['mono_aot_System_Diagnostics_DiagnosticSource_get_method'];
  _mono_aot_System_Diagnostics_StackTrace_get_method = Module['_mono_aot_System_Diagnostics_StackTrace_get_method'] = wasmExports['mono_aot_System_Diagnostics_StackTrace_get_method'];
  _mono_aot_System_Diagnostics_TraceSource_get_method = Module['_mono_aot_System_Diagnostics_TraceSource_get_method'] = wasmExports['mono_aot_System_Diagnostics_TraceSource_get_method'];
  _mono_aot_System_Diagnostics_Tracing_get_method = Module['_mono_aot_System_Diagnostics_Tracing_get_method'] = wasmExports['mono_aot_System_Diagnostics_Tracing_get_method'];
  _mono_aot_System_Drawing_Primitives_get_method = Module['_mono_aot_System_Drawing_Primitives_get_method'] = wasmExports['mono_aot_System_Drawing_Primitives_get_method'];
  _mono_aot_System_Drawing_get_method = Module['_mono_aot_System_Drawing_get_method'] = wasmExports['mono_aot_System_Drawing_get_method'];
  _mono_aot_System_Globalization_get_method = Module['_mono_aot_System_Globalization_get_method'] = wasmExports['mono_aot_System_Globalization_get_method'];
  _mono_aot_System_IO_Compression_get_method = Module['_mono_aot_System_IO_Compression_get_method'] = wasmExports['mono_aot_System_IO_Compression_get_method'];
  _mono_aot_System_IO_MemoryMappedFiles_get_method = Module['_mono_aot_System_IO_MemoryMappedFiles_get_method'] = wasmExports['mono_aot_System_IO_MemoryMappedFiles_get_method'];
  _mono_aot_System_IO_Pipelines_get_method = Module['_mono_aot_System_IO_Pipelines_get_method'] = wasmExports['mono_aot_System_IO_Pipelines_get_method'];
  _mono_aot_System_Linq_Expressions_get_method = Module['_mono_aot_System_Linq_Expressions_get_method'] = wasmExports['mono_aot_System_Linq_Expressions_get_method'];
  _mono_aot_System_Linq_Queryable_get_method = Module['_mono_aot_System_Linq_Queryable_get_method'] = wasmExports['mono_aot_System_Linq_Queryable_get_method'];
  _mono_aot_System_Linq_get_method = Module['_mono_aot_System_Linq_get_method'] = wasmExports['mono_aot_System_Linq_get_method'];
  _mono_aot_System_Memory_get_method = Module['_mono_aot_System_Memory_get_method'] = wasmExports['mono_aot_System_Memory_get_method'];
  _mono_aot_System_Net_Http_Json_get_method = Module['_mono_aot_System_Net_Http_Json_get_method'] = wasmExports['mono_aot_System_Net_Http_Json_get_method'];
  _mono_aot_System_Net_Http_get_method = Module['_mono_aot_System_Net_Http_get_method'] = wasmExports['mono_aot_System_Net_Http_get_method'];
  _mono_aot_System_Net_Primitives_get_method = Module['_mono_aot_System_Net_Primitives_get_method'] = wasmExports['mono_aot_System_Net_Primitives_get_method'];
  _mono_aot_System_ObjectModel_get_method = Module['_mono_aot_System_ObjectModel_get_method'] = wasmExports['mono_aot_System_ObjectModel_get_method'];
  _mono_aot_System_Private_Uri_get_method = Module['_mono_aot_System_Private_Uri_get_method'] = wasmExports['mono_aot_System_Private_Uri_get_method'];
  _mono_aot_System_Private_Xml_Linq_get_method = Module['_mono_aot_System_Private_Xml_Linq_get_method'] = wasmExports['mono_aot_System_Private_Xml_Linq_get_method'];
  _mono_aot_System_Private_Xml_get_method = Module['_mono_aot_System_Private_Xml_get_method'] = wasmExports['mono_aot_System_Private_Xml_get_method'];
  _mono_aot_System_Reflection_DispatchProxy_get_method = Module['_mono_aot_System_Reflection_DispatchProxy_get_method'] = wasmExports['mono_aot_System_Reflection_DispatchProxy_get_method'];
  _mono_aot_System_Reflection_Metadata_get_method = Module['_mono_aot_System_Reflection_Metadata_get_method'] = wasmExports['mono_aot_System_Reflection_Metadata_get_method'];
  _mono_aot_System_Reflection_Primitives_get_method = Module['_mono_aot_System_Reflection_Primitives_get_method'] = wasmExports['mono_aot_System_Reflection_Primitives_get_method'];
  _mono_aot_System_Runtime_InteropServices_JavaScript_get_method = Module['_mono_aot_System_Runtime_InteropServices_JavaScript_get_method'] = wasmExports['mono_aot_System_Runtime_InteropServices_JavaScript_get_method'];
  _mono_aot_System_Runtime_InteropServices_get_method = Module['_mono_aot_System_Runtime_InteropServices_get_method'] = wasmExports['mono_aot_System_Runtime_InteropServices_get_method'];
  _mono_aot_System_Runtime_Intrinsics_get_method = Module['_mono_aot_System_Runtime_Intrinsics_get_method'] = wasmExports['mono_aot_System_Runtime_Intrinsics_get_method'];
  _mono_aot_System_Runtime_Loader_get_method = Module['_mono_aot_System_Runtime_Loader_get_method'] = wasmExports['mono_aot_System_Runtime_Loader_get_method'];
  _mono_aot_System_Runtime_Numerics_get_method = Module['_mono_aot_System_Runtime_Numerics_get_method'] = wasmExports['mono_aot_System_Runtime_Numerics_get_method'];
  _mono_aot_System_Runtime_Serialization_Formatters_get_method = Module['_mono_aot_System_Runtime_Serialization_Formatters_get_method'] = wasmExports['mono_aot_System_Runtime_Serialization_Formatters_get_method'];
  _mono_aot_System_Runtime_Serialization_Primitives_get_method = Module['_mono_aot_System_Runtime_Serialization_Primitives_get_method'] = wasmExports['mono_aot_System_Runtime_Serialization_Primitives_get_method'];
  _mono_aot_System_Runtime_get_method = Module['_mono_aot_System_Runtime_get_method'] = wasmExports['mono_aot_System_Runtime_get_method'];
  _mono_aot_System_Security_Claims_get_method = Module['_mono_aot_System_Security_Claims_get_method'] = wasmExports['mono_aot_System_Security_Claims_get_method'];
  _mono_aot_System_Security_Cryptography_get_method = Module['_mono_aot_System_Security_Cryptography_get_method'] = wasmExports['mono_aot_System_Security_Cryptography_get_method'];
  _mono_aot_System_Text_Encoding_CodePages_get_method = Module['_mono_aot_System_Text_Encoding_CodePages_get_method'] = wasmExports['mono_aot_System_Text_Encoding_CodePages_get_method'];
  _mono_aot_System_Text_Encoding_Extensions_get_method = Module['_mono_aot_System_Text_Encoding_Extensions_get_method'] = wasmExports['mono_aot_System_Text_Encoding_Extensions_get_method'];
  _mono_aot_System_Text_Encodings_Web_get_method = Module['_mono_aot_System_Text_Encodings_Web_get_method'] = wasmExports['mono_aot_System_Text_Encodings_Web_get_method'];
  _mono_aot_System_Text_Json_get_method = Module['_mono_aot_System_Text_Json_get_method'] = wasmExports['mono_aot_System_Text_Json_get_method'];
  _mono_aot_System_Text_RegularExpressions_get_method = Module['_mono_aot_System_Text_RegularExpressions_get_method'] = wasmExports['mono_aot_System_Text_RegularExpressions_get_method'];
  _mono_aot_System_Threading_Tasks_Parallel_get_method = Module['_mono_aot_System_Threading_Tasks_Parallel_get_method'] = wasmExports['mono_aot_System_Threading_Tasks_Parallel_get_method'];
  _mono_aot_System_Threading_ThreadPool_get_method = Module['_mono_aot_System_Threading_ThreadPool_get_method'] = wasmExports['mono_aot_System_Threading_ThreadPool_get_method'];
  _mono_aot_System_Threading_get_method = Module['_mono_aot_System_Threading_get_method'] = wasmExports['mono_aot_System_Threading_get_method'];
  _mono_aot_System_Web_HttpUtility_get_method = Module['_mono_aot_System_Web_HttpUtility_get_method'] = wasmExports['mono_aot_System_Web_HttpUtility_get_method'];
  _mono_aot_System_Xml_Linq_get_method = Module['_mono_aot_System_Xml_Linq_get_method'] = wasmExports['mono_aot_System_Xml_Linq_get_method'];
  _mono_aot_System_Xml_ReaderWriter_get_method = Module['_mono_aot_System_Xml_ReaderWriter_get_method'] = wasmExports['mono_aot_System_Xml_ReaderWriter_get_method'];
  _mono_aot_System_Xml_XDocument_get_method = Module['_mono_aot_System_Xml_XDocument_get_method'] = wasmExports['mono_aot_System_Xml_XDocument_get_method'];
  _mono_aot_System_Xml_XPath_XDocument_get_method = Module['_mono_aot_System_Xml_XPath_XDocument_get_method'] = wasmExports['mono_aot_System_Xml_XPath_XDocument_get_method'];
  _mono_aot_System_get_method = Module['_mono_aot_System_get_method'] = wasmExports['mono_aot_System_get_method'];
  _mono_aot_netstandard_get_method = Module['_mono_aot_netstandard_get_method'] = wasmExports['mono_aot_netstandard_get_method'];
  _cos = Module['_cos'] = wasmExports['cos'];
  _sin = Module['_sin'] = wasmExports['sin'];
  _cosf = Module['_cosf'] = wasmExports['cosf'];
  _sinf = Module['_sinf'] = wasmExports['sinf'];
  _logf = Module['_logf'] = wasmExports['logf'];
  _log2 = Module['_log2'] = wasmExports['log2'];
  _exp = Module['_exp'] = wasmExports['exp'];
  _fma = Module['_fma'] = wasmExports['fma'];
  _log2f = Module['_log2f'] = wasmExports['log2f'];
  _expf = Module['_expf'] = wasmExports['expf'];
  _powf = Module['_powf'] = wasmExports['powf'];
  _fmaf = Module['_fmaf'] = wasmExports['fmaf'];
  _log10f = Module['_log10f'] = wasmExports['log10f'];
  _fmodf = Module['_fmodf'] = wasmExports['fmodf'];
  _mono_aot_corlib_get_method = Module['_mono_aot_corlib_get_method'] = wasmExports['mono_aot_corlib_get_method'];
  _mono_aot_AntDesign_Docs_Wasm_get_method = Module['_mono_aot_AntDesign_Docs_Wasm_get_method'] = wasmExports['mono_aot_AntDesign_Docs_Wasm_get_method'];
  _mono_aot_aot_instances_get_method = Module['_mono_aot_aot_instances_get_method'] = wasmExports['mono_aot_aot_instances_get_method'];
  _mono_wasm_send_dbg_command_with_parms = Module['_mono_wasm_send_dbg_command_with_parms'] = wasmExports['mono_wasm_send_dbg_command_with_parms'];
  _mono_wasm_send_dbg_command = Module['_mono_wasm_send_dbg_command'] = wasmExports['mono_wasm_send_dbg_command'];
  _mono_jiterp_register_jit_call_thunk = Module['_mono_jiterp_register_jit_call_thunk'] = wasmExports['mono_jiterp_register_jit_call_thunk'];
  _mono_jiterp_stackval_to_data = Module['_mono_jiterp_stackval_to_data'] = wasmExports['mono_jiterp_stackval_to_data'];
  _mono_jiterp_stackval_from_data = Module['_mono_jiterp_stackval_from_data'] = wasmExports['mono_jiterp_stackval_from_data'];
  _mono_jiterp_get_arg_offset = Module['_mono_jiterp_get_arg_offset'] = wasmExports['mono_jiterp_get_arg_offset'];
  _mono_jiterp_overflow_check_i4 = Module['_mono_jiterp_overflow_check_i4'] = wasmExports['mono_jiterp_overflow_check_i4'];
  _mono_jiterp_overflow_check_u4 = Module['_mono_jiterp_overflow_check_u4'] = wasmExports['mono_jiterp_overflow_check_u4'];
  _mono_jiterp_ld_delegate_method_ptr = Module['_mono_jiterp_ld_delegate_method_ptr'] = wasmExports['mono_jiterp_ld_delegate_method_ptr'];
  _mono_jiterp_interp_entry = Module['_mono_jiterp_interp_entry'] = wasmExports['mono_jiterp_interp_entry'];
  _fmod = Module['_fmod'] = wasmExports['fmod'];
  _asin = Module['_asin'] = wasmExports['asin'];
  _asinh = Module['_asinh'] = wasmExports['asinh'];
  _acos = Module['_acos'] = wasmExports['acos'];
  _acosh = Module['_acosh'] = wasmExports['acosh'];
  _atan = Module['_atan'] = wasmExports['atan'];
  _atanh = Module['_atanh'] = wasmExports['atanh'];
  _cbrt = Module['_cbrt'] = wasmExports['cbrt'];
  _cosh = Module['_cosh'] = wasmExports['cosh'];
  _sinh = Module['_sinh'] = wasmExports['sinh'];
  _tan = Module['_tan'] = wasmExports['tan'];
  _tanh = Module['_tanh'] = wasmExports['tanh'];
  _atan2 = Module['_atan2'] = wasmExports['atan2'];
  _scalbn = Module['_scalbn'] = wasmExports['scalbn'];
  _asinf = Module['_asinf'] = wasmExports['asinf'];
  _asinhf = Module['_asinhf'] = wasmExports['asinhf'];
  _acosf = Module['_acosf'] = wasmExports['acosf'];
  _acoshf = Module['_acoshf'] = wasmExports['acoshf'];
  _atanf = Module['_atanf'] = wasmExports['atanf'];
  _atanhf = Module['_atanhf'] = wasmExports['atanhf'];
  _cbrtf = Module['_cbrtf'] = wasmExports['cbrtf'];
  _coshf = Module['_coshf'] = wasmExports['coshf'];
  _sinhf = Module['_sinhf'] = wasmExports['sinhf'];
  _tanf = Module['_tanf'] = wasmExports['tanf'];
  _tanhf = Module['_tanhf'] = wasmExports['tanhf'];
  _atan2f = Module['_atan2f'] = wasmExports['atan2f'];
  _scalbnf = Module['_scalbnf'] = wasmExports['scalbnf'];
  _mono_jiterp_get_polling_required_address = Module['_mono_jiterp_get_polling_required_address'] = wasmExports['mono_jiterp_get_polling_required_address'];
  _mono_jiterp_prof_enter = Module['_mono_jiterp_prof_enter'] = wasmExports['mono_jiterp_prof_enter'];
  _mono_jiterp_prof_samplepoint = Module['_mono_jiterp_prof_samplepoint'] = wasmExports['mono_jiterp_prof_samplepoint'];
  _mono_jiterp_prof_leave = Module['_mono_jiterp_prof_leave'] = wasmExports['mono_jiterp_prof_leave'];
  _mono_jiterp_do_safepoint = Module['_mono_jiterp_do_safepoint'] = wasmExports['mono_jiterp_do_safepoint'];
  _mono_jiterp_imethod_to_ftnptr = Module['_mono_jiterp_imethod_to_ftnptr'] = wasmExports['mono_jiterp_imethod_to_ftnptr'];
  _mono_jiterp_enum_hasflag = Module['_mono_jiterp_enum_hasflag'] = wasmExports['mono_jiterp_enum_hasflag'];
  _mono_jiterp_get_simd_intrinsic = Module['_mono_jiterp_get_simd_intrinsic'] = wasmExports['mono_jiterp_get_simd_intrinsic'];
  _mono_jiterp_get_simd_opcode = Module['_mono_jiterp_get_simd_opcode'] = wasmExports['mono_jiterp_get_simd_opcode'];
  _mono_jiterp_get_opcode_info = Module['_mono_jiterp_get_opcode_info'] = wasmExports['mono_jiterp_get_opcode_info'];
  _mono_jiterp_placeholder_trace = Module['_mono_jiterp_placeholder_trace'] = wasmExports['mono_jiterp_placeholder_trace'];
  _mono_jiterp_placeholder_jit_call = Module['_mono_jiterp_placeholder_jit_call'] = wasmExports['mono_jiterp_placeholder_jit_call'];
  _mono_jiterp_get_interp_entry_func = Module['_mono_jiterp_get_interp_entry_func'] = wasmExports['mono_jiterp_get_interp_entry_func'];
  _mono_jiterp_is_enabled = Module['_mono_jiterp_is_enabled'] = wasmExports['mono_jiterp_is_enabled'];
  _mono_jiterp_encode_leb64_ref = Module['_mono_jiterp_encode_leb64_ref'] = wasmExports['mono_jiterp_encode_leb64_ref'];
  _mono_jiterp_encode_leb52 = Module['_mono_jiterp_encode_leb52'] = wasmExports['mono_jiterp_encode_leb52'];
  _mono_jiterp_encode_leb_signed_boundary = Module['_mono_jiterp_encode_leb_signed_boundary'] = wasmExports['mono_jiterp_encode_leb_signed_boundary'];
  _mono_jiterp_increase_entry_count = Module['_mono_jiterp_increase_entry_count'] = wasmExports['mono_jiterp_increase_entry_count'];
  _mono_jiterp_object_unbox = Module['_mono_jiterp_object_unbox'] = wasmExports['mono_jiterp_object_unbox'];
  _mono_jiterp_type_is_byref = Module['_mono_jiterp_type_is_byref'] = wasmExports['mono_jiterp_type_is_byref'];
  _mono_jiterp_value_copy = Module['_mono_jiterp_value_copy'] = wasmExports['mono_jiterp_value_copy'];
  _mono_jiterp_try_newobj_inlined = Module['_mono_jiterp_try_newobj_inlined'] = wasmExports['mono_jiterp_try_newobj_inlined'];
  _mono_jiterp_try_newstr = Module['_mono_jiterp_try_newstr'] = wasmExports['mono_jiterp_try_newstr'];
  _mono_jiterp_try_newarr = Module['_mono_jiterp_try_newarr'] = wasmExports['mono_jiterp_try_newarr'];
  _mono_jiterp_gettype_ref = Module['_mono_jiterp_gettype_ref'] = wasmExports['mono_jiterp_gettype_ref'];
  _mono_jiterp_has_parent_fast = Module['_mono_jiterp_has_parent_fast'] = wasmExports['mono_jiterp_has_parent_fast'];
  _mono_jiterp_implements_interface = Module['_mono_jiterp_implements_interface'] = wasmExports['mono_jiterp_implements_interface'];
  _mono_jiterp_is_special_interface = Module['_mono_jiterp_is_special_interface'] = wasmExports['mono_jiterp_is_special_interface'];
  _mono_jiterp_implements_special_interface = Module['_mono_jiterp_implements_special_interface'] = wasmExports['mono_jiterp_implements_special_interface'];
  _mono_jiterp_cast_v2 = Module['_mono_jiterp_cast_v2'] = wasmExports['mono_jiterp_cast_v2'];
  _mono_jiterp_localloc = Module['_mono_jiterp_localloc'] = wasmExports['mono_jiterp_localloc'];
  _mono_jiterp_ldtsflda = Module['_mono_jiterp_ldtsflda'] = wasmExports['mono_jiterp_ldtsflda'];
  _mono_jiterp_box_ref = Module['_mono_jiterp_box_ref'] = wasmExports['mono_jiterp_box_ref'];
  _mono_jiterp_conv = Module['_mono_jiterp_conv'] = wasmExports['mono_jiterp_conv'];
  _mono_jiterp_relop_fp = Module['_mono_jiterp_relop_fp'] = wasmExports['mono_jiterp_relop_fp'];
  _mono_jiterp_get_size_of_stackval = Module['_mono_jiterp_get_size_of_stackval'] = wasmExports['mono_jiterp_get_size_of_stackval'];
  _mono_jiterp_type_get_raw_value_size = Module['_mono_jiterp_type_get_raw_value_size'] = wasmExports['mono_jiterp_type_get_raw_value_size'];
  _mono_jiterp_trace_bailout = Module['_mono_jiterp_trace_bailout'] = wasmExports['mono_jiterp_trace_bailout'];
  _mono_jiterp_get_trace_bailout_count = Module['_mono_jiterp_get_trace_bailout_count'] = wasmExports['mono_jiterp_get_trace_bailout_count'];
  _mono_jiterp_adjust_abort_count = Module['_mono_jiterp_adjust_abort_count'] = wasmExports['mono_jiterp_adjust_abort_count'];
  _mono_jiterp_interp_entry_prologue = Module['_mono_jiterp_interp_entry_prologue'] = wasmExports['mono_jiterp_interp_entry_prologue'];
  _mono_jiterp_get_opcode_value_table_entry = Module['_mono_jiterp_get_opcode_value_table_entry'] = wasmExports['mono_jiterp_get_opcode_value_table_entry'];
  _mono_jiterp_get_trace_hit_count = Module['_mono_jiterp_get_trace_hit_count'] = wasmExports['mono_jiterp_get_trace_hit_count'];
  _mono_jiterp_parse_option = Module['_mono_jiterp_parse_option'] = wasmExports['mono_jiterp_parse_option'];
  _mono_jiterp_get_options_version = Module['_mono_jiterp_get_options_version'] = wasmExports['mono_jiterp_get_options_version'];
  _mono_jiterp_get_options_as_json = Module['_mono_jiterp_get_options_as_json'] = wasmExports['mono_jiterp_get_options_as_json'];
  _mono_jiterp_get_option_as_int = Module['_mono_jiterp_get_option_as_int'] = wasmExports['mono_jiterp_get_option_as_int'];
  _mono_jiterp_object_has_component_size = Module['_mono_jiterp_object_has_component_size'] = wasmExports['mono_jiterp_object_has_component_size'];
  _mono_jiterp_get_hashcode = Module['_mono_jiterp_get_hashcode'] = wasmExports['mono_jiterp_get_hashcode'];
  _mono_jiterp_try_get_hashcode = Module['_mono_jiterp_try_get_hashcode'] = wasmExports['mono_jiterp_try_get_hashcode'];
  _mono_jiterp_get_signature_has_this = Module['_mono_jiterp_get_signature_has_this'] = wasmExports['mono_jiterp_get_signature_has_this'];
  _mono_jiterp_get_signature_return_type = Module['_mono_jiterp_get_signature_return_type'] = wasmExports['mono_jiterp_get_signature_return_type'];
  _mono_jiterp_get_signature_param_count = Module['_mono_jiterp_get_signature_param_count'] = wasmExports['mono_jiterp_get_signature_param_count'];
  _mono_jiterp_get_signature_params = Module['_mono_jiterp_get_signature_params'] = wasmExports['mono_jiterp_get_signature_params'];
  _mono_jiterp_type_to_ldind = Module['_mono_jiterp_type_to_ldind'] = wasmExports['mono_jiterp_type_to_ldind'];
  _mono_jiterp_type_to_stind = Module['_mono_jiterp_type_to_stind'] = wasmExports['mono_jiterp_type_to_stind'];
  _mono_jiterp_get_array_rank = Module['_mono_jiterp_get_array_rank'] = wasmExports['mono_jiterp_get_array_rank'];
  _mono_jiterp_get_array_element_size = Module['_mono_jiterp_get_array_element_size'] = wasmExports['mono_jiterp_get_array_element_size'];
  _mono_jiterp_set_object_field = Module['_mono_jiterp_set_object_field'] = wasmExports['mono_jiterp_set_object_field'];
  _mono_jiterp_debug_count = Module['_mono_jiterp_debug_count'] = wasmExports['mono_jiterp_debug_count'];
  _mono_jiterp_stelem_ref = Module['_mono_jiterp_stelem_ref'] = wasmExports['mono_jiterp_stelem_ref'];
  _mono_jiterp_get_member_offset = Module['_mono_jiterp_get_member_offset'] = wasmExports['mono_jiterp_get_member_offset'];
  _mono_jiterp_get_counter = Module['_mono_jiterp_get_counter'] = wasmExports['mono_jiterp_get_counter'];
  _mono_jiterp_modify_counter = Module['_mono_jiterp_modify_counter'] = wasmExports['mono_jiterp_modify_counter'];
  _mono_jiterp_write_number_unaligned = Module['_mono_jiterp_write_number_unaligned'] = wasmExports['mono_jiterp_write_number_unaligned'];
  _mono_jiterp_get_rejected_trace_count = Module['_mono_jiterp_get_rejected_trace_count'] = wasmExports['mono_jiterp_get_rejected_trace_count'];
  _mono_jiterp_boost_back_branch_target = Module['_mono_jiterp_boost_back_branch_target'] = wasmExports['mono_jiterp_boost_back_branch_target'];
  _mono_jiterp_is_imethod_var_address_taken = Module['_mono_jiterp_is_imethod_var_address_taken'] = wasmExports['mono_jiterp_is_imethod_var_address_taken'];
  _mono_jiterp_initialize_table = Module['_mono_jiterp_initialize_table'] = wasmExports['mono_jiterp_initialize_table'];
  _mono_jiterp_allocate_table_entry = Module['_mono_jiterp_allocate_table_entry'] = wasmExports['mono_jiterp_allocate_table_entry'];
  _mono_jiterp_tlqueue_next = Module['_mono_jiterp_tlqueue_next'] = wasmExports['mono_jiterp_tlqueue_next'];
  _mono_jiterp_tlqueue_add = Module['_mono_jiterp_tlqueue_add'] = wasmExports['mono_jiterp_tlqueue_add'];
  _mono_jiterp_tlqueue_clear = Module['_mono_jiterp_tlqueue_clear'] = wasmExports['mono_jiterp_tlqueue_clear'];
  _mono_interp_pgo_load_table = Module['_mono_interp_pgo_load_table'] = wasmExports['mono_interp_pgo_load_table'];
  _mono_interp_pgo_save_table = Module['_mono_interp_pgo_save_table'] = wasmExports['mono_interp_pgo_save_table'];
  _mono_llvm_cpp_catch_exception = Module['_mono_llvm_cpp_catch_exception'] = wasmExports['mono_llvm_cpp_catch_exception'];
  _mono_jiterp_begin_catch = Module['_mono_jiterp_begin_catch'] = wasmExports['mono_jiterp_begin_catch'];
  _mono_jiterp_end_catch = Module['_mono_jiterp_end_catch'] = wasmExports['mono_jiterp_end_catch'];
  _sbrk = Module['_sbrk'] = wasmExports['sbrk'];
  _posix_memalign = Module['_posix_memalign'] = wasmExports['posix_memalign'];
  _mono_background_exec = Module['_mono_background_exec'] = wasmExports['mono_background_exec'];
  _SystemJS_ExecuteDiagnosticServerCallback = Module['_SystemJS_ExecuteDiagnosticServerCallback'] = wasmExports['SystemJS_ExecuteDiagnosticServerCallback'];
  _mono_wasm_gc_lock = Module['_mono_wasm_gc_lock'] = wasmExports['mono_wasm_gc_lock'];
  _mono_wasm_gc_unlock = Module['_mono_wasm_gc_unlock'] = wasmExports['mono_wasm_gc_unlock'];
  _mono_print_method_from_ip = Module['_mono_print_method_from_ip'] = wasmExports['mono_print_method_from_ip'];
  _mono_wasm_execute_timer = Module['_mono_wasm_execute_timer'] = wasmExports['mono_wasm_execute_timer'];
  _wasm_load_icu_data = Module['_wasm_load_icu_data'] = wasmExports['wasm_load_icu_data'];
  ___funcs_on_exit = wasmExports['__funcs_on_exit'];
  _memset = Module['_memset'] = wasmExports['memset'];
  _htons = Module['_htons'] = wasmExports['htons'];
  _emscripten_builtin_memalign = wasmExports['emscripten_builtin_memalign'];
  _ntohs = Module['_ntohs'] = wasmExports['ntohs'];
  _memalign = Module['_memalign'] = wasmExports['memalign'];
  ___trap = wasmExports['__trap'];
  __emscripten_stack_restore = wasmExports['_emscripten_stack_restore'];
  __emscripten_stack_alloc = wasmExports['_emscripten_stack_alloc'];
  _emscripten_stack_get_current = wasmExports['emscripten_stack_get_current'];
  memory = wasmMemory = wasmExports['memory'];
  __indirect_function_table = wasmTable = wasmExports['__indirect_function_table'];
  ___cpp_exception = Module['___cpp_exception'] = wasmExports['__cpp_exception'];
}

var wasmImports = {
  /** @export */
  SystemInteropJS_BindJSImportST: _SystemInteropJS_BindJSImportST,
  /** @export */
  SystemInteropJS_CancelPromise: _SystemInteropJS_CancelPromise,
  /** @export */
  SystemInteropJS_InvokeJSFunction: _SystemInteropJS_InvokeJSFunction,
  /** @export */
  SystemInteropJS_InvokeJSImportST: _SystemInteropJS_InvokeJSImportST,
  /** @export */
  SystemInteropJS_ReleaseCSOwnedObject: _SystemInteropJS_ReleaseCSOwnedObject,
  /** @export */
  SystemInteropJS_ResolveOrRejectPromise: _SystemInteropJS_ResolveOrRejectPromise,
  /** @export */
  SystemJS_ConsoleClear: _SystemJS_ConsoleClear,
  /** @export */
  SystemJS_GetCurrentProcessId: _SystemJS_GetCurrentProcessId,
  /** @export */
  SystemJS_GetLocaleInfo: _SystemJS_GetLocaleInfo,
  /** @export */
  SystemJS_RandomBytes: _SystemJS_RandomBytes,
  /** @export */
  SystemJS_ScheduleBackgroundJobImpl: _SystemJS_ScheduleBackgroundJobImpl,
  /** @export */
  SystemJS_ScheduleTimerImpl: _SystemJS_ScheduleTimerImpl,
  /** @export */
  __assert_fail: ___assert_fail,
  /** @export */
  __syscall_chdir: ___syscall_chdir,
  /** @export */
  __syscall_chmod: ___syscall_chmod,
  /** @export */
  __syscall_faccessat: ___syscall_faccessat,
  /** @export */
  __syscall_fchmod: ___syscall_fchmod,
  /** @export */
  __syscall_fcntl64: ___syscall_fcntl64,
  /** @export */
  __syscall_fstat64: ___syscall_fstat64,
  /** @export */
  __syscall_ftruncate64: ___syscall_ftruncate64,
  /** @export */
  __syscall_getcwd: ___syscall_getcwd,
  /** @export */
  __syscall_getdents64: ___syscall_getdents64,
  /** @export */
  __syscall_ioctl: ___syscall_ioctl,
  /** @export */
  __syscall_lstat64: ___syscall_lstat64,
  /** @export */
  __syscall_mkdirat: ___syscall_mkdirat,
  /** @export */
  __syscall_newfstatat: ___syscall_newfstatat,
  /** @export */
  __syscall_openat: ___syscall_openat,
  /** @export */
  __syscall_poll: ___syscall_poll,
  /** @export */
  __syscall_poll_nonblocking: ___syscall_poll_nonblocking,
  /** @export */
  __syscall_readlinkat: ___syscall_readlinkat,
  /** @export */
  __syscall_renameat: ___syscall_renameat,
  /** @export */
  __syscall_rmdir: ___syscall_rmdir,
  /** @export */
  __syscall_stat64: ___syscall_stat64,
  /** @export */
  __syscall_unlinkat: ___syscall_unlinkat,
  /** @export */
  __syscall_utimensat: ___syscall_utimensat,
  /** @export */
  _abort_js: __abort_js,
  /** @export */
  _gmtime_js: __gmtime_js,
  /** @export */
  _localtime_js: __localtime_js,
  /** @export */
  _mmap_js: __mmap_js,
  /** @export */
  _msync_js: __msync_js,
  /** @export */
  _munmap_js: __munmap_js,
  /** @export */
  _tzset_js: __tzset_js,
  /** @export */
  clock_res_get: _clock_res_get,
  /** @export */
  clock_time_get: _clock_time_get,
  /** @export */
  emscripten_date_now: _emscripten_date_now,
  /** @export */
  emscripten_force_exit: _emscripten_force_exit,
  /** @export */
  emscripten_get_heap_max: _emscripten_get_heap_max,
  /** @export */
  emscripten_get_now: _emscripten_get_now,
  /** @export */
  emscripten_resize_heap: _emscripten_resize_heap,
  /** @export */
  environ_get: _environ_get,
  /** @export */
  environ_sizes_get: _environ_sizes_get,
  /** @export */
  exit: _exit,
  /** @export */
  fd_close: _fd_close,
  /** @export */
  fd_fdstat_get: _fd_fdstat_get,
  /** @export */
  fd_pread: _fd_pread,
  /** @export */
  fd_pwrite: _fd_pwrite,
  /** @export */
  fd_read: _fd_read,
  /** @export */
  fd_seek: _fd_seek,
  /** @export */
  fd_sync: _fd_sync,
  /** @export */
  fd_write: _fd_write,
  /** @export */
  mono_interp_flush_jitcall_queue: _mono_interp_flush_jitcall_queue,
  /** @export */
  mono_interp_invoke_wasm_jit_call_trampoline: _mono_interp_invoke_wasm_jit_call_trampoline,
  /** @export */
  mono_interp_jit_wasm_entry_trampoline: _mono_interp_jit_wasm_entry_trampoline,
  /** @export */
  mono_interp_jit_wasm_jit_call_trampoline: _mono_interp_jit_wasm_jit_call_trampoline,
  /** @export */
  mono_interp_record_interp_entry: _mono_interp_record_interp_entry,
  /** @export */
  mono_interp_tier_prepare_jiterpreter: _mono_interp_tier_prepare_jiterpreter,
  /** @export */
  mono_wasm_free_method_data: _mono_wasm_free_method_data,
  /** @export */
  mono_wasm_set_entrypoint_breakpoint: _mono_wasm_set_entrypoint_breakpoint,
  /** @export */
  mono_wasm_trace_logger: _mono_wasm_trace_logger
};


// include: postamble.js
// === Auto-generated postamble setup entry stuff ===

async function run() {

  preRun();

  if (runDependencies) {
    await resolveRunDependencies();
  }

  var setStatus = Module['setStatus'];
  if (setStatus) {
    setStatus('Running...');
    // Yield to the event loop to allow the browser to paint "Running..."
    await new Promise((resolve) => setTimeout(resolve, 1));
    // Then we want to clear the status text, but only after the rest of this function runs.
    setTimeout(setStatus, 1, '');
  }

  if (ABORT) return;

  initRuntime();

  Module['onRuntimeInitialized']?.();

  postRun();
}

var wasmExports;

// In modularize mode the generated code is within a factory function so we
// can use await here (since it's not top-level-await).
wasmExports = await createWasm();
await run();

// end include: postamble.js

// include: postamble_modularize.js
// In MODULARIZE mode we wrap the generated code in a factory function
// and return either the Module itself, or a promise of the module.

// end include: postamble_modularize.js



  return Module;
}

// Export using a UMD style export, or ES6 exports if selected
export default createDotnetRuntime;


var fetch = fetch || undefined; var _nativeModuleLoaded = false;
