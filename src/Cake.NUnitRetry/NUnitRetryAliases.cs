using System;
using System.Collections.Generic;
using Cake.Common.Tools.NUnit;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.NUnitRetry.Model;

namespace Cake.NUnitRetry
{
    [CakeAliasCategory("Testing")]
    public static class NUnitRetryAliases
    {
        /// <summary>
        /// Parses the given results file for test failures.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testResultsPath">The nunit3 test result path to check.</param>
        /// <returns><c>true</c> if there are any failed tests, otherwise <c>false</c></returns>
        /// <example>
        /// <code>
        /// var hasFailures = NUnitRetryHasFailedTests("./BuildArtifacts/Tests/TestRun.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static bool NUnitRetryHasFailedTests(this ICakeContext context, FilePath testResultsPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parser = new XmlParser(context.FileSystem, context.Environment);

            return parser.HasFailedTests(testResultsPath);
        }

        /// <summary>
        /// Parses the given results file and returns the <see cref="Cake.NUnitRetry.Model.TestRun"/> information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testResultsPath">The nunit3 test result path to parse.</param>
        /// <returns>The <see cref="Cake.NUnitRetry.Model.TestRun"/> summarizing the test run.</returns>
        /// <example>
        /// <code>
        /// var testRun = NUnitRetryParseResults("./BuildArtifacts/Tests/TestRun.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.NUnitRetry.Model")]
        public static TestRun NUnitRetryParseResults(this ICakeContext context, FilePath testResultsPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parser = new XmlParser(context.FileSystem, context.Environment);

            return parser.ParseResults(testResultsPath);
        }

        /// <summary>
        /// Writes the given test cases to the test list in a formatted for NUnit3 to run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testListPath">The path of the file to write to.</param>
        /// <param name="testCaseNames">The list of test case fullnames to write.</param>
        /// <example>
        /// <code>
        /// NUnitRetryTestListWriter("./BuildArtifacts/Tests/TestList.txt", testCaseNames);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.NUnitRetry.Model")]
        public static void NUnitRetryTestListWriter(this ICakeContext context, FilePath testListPath, List<string> testCaseNames)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var testListWriter = new TestListWriter(context.FileSystem, context.Environment);

            testListWriter.Write(testListPath, testCaseNames);
        }

        /// <summary>
        /// Writes the given test cases to the test list in a formatted for NUnit3 to run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="resultsToUpdatePath">The nunit3 test result path to update.</param>
        /// /// <param name="updateSourcePath">The nunit3 test result path used as the source to update.</param>
        /// <example>
        /// <code>
        /// NUnitRetryUpdateResults("./BuildArtifacts/Tests/OriginalTestRun.xml", "./BuildArtifacts/Tests/SecondTestRun.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.NUnitRetry.Model")]
        public static void NUnitRetryUpdateResults(this ICakeContext context, FilePath resultsToUpdatePath, FilePath updateSourcePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parser = new XmlParser(context.FileSystem, context.Environment);

            var resultUpdater = new XmlResultUpdater(parser, context.FileSystem, context.Environment);

            resultUpdater.Update(resultsToUpdatePath, updateSourcePath);
        }
    }
}
