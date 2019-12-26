package com.andrey.carsmatter.ui.top_auto;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import com.andrey.carsmatter.R;

public class TopAutoFragment extends Fragment {

    private TopAutoViewModel shareViewModel;

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        shareViewModel = ViewModelProviders.of(this).get(TopAutoViewModel.class);
        View root = inflater.inflate(R.layout.fragment_journal, container, false);
        return root;
    }
}