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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RedirectToAttribute : RedirectAttribute
    {
        /// <param name="classType">The class of the target method</param>
        /// <param name="methodName">The name of the target method. If null,
        /// the name of the attribute's target method will be used.</param>
        public RedirectToAttribute(Type classType, string methodName, ulong bitSetOption = 0)
            : base(classType, methodName, bitSetOption)
        { }

        public RedirectToAttribute(Type classType, ulong bitSetOption = 0)
            : base(classType, bitSetOption)
        { }
    }
}
