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

namespace System.Reflection.Tests
{
    using NUnit.Framework;

    using System.Collections.Generic;
    using System.Text;

    public class RecordHelpersTests
    {
        private static IEnumerable<(Type CandidateType, bool Expected)> Cases
        {
            get
            {
                yield return (typeof(NotRecord), false);
                yield return (typeof(RecordClass), true);
                yield return (typeof(InheritedRecordClass), true);
                yield return (typeof(RecordStruct), true);
            }
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void DetermineTypeIsRecordType((Type CandidateType, bool Expected) @case)
        {   
            Assert.AreEqual(@case.Expected, RecordHelpers.IsRecord(@case.CandidateType));
        } 
    }


    public class NotRecord
    {
    }

    public record struct RecordStruct
    {
        public override int GetHashCode() => throw new NotImplementedException();
        public override string ToString() => throw new NotImplementedException();
    }

    public record RecordClass
    {
        public override int GetHashCode() => throw new NotImplementedException();
        public override string ToString() => throw new NotImplementedException();
    }

    public record InheritedRecordClass : RecordClass
    {
        protected override Type EqualityContract => throw new NotImplementedException();

        public override int GetHashCode() => throw new NotImplementedException();
        public override string ToString() => throw new NotImplementedException();
        protected override bool PrintMembers(StringBuilder builder) => throw new NotImplementedException();
    }
}