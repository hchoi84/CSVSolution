using System;
using System.Collections.Generic;
using System.Text;

namespace ZendeskTalkCSVLibrary
{
  public class CallModel
  {
    public DateTime DateTime { get; set; }
    public string Category { get; set; }
    public int WaitMin { get; set; }
    public int TalkMin { get; set; }
    public decimal Charge { get; set; }
  }
}
