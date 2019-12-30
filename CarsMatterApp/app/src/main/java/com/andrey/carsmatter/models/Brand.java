package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Brand{
    @SerializedName("brandName")
    @Expose
    public String BrandName;

    @SerializedName("httpPath")
    @Expose
    public String HttpPath;
}
