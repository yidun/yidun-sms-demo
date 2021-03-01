using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace csharp_demo
{
  class Program
  {
    static void Main(string[] args)
    {
      // 请根据你在易盾官网的实际业务信息，先调整相关示例中的配置，再执行示例。

      //SendDemo.SendOtp();
      //SendDemo.SendArn();
      //SendDemo.sendMkt();
      //SendDemo.SendInternationalOtp();

      //OtpVerifyDemo.SendAndVerifyOtp();

      Console.WriteLine("示例执行完毕。");
    }

    static void TestGenSignature()
    {
      var secretKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      var paramDict = new Dictionary<string, string>
      {
        ["nonce"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        ["timestamp"] = "1598889600000",
        ["version"] = "v2",
        ["secretId"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        ["businessId"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        ["templateId"] = "xxxxx",
        ["mobile"] = "xxxxxxxxxxx",
        ["paramType"] = "json",
        ["params"] = @"{""code"":""123456""}"
      };

      var expect = "22d28d2ce543e8c97bd12f078a30784b";

      var sign = ParamUtils.GenSignature(secretKey, paramDict);

      if (String.Equals(expect, sign))
      {
        Console.WriteLine("signature test: pass");
      }
      else
      {
        Console.WriteLine(sign);
        Console.WriteLine("signature test: fail");
      }
    }
  }
}
