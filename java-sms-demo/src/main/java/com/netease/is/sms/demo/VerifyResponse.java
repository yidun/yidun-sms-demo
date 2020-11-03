/*
 * @(#) VerifyResponse.java 2020-11-03
 *
 * Copyright 2020 NetEase.com, Inc. All rights reserved.
 */

package com.netease.is.sms.demo;

/**
 * 验证码校验操作的响应信息
 * 
 * @author huangchao04
 * @version 2020-11-03
 */
public class VerifyResponse {
    private int code;
    private String msg;
    private Data data;

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getMsg() {
        return msg;
    }

    public void setMsg(String msg) {
        this.msg = msg;
    }

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "code=" + code + ", "
                + "msg=" + msg + ", "
                + "data=" + data;
    }

    public static class Data {
        private boolean match;
        private int reasonType;

        public boolean isMatch() {
            return match;
        }

        public void setMatch(boolean match) {
            this.match = match;
        }

        public int getReasonType() {
            return reasonType;
        }

        public void setReasonType(int reasonType) {
            this.reasonType = reasonType;
        }

        @Override
        public String toString() {
            return "match=" + match + ", "
                    + "reasonType=" + reasonType;
        }
    }
}
