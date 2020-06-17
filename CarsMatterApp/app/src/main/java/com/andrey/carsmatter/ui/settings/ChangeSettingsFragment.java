package com.andrey.carsmatter.ui.settings;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.MyCar;
import com.andrey.carsmatter.services.CarsRepository;

public class ChangeSettingsFragment extends Fragment {

    private CarsRepository carsRepository;

    private TextView nameEditView;
    private TextView brandEditView;
    private TextView modelEditView;
    private TextView yearEditView;

    private MyCar currentCar;

    private Dialog dialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        this.dialog = builder.create();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_change_user_car, container, false);

        this.nameEditView = view.findViewById(R.id.change_user_car_name);
        this.brandEditView = view.findViewById(R.id.change_user_car_brand);
        this.modelEditView = view.findViewById(R.id.change_user_car_model);
        this.yearEditView = view.findViewById(R.id.change_user_car_year);

        try {
            currentCar = new MyCar();
            currentCar.Id = getArguments().getString("id");
            currentCar.Name = getArguments().getString("name");
            currentCar.Brand = getArguments().getString("brand");
            currentCar.Model = getArguments().getString("model");
            currentCar.Year = getArguments().getInt("year");
        }catch (Exception e){
            currentCar = null;
        }

        if(currentCar != null) {
            nameEditView.setText(currentCar.Name);
            brandEditView.setText(currentCar.Brand);
            modelEditView.setText(currentCar.Model);
            yearEditView.setText(Integer.toString(currentCar.Year));
        }

        Button applyButton = view.findViewById(R.id.change_user_car_apply);

        applyButton.setOnClickListener(view1 -> {
            final boolean isNewNote = currentCar == null;

            if(isNewNote){
                currentCar = new MyCar();
            }

            currentCar.Name = nameEditView.getText().toString();
            currentCar.Brand = brandEditView.getText().toString();
            currentCar.Model = modelEditView.getText().toString();
            currentCar.Year = Integer.parseInt(yearEditView.getText().toString());

            dialog.show();
            new Thread(() -> {
                if (isNewNote) {
                    carsRepository.AddUserCar(currentCar);
                } else {
                    carsRepository.UpdateUserCar(currentCar);
                }
                getActivity().runOnUiThread(() -> {
                    dialog.dismiss();
                    NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                    navController.navigate(R.id.nav_settings);
                });
            }).start();
        });

        Button setSelectedButton = view.findViewById(R.id.change_user_car_set_selected);

        if(this.currentCar != null && !this.currentCar.Id.isEmpty()) {
            setSelectedButton.setVisibility(View.VISIBLE);
        }
        else{
            setSelectedButton.setVisibility(View.INVISIBLE);
        }

        setSelectedButton.setOnClickListener(view1 -> {
            new Thread(null, () -> {
                this.carsRepository.SetSelectedUserCar(this.currentCar.Id);

                getActivity().runOnUiThread(() -> {
                    NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                    navController.navigate(R.id.nav_settings);
                });
            }).start();
        });
        return view;
    }
}
