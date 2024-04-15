# Biome Parser

Gets a Biome report text file and makes it readable.

## Diag file procurement

Currently, Biome does **not** support exporting diagnostics to a file, so a workaround is needed. For example

```sh
npx @biomejs/biome check . --max-diagnostics 500 *> biome.diag
```

## Usage

1. Launch
2. Enter the path to the file with Biome diagnostics
3. Let it rip
4. Open `diag.html` file in the browser

## Example diagnostic file

Make sure your diag file looks something like this, otherwise the parser won't work

```
.\biome.jsonc format ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  i Formatter would have printed the following content:
  
     7  7 │   	},
     8  8 │   	"files": {
     9    │ - → → "include":·[
    10    │ - → → → "wwwroot/js/src*/*.js",
    11    │ - → → → "wwwroot/js/src*/*.ts",
    12    │ - → → → "*.mjs"
    13    │ - → → ]
        9 │ + → → "include":·["wwwroot/js/src*/*.js",·"wwwroot/js/src*/*.ts",·"*.mjs"]
    14 10 │   	},
    15 11 │   	"formatter": {
  

.\gulpfile.mjs:1:1 lint/suspicious/noRedundantUseStrict  FIXABLE  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  × Redundant use strict directive.
  
  > 1 │ "use strict";
      │ ^^^^^^^^^^^^^
    2 │ import { pipeline } from "stream";
    3 │ import { dest, lastRun, parallel, src, watch } from "gulp";
  
  i The entire contents of JavaScript modules are automatically in strict mode, with no statement needed to initiate it.
  
  i Safe fix: Remove the redundant use strict directive.
  
    1 │ "use·strict";␍
      │ -------------

.\gulpfile.mjs:2:26 lint/style/useNodejsImportProtocol  FIXABLE  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  × A Node.js builtin module should be imported with the node: protocol.
  
    1 │ "use strict";
  > 2 │ import { pipeline } from "stream";
      │                          ^^^^^^^^
    3 │ import { dest, lastRun, parallel, src, watch } from "gulp";
    4 │ import sourcemaps from "gulp-sourcemaps";
  
  i Using the node: protocol is more explicit and signals that the imported module belongs to Node.js.
  
  i Unsafe fix: Add the node: protocol.
  
      1   1 │   "use strict";␍
      2     │ - import·{·pipeline·}·from·"stream";␍
          2 │ + import·{·pipeline·}·from·"node:stream";␍
      3   3 │   import { dest, lastRun, parallel, src, watch } from "gulp";␍
      4   4 │   import sourcemaps from "gulp-sourcemaps";␍
  
```