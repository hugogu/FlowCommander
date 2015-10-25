using System;
using System.Collections.Generic;

namespace FlowCommander.Collections
{
    public static class Utils
    {
        private class MemberEqualityComparer<T, S> : IEqualityComparer<T>
        {
            private readonly Func<T, S> _memberAccessor;

            public MemberEqualityComparer(Func<T, S> memberAccessor)
            {
                _memberAccessor = memberAccessor;
            }

            public bool Equals(T x, T y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return EqualityComparer<S>.Default.Equals(_memberAccessor(x), _memberAccessor(y));
            }

            public int GetHashCode(T obj)
            {
                return EqualityComparer<S>.Default.GetHashCode(_memberAccessor(obj));
            }
        }

        public static IEqualityComparer<T> GetMemberEqualityComparer<T, S>(Func<T, S> memberAccessor)
        {
            return new MemberEqualityComparer<T, S>(memberAccessor);
        }
    }
}
