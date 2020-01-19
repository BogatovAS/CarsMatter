package com.andrey.carsmatter.ui.top_auto;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

public class TopAutoViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public TopAutoViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is top auto fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}