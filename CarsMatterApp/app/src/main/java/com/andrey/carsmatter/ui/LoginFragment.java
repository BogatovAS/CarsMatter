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
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.RememberedAccount;
import com.andrey.carsmatter.models.User;
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

    private EditText username;
    private EditText password;
    private CheckBox rememberMeCheckbox;

    private String filePath;

    DrawerLayout drawer;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.filePath = getContext().getFilesDir().getAbsolutePath() + File.separator + "creds.json";
    }

    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_login, container, false);

        ((AppCompatActivity) getActivity()).getSupportActionBar().hide();

        drawer = getActivity().findViewById(R.id.drawer_layout);
        drawer.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_CLOSED);

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

        view.findViewById(R.id.login_button).setOnClickListener(v -> new Thread(null, () -> {
            if(username.getText().toString().isEmpty() || password.getText().toString().isEmpty()){
                Toast.makeText(getContext(),"Поля 'Имя пользователя' и 'Пароль' должны быть заполнены", Toast.LENGTH_LONG).show();
                return;
            }

            boolean correctUser = carsRepository.Login(username.getText().toString(), password.getText().toString());

            if (!correctUser) {
                getActivity().runOnUiThread(() -> {
                    counter--;
                    Toast.makeText(getContext(), "Неверные имя пользователя или пароль. Попыток осталось: " + counter, Toast.LENGTH_SHORT).show();
                    if (counter == 0) {
                        view.findViewById(R.id.login_button).setEnabled(false);
                        final Handler handler = new Handler();
                        handler.postDelayed(() -> {
                            view.findViewById(R.id.login_button).setEnabled(true);
                            counter = 3;
                        }, 10000);
                    }
                });
            } else {
                if(rememberMeCheckbox.isChecked()) {
                    RememberAccount();
                }
                else{
                    DeleteRememberedAccount();
                }
                getActivity().runOnUiThread(() -> {
                    ((AppCompatActivity) getActivity()).getSupportActionBar().show();
                    drawer.setDrawerLockMode(DrawerLayout.LOCK_MODE_UNLOCKED);
                    NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                    if(User.getCurrentUser().SelectedCar == null){
                        navController.navigate(R.id.nav_usercars_change);
                        Toast.makeText(getContext(), "Добавьте свою первую машину", Toast.LENGTH_SHORT).show();
                    }
                    else {
                        navController.navigate(R.id.nav_journal);
                    }
                });
            }

        }).start());

        view.findViewById(R.id.create_an_account).setOnClickListener(view -> new Thread(null, () -> {
            String signUpResult = carsRepository.SignUp(username.getText().toString(), password.getText().toString());

            getActivity().runOnUiThread(() -> {
                if (Boolean.parseBoolean(signUpResult)) {
                    Toast.makeText(getContext(), "Аккаунт успешно создан", Toast.LENGTH_SHORT).show();
                } else {
                    Toast.makeText(getContext(), signUpResult, Toast.LENGTH_SHORT).show();
                }
            });
        }).start());

        return view;
    }

    private void RememberAccount() {

        RememberedAccount account = new RememberedAccount();

        Calendar currentDate = Calendar.getInstance();
        currentDate.add(Calendar.DATE, 7);

        account.Username = User.getCurrentUser().Username;
        account.Password = User.getCurrentUser().Password;
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
