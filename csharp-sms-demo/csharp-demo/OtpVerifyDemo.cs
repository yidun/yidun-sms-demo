using System;
using System.Collections.Generic;

namespace csharp_demo
{
  /// <summary>
  /// 验证码发送与校验示例。
  /// 
  /// 场景示例：
  /// 第一步：你的用户请求你发出验证码短信。你会为此次用户请求生成一个唯一标识，假设为 operationId。
  /// 第二步：你通过易盾的短信发送接口，向你的用户发送验证码短信（指明让易盾帮你生成验证码），并记住易盾返回的 requestId。
  ///   （注：你可以建立 operationId 和 requestId 的映射关系。）
  /// 第三步：你的用户收到验证码短信后，填写验证码并提交到你的业务系统。
  /// 第四步：你将用户提交的验证码和之前易盾返回的 requestId 提交给易盾的验证码校验接口进行校验。易盾会返回校验结果。
  ///   （注：此处的 requestId 可以是你根据之前创建的 operationId 和 requestId 映射关系找到的。）
  /// </summary>
  public class OtpVerifyDemo
  {
    private static readonly string URI_SEND_SMS = "https://sms.dun.163.com/v2/sendsms";
    private static readonly string URI_VERIFY_OTP = "https://sms.dun.163.com/v2/verifysms";

    // SECRET_ID 和 SECRET_KEY 是产品密钥。可以登录易盾官网找到自己的凭证信息。请妥善保管，避免泄露。
    private static string SECRET_ID = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
    private static string SECRET_KEY = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

    /// <summary>
    /// 示例：发送验证码短信 并 校验用户的回填的验证码是否正确
    /// </summary>
    public static void SendAndVerifyOtp()
    {
      // 发送验证码短信：指明由易盾生成验证码
      var sendResponse = SendOtp();
      var requestId = sendResponse.data.requestId;

      // 过了一段时间（很短），你收到了用户回填的验证码
      // code 就是你的用户填写的验证码。你的业务系统收到此验证码后，就可以通过下述操作验证它是否正确。
      var code = "123456";

      // 验证验证码是否正确
      var verifyResponse = VerifyOtp(requestId, code);

      if (verifyResponse.data.match)
      {
        Console.WriteLine("你的用户填写了正确的验证码。");
      }
      else
      {
        if (verifyResponse.data.reasonType == 2)
        {
          Console.WriteLine("你的用户填写的验证码是错误的。");
        }
        else
        {
          Console.WriteLine("请求ID过期或不存在。");
        }
      }
    }

    /// <summary>
    /// 发送验证码短信：指明由易盾生成验证码
    /// </summary>
    private static SendResponse SendOtp()
    {
      // 这是你的 国内验证码短信 业务的ID。可以登录易盾官网查看此业务ID。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
      // 这是你事先创建好的模板，且已通过审核。
      var templateId = "xxxxx";
      // 这是收信方号码。如，134开头的号码一般是中国移动的号码。
      var to = "xxxxxxxxxxx";

      // 此处假设目标模板内容里只有验证码一个变量，所以没有其它变量需要指定
      var variables = new Dictionary<string, string>();

      // 发国内短信时，不指定 Country Calling Code
      var paramDict = CreateSendParam(businessId, templateId, variables, to, null);

      var response = RequestUtils.PostForEntity<SendResponse>(URI_SEND_SMS, paramDict);

      Console.WriteLine("response: " + response);

      return response;
    }

    /// <summary>
    /// 验证用户回填的验证码
    /// </summary>
    /// <param name="requestId">之前发送验证码短信时，易盾返回的请求ID</param>
    /// <param name="code">用户回填的验证码</param>
    private static VerifyResponse VerifyOtp(string requestId, string code)
    {
      // 业务ID。与前述发短信时所用的业务ID相同。
      var businessId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

      var paramDict = CreateVerifyParam(businessId, requestId, code);

      var response = RequestUtils.PostForEntity<VerifyResponse>(URI_VERIFY_OTP, paramDict);

      Console.WriteLine("response: " + response);

      return response;
    }

    /// <summary>
    /// 构建发送验证码短信的请求参数：指明由易盾生成验证码
    /// </summary>
    private static Dictionary<String, String> CreateSendParam(
            String businessId, String templateId, IDictionary<string, string> variables, String to, String countryCallingCode)
    {
      var paramDict = new Dictionary<string, string>
      {
        ["nonce"] = ParamUtils.CreateNonce(),
        ["timestamp"] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
        ["version"] = "v2",

        ["secretId"] = SECRET_ID,
        ["businessId"] = businessId,

        ["templateId"] = templateId,
        ["mobile"] = to,

        ["paramType"] = "json",
        ["params"] = ParamUtils.SerializeVariables(variables),

        // 指明由易盾生成验证码：
        // codeName 表示目标模板内容中，验证码的占位符变量名。如，模板内容为 “您的验证码为${code}，5分钟内有效，请勿泄露。”，则 codeName 的值应为 code
        ["codeName"] = "code",
        // codeLen 表示验证码的数字个数
        ["codeLen"] = "6",
        // codeValidSec 表示验证码的有效期。单位：秒
        ["codeValidSec"] = "300"
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

    /// <summary>
    /// 构建验证码校验请求的参数
    /// </summary>
    private static Dictionary<string, string> CreateVerifyParam(string businessId, string requestId, string code)
    {
      var paramDict = new Dictionary<string, string>()
      {
        ["nonce"] = ParamUtils.CreateNonce(),
        ["timestamp"] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
        ["version"] = "v2",

        ["secretId"] = SECRET_ID,
        ["businessId"] = businessId,

        ["requestId"] = requestId,
        ["code"] = code
      };

      // 在最后一步生成此次请求的签名
      paramDict["signature"] = ParamUtils.GenSignature(SECRET_KEY, paramDict);

      return paramDict;
    }
  }
}