package com.andrey.carsmatter.services;

import android.content.Context;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.http.OkHttpHandler;
import com.andrey.carsmatter.models.Brand;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.RefillNote;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;


public class CarsRepository {

    String apiUrl;

    OkHttpHandler httpHandler = new OkHttpHandler();

    public CarsRepository(Context context){
        this.apiUrl = context.getResources().getString(R.string.api_base_path);
    }

    public ArrayList<RefillNote> GetAllRefillNotes() {
        return new ArrayList<RefillNote>();
    }

    public ArrayList<ConsumablesNote> GetAllConsumablesNotes() {
        return new ArrayList<ConsumablesNote>();
    }

    public boolean DeleteRefillNote(RefillNote refillNote) {
        return true;
    }

    public boolean AddRefillNote(RefillNote refillNote) {
        return true;
    }

    public boolean AddConsumablesNote(ConsumablesNote consumablesNote) {
        return true;
    }

    public boolean DeleteConsumablesNote(ConsumablesNote consumablesNote) {
        return true;
    }

    public ArrayList<Brand> GetAllBrands(){
        String url = apiUrl + "/brands";
        try {
            String responseString = this.httpHandler.execute(url).get();
            ArrayList<Brand> brands = new Gson().fromJson(responseString,  new TypeToken<ArrayList<Brand>>(){}.getType());
            return brands;
        } catch (InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }
        return null;
    }

    public ArrayList<BrandModel> GetModelsForBrand(String brandHttpPath){
        String url = apiUrl + "/brands/models?brandHttpPath=" + brandHttpPath;
        try {
            String responseString = this.httpHandler.execute(url).get();
            ArrayList<BrandModel> brandModels = new Gson().fromJson(responseString,  new TypeToken<ArrayList<BrandModel>>(){}.getType());
            return brandModels;
        } catch (InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }
        return null;
    }
}
