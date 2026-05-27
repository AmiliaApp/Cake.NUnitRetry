using System;
using System.Linq;
using System.Xml.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.NUnitRetry
{
    public class XmlResultUpdater
    {
        private readonly XmlParser _parser;
        private readonly IFileSystem _fileSystem;

        public XmlResultUpdater(XmlParser xmlParser, IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            _parser = xmlParser;
            _fileSystem = fileSystem;
        }

        public void Update(FilePath resultsToUpdatePath, FilePath updateSourcePath)
        {
            var latestResults = _parser.Parse(updateSourcePath);

            var file = _fileSystem.GetFile(resultsToUpdatePath);
            if (!file.Exists)
            {
                throw new CakeException($"Test result file '{resultsToUpdatePath.FullPath}' does not exist.");
            }

            XDocument doc;
            using (var stream = file.OpenRead())
            {
                doc = XDocument.Load(stream);
            }

            foreach (var pass in latestResults.TestCases.Where(x => x.Result == Model.Result.Passed))
            {
                var tcx = doc.Descendants("test-case").Single(x => (string)x.Attribute("fullname") == pass.FullName);

                var parent = tcx.Parent;

                tcx.ReplaceWith(pass.Element);

                while(parent != null)
                {
                    var passedCount = Convert.ToInt64(parent.Attribute("passed").Value);
                    passedCount++;
                    parent.Attribute("passed").Value = passedCount.ToString();

                    var failedCount = Convert.ToInt64(parent.Attribute("failed").Value);
                    failedCount--;
                    parent.Attribute("failed").Value = failedCount.ToString();

                    if(failedCount == 0)
                    {
                        var failures = parent.Descendants("failure");
                        failures.Remove();

                        var skippedCount = Convert.ToInt64(parent.Attribute("skipped").Value);

                        parent.Attribute("result").Value = skippedCount == 0 ? "Passed" : "Skipped";
                    }

                    parent = parent.Parent;
                }
            }

            using (var stream = _fileSystem.GetFile(resultsToUpdatePath).OpenWrite())
            {
                doc.Save(stream);
            }
        }
    }
}
