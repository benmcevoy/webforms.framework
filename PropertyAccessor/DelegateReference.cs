using System;
using System.Reflection;

namespace PropertyAccessor
{
    public class DelegateReference
    {
        private readonly MethodInfo _method;
        private readonly Type _type;

        /// <summary>
        /// Delegate Reference
        /// </summary>
        /// <remarks>
        /// See: https://compositewpf.codeplex.com/SourceControl/changeset/view/65392#1024790
        /// as this class is pretty much a copy of that
        /// </remarks>
        /// <param name="delegate"></param>
        public DelegateReference(object target, MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            _method = method;
            _type = method.ReturnType;
        }

        public Delegate TryGetDelegate(object target)
        {
            if (_method.IsStatic)
            {
                return Delegate.CreateDelegate(_type, null, _method);
            }

            if (target == null)
            {
                return null;
            }

            return Delegate.CreateDelegate(_type, target, _method);
        }
    }
}
