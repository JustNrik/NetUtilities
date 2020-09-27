using System.ComponentModel;

namespace System.Diagnostics
{
    /// <summary>
    ///     Utility class for <see cref="Process"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class ProcessExtensions
    {
        /// <summary>
        ///     Extension for a Shell() function that allows overloading of the working directory variable.
        ///     It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="process">
        ///      The <see cref="Process" /> component for which is used to execute the target process.
        ///  </param>
        /// <param name="fileName">
        ///     The process file name to execute.
        /// </param>
        /// <param name="arguments">
        ///     The commands to pass to the process file to execute.
        /// </param>
        /// <param name="redirectStandardOutput">
        ///     Indicates if the standard output should be redirected on the target process.
        /// </param>
        /// <param name="redirectStandardError">
        ///     Indicates if the standard error should be redirected on the target process.
        /// </param>
        /// <param name="useShellExecute">
        ///     Indicates if shell execute should be used instead.
        /// </param>
        /// <param name="createNoWindow">
        ///     Indicates if a no window should be created for the process.
        /// </param>
        /// <param name="windowStyle">
        ///     The window style for the target process.
        /// </param>
        /// <param name="workingDirectory">
        ///     The working directory for the target process.
        /// </param>
        /// <param name="options"> 
        ///     The options for which to also include for the <see cref="Process"/> component.
        /// </param>
        /// <returns>
        ///     <see cref="string.Empty"/>, process stdout data or process stderr data.
        /// </returns>
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
        ///     Extension for a Shell() function that allows overloading of the working directory variable.
        ///     It must be a <see cref="string"/> but can be variables that returns strings.
        /// </summary>
        /// <param name="process">
        ///      The <see cref="Process" /> component for which is used to execute the target process.
        ///  </param>
        /// <param name="options"> 
        ///     The options for which to also include for the <see cref="Process"/> component.
        /// </param>
        /// <returns>
        ///     <see cref="string.Empty"/>, process stdout data or process stderr data.
        /// </returns>
        public static string Shell(this Process process, ProcessOptions options)
        {
            var ret = string.Empty;

            process.Start();
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
