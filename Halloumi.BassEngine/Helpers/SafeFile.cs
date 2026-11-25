using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    /// <summary>
    ///     Saves files so that a crash or kill part-way through never leaves a truncated or empty file.
    ///     The new content goes to a temporary file beside the target, is flushed to disk, and then
    ///     replaces the target in one step. Until that step the old file is untouched.
    /// </summary>
    public static class SafeFile
    {
        /// <summary>
        ///     Serialises to XML. The output is the same as Halloumi.Common's SerializationHelper.ToXmlFile.
        /// </summary>
        public static void ToXmlFile<T>(T entity, string filename)
        {
            Write(filename, stream => new XmlSerializer(typeof(T)).Serialize(stream, entity));
        }

        public static void WriteAllText(string filename, string contents, Encoding encoding)
        {
            Write(filename, stream =>
            {
                var bytes = encoding.GetPreamble();
                stream.Write(bytes, 0, bytes.Length);
                bytes = encoding.GetBytes(contents);
                stream.Write(bytes, 0, bytes.Length);
            });
        }

        // one lock per file, so two saves of the same file (a background save and the exit save) take turns
        private static readonly ConcurrentDictionary<string, object> FileLocks =
            new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private static void Write(string filename, Action<FileStream> write)
        {
            var fullPath = Path.GetFullPath(filename);
            lock (FileLocks.GetOrAdd(fullPath, _ => new object()))
            {
                WriteAndReplace(fullPath, write);
            }
        }

        private static void WriteAndReplace(string fullPath, Action<FileStream> write)
        {
            var tempPath = fullPath + ".saving";

            try
            {
                using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    write(stream);
                    stream.Flush(true);
                }

                if (File.Exists(fullPath))
                    File.Replace(tempPath, fullPath, null, true);
                else
                    File.Move(tempPath, fullPath);
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); }
                    catch { /* a stray .saving file is harmless; the next save overwrites it */ }
                }
            }
        }
    }
}
