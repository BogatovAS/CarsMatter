package com.andrey.carsmatter.services;

import android.content.Context;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.http.HttpClient;
import com.andrey.carsmatter.models.Brand;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.models.User;
import com.andrey.carsmatter.models.UserModel;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;


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

        ArrayList<RefillNote> refillNotes = new ArrayList<>();

        if(responseString != null) {
           refillNotes = this.gson.fromJson(responseString, new TypeToken<ArrayList<RefillNote>>() {}.getType());
        }

        return refillNotes;
    }

    public boolean SendNotificationForRefill() {
        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");

        String url = this.apiUrl + "/refill_notes/notification?date=" + dateFormat.format(new Date());

        String responseString = this.httpHandler.getHttpResponse(url);

        boolean sendNotification = Boolean.parseBoolean(responseString);

        return sendNotification;
    }

    public ArrayList<ConsumablesNote> GetAllConsumablesNotes() {
        String url = this.apiUrl + "/consumables_notes";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<ConsumablesNote> consumablesNotes = new ArrayList<>();

        if(responseString != null) {
            consumablesNotes = this.gson.fromJson(responseString, new TypeToken<ArrayList<ConsumablesNote>>() {}.getType());
        }

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

    public boolean DeleteRefillNote(String id) {
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

    public boolean DeleteConsumablesNote(String id) {
        String url = this.apiUrl + "/consumables_notes/" + id;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public ArrayList<Brand> GetAllBrands(){
        String url = this.apiUrl + "/cars/brands";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Brand> brands = new ArrayList<>();

        if(responseString != null) {
           brands = this.gson.fromJson(responseString, new TypeToken<ArrayList<Brand>>() {}.getType());
        }

        return brands;
    }

    public ArrayList<BrandModel> GetModelsForBrand(String brandId){
        String url = this.apiUrl + "/cars/brands/" + brandId + "/models";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<BrandModel> brandModels = new ArrayList<>();

        if(responseString != null) {
            brandModels = this.gson.fromJson(responseString, new TypeToken<ArrayList<BrandModel>>() {}.getType());
        }

        return brandModels;
    }

    public ArrayList<Car> GetCarsForModel(String modelId){
        String url = this.apiUrl + "/cars/brands/models/" + modelId + "/cars";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if(responseString != null) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>() {}.getType());
        }

        return cars;
    }

    public boolean IsFavoriteCar(String carId){
        String url = this.apiUrl + "/favorite_cars/" + carId;
        boolean result = Boolean.parseBoolean(this.httpHandler.getHttpResponse(url));
        return result;
    }

    public boolean AddCarToFavorite(String carId) {
        String url = this.apiUrl + "/favorite_cars/" + carId;
        String responseString = this.httpHandler.postHttpRequest(url,  this.gson.toJson(""));
        return Boolean.parseBoolean(responseString);
    }

    public boolean RemoveCarFromFavorite(String carId){
        String url = this.apiUrl + "/favorite_cars/" + carId;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public  ArrayList<Car> GetAllFavoriteCars(){
        String url = this.apiUrl + "/favorite_cars/";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if(responseString != null) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>(){}.getType());
        }

        return cars;
    }

    public ArrayList<Car> SearchCars(String searchString){
        String url = this.apiUrl + "/cars/search" + searchString;
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if(responseString != null) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>() {}.getType());
        }

        return cars;
    }

    public boolean Login(String username, String password) {
        String url = this.apiUrl + "/user/logIn";

        User.setCurrentUser(username, password);

        UserModel user = new UserModel();
        user.Username = User.getCurrentUser().Username;
        user.Password = User.getCurrentUser().Password;

        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(user));
        return Boolean.parseBoolean(responseString);
    }

    public String SignUp(String username, String password) {
        String url = this.apiUrl + "/user/signUp";

        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;

        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(user));
        return responseString;
    }
}
