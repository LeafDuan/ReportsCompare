using System.IO;
using System.IO.Compression;

namespace ReportsCompare.Web.Helper
{
    public static class ZipHelper
    {
        public static void Extract(string filename,string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }

            ZipFile.ExtractToDirectory(filename, path);
        }
    }
}
