using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyRoadsMod.Redirection
{
    public abstract class RedirectAttribute : Attribute
    {
        public RedirectAttribute(Type classType, string methodName, ulong bitSetOption = 0)
        {
            ClassType = classType;
            MethodName = methodName;
            BitSetRequiredOption = bitSetOption;
        }

        public RedirectAttribute(Type classType, ulong bitSetOption = 0)
            : this(classType, null, bitSetOption)
        { }

        public Type ClassType { get; set; }
        public string MethodName { get; set; }
        public ulong BitSetRequiredOption { get; set; }
    }
}
