﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SwaggerCodeAnalyzer.Test.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SwaggerCodeAnalyzer.Test.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///using System.Threading.Tasks;
        ///
        ///namespace ConsoleApp1
        ///{
        ///    public class Route : Attribute
        ///    {
        ///        //...
        ///    }
        ///
        ///    public class Swagger : Attribute
        ///    {
        ///        //...
        ///    }
        ///
        ///    public class SwaggerOperation: Attribute
        ///    {
        ///        public SwaggerOperation(string name) { }
        ///
        ///    }
        ///
        ///    
        ///    class Program
        ///    {
        ///        [Route]
        ///        [Swagger]
        ///        [SwaggerOperation(&quot;Program_NoClassMet [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AfterCheckMissingSwaggerOperationAttribute {
            get {
                return ResourceManager.GetString("AfterCheckMissingSwaggerOperationAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///using System.Threading.Tasks;
        ///
        ///namespace ConsoleApp1
        ///{
        ///    public class Route : Attribute
        ///    {
        ///        //...
        ///    }
        ///
        ///    public class Swagger : Attribute
        ///    {
        ///        //...
        ///    }
        ///
        ///    public class SwaggerOperation: Attribute
        ///    {
        ///        public SwaggerOperation(string name) { }
        ///
        ///    }
        ///
        ///    
        ///    class Program
        ///    {
        ///        [Route]
        ///        [Swagger]
        ///        public void NoClassMethod(string name [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BeforeCheckMissingSwaggerOperationAttribute {
            get {
                return ResourceManager.GetString("BeforeCheckMissingSwaggerOperationAttribute", resourceCulture);
            }
        }
    }
}
