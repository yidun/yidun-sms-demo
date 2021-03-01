using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using Newtonsoft.Json;

namespace csharp_demo
{
  public static class ParamUtils
  {
    public static string GenSignature(string secretKey, IEnumerable<KeyValuePair<string, string>> paramDict)
    {
      var sb = new StringBuilder();
      foreach (var param in paramDict.OrderBy(p => p.Key, StringComparer.Ordinal))
      {
        var name = param.Key;
        var value = param.Value ?? String.Empty;
        sb.Append(name).Append(value);
      }

      sb.Append(secretKey);

      using (MD5 md5 = MD5.Create())
      {
        var md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
        return String.Concat(md5Bytes.Select(c => c.ToString("x2")));
      }
    }

    public static string CreateNonce()
    {
      return Guid.NewGuid().ToString().Replace("-", String.Empty);
    }

    public static string SerializeVariables(IDictionary<string, string> variables)
    {
      if (variables == null || variables.Count == 0)
      {
        return "{}";
      }

      return JsonConvert.SerializeObject(variables);
    }
  }
}