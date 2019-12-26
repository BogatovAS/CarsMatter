package com.andrey.carsmatter.ui.search_car;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

public class SearchCarViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public SearchCarViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is search car fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}