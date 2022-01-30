using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorkerServiceApp1.Models.DBModels;

namespace WorkerServiceApp1.Services
{
    public class DbHelper
    {
        private SWIFTParserDBContext dbContext;
        private Regex regex = new Regex(@"(?'Msg20'(?<=20:).*)|(?'Msg21'(?<=21:).*)|(?'Msg79'(?<=79:).*)");

        public DbHelper()
        {
            this.dbContext = new SWIFTParserDBContext(GetAllOptions());
        }

        private DbContextOptions<SWIFTParserDBContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<SWIFTParserDBContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionBuilder.Options;
        }

        public async void ParseAllFiles(ConcurrentBag<string> allFilesText)
        {
            foreach (var file in allFilesText)
            {

                var matches = regex.Matches(file);

                if (matches.Count > 0)
                {
                    var msg20 = matches[0];
                    await AddNewMessage20(msg20);

                    var msg21 = matches[1];
                    await AddNewMessage21(msg20);

                    var msg79 = matches[2];
                    await AddNewMessage79(msg79);

                }
               
            }
        }

        public async Task AddNewMessage20(Match match)
        {
            if (string.IsNullOrEmpty(match.Value)) return;
            
            using (dbContext = new SWIFTParserDBContext(GetAllOptions()))
            {
                dbContext.BodyMessages20.Add(new BodyMessage20
                {
                    MessageWithCode20 = match.Value
                }); ;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddNewMessage21(Match match)
        {
            if (string.IsNullOrEmpty(match.Value)) return;

            using (dbContext = new SWIFTParserDBContext(GetAllOptions()))
            {
                dbContext.BodyMessages21.Add(new BodyMessage21
                {
                    MessageWithCode21 = match.Value
                }); ;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddNewMessage79(Match match)
        {
            if (string.IsNullOrEmpty(match.Value)) return;

            using (dbContext = new SWIFTParserDBContext(GetAllOptions()))
            {
                dbContext.BodyMessages79.Add(new BodyMessage79
                {
                    MessageWithCode79 = match.Value
                }); ;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
