using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyRoadsMod.Redirection
{
    /// <summary>
    /// Marks a method for redirection. All marked methods are redirected by calling
    /// <see cref="Redirector.PerformRedirections"/> and reverted by <see cref="Redirector.RevertRedirections"/>
    /// <para>NOTE: only the methods belonging to the same assembly that calls Perform/RevertRedirections are redirected.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RedirectFromAttribute : RedirectAttribute
    {
        /// <param name="classType">The class of the method that will be redirected</param>
        /// <param name="methodName">The name of the method that will be redirected. If null,
        /// the name of the attribute's target method will be used.</param>
        public RedirectFromAttribute(Type classType, string methodName, ulong bitSetOption = 0)
            : base(classType, methodName, bitSetOption)
        { }

        public RedirectFromAttribute(Type classType, ulong bitSetOption = 0)
            : base(classType, bitSetOption)
        { }
    }
}
