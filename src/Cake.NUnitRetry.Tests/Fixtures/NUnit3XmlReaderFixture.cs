using Cake.Core.IO;
using Cake.NUnitRetry.Model;
using Cake.Testing;

namespace Cake.NUnitRetry.Tests.Fixtures
{

    public sealed class NUnit3XmlReaderFixture
    {
        public FakeEnvironment Environment { get; }
        public FakeFileSystem FileSystem { get; }
        public FilePath NUnit2ResultPath { get; }
        public FilePath NUnit3ResultPath { get; }
        public FilePath NUnit3ResultDuplicateNamePath { get; }
        public FilePath NonExistantPath { get; }

        public NUnit3XmlReaderFixture()
        {
            NUnit2ResultPath = "/Working/nunit2results.xml";
            NUnit3ResultPath = "/Working/nunit3results.xml";
            NUnit3ResultDuplicateNamePath = "/Working/nunit3duplicateresults.xml";
            NonExistantPath = "/Working/nonexistantpath.xml";


            var environment = FakeEnvironment.CreateUnixEnvironment();
            Environment = environment;
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile(NUnit2ResultPath.FullPath).SetContent(Resources.XmlTestResults_NUnit2);
            fileSystem.CreateFile(NUnit3ResultPath.FullPath).SetContent(Resources.XmlTestResults_NUnit3);
            fileSystem.CreateFile(NUnit3ResultDuplicateNamePath.FullPath).SetContent(Resources.XmlTestResults_NUnit3_DuplicateTestName);
            FileSystem = fileSystem;
        }

        public TestRun ParseTestRunResultsNUnit2()
        {
            var deserializer = new XmlParser(FileSystem, Environment);
            return deserializer.Parse(NUnit2ResultPath);
        }

        public TestRun ParseTestRunResultsNUnit3()
        {
            var deserializer = new XmlParser(FileSystem, Environment);
            return deserializer.Parse(NUnit3ResultPath);
        }

        public TestRun ParseTestRunResultsNUnit3DuplicateTestName()
        {
            var deserializer = new XmlParser(FileSystem, Environment);
            return deserializer.Parse(NUnit3ResultDuplicateNamePath);
        }

        public TestRun ParseTestRunResultsNull()
        {
            var deserializer = new XmlParser(FileSystem, Environment);
            return deserializer.Parse(null);
        }

        public TestRun ParseTestRunResultsNonExistant()
        {
            var deserializer = new XmlParser(FileSystem, Environment);
            return deserializer.Parse(NonExistantPath);
        }
    }
}
