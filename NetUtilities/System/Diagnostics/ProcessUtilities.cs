namespace System.Diagnostics
{
    public static partial class ProcessUtilities
    {
        public static bool WaitForProcessExit { get; set; } = false;

        /// <summary>
        /// Extension for a Shell() Function that Allows Overloading of the Working directory Variable.
        /// It must be a String but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The <see cref="Process"> component for which is used to execute the target process.</param>
        /// <param name="fileName">Process file name to execute.</param>
        /// <param name="arguments">Commands to pass to the process file to execute.</param>
        /// <param name="redirectStandardOutput">redirects stdout of the target process.</param>
        /// <param name="redirectStandardError">redirects stderr of the target process.</param>
        /// <param name="useShellExecute">uses shell execute instead.</param>
        /// <param name="createNoWindow">Creates no new window for the process.</param>
        /// <param name="windowStyle">Window style for the target process.</param>
        /// <param name="workingDirectory">Working directory for the target process.</param>
        /// <param name="waitForProcessExit">Waits for the target process to terminate.</param>
        /// <param name="executing">Tells the calling method that the process was executed if true, false to not notify.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process proc, string fileName, string arguments, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow, ProcessWindowStyle windowStyle, string workingDirectory, bool waitForProcessExit, ref bool executing)
        {
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.RedirectStandardOutput = redirectStandardOutput;
            proc.StartInfo.RedirectStandardError = redirectStandardError;
            proc.StartInfo.UseShellExecute = useShellExecute;
            proc.StartInfo.CreateNoWindow = createNoWindow;
            proc.StartInfo.WindowStyle = windowStyle;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            WaitForProcessExit = waitForProcessExit;
            return proc.Shell(ref executing);
        }

        /// <summary>
        /// Extension for a Shell() Function that Allows Overloading of the Working directory Variable.
        /// It must be a String but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The <see cref="Process"> component for which is used to execute the target process.</param>
        /// <param name="executing">Tells the calling method that the process was executed if true, false to not notify.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process proc, ref bool executing)
        {
            var ret = string.Empty;
            proc.Start();

            if (executing)
            {
                executing = false;
            }

            if (proc.StartInfo.RedirectStandardError)
            {
                ret = proc.StandardError.ReadToEnd();
            }

            if (proc.StartInfo.RedirectStandardOutput)
            {
                ret = proc.StandardOutput.ReadToEnd();
            }

            if (WaitForProcessExit)
            {
                proc.WaitForExit();
            }

            return ret;
        }
    }
}
