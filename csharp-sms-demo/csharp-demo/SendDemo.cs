using System;
using System.Collections.Generic;

namespace csharp_demo
{
  /// <summary>
  /// 短信发送示例。
  /// 
  /// 发送不同类型的短信本质上是一样的。都是通过同一个接口发送，参数大同小异。
  /// 核心参数说明：
  /// · 业务ID：根据自己的业务需求选择匹配的业务。如，你需要发送一条国内验证码短信，则选择国内验证短信业务的ID。可以登录易盾官网查看已开通的业务。
  /// · 模板ID：事先创建好短信模板，且已通过审核。模板必须与所选业务匹配。你可以在创建模板的时候选择其所属业务。
  /// · 收信方号码：国内手机号通常是11位数字。国际号码则需要额外指定国际电话区号。
  /// 
  /// 扩展参数说明：
  /// · 模板变量：你可以在创建模板时，在其内容中添加占位符来表示变量。如，验证码短信模板中，可以添加 ${code}，并在发送短信时提供此变量的值。
  /// · 国际电话区号：发送国际短信时需要提供此参数（国内短信不提供）。如，美国是1，英国是44，法国是33，俄罗斯是79。注：不要将此区号作为收信方号码的前缀。
  /// </summary>
  public class SendDemo
  {
    private static readonly string URI_SEND_SMS = "https://sms.dun.163.com/v2/sendsms";

    // SECRET_ID 和 SECRET_KEY 是产品密钥。可以登录易盾官网找到自己的凭证信息。请妥善保管，避免泄露。
    private static readonly string SECRET_ID = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
    private static readonly string SECRET_KEY = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

    /// <summary>
    /// 示例：发送一条验证码短信
    /// </summary>
    public static void SendOtp()
    {
      // 这是你的 国内验证码短信 业务的ID。可以登录易盾官网查看此业务ID。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      // 这是你事先创建好的模板，且已通过审核。
      var templateId = "xxxxx";
      // 这是你自己的业务系统生成的验证码。如果你希望由易盾生成验证码，并通过验证码校验接口来验证，请参考 OptVerifyDemo。
      var code = "123456";
      // 这是收信方号码。如，134开头的号码一般是中国移动的号码。
      var to = "xxxxxxxxxxx";

      // 此处假设你的模板中只有验证码这一个变量，且变量名为 code。如，模板内容为 “您的验证码为${code}，5分钟内有效，请勿泄露。”
      var variables = new Dictionary<string, string>
      {
        ["code"] = code
      };

      var paramDict = CreateParam(businessId, templateId, variables, to);
      var response = Send(paramDict);

      Console.WriteLine("response: " + response);
    }

    /// <summary>
    /// 示例：发送一条通知短信
    /// </summary>
    public static void SendArn()
    {
      // 这是你的 国内通知类短信 业务的ID。可以登录易盾官网查看此业务ID。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      // 这是你事先创建好的模板，且已通过审核。
      var templateId = "xxxxx";
      // 这是收信方号码。如，134开头的号码一般是中国移动的号码。
      var to = "xxxxxxxxxxx";

      // 此处假设你的模板中只有orderId这一个变量。如，模板内容为 “您的订单已发货，订单号为 ${orderId}。”
      var variables = new Dictionary<string, string>
      {
        ["orderId"] = "1020304050"
      };

      var paramDict = CreateParam(businessId, templateId, variables, to);
      var response = Send(paramDict);

      Console.WriteLine("response: " + response);
    }

    /// <summary>
    /// 示例：发送一条营销短信
    /// </summary>
    public static void sendMkt()
    {
      // 这是你的 国内营销类短信 业务的ID。可以登录易盾官网查看此业务ID。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      // 这是你事先创建好的模板，且已通过审核。
      var templateId = "xxxxx";
      // 这是收信方号码。如，134开头的号码一般是中国移动的号码。
      var to = "xxxxxxxxxxx";

      // 此处假设你的模板没有变量。如，模板内容为 “即日起至12月底，每日登录APP签到，就有机会参与新年抽奖活动。”
      var variables = new Dictionary<string, string>();

      var paramDict = CreateParam(businessId, templateId, variables, to);
      var response = Send(paramDict);

      Console.WriteLine("response: " + response);
    }

    /// <summary>
    /// 示例：发送一条国际验证码短信
    /// </summary>
    public static void SendInternationalOtp()
    {
      // 这是你的 国际验证码短信 业务的ID。可以登录易盾官网查看此业务ID。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      // 这是你事先创建好的模板，且已通过审核。
      var templateId = "xxxxx";
      // 这是你自己的业务系统生成的验证码。如果你希望由易盾生成验证码，并通过验证码校验接口来验证，请参考 OptVerifyDemo。
      var code = "123456";
      // 这是收信方号码。不包含国际电话区号。
      var to = "xxxxxxxxxxx";
      // 这是收信方的国际电话区号。如，美国是1，英国是44，法国是33，俄罗斯是79。
      var countryCallingCode = "1";

      // 此处假设你的模板中只有验证码这一个变量，且变量名为 code。如，模板内容为 “Your verification code is ${code}, valid for 5 minutes.”
      var variables = new Dictionary<string, string>
      {
        ["code"] = code
      };

      var paramDict = CreateParamForInternational(businessId, templateId, variables, to, countryCallingCode);
      var response = Send(paramDict);

      Console.WriteLine("response: " + response);
    }

    private static SendResponse Send(IDictionary<string, string> paramDict)
    {
      return RequestUtils.PostForEntity<SendResponse>(URI_SEND_SMS, paramDict);
    }
    private static IDictionary<string, string> CreateParam(
        string businessId, string templateId, IDictionary<string, string> variables, string to)
    {
      return CreateSendParam(businessId, templateId, variables, to, null);
    }

    private static IDictionary<string, string> CreateParamForInternational(
        string businessId, string templateId, IDictionary<string, string> variables, string to, string countryCallingCode)
    {
      return CreateSendParam(businessId, templateId, variables, to, countryCallingCode);
    }

    private static IDictionary<string, string> CreateSendParam(
        string businessId, string templateId, IDictionary<string, string> variables, string to, string countryCallingCode)
    {
      var paramDict = new Dictionary<string, string>()
      {
        ["nonce"] = ParamUtils.CreateNonce(),
        ["timestamp"] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
        ["version"] = "v2",

        ["secretId"] = SECRET_ID,
        ["businessId"] = businessId,

        ["templateId"] = templateId,
        ["mobile"] = to,

        ["paramType"] = "json",
        ["params"] = ParamUtils.SerializeVariables(variables)
      };

      // 如果要发送国际短信，则需要指明国际电话区号。如果不是国际短信，则不要指定此参数
      if (!string.IsNullOrWhiteSpace(countryCallingCode))
      {
        paramDict["internationalCode"] = countryCallingCode;
      }


      // 在最后一步生成此次请求的签名
      paramDict["signature"] = ParamUtils.GenSignature(SECRET_KEY, paramDict);

      return paramDict;
    }
  }
}
