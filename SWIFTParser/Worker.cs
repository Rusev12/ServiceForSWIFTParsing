
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerServiceApp1.Services;

namespace WorkerServiceApp1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DbHelper _dbHelper;
        private readonly FileService _fileService;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _dbHelper = new DbHelper();
            this._fileService = new FileService();  
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                ConcurrentBag<string> AllFilesText = new ConcurrentBag<string>();
                AllFilesText = this._fileService.GetAllFileText(AllFilesText);

                this._dbHelper.ParseAllFiles(AllFilesText);

                await Task.Delay(5 * 10000, stoppingToken);
            }
        }

       
    }
}
