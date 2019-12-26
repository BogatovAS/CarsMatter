package com.andrey.carsmatter.ui.search_car;

import android.os.Bundle;
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

import androidx.annotation.Nullable;
import androidx.annotation.NonNull;
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

    private ArrayList<Brand> brands;

    CarsRepository carsRepository;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.brands = this.carsRepository.GetAllBrands();
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_search_auto, container, false);

        ListView searchAutoListView = view.findViewById(R.id.search_auto_list_view);

        ArrayList<String> brandsNames = new ArrayList<>();

        for (Brand brand: brands) {
            brandsNames.add(brand.BrandName);
        }

        final ArrayAdapter<String> adapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_list_item_1, brandsNames);

        searchAutoListView.setAdapter(adapter);

        searchAutoListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putString("brandHttpPath", brands.get(i).HttpPath);

                navController.navigate(R.id.nav_brand_models, params);
                adapter.notifyDataSetChanged();
            }
        });


        return view;
    }
}