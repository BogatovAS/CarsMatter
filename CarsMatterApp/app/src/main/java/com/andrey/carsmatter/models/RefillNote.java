package com.andrey.carsmatter.models;

import java.util.Date;

public class RefillNote {
    public String Location;
    public float Petrol;
    public long Odo;
    public float Price;
    public Date Date;

    public RefillNote(String location, float petrol, long odo, float price, Date date) {
        this.Location = location;
        this.Petrol = petrol;
        this.Odo = odo;
        this.Price = price;
        this.Date = date;
    }

    public RefillNote(){

    }
}
