package com.andrey.carsmatter.ui;

import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.BrandModel;
import com.andrey.carsmatter.services.CarsRepository;

public class LoginFragment extends Fragment {

    private CarsRepository carsRepository;

    int counter;
    View view;

    Handler handler;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        carsRepository = new CarsRepository(getContext());

        this.handler = new Handler() {
            @Override
            public void handleMessage(Message msg) {
                boolean isUserCorrect = msg.getData().getBoolean("isUserCorrect");

                if(!isUserCorrect) {
                    getActivity().runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            counter--;
                            Toast.makeText(getContext(), "Неверные имя пользователя или пароль. Попыток осталось: " + counter, Toast.LENGTH_SHORT).show();
                            if (counter == 0) {
                                view.findViewById(R.id.login_button).setEnabled(false);
                                final Handler handler = new Handler();
                                handler.postDelayed(new Runnable() {
                                    @Override
                                    public void run() {
                                        view.findViewById(R.id.login_button).setEnabled(true);
                                        counter = 3;
                                    }
                                }, 10000);
                            }
                        }});
                }
                else {
                    getActivity().runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            ((AppCompatActivity)getActivity()).getSupportActionBar().show();
                            NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                            navController.navigate(R.id.nav_journal);
                        }});
                }
            }
        };
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_login, container, false);

        ((AppCompatActivity)getActivity()).getSupportActionBar().hide();

        final EditText username = view.findViewById(R.id.username);
        final EditText password = view.findViewById(R.id.password);

        counter = 3;

        view.findViewById(R.id.login_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                new Thread(null, new Runnable() {
                    @Override
                    public void run() {
                        boolean correctUser = carsRepository.Login(username.getText().toString(), password.getText().toString());

                        Bundle args = new Bundle();
                        args.putBoolean("isUserCorrect", correctUser);
                        Message message = new Message();
                        message.setData(args);
                        handler.handleMessage(message);
                    }
                }).start();
            }
        });

        return view;
    }
}
