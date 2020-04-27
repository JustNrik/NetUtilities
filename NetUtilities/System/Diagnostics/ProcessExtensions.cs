using System.ComponentModel;

namespace System.Diagnostics
{
    /// <summary>
    /// Utility class for <see cref="Process"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class ProcessExtensions
    {
        /// <summary>
        /// Extension for a Shell() function that allows overloading of the working directory variable.
        /// It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="process">The <see cref="Process" /> component for which is used to execute the target process.</param>
        /// <param name="fileName">Process file name to execute.</param>
        /// <param name="arguments">Commands to pass to the process file to execute.</param>
        /// <param name="redirectStandardOutput">redirects stdout of the target process.</param>
        /// <param name="redirectStandardError">redirects stderr of the target process.</param>
        /// <param name="useShellExecute">uses shell execute instead.</param>
        /// <param name="createNoWindow">Creates no new window for the process.</param>
        /// <param name="windowStyle">Window style for the target process.</param>
        /// <param name="workingDirectory">Working directory for the target process.</param>
        /// <param name="options"> The options for which to also include for the <see cref="Process" /> component.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process process, string fileName, string arguments, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow, ProcessWindowStyle windowStyle, string workingDirectory, ProcessOptions options)
        {
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = redirectStandardOutput;
            process.StartInfo.RedirectStandardError = redirectStandardError;
            process.StartInfo.UseShellExecute = useShellExecute;
            process.StartInfo.CreateNoWindow = createNoWindow;
            process.StartInfo.WindowStyle = windowStyle;
            process.StartInfo.WorkingDirectory = workingDirectory;
            return process.Shell(options);
        }

        /// <summary>
        /// Extension for a Shell() function that allows overloading of the working directory variable.
        /// It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="process">The <see cref="Process" /> component for which is used to execute the target process.</param>
        /// <param name="options">The options for which to also include for the <see cref="Process" /> component.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process process, ProcessOptions options)
        {
            var ret = string.Empty;

            process.Start();

            if (options.Executing)
                options.Executing = false;

            if (process.StartInfo.RedirectStandardError)
                ret = process.StandardError.ReadToEnd();

            if (process.StartInfo.RedirectStandardOutput)
                ret = process.StandardOutput.ReadToEnd();

            if (options.WaitForProcessExit)
                process.WaitForExit();

            return ret;
        }
    }
}
