package com.andrey.carsmatter.services;

import android.content.Context;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.http.HttpClient;
import com.andrey.carsmatter.models.Brand;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.RefillNote;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;


public class CarsRepository {

    private String apiUrl;
    private HttpClient httpHandler;
    private Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();

    public CarsRepository(Context context){
        this.apiUrl = context.getResources().getString(R.string.api_base_path);
        this.httpHandler = new HttpClient();
    }

    public ArrayList<RefillNote> GetAllRefillNotes() {
        String url = this.apiUrl + "/refill_notes";

        String responseString = this.httpHandler.getHttpResponse(url);
        ArrayList<RefillNote> refillNotes = this.gson.fromJson(responseString,  new TypeToken<ArrayList<RefillNote>>(){}.getType());
        return refillNotes;
    }

    public ArrayList<ConsumablesNote> GetAllConsumablesNotes() {
        String url = this.apiUrl + "/consumables_notes";
        String responseString = this.httpHandler.getHttpResponse(url);
        ArrayList<ConsumablesNote> consumablesNotes = this.gson.fromJson(responseString,  new TypeToken<ArrayList<ConsumablesNote>>(){}.getType());
        return consumablesNotes;
    }

    public boolean AddRefillNote(RefillNote refillNote) {
        String url = this.apiUrl + "/refill_notes";
        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(refillNote));
        return Boolean.parseBoolean(responseString);
    }

    public boolean UpdateRefillNote(RefillNote refillNote) {
        String url = this.apiUrl + "/refill_notes";
        String responseString = this.httpHandler.putHttpRequest(url, this.gson.toJson(refillNote));
        return Boolean.parseBoolean(responseString);
    }

    public boolean DeleteRefillNote(int id) {
        String url = this.apiUrl + "/refill_notes/" + id;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public boolean AddConsumablesNote(ConsumablesNote consumablesNote) {
        String url = this.apiUrl + "/consumables_notes";
        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(consumablesNote));
        return Boolean.parseBoolean(responseString);
    }

    public boolean UpdateConsumablesNote(ConsumablesNote consumablesNote){
        String url = this.apiUrl + "/consumables_notes";
        String responseString = this.httpHandler.putHttpRequest(url, this.gson.toJson(consumablesNote));
        return Boolean.parseBoolean(responseString);
    }

    public boolean DeleteConsumablesNote(int id) {
        String url = this.apiUrl + "/consumables_notes/" + id;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public ArrayList<Brand> GetAllBrands(){
        String url = this.apiUrl + "/cars/brands";
        String responseString = this.httpHandler.getHttpResponse(url);
        ArrayList<Brand> brands = this.gson.fromJson(responseString,  new TypeToken<ArrayList<Brand>>(){}.getType());
        return brands;
    }

    public ArrayList<BrandModel> GetModelsForBrand(String brandHttpPath){
        String url = this.apiUrl + "/cars/brands/models?brandHttpPath=" + brandHttpPath;
        String responseString = this.httpHandler.getHttpResponse(url);
        ArrayList<BrandModel> brandModels = this.gson.fromJson(responseString,  new TypeToken<ArrayList<BrandModel>>(){}.getType());
        return brandModels;
    }

    public ArrayList<Car> GetCarsForModel(String modelHttpPath){
        String url = this.apiUrl + "/cars/brands/models/cars?modelHttpPath=" + modelHttpPath;
        String responseString = this.httpHandler.getHttpResponse(url);
        ArrayList<Car> cars = this.gson.fromJson(responseString,  new TypeToken<ArrayList<Car>>(){}.getType());
        return cars;
    }
}
