package com.andrey.carsmatter.ui.journal;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.ProgressBar;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.viewpager.widget.PagerAdapter;
import androidx.viewpager.widget.ViewPager;


import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.JournalViewPagerAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.User;
import com.google.android.material.tabs.TabLayout;


public class JournalFragment extends Fragment {

    ViewPager pager;
    PagerAdapter pagerAdapter;

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_journal, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        TabLayout tabLayout = view.findViewById(R.id.journal_tab_layout);

        pager = view.findViewById(R.id.journal_view_pager);
        pagerAdapter = new JournalViewPagerAdapter(getChildFragmentManager());
        pager.setAdapter(pagerAdapter);

        tabLayout.setupWithViewPager(pager);

        int tabNumber = 0;
        try {
            tabNumber = getArguments().getInt("tabNumber");
        }
        catch (Exception e){
        }

        tabLayout.setScrollPosition(tabNumber, 0f, true);
        pager.setCurrentItem(tabNumber);

        TextView selectedCarTexView = view.findViewById(R.id.journal_selected_car_name);

        if(User.getCurrentUser().SelectedCar != null) {
            selectedCarTexView.setText("Текущий автомобиль: ".toUpperCase() + User.getCurrentUser().SelectedCar.Name.toUpperCase());
            pager.setVisibility(View.VISIBLE);
        }
        else{
            selectedCarTexView.setText("Выберите автомобиль в пункте 'Настройки'");
            pager.setVisibility(View.INVISIBLE);
        }

        return view;
    }
}