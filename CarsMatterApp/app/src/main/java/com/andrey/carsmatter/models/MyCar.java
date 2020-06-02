package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class MyCar {
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("name")
    @Expose
    public String Name;

    @SerializedName("brand")
    @Expose
    public String Brand;

    @SerializedName("model")
    @Expose
    public String Model;

    @SerializedName("year")
    @Expose
    public int Year;
}
