using System.IO;

namespace CourseProject.FileWriter
{
    public class FileWriter
    {
        public static void WriteData(string filepath, byte[] data)
        {
            using var stream = new StreamWriter(filepath, false);
            stream.BaseStream.Write(data, 0, data.Length);
        }

        public static string? GetThreeFishEncryptedFilepath(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }

            var fileinfo = new FileInfo(filepath);
            var filename = fileinfo.Name + ".tfish";
            return fileinfo.DirectoryName + "\\" + filename;
        }

        public static string? GetThreeFishDecryptedFilepath(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }

            var file = Path.GetFileNameWithoutExtension(filepath);
            var fileinfo = new FileInfo(filepath);
            return fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(file) + "_tfish_decrypted" + file[file.LastIndexOf('.')..];
        }

        public static string? GetSM4EncryptedFilepath(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }

            var fileinfo = new FileInfo(filepath);
            var filename = fileinfo.Name + ".sm4";
            return fileinfo.DirectoryName + "\\" + filename;
        }

        public static string? GetSM4DecryptedFilepath(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }

            var file = Path.GetFileNameWithoutExtension(filepath);
            var fileinfo = new FileInfo(filepath);
            return fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(file) + "_sm4_decrypted" + file[file.LastIndexOf('.')..];
        }
    }
}
