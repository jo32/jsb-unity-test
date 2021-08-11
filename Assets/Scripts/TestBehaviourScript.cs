using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using QuickJS.Native;

public class TestBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var _rt = JSApi.JS_NewRuntime();
        var _ctx = JSApi.JS_NewContext(_rt);
        JSApi.JS_SetContextOpaque(_ctx, (IntPtr)1);
        JSApi.JS_AddIntrinsicOperators(_ctx);
        // var _atoms = new AtomCache(_ctx);
        // var _stringCache = new JSStringCache(_ctx);
        var _moduleCache = JSApi.JS_NewObject(_ctx);
        var _globalObject = JSApi.JS_GetGlobalObject(_ctx);
        var _objectConstructor = JSApi.JS_GetProperty(_ctx, _globalObject, JSApi.JS_ATOM_Object);
        var _numberConstructor = JSApi.JS_GetProperty(_ctx, _globalObject, JSApi.JS_ATOM_Number);
        var _proxyConstructor = JSApi.JS_GetProperty(_ctx, _globalObject, JSApi.JS_ATOM_Proxy);
        var _stringConstructor = JSApi.JS_GetProperty(_ctx, _globalObject, JSApi.JS_ATOM_String);
        var _functionConstructor = JSApi.JS_GetProperty(_ctx, _globalObject, JSApi.JS_ATOM_Function);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
