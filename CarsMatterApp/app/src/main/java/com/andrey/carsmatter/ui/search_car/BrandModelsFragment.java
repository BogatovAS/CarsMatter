package com.andrey.carsmatter.ui.search_car;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class BrandModelsFragment extends Fragment {

    private CarsRepository carsRepository;
    private ArrayList<BrandModel> brandModels;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_brand_models, container, false);

        ListView brandModelsListView = view.findViewById(R.id.brand_models_list_view);

        String brandHttpPath = getArguments().getString("brandHttpPath");

        this.brandModels = this.carsRepository.GetModelsForBrand(brandHttpPath);

        ArrayList<String> brandModelsNames = new ArrayList<>();

        for (BrandModel brandModel : brandModels) {
            brandModelsNames.add(brandModel.ModelName);
        }

        ArrayAdapter<String> adapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_list_item_1, brandModelsNames);

        brandModelsListView.setAdapter(adapter);
        return view;
    }
}
