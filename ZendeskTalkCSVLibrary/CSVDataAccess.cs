using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ZendeskTalkCSVLibrary
{
  public class CSVDataAccess
  {
    private List<string> _headerNames = new List<string>()
    {
      "Date/Time", "Agent", "Call Status", "Wait Time", "Minutes", "Charge (USD)"
    };
    private List<int> _headerIndexes = new List<int>();

    public List<CallModel> ReadFile(string filePath)
    {
      List<CallModel> callHistory = new List<CallModel>();
      string[] lines = File.ReadAllLines(filePath);

      TextFieldParser parser = new TextFieldParser(new StringReader(lines[0]));
      parser.SetDelimiters(",");
      List<string> headers = parser.ReadFields().ToList();

      foreach (string header in _headerNames)
      {
        int index = headers.IndexOf(header);
        _headerIndexes.Add(index);
      }

      foreach (string line in lines.Skip(1))
      {
        parser = new TextFieldParser(new StringReader(line));
        parser.SetDelimiters(",");
        string[] rawFields = parser.ReadFields();

        CallModel call = new CallModel();

        call.DateTime = DateTime.Parse(rawFields[_headerIndexes[0]]).Date;
        call.Category = string.IsNullOrWhiteSpace(rawFields[_headerIndexes[1]]) ? rawFields[_headerIndexes[2]] : rawFields[_headerIndexes[1]];
        call.WaitMin = Int32.Parse(rawFields[_headerIndexes[3]]) / 60;
        call.TalkMin = Int32.Parse(rawFields[_headerIndexes[4]]);
        call.Charge = decimal.Parse(rawFields[_headerIndexes[5]]);


        callHistory.Add(call);
      }

      return callHistory;
    }

    public List<CallSummaryModel> SummarizeCallHistory(List<CallModel> model)
    {
      List<IGrouping<DateTime, CallModel>> groupedByDate = model.GroupBy(c => c.DateTime).ToList();

      List<string> categories = model.Select(m => m.Category).Distinct().ToList();

      List<CallSummaryModel> callSummaries = new List<CallSummaryModel>();

      foreach (var item in groupedByDate)
      {
        var groupedByCategory = item.GroupBy(i => i.Category).ToList();

        List<CallSummaryModel> callSummaryByCategory = new List<CallSummaryModel>();

        foreach (var g in groupedByCategory)
        {
          CallSummaryModel callSummary = new CallSummaryModel
          {
            Date = item.Key,
            Category = g.Key,
            Count = g.Count(),
            AvgWaitMin = g.Average(i => i.WaitMin).ToString("F2"),
            AvgTalkMin = g.Average(i => i.TalkMin).ToString("F2"),
            EndOfDate = false
          };

          callSummaryByCategory.Add(callSummary);
        }

        CallSummaryModel dateSummary = new CallSummaryModel
        {
          Date = item.Key,
          Category = "Total",
          Count = item.Count(),
          AvgWaitMin = "",
          AvgTalkMin = "",
          EndOfDate = true
        };
        callSummaryByCategory.OrderBy(c => c.Category).ToList();

        callSummaryByCategory.Add(dateSummary);

        callSummaries.AddRange(callSummaryByCategory);
      }

      return callSummaries;
    }
  }
}
