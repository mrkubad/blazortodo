using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebApplication1.Data
{
    public class ExampleJsInterop
    {
        
            private readonly IJSRuntime _jsRuntime;
            public static object CreateDotNetObjectRefSyncObj = new object();
        protected DotNetObjectRef<T> CreateDotNetObjectRef<T>(T value) where T : class
        {
            lock (CreateDotNetObjectRefSyncObj)
            {
                JSRuntime.SetCurrentJSRuntime(_jsRuntime);
                return DotNetObjectRef.Create(value);
            }
        }

        protected void DisposeDotNetObjectRef<T>(DotNetObjectRef<T> value) where T : class
        {
            if (value != null)
            {
                lock (CreateDotNetObjectRefSyncObj)
                {
                    JSRuntime.SetCurrentJSRuntime(_jsRuntime);
                    value.Dispose();
                }
            }
        }

        public ExampleJsInterop(IJSRuntime jsRuntime)
            {
                _jsRuntime = jsRuntime;
            }

            public Task CallHelloHelperSayHello<T>(T obj) where T : class
            {
                // sayHello is implemented in wwwroot/exampleJsInterop.js
                return _jsRuntime.InvokeAsync<object>(
                    "SaveObjectRef",
                    CreateDotNetObjectRef(obj));
            }
    }
}
