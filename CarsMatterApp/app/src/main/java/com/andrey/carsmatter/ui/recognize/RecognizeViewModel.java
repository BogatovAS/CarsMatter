package com.andrey.carsmatter.ui.recognize;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

public class RecognizeViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public RecognizeViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is recognize fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}