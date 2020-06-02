package com.andrey.carsmatter.models;

public class User {
    private static User currentUser;

    public String Username;
    public String Password;
    public MyCar SelectedCar;

    public static User getCurrentUser(){
        if(currentUser == null){
            currentUser = new User();
        }
        return currentUser;
    }

    public static void setCurrentUser(String username, String password, MyCar selectedCar){
        currentUser = new User();
        currentUser.Username = username;
        currentUser.Password = password;
        currentUser.SelectedCar = selectedCar;
    }
}
