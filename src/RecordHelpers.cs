#region LICENSE

//   MIT License
//   
//   Copyright © 2022 Orlys Ma
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//   SOFTWARE.

#endregion

namespace System.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Text;

    using static BindingFlags;

    [ExcludeFromCodeCoverage]
    public static class RecordHelpers
    {
        private static readonly ConcurrentDictionary<Type, bool> s_recordTypeCaches = new();

        /// <summary>
        /// Determines type is or not a <see langword="record"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see langword="true"/> if <typeparamref name="T"/> is a record type, otherwise, <see langword="false"/>.</returns>
        public static bool IsRecord<T>() => IsRecord(typeof(T));

        /// <summary>
        /// Determines type is or not a <see langword="record"/> type.
        /// </summary> 
        /// <param name="t"></param>
        /// <returns><see langword="true"/> if <paramref name="t"/> is a record type, otherwise, <see langword="false"/>.</returns>
        public static bool IsRecord(this Type type!!)
        {
            return s_recordTypeCaches.GetOrAdd(type, IsRecordType);

            static bool IsRecordType(Type t) =>
                !t.IsInterface && 
                !t.IsEnum &&
                typeof(IEquatable<>).MakeGenericType(t).IsAssignableFrom(t) &&
                t.GetMethod("ToString", Public | Instance, Type.EmptyTypes) is MethodInfo toString && (IsOverridden(toString) || IsCompilerGenerated(toString)) &&
                t.GetMethod("GetHashCode", Public | Instance, Type.EmptyTypes) is MethodInfo getHashCode && (IsOverridden(getHashCode) || IsCompilerGenerated(getHashCode)) &&
                t.GetMethod("PrintMembers", NonPublic | Instance, new[] { typeof(StringBuilder) }) is MethodInfo printMembers && IsCompilerGenerated(printMembers) &&
                t.GetMethod("op_Equality", Public | Static, new[] { t, t }) is MethodInfo op_Equality && IsCompilerGenerated(op_Equality) &&
                t.GetMethod("op_Inequality", Public | Static, new[] { t, t }) is MethodInfo op_Inequality && IsCompilerGenerated(op_Inequality) &&
                t.GetMethod("Equals", Public | Instance, new[] { typeof(object) }) is MethodInfo equals && IsCompilerGenerated(equals) &&
                t.GetMethod("Equals", Public | Instance, new[] { t }) is MethodInfo typeEquals && (IsCompilerGenerated(typeEquals)) &&
                t.IsValueType || t.GetMethod("<Clone>$", Public | Instance) is MethodInfo clone && IsCompilerGenerated(clone) &&
                t.IsValueType || t.GetConstructor(NonPublic | Instance, new[] { t }) is ConstructorInfo forClone && IsCompilerGenerated(forClone) &&
                t.IsValueType || t.GetMethod("get_EqualityContract", NonPublic | Instance) is MethodInfo getEqualityContract && IsCompilerGenerated(getEqualityContract) &&
                true
                ;

            static bool IsCompilerGenerated(MemberInfo member) => member.IsDefined(typeof(CompilerGeneratedAttribute));
            static bool IsOverridden(MethodInfo m) => m.GetBaseDefinition().DeclaringType != m.DeclaringType;
        } 
    }

}
