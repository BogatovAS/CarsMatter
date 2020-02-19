package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UserModel {

    @SerializedName("username")
    @Expose
    public String Username;

    @SerializedName("password")
    @Expose
    public String Password;
}
