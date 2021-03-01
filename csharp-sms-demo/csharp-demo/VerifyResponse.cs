using System;

namespace csharp_demo
{
  public class VerifyResponse
  {
    public int code { get; set; }
    public string msg { get; set; }
    public Data data { get; set; }

    public override string ToString()
    {
      return "code=" + code + ", "
            + "msg=" + msg + ", "
            + "data=" + data;
    }

    public class Data
    {
      public bool match { get; set; }
      public int reasonType { get; set; }

      public override string ToString()
      {
        return "match=" + match + ", "
              + "reasonType=" + reasonType;
      }
    }
  }
}