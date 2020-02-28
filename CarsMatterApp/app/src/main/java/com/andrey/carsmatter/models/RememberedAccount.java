package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class RememberedAccount {
    @SerializedName("username")
    @Expose
    public String Username;

    @SerializedName("password")
    @Expose
    public String Password;

    @SerializedName("date")
    @Expose
    public java.util.Date Date;
}
