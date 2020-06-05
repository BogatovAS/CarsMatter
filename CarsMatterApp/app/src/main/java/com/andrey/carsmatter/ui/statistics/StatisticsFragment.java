package com.andrey.carsmatter.ui.statistics;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.viewpager.widget.PagerAdapter;
import androidx.viewpager.widget.ViewPager;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.StatisticsViewPagerAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.User;
import com.google.android.material.tabs.TabLayout;

public class StatisticsFragment extends Fragment {

    ViewPager pager;
    PagerAdapter pagerAdapter;

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_statistics, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        TabLayout tabLayout = view.findViewById(R.id.statistics_tab_layout);

        pager = view.findViewById(R.id.statistics_view_pager);
        pagerAdapter = new StatisticsViewPagerAdapter(getChildFragmentManager());
        pager.setAdapter(pagerAdapter);

        tabLayout.setupWithViewPager(pager);

        int tabNumber = 0;
        try {
            tabNumber = getArguments().getInt("tabNumber");
        } catch (Exception e) {
        }

        tabLayout.setScrollPosition(tabNumber, 0f, true);
        pager.setCurrentItem(tabNumber);

        TextView selectedCarTexView = view.findViewById(R.id.statistics_selected_car_name);

        selectedCarTexView.setText("Текущий автомобиль: ".toUpperCase() + User.getCurrentUser().SelectedCar.Name.toUpperCase());

        return view;
    }
}