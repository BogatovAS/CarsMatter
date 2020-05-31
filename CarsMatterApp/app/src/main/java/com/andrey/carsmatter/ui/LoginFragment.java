package com.andrey.carsmatter.ui;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.RememberedAccount;
import com.andrey.carsmatter.services.CarsRepository;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.File;
import java.io.FileInputStream;

import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Calendar;

public class LoginFragment extends Fragment {

    private CarsRepository carsRepository;

    private int counter;
    private View view;

    private Handler handler;
    private Handler signUpHandler;

    private EditText username;
    private EditText password;
    private CheckBox rememberMeCheckbox;

    private String filePath;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.filePath = getContext().getFilesDir().getAbsolutePath() + File.separator + "creds.json";

        this.handler = new Handler() {
            @Override
            public void handleMessage(Message msg) {
                boolean isUserCorrect = msg.getData().getBoolean("isUserCorrect");

                if (!isUserCorrect) {
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
                        }
                    });
                } else {
                    if(rememberMeCheckbox.isChecked()) {
                        RememberAccount();
                    }
                    else{
                        DeleteRememberedAccount();
                    }
                    getActivity().runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            ((AppCompatActivity) getActivity()).getSupportActionBar().show();
                            NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                            navController.navigate(R.id.nav_journal);
                        }
                    });
                }
            }
        };

        this.signUpHandler = new Handler() {
            @Override
            public void handleMessage(Message msg) {
                final String signUpResult = msg.getData().getString("signUpResult");


                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        if (Boolean.parseBoolean(signUpResult)) {
                            Toast.makeText(getContext(), "Аккаунт успешно создан", Toast.LENGTH_SHORT).show();
                        } else {
                            Toast.makeText(getContext(), signUpResult, Toast.LENGTH_SHORT).show();
                        }
                    }
                });
            }
        };
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_login, container, false);

        ((AppCompatActivity) getActivity()).getSupportActionBar().hide();

        this.username = view.findViewById(R.id.username);
        this.password = view.findViewById(R.id.password);
        this.rememberMeCheckbox = view.findViewById(R.id.remember_me);

        RememberedAccount account = this.GetRememberedAccount();

        if(account != null){
            this.username.setText(account.Username);
            this.password.setText(account.Password);
            this.rememberMeCheckbox.setChecked(true);
        }

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

        view.findViewById(R.id.create_an_account).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new Thread(null, new Runnable() {
                    @Override
                    public void run() {
                        String result = carsRepository.SignUp(username.getText().toString(), password.getText().toString());

                        Bundle args = new Bundle();
                        args.putString("signUpResult", result);
                        Message message = new Message();
                        message.setData(args);
                        signUpHandler.handleMessage(message);
                    }
                }).start();
            }
        });

        return view;
    }

    private void RememberAccount() {

        RememberedAccount account = new RememberedAccount();

        Calendar currentDate = Calendar.getInstance();
        currentDate.add(Calendar.DATE, 7);

        account.Username = username.getText().toString();
        account.Password = password.getText().toString();
        account.Date = currentDate.getTime();

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();

        String accountJson = gson.toJson(account);

        File file = new File(filePath);

        try {
            file.createNewFile();
            FileWriter fileWriter = new FileWriter(filePath);
            fileWriter.write(accountJson);
            fileWriter.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private RememberedAccount GetRememberedAccount() {
        File file = new File(filePath);

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();

        RememberedAccount account;

        if (file.exists()) {
            try {
                InputStream fileReader = new FileInputStream(filePath);
                byte[] buffer = new byte[fileReader.available()];
                fileReader.read(buffer);
                String fileData = new String(buffer, StandardCharsets.UTF_8);
                account = gson.fromJson(fileData, RememberedAccount.class);

                if(Calendar.getInstance().getTime().getTime() > account.Date.getTime()){
                    DeleteRememberedAccount();
                    return null;
                }

                return account;
            } catch (IOException e) {
                return null;
            }
        }
        return null;
    }

    private boolean DeleteRememberedAccount(){
        File file = new File(filePath);
        return file.delete();
    }
}
