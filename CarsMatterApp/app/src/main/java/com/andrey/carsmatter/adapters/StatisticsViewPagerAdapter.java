package com.andrey.carsmatter.adapters;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentPagerAdapter;

import com.andrey.carsmatter.ui.statistics.ConsumablesStatisticsFragment;
import com.andrey.carsmatter.ui.statistics.RefillChartStatisticsFragment;

public class StatisticsViewPagerAdapter extends FragmentPagerAdapter {
    private int NUM_ITEMS = 2;
    private String[] PAGE_TITLES = {"Заправки", "Замены"};

    public StatisticsViewPagerAdapter(@NonNull FragmentManager fm) {
        super(fm, BEHAVIOR_RESUME_ONLY_CURRENT_FRAGMENT);
    }


    @Override
    public int getCount() {
        return NUM_ITEMS;
    }

    @Override
    public Fragment getItem(int position) {
        switch (position) {
            case 0:
                return new RefillChartStatisticsFragment();
            case 1:
                return new ConsumablesStatisticsFragment();
            default:
                return null;
        }
    }

    @Override
    public CharSequence getPageTitle(int position) {
        return PAGE_TITLES[position];
    }

}
