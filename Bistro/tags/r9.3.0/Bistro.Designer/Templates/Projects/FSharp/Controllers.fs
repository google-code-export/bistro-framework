#light

namespace $safeprojectname$

    open Bistro.FS.Controller
    open Bistro.FS.Definitions
    open Bistro.FS.Inference
    
    open Bistro.Controllers
    open Bistro.Controllers.Descriptor
    open Bistro.Controllers.Descriptor.Data
    open Bistro.Http
    
    open System.Text.RegularExpressions
    open System.Web
    
    module $safeitemname$ =
                
        [<Bind("?"); RenderWith("Views/home.django"); ReflectedDefinition>]
        let defaultC (ctx: ictx) = () 
