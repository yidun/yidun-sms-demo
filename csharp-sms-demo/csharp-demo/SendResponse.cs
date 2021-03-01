using System;

namespace csharp_demo
{
  public class SendResponse
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
      public int result { get; set; }
      public string requestId { get; set; }

      public override string ToString()
      {
        return "result=" + result + ", "
                + "requestId=" + requestId;
      }
    }
  }
}
