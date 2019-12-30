package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class BrandModel {
    @SerializedName("modelName")
    @Expose
    public String ModelName;

    @SerializedName("httpPath")
    @Expose
    public String HttpPath;
}
