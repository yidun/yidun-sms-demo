/*
 * @(#) Main.java 2020-11-03
 *
 * Copyright 2020 NetEase.com, Inc. All rights reserved.
 */

package com.netease.is.sms.demo;

import com.netease.is.sms.demo.utils.ParamUtils;
import org.apache.commons.lang3.StringUtils;

import java.util.HashMap;
import java.util.Map;

/**
 * @author huangchao04
 * @version 2020-11-03
 */
public class Main {
    public static void main(String[] args) {
        // 请根据你在易盾官网的实际业务信息，先调整相关示例中的配置，再执行示例。

        // SendDemo.sendOtp();
        // SendDemo.sendArn();
        // SendDemo.sendMkt();
        // SendDemo.sendInternationalOtp();

        // OtpVerifyDemo.sendAndVerifyOtp();

        System.out.println("示例执行完毕。");
    }

    private static void testSignature() {
        String secretKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        Map<String, String> params = new HashMap<>();
        params.put("nonce", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        params.put("timestamp", "1598889600000");
        params.put("version", "v2");
        params.put("secretId", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        params.put("businessId", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        params.put("templateId", "xxxxx");
        params.put("mobile", "xxxxxxxxxxx");
        params.put("paramType", "json");
        params.put("params", "{\"code\":\"123456\"}");

        String expected = "22d28d2ce543e8c97bd12f078a30784b";

        String sign = ParamUtils.genSignature(secretKey, params);

        assert StringUtils.equals(expected, sign);
    }
}
