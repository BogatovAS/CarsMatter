package com.andrey.carsmatter.http;

import java.io.IOException;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class HttpClient {

    private OkHttpClient httpClient = new OkHttpClient();

    public String postHttpRequest(String url, String requestJson) {
        final MediaType mediaType = MediaType.parse("application/json");

        Request request = new Request.Builder()
                .url(url)
                .post(RequestBody.create(mediaType, requestJson))
                .build();
        Response response;
        try {
            response = httpClient.newCall(request).execute();
            if (response.isSuccessful()) {
                return response.body().string();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    public String putHttpRequest(String url, String requestJson) {
        final MediaType mediaType = MediaType.parse("application/json");

        Request request = new Request.Builder()
                .url(url)
                .put(RequestBody.create(mediaType, requestJson))
                .build();
        Response response;
        try {
            response = httpClient.newCall(request).execute();
            if (response.isSuccessful()) {
                return response.body().string();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    public String deleteHttpRequest(String url) {
        final MediaType mediaType = MediaType.parse("application/json");

        Request request = new Request.Builder()
                .url(url)
                .delete(RequestBody.create(mediaType, ""))
                .build();
        Response response;
        try {
            response = httpClient.newCall(request).execute();
            if (response.isSuccessful()) {
                return response.body().string();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    public String getHttpResponse(String url) {
        Request request = new Request.Builder()
            .url(url)
            .build();
        Response response;
        try {
            response = httpClient.newCall(request).execute();
            if (response.isSuccessful()) {
                return response.body().string();
            }
        }catch (Exception e){
            e.printStackTrace();
        }
        return null;
    }
}