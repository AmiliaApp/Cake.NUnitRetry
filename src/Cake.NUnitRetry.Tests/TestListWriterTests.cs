using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core.IO;
using Cake.Testing;
using NUnit.Framework;

namespace Cake.NUnitRetry.Tests
{
    [TestFixture]
    public class TestListWriterTests
    {
        public sealed class TheConstructor
        {
            [Test]
            public void ThrowsIfFileSystemIsNull()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var exception = Assert.Throws<ArgumentNullException>(() => new TestListWriter(null, environment));

                Assert.AreEqual("fileSystem", exception.ParamName);
            }

            [Test]
            public void ThrowsIfEnvironmentIsNull()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);

                var exception = Assert.Throws<ArgumentNullException>(() => new TestListWriter(fileSystem, null));

                Assert.AreEqual("environment", exception.ParamName);
            }
        }

        public class TheWriteMethod
        {
            private readonly FilePath _testListFile = "/Working/testlist.txt";
            private FakeEnvironment _environment;
            private FakeFileSystem _fileSystem;
            private List<string> _tests;

            [SetUp]
            public void SetUp()
            {
                var environment = FakeEnvironment.CreateUnixEnvironment();
                _environment = environment;
                var fileSystem = new FakeFileSystem(environment);
                _fileSystem = fileSystem;

                _tests = new List<string> { "Test.One", "Test.Two" };
            }

            [Test]
            public void ThrowsIfFilePathIsNull()
            {
                var sut = new TestListWriter(_fileSystem, _environment);

                var exception = Assert.Throws<ArgumentNullException>(() => sut.Write(null, _tests));
                Assert.AreEqual("filePath", exception.ParamName);
            }


            [Test]
            public void WritesExpectedContentsToTheFile()
            {
                var sut = new TestListWriter(_fileSystem, _environment);

                sut.Write(_testListFile, _tests);

                var fileLines = _fileSystem.GetFile(_testListFile).ReadLines(Encoding.UTF8).ToList();

                Assert.AreEqual(_tests.Count, fileLines.Count, "Unexpected amount of lines in file");

                for(int i = 0; i < _tests.Count; i++)
                {
                    Assert.AreEqual(_tests[i], fileLines[i], "Unexpected test file content");
                }
            }

            [Test]
            public void WritesExpectedContentsToTheFileWhenWrittenTwice()
            {
                var sut = new TestListWriter(_fileSystem, _environment);
                var secondTests = new List<string> { "Test.Three", "Test.Four" };

                sut.Write(_testListFile, _tests);
                sut.Write(_testListFile, secondTests);

                var fileLines = _fileSystem.GetFile(_testListFile).ReadLines(Encoding.UTF8).ToList();

                Assert.AreEqual(secondTests.Count, fileLines.Count, "Unexpected amount of lines in file");

                for (int i = 0; i < secondTests.Count; i++)
                {
                    Assert.AreEqual(secondTests[i], fileLines[i], "Unexpected test file content");
                }
            }
        }
    }
}
