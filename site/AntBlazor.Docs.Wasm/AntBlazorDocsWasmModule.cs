using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace AntBlazor.Docs.Wasm
{
    [DependsOn(typeof(AntBlazorDocsModule))]
    public class AntBlazorDocsWasmModule : AbpModule
    {

    }
}
