/*
 * @(#) ParamUtils.java 2020-11-03
 *
 * Copyright 2020 NetEase.com, Inc. All rights reserved.
 */

package com.netease.is.sms.demo.utils;

import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.Map;
import java.util.UUID;

import org.apache.commons.codec.digest.DigestUtils;
import org.apache.commons.lang3.ObjectUtils;
import org.apache.commons.lang3.StringUtils;

import com.alibaba.fastjson.JSONObject;

/**
 * 辅助处理请参数
 * 
 * @author huangchao04
 * @version 2020-11-03
 */
public class ParamUtils {

    /**
     * 生成签名信息
     *
     * @param secretKey 产品私钥
     * @param params 接口请求参数名和参数值map，不包括signature参数名
     */
    public static String genSignature(String secretKey, Map<String, String> params) {
        // 1. 参数名按照ASCII码表升序排序
        String[] paramNames = params.keySet().toArray(new String[0]);
        Arrays.sort(paramNames);

        // 2. 按照排序拼接参数名与参数值
        StringBuilder sb = new StringBuilder();
        for (String name : paramNames) {
            String value = ObjectUtils.defaultIfNull(params.get(name), StringUtils.EMPTY);
            sb.append(name).append(value);
        }
        // 3. 将secretKey拼接到最后
        sb.append(secretKey);

        // 4. MD5是128位长度的摘要算法，用16进制表示，一个十六进制的字符能表示4个位，所以签名后的字符串长度固定为32个十六进制字符。
        return DigestUtils.md5Hex(sb.toString().getBytes(StandardCharsets.UTF_8));
    }

    /**
     * 生成随机码。用于防重复提交
     */
    public static String createNonce() {
        return UUID.randomUUID().toString().replace("-", StringUtils.EMPTY);
    }

    /**
     * 序列化短信内容的变量
     */
    public static String serializeVariables(Map<String, String> variables) {
        if (variables == null || variables.size() == 0) {
            return "{}";
        }

        JSONObject json = new JSONObject();
        variables.forEach(json::put);
        return json.toJSONString();
    }
}
