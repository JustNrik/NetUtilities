
namespace System.Diagnostics
{
    /// <summary>
    /// Extra options for the <see cref="Process" /> component.
    /// </summary>
    public sealed class ProcessOptions
    {
        /// <summary>
        /// Instucts the <see cref="Process" /> <keyword cref="object" /> to wait until the executed process terminates.
        /// </summary>
        public bool WaitForProcessExit { get; set; }

        /// <summary>
        /// Gets if the target process is executing (has not been actually started yet), or is being executed (started). False if it was started.
        /// </summary>
        public bool Executing { get; internal set; } = true;
    }
}
