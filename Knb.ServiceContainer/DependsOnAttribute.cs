using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.ServiceContainer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DependsOnAttribute : Attribute
    {
        private readonly Type[] _types = null;
        public Type[] Dependencies { get { return _types.ToArray(); } }

        public DependsOnAttribute(params Type[] dependencies)
        {
            _types = dependencies.ToArray();
        }
    }
}
