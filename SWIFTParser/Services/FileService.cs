using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace WorkerServiceApp1.Services
{
    public class FileService
    {
        public ConcurrentBag<string> GetAllFileText(ConcurrentBag<string> allFilesText)
        {
            string path = @"C:\SWIFTMessages\";
            DirectoryInfo di = new DirectoryInfo(path);

            Parallel.ForEach(di.GetFiles("*.txt", SearchOption.TopDirectoryOnly), file => {

                allFilesText.Add(File.ReadAllText(file.FullName));

            });


           return allFilesText;
        }
    }
}
