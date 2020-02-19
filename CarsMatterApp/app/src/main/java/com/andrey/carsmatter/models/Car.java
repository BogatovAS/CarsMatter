package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Car {
    @SerializedName("id")
    @Expose
    public String Id;

    @SerializedName("carName")
    @Expose
    public String CarName;

    @SerializedName("lowPrice")
    @Expose
    public float LowPrice;

    @SerializedName("highPrice")
    @Expose
    public float HighPrice;

    @SerializedName("manufactureStartDate")
    @Expose
    public String ManufactureStartDate;

    @SerializedName("manufactureEndDate")
    @Expose
    public String ManufactureEndDate;

    @SerializedName("bodyType")
    @Expose
    public String BodyType;

    @SerializedName("avitoUri")
    @Expose
    public String AvitoUri;

    @SerializedName("base64CarImage")
    @Expose
    public String Base64CarImage;
}
