using System;
using System.Collections.Generic;
using Cake.Common.Tools.NUnit;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.NUnitRetry
{
    [CakeAliasCategory("Testing")]
    public static class NUnit3RetryFailedAliases
    {
        [CakeMethodAlias]
        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies,
        /// using the specified settings. If there are test failures it will retry the failed
        /// tests up to the amount of times specified.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="maxRetries">The maximum number of times to re-run when a test fails</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// NUnit3RetryFailed(testAssemblies, new NUnit3Settings {
        ///     NoResults = true
        ///     }, 3);
        /// </code>
        /// </example>
        public static void NUnit3RetryFailed(this ICakeContext context, IEnumerable<FilePath> assemblies, NUnit3Settings settings, int maxRetries)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new NUnit3Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }

        [CakeMethodAlias]
        /// <summary>
        /// Checks if there are test failures in the specified file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The nunit3 test result path to check.</param>
        /// <example>
        /// <code>
        /// var hasFailures = NUnit3HasFailedTests("./src/**/bin/Release/*.Tests.dll");
        /// NUnit3RetryFailed(testAssemblies, new NUnit3Settings {
        ///     NoResults = true
        ///     }, 3);
        /// </code>
        /// </example>
        public static bool NUnit3HasFailedTests(this ICakeContext context, FilePath testResultsPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parser = new XmlParser(context.FileSystem, context.Environment);

            return parser.HasFailedTests(testResultsPath);
        }
    }
}
