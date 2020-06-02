package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class KindOfService {
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("name")
    @Expose
    public String Name;
}
