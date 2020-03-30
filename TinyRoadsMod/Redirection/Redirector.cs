using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TinyRoadsMod.Redirection
{
    public static class Redirector
    {
        internal class MethodRedirection : IDisposable
        {
            private bool _isDisposed = false;

            private MethodInfo _originalMethod;
            private readonly RedirectCallsState _callsState;
            public Assembly RedirectionSource { get; set; }

            public MethodRedirection(MethodInfo originalMethod, MethodInfo newMethod, Assembly redirectionSource)
            {
                _originalMethod = originalMethod;
                _callsState = RedirectionHelper.RedirectCalls(_originalMethod, newMethod);
                RedirectionSource = redirectionSource;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    RedirectionHelper.RevertRedirect(_originalMethod, _callsState);
                    _originalMethod = null;
                    _isDisposed = true;
                }
            }

            public MethodInfo OriginalMethod
            {
                get
                {
                    return _originalMethod;
                }
            }
        }

        private static List<MethodRedirection> s_redirections = new List<MethodRedirection>();

        public static void PerformRedirections(ulong bitMask = 0)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            IEnumerable<MethodInfo> methods = from type in callingAssembly.GetTypes()
                                              from method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                              where method.GetCustomAttributes(typeof(RedirectAttribute), false).Length > 0
                                              select method;

            foreach (MethodInfo method in methods)
            {
                foreach (RedirectAttribute redirectAttr in method.GetCustomAttributes(typeof(RedirectAttribute), false))
                {
                    if (redirectAttr.BitSetRequiredOption != 0 && (bitMask & redirectAttr.BitSetRequiredOption) == 0)
                        continue;

                    string originalName = String.IsNullOrEmpty(redirectAttr.MethodName) ? method.Name : redirectAttr.MethodName;

                    MethodInfo originalMethod = null;
                    foreach (MethodInfo m in redirectAttr.ClassType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        if (m.Name != originalName)
                            continue;

                        if (method.IsCompatibleWith(m))
                        {
                            originalMethod = m;
                            break;
                        }
                    }

                    if (originalMethod == null)
                    {
                        throw new Exception(string.Format("TFW: Original method {0} has not been found for redirection", originalName));
                    }

                    if (redirectAttr is RedirectFromAttribute)
                    {
                        if (!s_redirections.Any(r => r.OriginalMethod == originalMethod))
                        {
                            Debug.Log(string.Format("TFW: Redirecting from {0}.{1} to {2}.{3}",
                                originalMethod.DeclaringType,
                                originalMethod.Name,
                                method.DeclaringType,
                                method.Name));
                            s_redirections.Add(originalMethod.RedirectTo(method, callingAssembly));
                        }
                    }

                    if (redirectAttr is RedirectToAttribute)
                    {
                        if (!s_redirections.Any(r => r.OriginalMethod == method))
                        {
                            Debug.Log(string.Format("TFW: Redirecting from {0}.{1} to {2}.{3}",
                                method.DeclaringType,
                                method.Name,
                                originalMethod.DeclaringType,
                                originalMethod.Name));
                            s_redirections.Add(method.RedirectTo(originalMethod, callingAssembly));
                        }
                    }
                }
            }
        }

        public static void RevertRedirections()
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            for (int i = s_redirections.Count - 1; i >= 0; --i)
            {
                var redirection = s_redirections[i];

                if (Equals(redirection.RedirectionSource, callingAssembly))
                {
                    Debug.Log(string.Format("TFW: Removing redirection {0}", s_redirections[i].OriginalMethod));
                    s_redirections[i].Dispose();
                    s_redirections.RemoveAt(i);
                }
            }
        }
    }
}
