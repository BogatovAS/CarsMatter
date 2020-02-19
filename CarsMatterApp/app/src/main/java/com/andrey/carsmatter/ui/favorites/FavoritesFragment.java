package com.andrey.carsmatter.ui.favorites;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.CarsAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class FavoritesFragment extends Fragment {

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
                cars = carsRepository.GetAllFavoriteCars();

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
        View view = inflater.inflate(R.layout.fragment_favorites, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView carsListView = view.findViewById(R.id.favorite_cars_list_view);
        carsListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        carsListView.setAdapter(adapter);
        return view;
    }
}