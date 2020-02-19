package com.andrey.carsmatter.ui.search_car;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.services.CarsRepository;

import java.util.ArrayList;

public class BrandModelsFragment extends Fragment {

    private CarsRepository carsRepository;
    private ArrayList<BrandModel> brandModels;
    ArrayAdapter<String> adapter;

    Dialog dialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.adapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_list_item_1);

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        dialog = builder.create();

        dialog.show();
        new Thread(null, new Runnable() {
            @Override
            public void run() {
                String brandId = getArguments().getString("brandId");
                brandModels = carsRepository.GetModelsForBrand(brandId);

                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        adapter.clear();
                        for (BrandModel brandModel: brandModels) {
                            adapter.add(brandModel.ModelName);
                        }
                        dialog.dismiss();
                    }});
            }
        }).start();
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_brand_models, container, false);

        ListView brandModelsListView = view.findViewById(R.id.brand_models_list_view);

        brandModelsListView.setAdapter(adapter);

        brandModelsListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putString("brandModelId", brandModels.get(i).Id);

                navController.navigate(R.id.nav_cars, params);
            }
        });
        return view;
    }
}
