package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Brand{
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("brandName")
    @Expose
    public String BrandName;
}
