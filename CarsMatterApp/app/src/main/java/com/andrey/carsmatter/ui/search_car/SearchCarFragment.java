package com.andrey.carsmatter.ui.search_car;

import android.app.Dialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.SimpleExpandableListAdapter;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.Brand;
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.services.CarsRepository;

import java.text.SimpleDateFormat;
import java.util.ArrayList;

public class SearchCarFragment extends Fragment {

    private ArrayList<Brand> brands = new ArrayList<>();

    private Dialog dialog;
    private CarsRepository carsRepository;
    private ArrayAdapter<String> adapter;

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
                brands = carsRepository.GetAllBrands();
                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        adapter.clear();
                        for (Brand brand: brands) {
                            adapter.add(brand.BrandName);
                        }
                        dialog.dismiss();
                    }});
            }
        }).start();
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_search_auto, container, false);

        ListView searchAutoListView = view.findViewById(R.id.search_auto_list_view);

        searchAutoListView.setAdapter(adapter);

        searchAutoListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putString("brandHttpPath", brands.get(i).HttpPath);

                navController.navigate(R.id.nav_brand_models, params);
            }
        });
        return view;
    }
}