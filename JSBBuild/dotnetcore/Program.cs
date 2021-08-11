﻿using System;
using System.Threading;
using QuickJS;
using QuickJS.IO;
using QuickJS.Utils;
using QuickJS.Binding;

namespace DotnetCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new DefaultScriptLogger();
            var pathResolver = new PathResolver();
            var fileSystem = new DefaultFileSystem(logger);
            var asyncManager = new DefaultAsyncManager();
            var runtime = ScriptEngine.CreateRuntime();

            runtime.AddModuleResolvers();
            runtime.Initialize(new ScriptRuntimeArgs
            {
                fileSystem = fileSystem,
                pathResolver = pathResolver,
                asyncManager = asyncManager,
                logger = logger,
                byteBufferAllocator = new ByteBufferPooledAllocator(),
                binder = BindingManager.UnitylessReflectBind,
            });
            runtime.AddSearchPath("./");
            runtime.AddSearchPath("./node_modules");
            runtime.EvalMain("main");
            while (runtime.isRunning)
            {
                runtime.Update(1);
                Thread.Sleep(1);
            }
            runtime.Shutdown();
        }
    }
}
