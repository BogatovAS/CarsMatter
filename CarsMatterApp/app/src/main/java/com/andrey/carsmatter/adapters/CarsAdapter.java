package com.andrey.carsmatter.adapters;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;

import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class CarsAdapter extends BaseAdapter {

    ArrayList<Car> cars;
    private static LayoutInflater inflater = null;

    Context context;
    Activity parentActivity;

    CarsRepository carsRepository;

    public CarsAdapter(Context context, Activity parentActivity, ArrayList<Car> cars) {
        this.context = context;
        this.parentActivity = parentActivity;
        this.cars = cars;
        this.carsRepository = new CarsRepository(context);
        inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    @Override
    public int getCount() {
        return cars.size();
    }

    @Override
    public Car getItem(int position) {
        return cars.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    public void remove(Car car){
        this.cars.remove(car);
    }

    public void clear() {
        this.cars.clear();
    }

    public void add(Car car) {
        this.cars.add(car);
    }

    public void addRange(ArrayList<Car> newCars) {
        this.cars.addAll(newCars);
    }

    @Override
    public View getView(final int position, View view, ViewGroup viewGroup) {
        if (view == null) {
            view = inflater.inflate(R.layout.card_car, null);
        }


        if(this.cars.get(position).Base64CarImage != null) {
            byte[] imageBytes = Base64.decode(this.cars.get(position).Base64CarImage, 0);
            Bitmap carImage = BitmapFactory.decodeByteArray(imageBytes, 0, imageBytes.length);
            ((ImageView) view.findViewById(R.id.car_image)).setImageBitmap(carImage);
        }
        ((TextView)view.findViewById(R.id.car_model_name)).setText(this.cars.get(position).CarName);
        ((TextView)view.findViewById(R.id.car_body_type)).setText(this.cars.get(position).BodyType);
        ((TextView)view.findViewById(R.id.car_prices)).setText(this.cars.get(position).LowPrice + " руб - " + this.cars.get(position).HighPrice + " руб");

        String manufactureDatesString = this.cars.get(position).ManufactureStartDate + " г - " + this.cars.get(position).ManufactureEndDate;
        if(!this.cars.get(position).ManufactureEndDate.contains("в производстве")){
            manufactureDatesString += " г";
        }

        ((TextView)view.findViewById(R.id.car_manufacture_dates)).setText(manufactureDatesString);

        final View finalView = view;
        new Thread(new Runnable() {
            @Override
            public void run() {
                final boolean isFavorite = carsRepository.IsFavoriteCar(cars.get(position).Id);
                parentActivity.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        if (isFavorite) {
                            finalView.setBackgroundResource(R.drawable.ic_favorite_black_24dp);
                        } else {
                            finalView.setBackgroundResource(R.drawable.ic_favorite_border_black_24dp);
                        }
                    }});
            }
        }).start();


        view.findViewById(R.id.car_avito_uri).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Uri uri = Uri.parse(cars.get(position).AvitoUri);
                context.startActivity(new Intent(Intent.ACTION_VIEW, uri));
            }
        });

        view.findViewById(R.id.car_add_to_favorite_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        final boolean isFavorite = carsRepository.IsFavoriteCar(cars.get(position).Id);
                        if (!isFavorite) {
                            carsRepository.AddCarToFavorite(cars.get(position).Id);
                        } else {
                            carsRepository.RemoveCarFromFavorite(cars.get(position).Id);
                        }
                    }
                }).start();
            }
        });

        return view;
    }
}
