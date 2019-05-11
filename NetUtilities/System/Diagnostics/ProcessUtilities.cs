namespace System.Diagnostics
{
    public static partial class ProcessUtilities
    {
        /// <summary>
        /// Extension for a Shell() function that allows overloading of the working directory variable.
        /// It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The <see cref="Process" /> component for which is used to execute the target process.</param>
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
        public static string Shell(this Process proc, string fileName, string arguments, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow, ProcessWindowStyle windowStyle, string workingDirectory, ProcessOptions options)
        {
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.RedirectStandardOutput = redirectStandardOutput;
            proc.StartInfo.RedirectStandardError = redirectStandardError;
            proc.StartInfo.UseShellExecute = useShellExecute;
            proc.StartInfo.CreateNoWindow = createNoWindow;
            proc.StartInfo.WindowStyle = windowStyle;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            return proc.Shell(options);
        }

        /// <summary>
        /// Extension for a Shell() function that allows overloading of the working directory variable.
        /// It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The <see cref="Process" /> component for which is used to execute the target process.</param>
        /// <param name="options">The options for which to also include for the <see cref="Process" /> component.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process proc, ProcessOptions options)
        {
            var ret = string.Empty;
            proc.Start();

            if (options.Executing)
            {
                options.Executing = false;
            }

            if (proc.StartInfo.RedirectStandardError)
            {
                ret = proc.StandardError.ReadToEnd();
            }

            if (proc.StartInfo.RedirectStandardOutput)
            {
                ret = proc.StandardOutput.ReadToEnd();
            }

            if (options.WaitForProcessExit)
            {
                proc.WaitForExit();
            }

            return ret;
        }
    }
}
