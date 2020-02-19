package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class BrandModel {
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("modelName")
    @Expose
    public String ModelName;
}
