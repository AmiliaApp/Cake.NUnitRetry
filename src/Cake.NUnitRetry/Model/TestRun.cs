using System.Collections.Generic;

namespace Cake.NUnitRetry.Model
{
    public class TestRun
    {
        public long Total { get; }

        public long Passed { get; }

        public long Failed { get; }

        public long Skipped { get; }

        public long Inconclusive { get; }

        public IReadOnlyList<TestCase> TestCases { get; }

        public TestRun(long total, long passed, long failed, long inconclusive, long skipped, List<TestCase> testCases)
        {
            Total = total;
            Passed = passed;
            Failed = failed;
            Inconclusive = inconclusive;
            Skipped = skipped;
            TestCases = testCases?.AsReadOnly();
        }

    }
}
