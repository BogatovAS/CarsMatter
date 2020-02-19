package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.Date;

public class ConsumablesNote {
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("date")
    @Expose
    public Date Date;

    @SerializedName("kindOfService")
    @Expose
    public String KindOfService;

    @SerializedName("price")
    @Expose
    public float Price;

    @SerializedName("odo")
    @Expose
    public int Odo;

    @SerializedName("location")
    @Expose
    public String Location;

    @SerializedName("notes")
    @Expose
    public String Notes;
}
