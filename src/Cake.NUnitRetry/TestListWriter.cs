using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.NUnitRetry
{
    public class TestListWriter
    {
        private IFileSystem _fileSystem { get; }
        private ICakeEnvironment _environment { get; }

        public TestListWriter(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _fileSystem = fileSystem;
            _environment = environment;
        }

        public void Write(FilePath filePath, List<string> testCaseNames)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (filePath.IsRelative)
            {
                filePath = filePath.MakeAbsolute(_environment);
            }

            var directory = filePath.GetDirectory();


            _fileSystem.GetDirectory(directory).Create();


            if (_fileSystem.GetFile(filePath).Exists)
            {
                _fileSystem.GetFile(filePath).Delete();
            }

            using (var stream = _fileSystem.GetFile(filePath).OpenWrite())
            using(var writer = new System.IO.StreamWriter(stream))
            {
                foreach (var test in testCaseNames)
                {
                    writer.WriteLine(test);
                }
            }
        }
    }
}
