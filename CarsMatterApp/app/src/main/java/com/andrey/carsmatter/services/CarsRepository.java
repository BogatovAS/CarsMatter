package com.andrey.carsmatter.services;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.http.HttpClient;
import com.andrey.carsmatter.models.Brand;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.ConsumablesNotesReport;
import com.andrey.carsmatter.models.KindOfService;
import com.andrey.carsmatter.models.MyCar;
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.models.RefillNotesReport;
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

    private Context context;
    private Activity activity;

    public CarsRepository(Context context) {
        this.context = context;
        try {
            this.activity = (Activity) context;
        }
        catch (Exception e) {

        }
        this.apiUrl = context.getResources().getString(R.string.api_base_path);
        this.httpHandler = new HttpClient();
    }

    public CarsRepository(Context context, HttpClient httpClient) {
        this.context = context;
        try {
            this.activity = (Activity) context;
        }
        catch (Exception e) {

        }
        this.apiUrl = "asd";
        this.httpHandler = httpClient;
    }

    public ArrayList<RefillNote> GetAllRefillNotes() {
        String url = this.apiUrl + "/refill_notes";

        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<RefillNote> refillNotes = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            refillNotes = this.gson.fromJson(responseString, new TypeToken<ArrayList<RefillNote>>() {
            }.getType());
        }

        return refillNotes;
    }

    public RefillNotesReport GetRefillNotesReport() {
        String url = this.apiUrl + "/refill_notes/report";

        String responseString = this.httpHandler.getHttpResponse(url);

        RefillNotesReport report = new RefillNotesReport();

        if (responseString != null && !responseString.isEmpty()) {
            report = this.gson.fromJson(responseString, RefillNotesReport.class);
        }

        return report;
    }

    public ConsumablesNotesReport GetConsumablesNotesReport() {
        String url = this.apiUrl + "/consumables_notes/report";

        String responseString = this.httpHandler.getHttpResponse(url);

        ConsumablesNotesReport report = new ConsumablesNotesReport();

        if (responseString != null && !responseString.isEmpty()) {
            report = this.gson.fromJson(responseString, ConsumablesNotesReport.class);
        }

        return report;
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

        if (responseString != null && !responseString.isEmpty()) {
            consumablesNotes = this.gson.fromJson(responseString, new TypeToken<ArrayList<ConsumablesNote>>() {
            }.getType());
        }

        return consumablesNotes;
    }

    public MyCar GetSelectedUserCar() {

        String url = this.apiUrl + "/user/selectedCar";

        String responseString = this.httpHandler.getHttpResponse(url);

        MyCar selectedCar = null;

        if (responseString != null && !responseString.isEmpty()) {
            selectedCar = this.gson.fromJson(responseString, MyCar.class);
        }

        return selectedCar;
    }

    public ArrayList<MyCar> GetUserCars() {
        String url = this.apiUrl + "/user/cars";

        String responseString = this.httpHandler.getHttpResponse(url);

        return this.gson.fromJson(responseString, new TypeToken<ArrayList<MyCar>>() {
        }.getType());
    }

    public Car SendCarForRecognition(byte[] imageBytes) {
        String url = this.apiUrl + "cars/recognize";

        String responseString = this.httpHandler.postFile(url, imageBytes);

        return this.gson.fromJson(responseString, Car.class);
    }

    public boolean DeleteMyCar(MyCar myCar){
        String url = this.apiUrl + "/user/car/" + myCar.Id;

        String responseString = this.httpHandler.deleteHttpRequest(url);

        boolean result = Boolean.parseBoolean(responseString);

        if (!result) {
            this.ShowToast(responseString);
        }

        return Boolean.parseBoolean(responseString);
    }

    public ArrayList<KindOfService> GetKindOfServices() {
        String url = this.apiUrl + "/consumables_notes/kindOfServices";

        String responseString = this.httpHandler.getHttpResponse(url);

        return this.gson.fromJson(responseString, new TypeToken<ArrayList<KindOfService>>() {}.getType());
    }

    public boolean SetSelectedUserCar(String userCarId) {
        String url = this.apiUrl + "/user/selectedCar/?userCarId=" + userCarId;

        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(""));

        if (responseString != null && !responseString.isEmpty()) {
            User.getCurrentUser().SelectedCar = this.gson.fromJson(responseString, MyCar.class);
            return true;
        } else {
            return false;
        }
    }

    public boolean AddRefillNote(RefillNote refillNote) {
        String url = this.apiUrl + "/refill_notes";
        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(refillNote));

        boolean result = Boolean.parseBoolean(responseString);

        if (!result) {
            this.ShowToast(responseString);
        }

        return Boolean.parseBoolean(responseString);
    }

    public boolean UpdateRefillNote(RefillNote refillNote) {
        String url = this.apiUrl + "/refill_notes";
        String responseString = this.httpHandler.putHttpRequest(url, this.gson.toJson(refillNote));

        boolean result = Boolean.parseBoolean(responseString);

        if (!result) {
            this.ShowToast(responseString);
        }

        return Boolean.parseBoolean(responseString);
    }

    public boolean DeleteRefillNote(String id) {
        String url = this.apiUrl + "/refill_notes/" + id;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public boolean AddConsumablesNote(ConsumablesNote consumablesNote) {
        String url = this.apiUrl + "/consumables_notes";

        consumablesNote.KindOfService = null;

        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(consumablesNote));

        boolean result = Boolean.parseBoolean(responseString);

        if (!result) {
            this.ShowToast(responseString);
        }

        return Boolean.parseBoolean(responseString);
    }

    public boolean UpdateConsumablesNote(ConsumablesNote consumablesNote) {
        String url = this.apiUrl + "/consumables_notes";

        consumablesNote.KindOfService = null;

        String responseString = this.httpHandler.putHttpRequest(url, this.gson.toJson(consumablesNote));

        boolean result = Boolean.parseBoolean(responseString);

        if (!result) {
            this.ShowToast(responseString);
        }

        return Boolean.parseBoolean(responseString);
    }

    public boolean DeleteConsumablesNote(String id) {
        String url = this.apiUrl + "/consumables_notes/" + id;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public ArrayList<Brand> GetAllBrands() {
        String url = this.apiUrl + "/cars/brands";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Brand> brands = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            brands = this.gson.fromJson(responseString, new TypeToken<ArrayList<Brand>>() {
            }.getType());
        } else {
            this.ShowToast(responseString);
        }

        return brands;
    }

    public ArrayList<BrandModel> GetModelsForBrand(String brandId) {
        String url = this.apiUrl + "/cars/brands/" + brandId + "/models";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<BrandModel> brandModels = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            brandModels = this.gson.fromJson(responseString, new TypeToken<ArrayList<BrandModel>>() {
            }.getType());
        } else {
            this.ShowToast(responseString);
        }

        return brandModels;
    }

    public ArrayList<Car> GetCarsForModel(String modelId) {
        String url = this.apiUrl + "/cars/brands/models/" + modelId + "/cars";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>() {
            }.getType());
        } else {
            this.ShowToast(responseString);
        }

        return cars;
    }

    public boolean IsFavoriteCar(String carId) {
        String url = this.apiUrl + "/favorite_cars/" + carId;
        boolean result = Boolean.parseBoolean(this.httpHandler.getHttpResponse(url));
        return result;
    }

    public boolean AddCarToFavorite(String carId) {
        String url = this.apiUrl + "/favorite_cars/" + carId;
        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(""));
        return Boolean.parseBoolean(responseString);
    }

    public boolean RemoveCarFromFavorite(String carId) {
        String url = this.apiUrl + "/favorite_cars/" + carId;
        String responseString = this.httpHandler.deleteHttpRequest(url);
        return Boolean.parseBoolean(responseString);
    }

    public ArrayList<Car> GetAllFavoriteCars() {
        String url = this.apiUrl + "/favorite_cars/";
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>() {
            }.getType());
        } else {
            this.ShowToast(responseString);
        }

        return cars;
    }

    public ArrayList<Car> SearchCars(String searchString) {
        String url = this.apiUrl + "/cars/search" + searchString;
        String responseString = this.httpHandler.getHttpResponse(url);

        ArrayList<Car> cars = new ArrayList<>();

        if (responseString != null && !responseString.isEmpty()) {
            cars = this.gson.fromJson(responseString, new TypeToken<ArrayList<Car>>() {
            }.getType());
        } else {
            this.ShowToast(responseString);
        }

        return cars;
    }

    public boolean Login(String username, String password) {
        String url = this.apiUrl + "/user/logIn";

        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;

        String responseString = this.httpHandler.loginRequest(url, this.gson.toJson(user));

        if (Boolean.parseBoolean(responseString)) {
            User.setCurrentUser(username, password, null);
            User.getCurrentUser().SelectedCar = GetSelectedUserCar();
        }

        return Boolean.parseBoolean(responseString);
    }

    public String SignUp(String username, String password) {
        String url = this.apiUrl + "/user/signUp";

        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;

        String responseString = this.httpHandler.loginRequest(url, this.gson.toJson(user));
        return responseString;
    }

    public boolean AddUserCar(MyCar userCar) {
        String url = this.apiUrl + "/user/car";
        String responseString = this.httpHandler.postHttpRequest(url, this.gson.toJson(userCar));

        if (responseString != null && !responseString.isEmpty()) {
            return true;
        } else {
            this.ShowToast(responseString);
        }

        return false;
    }

    public boolean UpdateUserCar(MyCar userCar) {
        String url = this.apiUrl + "/user/car";
        String responseString = this.httpHandler.putHttpRequest(url, this.gson.toJson(userCar));

        if (responseString != null && !responseString.isEmpty()) {
            return true;
        } else {
            this.ShowToast(responseString);
        }

        return false;
    }

    private void ShowToast(String message) {
        if(activity != null) {
            activity.runOnUiThread(() -> {
                Toast.makeText(context, message, Toast.LENGTH_LONG).show();
            });
        }
    }
}
