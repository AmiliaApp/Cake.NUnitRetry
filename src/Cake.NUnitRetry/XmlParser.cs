using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.NUnitRetry.Model;

namespace Cake.NUnitRetry
{
    public class XmlParser
    {

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public XmlParser(IFileSystem fileSystem, ICakeEnvironment environment)
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

        public bool HasFailedTests(FilePath testResultsPath)
        {
            var result = ParseResults(testResultsPath);

            return result.Failed != 0;
            
        }

        public TestRun ParseResults(FilePath testResultsPath)
        {
            if (testResultsPath == null)
            {
                throw new ArgumentNullException(nameof(testResultsPath));
            }

            if (testResultsPath.IsRelative)
            {
                testResultsPath = testResultsPath.MakeAbsolute(_environment);
            }

            var file = _fileSystem.GetFile(testResultsPath);
            if (!file.Exists)
            {
                throw new CakeException($"Test result file '{testResultsPath.FullPath}' does not exist.");
            }

            XDocument doc;
            using (var stream = file.OpenRead())
            {
                doc = XDocument.Load(stream);
            }

            long total;
            long passed;
            long failed;
            long inconclusive;
            long skipped;
            List<TestCase> testCases;

            try
            {
                var testRunXml = doc.Descendants("test-run").Single();

                total = Convert.ToInt64(testRunXml.Attribute("total").Value);
                passed = Convert.ToInt64(testRunXml.Attribute("passed").Value);
                failed = Convert.ToInt64(testRunXml.Attribute("failed").Value);
                inconclusive = Convert.ToInt64(testRunXml.Attribute("inconclusive").Value);
                skipped = Convert.ToInt64(testRunXml.Attribute("skipped").Value);

                testCases = doc.Descendants("test-case").Select(x => new TestCase(x)).ToList();
            }
            catch (InvalidOperationException e)
            {
                throw new XmlParsingException("The test results file must be in the nunit3 format", e);
            }

            if (testCases.Select(x => x.FullName).Count() != testCases.Select(x => x.FullName).Distinct().Count())
            {
                throw new CakeException($"Test result file has test case(s) with duplicate fullname attributes.");
            }

            return new TestRun(total, passed, failed, inconclusive, skipped, testCases);
        }
    }
}
