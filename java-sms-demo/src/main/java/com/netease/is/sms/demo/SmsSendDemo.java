/*
 * @(#) SmsSendDemo.java 2018-11-29
 *
 * Copyright 2018 NetEase.com, Inc. All rights reserved.
 */

package com.netease.is.sms.demo;

import com.netease.is.sms.demo.utils.HttpClient4Utils;
import com.netease.is.sms.demo.utils.SignatureUtils;
import org.apache.http.Consts;
import org.apache.http.client.HttpClient;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

/**
 * @author haoshijing
 * @version 2018-11-29
 */
public class SmsSendDemo {
    /**
     * 业务ID，易盾根据产品业务特点分配
     */
    private final static String BUSINESSID = "your_business_id";
    /**
     * 产品密钥ID，产品标识
     */
    private final static String SECRETID = "your_secret_id";
    /**
     * 产品私有密钥，服务端生成签名信息使用，请严格保管，避免泄露
     */
    private final static String SECRETKEY = "your_secret_key";

    /**
     * 接口地址
     */
    private final static String API_URL = "https://sms.dun.163yun.com/v2/sendsms";
    /**
     * 实例化HttpClient，发送http请求使用，可根据需要自行调参
     */
    private static HttpClient httpClient = HttpClient4Utils.createHttpClient(100, 20, 10000, 2000, 2000);

    /**
     * @param args
     * @throws Exception
     */
    public static void main(String[] args) throws Exception {
        //此处用申请通过的模板id
        String templateId = "xxx";
        //模板参数对应,例如模板为您的验证码为${p1},请于${p2}时间登陆到我们的服务器
        String params = "p1=xx&p2=xx";
        String mobile = "xx";
        Map<String, String> datas = buildParam(mobile, templateId, params);
        String result = HttpClient4Utils.sendPost(httpClient, API_URL, datas, Consts.UTF_8);
        System.out.println("result = [" + result + "]");
    }

    private static Map<String, String> buildParam(String mobile, String templateId, String params) throws IOException {
        Map map = new HashMap<String, String>();
        map.put("businessId", BUSINESSID);
        map.put("timestamp", String.valueOf(System.currentTimeMillis()));
        map.put("version", "v2");
        map.put("needUp", "false");
        map.put("templateId", templateId);
        map.put("mobile", mobile);
        //国际短信
        map.put("internationalCode", "对应的国家编码");
        map.put("params", params);
        map.put("nonce", UUID.randomUUID().toString().replace("-", ""));
        map.put("secretId", SECRETID);
        String sign = SignatureUtils.genSignature(SECRETKEY, map);
        map.put("signature", sign);
        return map;
    }
}
