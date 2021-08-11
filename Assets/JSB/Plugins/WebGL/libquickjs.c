
#ifndef UNITY_WEBGL
#define UNITY_WEBGL
#endif

#ifndef EMSCRIPTEN
#define EMSCRIPTEN
#endif

// #define DUMP_MEM
// #define CONFIG_BIGNUM

#define CONFIG_VERSION "quickjs-latest"

#define compute_stack_size compute_stack_size_regexp
#define is_digit is_digit_regexp
#include "../../../../JSBBuild/quickjs/quickjs-latest/libregexp.c"
#undef is_digit
#undef compute_stack_size

#include "../../../../JSBBuild/quickjs/quickjs-latest/cutils.c"
#include "../../../../JSBBuild/quickjs/quickjs-latest/libunicode.c"

#ifdef CONFIG_BIGNUM
#define floor_div floor_div_bbf
#define to_digit to_digit_bbf
#include "../../../../JSBBuild/quickjs/quickjs-latest/libbf.c"
#undef floor_div
#undef to_digit
#undef malloc
#undef free
#undef realloc
#endif

#include "../../../../JSBBuild/quickjs/quickjs-latest/quickjs.c"

#define DEF(name, str) \
    JSAtom JSB_ATOM_##name() { return JS_ATOM_##name; }
#include "../../../../JSBBuild/quickjs/quickjs-latest/quickjs-atom.h"
#undef DEF

#include "../../../../JSBBuild/quickjs/unity_qjs.c"
