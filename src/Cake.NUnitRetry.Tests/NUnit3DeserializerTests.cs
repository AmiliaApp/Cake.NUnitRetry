using System;
using System.Linq;
using Cake.Core;
using Cake.NUnitRetry.Model;
using Cake.NUnitRetry.Tests.Fixtures;
using Cake.Testing;
using NUnit.Framework;

namespace Cake.NUnitRetry.Tests
{
    [TestFixture]
    public sealed class NUnit3DeserializerTests
    {
        public sealed class TheConstructor
        {
            [Test]
            public void ThrowsIfFileSystemIsNull()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var exception = Assert.Throws<ArgumentNullException>(() => new XmlParser(null, environment));

                Assert.AreEqual("fileSystem", exception.ParamName);
            }

            [Test]
            public void ThrowsIfEnvironmentIsNull()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);

                var exception = Assert.Throws<ArgumentNullException>(() => new XmlParser(fileSystem, null));

                Assert.AreEqual("environment", exception.ParamName);
            }
        }

        public sealed class TheGetTestRunMethod
        {
            [Test]
            public void ReturnsTheExpectedTestRunResults()
            {
                var startTime = new DateTime(2019, 10, 01, 14, 31, 42, DateTimeKind.Utc);
                var endTime = new DateTime(2019, 10, 01, 14, 31, 55, DateTimeKind.Utc);
                var duration = 12.351793;

                var fixture = new NUnit3XmlReaderFixture();
                TestRun results = fixture.ParseTestRunResultsNUnit3();

                Assert.AreEqual(results.Total, 18, "Wrong total amount");
                Assert.AreEqual(results.Passed, 12, "Wrong passed amount");
                Assert.AreEqual(results.Failed, 2, "Wrong failed amount");
                Assert.AreEqual(results.Inconclusive, 1, "Wrong inconclusive amount");
                Assert.AreEqual(results.Skipped, 3, "Wrong skipped amount");

                Assert.AreEqual(results.Passed, results.TestCases.Count(x => x.Result == Result.Passed), "Wrong passed test cases amount");
                Assert.AreEqual(results.Failed, results.TestCases.Count(x => x.Result == Result.Failed), "Wrong failed test cases amount");
                Assert.AreEqual(results.Inconclusive, results.TestCases.Count(x => x.Result == Result.Inconclusive), "Wrong inconclusive test cases amount");
                Assert.AreEqual(results.Skipped, results.TestCases.Count(x => x.Result == Result.Skipped), "Wrong skipped test cases amount");

                var fullName = "NUnit.Tests.Assemblies.MockTestFixture.MockTest3";
                var testCase = results.TestCases.SingleOrDefault(x => x.FullName == fullName);

                Assert.NotNull(testCase, $"Could not find test case with full name '{fullName}'");

                Assert.AreEqual(fullName, testCase.FullName, "Unexpected full name");
                Assert.AreEqual(Result.Passed, testCase.Result, "Unexpected result");
                Assert.AreEqual(DateTimeKind.Utc, testCase.StartTime.Kind, "Start time isn't UTC");
                Assert.AreEqual(DateTimeKind.Utc, testCase.EndTime.Kind, "End time isn't UTC");
                Assert.AreEqual(startTime, testCase.StartTime, "Unexpected start time");
                Assert.AreEqual(endTime, testCase.EndTime, "Unexpected end time");
                Assert.AreEqual(duration, testCase.Duration.TotalSeconds,  .001, "Unexpected duration");
            }

            [Test]
            public void ThrowsIfWrongFileFormatIsUsed()
            {
                var fixture = new NUnit3XmlReaderFixture();
                Assert.Throws<XmlParsingException>(() => fixture.ParseTestRunResultsNUnit2());
            }

            [Test]
            public void ThrowsIfItHasADuplicateTestName()
            {
                var fixture = new NUnit3XmlReaderFixture();
                Assert.Throws<CakeException>(() => fixture.ParseTestRunResultsNUnit3DuplicateTestName());
            }

            [Test]
            public void ThrowsIfFilePathIsNull()
            {
                var fixture = new NUnit3XmlReaderFixture();

                var exception = Assert.Throws<ArgumentNullException>(() => fixture.ParseTestRunResultsNull());

                Assert.AreEqual("testResultsPath", exception.ParamName);
            }

            [Test]
            public void ThrowsIfFilePathDoesntExist()
            {
                var fixture = new NUnit3XmlReaderFixture();

                var exception = Assert.Throws<CakeException>(() => fixture.ParseTestRunResultsNonExistant());
            }
        }
    }
}
