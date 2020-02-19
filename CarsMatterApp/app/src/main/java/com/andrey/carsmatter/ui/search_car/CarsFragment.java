package com.andrey.carsmatter.ui.search_car;

import android.app.Dialog;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.CarsAdapter;
import com.andrey.carsmatter.adapters.RefillNotesAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class CarsFragment extends Fragment {

    private CarsRepository carsRepository;
    private ArrayList<Car> cars = new ArrayList<>();
    CarsAdapter adapter;

    Dialog dialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.adapter = new CarsAdapter(getContext(), getActivity(), this.cars);

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        dialog = builder.create();

        dialog.show();
        new Thread(null, new Runnable() {
            @Override
            public void run() {
                String brandModelId = getArguments().getString("brandModelId");
                cars = carsRepository.GetCarsForModel(brandModelId);

                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        adapter.clear();
                        adapter.addRange(cars);
                        adapter.notifyDataSetChanged();
                        dialog.dismiss();
                    }});
            }
        }).start();
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_cars, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView carsListView = view.findViewById(R.id.cars_list_view);
        carsListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        carsListView.setAdapter(adapter);
        return view;
    }
}
