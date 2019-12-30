package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.Date;

public class RefillNote{
    @SerializedName("id")
    @Expose
    public int Id;

    @SerializedName("location")
    @Expose
    public String Location;

    @SerializedName("petrol")
    @Expose
    public float Petrol;

    @SerializedName("odo")
    @Expose
    public int Odo;

    @SerializedName("price")
    @Expose
    public float Price;

    @SerializedName("date")
    @Expose
    public Date Date;
}
