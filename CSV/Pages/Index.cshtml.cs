using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ZendeskTalkCSVLibrary;
using System.Collections.Generic;
using System.Linq;

namespace CSV.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _config;
    private string _fileLoc;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
      _logger = logger;
      _config = config;
      _fileLoc = _config.GetValue<string>("TextFile");
    }

    CSVDataAccess cSVDataAccess = new CSVDataAccess();

    public List<CallModel> CallHistories { get; set; }

    public List<CallSummaryModel> CallSummaries { get; set; }

    public void OnGet()
    {
      CallHistories = cSVDataAccess.ReadFile(_fileLoc);
      CallSummaries = cSVDataAccess.SummarizeCallHistory(CallHistories);
    }
  }
}
