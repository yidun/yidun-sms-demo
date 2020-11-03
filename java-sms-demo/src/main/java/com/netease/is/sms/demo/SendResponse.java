/*
 * @(#) Response.java 2020-11-03
 *
 * Copyright 2020 NetEase.com, Inc. All rights reserved.
 */

package com.netease.is.sms.demo;

/**
 * 短信发送操作的响应信息
 * 
 * @author huangchao04
 * @version 2020-11-03
 */
public class SendResponse {
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
        private int result;
        private String requestId;

        public int getResult() {
            return result;
        }

        public void setResult(int result) {
            this.result = result;
        }

        public String getRequestId() {
            return requestId;
        }

        public void setRequestId(String requestId) {
            this.requestId = requestId;
        }

        @Override
        public String toString() {
            return "result=" + result + ", "
                    + "requestId=" + requestId;
        }
    }
}
