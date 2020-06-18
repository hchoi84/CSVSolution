using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccessLibrary
{
  public class TextFileDataAccess
  {
    public List<ContactModel> ReadAllRecords(string textFile)
    {
      if (File.Exists(textFile) == false)
      {
        return new List<ContactModel>();
      }

      var lines = File.ReadAllLines(textFile);
      List<ContactModel> output = new List<ContactModel>();

      foreach (var line in lines)
      {
        ContactModel c = new ContactModel();
        var vals = line.Split(',');

        if (vals.Length < 4)
        {
          throw new Exception($"Invalid row of data: { line }");
        }

        c.FirstName = vals[0];
        c.LastName = vals[1];
        c.EmailAddresses = vals[2].Split(';').ToList();
        c.PhoneNumbers = vals[3].Split(';').ToList();

        output.Add(c);
      }

      return output;
    }
  }
}
