package com.andrey.carsmatter.ui.search_car;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.CarsAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.Car;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class SearchFragment extends Fragment {

    private CarsRepository carsRepository;
    private ArrayList<Car> cars = new ArrayList<>();
    CarsAdapter adapter;

    Dialog dialog;

    View view;

    EditText carName;
    EditText startDate;
    EditText endDate;
    EditText lowPrice;
    EditText highPrice;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.adapter = new CarsAdapter(getContext(), getActivity(), this.cars);

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        dialog = builder.create();
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_search_auto, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView carsListView = view.findViewById(R.id.search_auto_list_view);
        carsListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        carsListView.setAdapter(adapter);

        this.carName = view.findViewById(R.id.filter_search_auto_name);
        this.startDate = view.findViewById(R.id.filter_start_date);
        this.endDate = view.findViewById(R.id.filter_end_date);
        this.lowPrice = view.findViewById(R.id.filter_low_price);
        this.highPrice = view.findViewById(R.id.filter_high_price);

        view.findViewById(R.id.filter_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View filterButtonView) {
                if(view.findViewById(R.id.filter_layout).getVisibility() == View.VISIBLE) {
                    view.findViewById(R.id.filter_layout).setVisibility(View.GONE);
                }
                else{
                    view.findViewById(R.id.filter_layout).setVisibility(View.VISIBLE);
                }
            }
        });

        view.findViewById(R.id.apply_filters_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View applyFilterButtonView) {
                String queryString = "";

                if(!carName.getText().toString().isEmpty()) {
                    queryString += "?carName=" + carName.getText().toString();
                }
                if(!startDate.getText().toString().isEmpty()) {
                    if(queryString.isEmpty()){
                        queryString += "?";
                    }
                    else{
                        queryString += "&";
                    }
                    queryString += "manufactureStartDate=" + startDate.getText().toString();
                }
                if(!endDate.getText().toString().isEmpty()) {
                    if(queryString.isEmpty()){
                        queryString += "?";
                    }
                    else{
                        queryString += "&";
                    }
                    queryString += "manufactureEndDate=" + endDate.getText().toString();
                }
                if(!lowPrice.getText().toString().isEmpty()) {
                    if(queryString.isEmpty()){
                        queryString += "?";
                    }
                    else{
                        queryString += "&";
                    }
                    queryString += "lowPrice=" + lowPrice.getText().toString();
                }
                if(!highPrice.getText().toString().isEmpty()) {
                    if(queryString.isEmpty()){
                        queryString += "?";
                    }
                    else{
                        queryString += "&";
                    }
                    queryString += "highPrice=" + highPrice.getText().toString();
                }

                SearchCar(queryString);
                view.findViewById(R.id.filter_layout).setVisibility(View.GONE);
            }
        });

        return view;
    }

    private void SearchCar(final String searchString){
        dialog.show();
        new Thread(null, new Runnable() {
            @Override
            public void run() {
                cars = carsRepository.SearchCars(searchString);

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
}
