using System.Linq;
using Cake.Core.IO;
using Cake.NUnitRetry.Model;
using Cake.Testing;
using NUnit.Framework;

namespace Cake.NUnitRetry.Tests
{
    public class XmlResultUpdaterTests
    {
        public class TheUpdateMethod
        {
            private readonly FilePath _failedResultPath = "/Working/failedresult.xml";
            private readonly FilePath _failedResultFixedPath = "/Working/failedresultfixed.xml";
            private FakeEnvironment _environment;
            private FakeFileSystem _fileSystem;
            private XmlParser _parser;

            [SetUp]
            public void SetUp()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                _environment = environment;
                var fileSystem = new FakeFileSystem(environment);
                _fileSystem = fileSystem;
                _parser = new XmlParser(_fileSystem, _environment);
            }

            [Test]
            public void UpdatesAFailedTestWhenItsFixed()
            {
                _fileSystem.CreateFile(_failedResultPath.FullPath).SetContent(Resources.FailureResult);
                _fileSystem.CreateFile(_failedResultFixedPath.FullPath).SetContent(Resources.FailureFixedResult);

                var parser = new XmlParser(_fileSystem, _environment);

                var updater = new XmlResultUpdater(parser, _fileSystem, _environment);
                updater.Update(_failedResultPath, _failedResultFixedPath);

                var results = _parser.Parse(_failedResultPath);

                Assert.AreEqual(1, results.Passed);
                Assert.AreEqual(1, results.TestCases.Count(x => x.Result == Result.Passed));
                Assert.AreEqual(0, results.Failed);
                Assert.AreEqual(0, results.TestCases.Count(x => x.Result == Result.Failed));
            }

            [Test]
            public void UpdatesMultipleFailedTestsWhenItsFixed()
            {
                _fileSystem.CreateFile(_failedResultPath.FullPath).SetContent(Resources.TwoFailingTestsResult);
                _fileSystem.CreateFile(_failedResultFixedPath.FullPath).SetContent(Resources.TwoFailingTestsFixedResult);

                var parser = new XmlParser(_fileSystem, _environment);

                var updater = new XmlResultUpdater(parser, _fileSystem, _environment);
                updater.Update(_failedResultPath, _failedResultFixedPath);

                var results = _parser.Parse(_failedResultPath);

                Assert.AreEqual(2, results.Passed);
                Assert.AreEqual(2, results.TestCases.Count(x => x.Result == Result.Passed));
                Assert.AreEqual(0, results.Failed);
                Assert.AreEqual(0, results.TestCases.Count(x => x.Result == Result.Failed));
            }

        }
    }
}
