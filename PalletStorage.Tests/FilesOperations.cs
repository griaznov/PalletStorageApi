using static System.IO.Path;

namespace PalletStorage.Tests
{
    public static class FilesOperations
    {
        public static string GenerateFileName(string extension = "")
        {
            return string.Concat(GetRandomFileName().Replace(".", ""),
                (!string.IsNullOrEmpty(extension)) ? (extension.StartsWith(".") ? extension : string.Concat(".", extension)) : "");
        }

        public static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

    }
}
