package com.andrey.carsmatter.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ConsumablesNotesReport {
    @SerializedName("totalCost")
    @Expose
    public float TotalCost;

    @SerializedName("costPerDay")
    @Expose
    public float CostPerDay;

    @SerializedName("costPerKm")
    @Expose
    public float CostPerKm;
}
