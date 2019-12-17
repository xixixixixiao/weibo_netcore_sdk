namespace SinaWeibo.Core
{
    /// <summary>
    /// The content of memory file.
    /// </summary>
    public class MemoryFileContent
    {
        /// <summary>
        /// File's name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The bytes of the file's content.
        /// </summary>
        public byte[] Content { get; set; }
    }
}
