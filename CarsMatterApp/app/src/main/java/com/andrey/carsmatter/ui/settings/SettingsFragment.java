package com.andrey.carsmatter.ui.settings;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.MyCar;
import com.andrey.carsmatter.models.User;
import com.andrey.carsmatter.services.CarsRepository;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.stream.Collectors;

public class SettingsFragment extends Fragment {

    private CarsRepository carsRepository;

    private ArrayList<MyCar> userCars = new ArrayList<>();

    private ArrayAdapter<String> adapter;

    private Dialog dialog;

    private ListView userCarsView;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());

        this.adapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_list_item_1);

        KeyboardHelper.hideKeyboard(getActivity());

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        dialog = builder.create();

        dialog.show();
        new Thread(null, () -> {
            this.userCars = carsRepository.GetUserCars();

            getActivity().runOnUiThread(() -> {
                adapter.clear();
                adapter.addAll(userCars.stream().map(car -> car.Name).collect(Collectors.toList()));
                dialog.dismiss();

                MyCar selectedCar = userCars.stream().filter(car -> car.Id.equals(User.getCurrentUser().SelectedCar.Id)).findFirst().orElse(null);

                int position = userCars.indexOf(selectedCar);

                if(position > -1) {
                    View selectedItem = this.userCarsView.getAdapter().getView(position, null, this.userCarsView);

                    selectedItem.setBackgroundColor(getResources().getColor(R.color.colorPrimaryLight));
                    adapter.notifyDataSetChanged();
                }
            });
        }).start();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_settings, container, false);

        this.userCarsView = view.findViewById(R.id.settings_mycars_list_view);

        this.userCarsView.setAdapter(this.adapter);

        view.findViewById(R.id.add_user_car).setOnClickListener((buttonView) -> {
            NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
            navController.navigate(R.id.nav_usercars_change);
        });

        this.userCarsView.setOnItemClickListener((adapterView, view1, i, l) -> {
            NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

            MyCar selectedNote = this.userCars.get(i);

            Bundle params = new Bundle();
            params.putString("id", selectedNote.Id);
            params.putString("name", selectedNote.Name);
            params.putString("brand", selectedNote.Brand);
            params.putString("model", selectedNote.Model);
            params.putInt("year", selectedNote.Year);

            navController.navigate(R.id.nav_usercars_change, params);
        });

        userCarsView.setOnItemLongClickListener((adapterView, view12, i, l) -> {
            new Thread(null, () -> this.carsRepository.SetSelectedUserCar(this.userCars.get(i).Id)).start();
            return true;
        });

        return view;
    }
}
