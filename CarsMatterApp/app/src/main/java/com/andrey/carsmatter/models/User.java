package com.andrey.carsmatter.models;

public class User {
    private static User currentUser;

    public String Username;
    public String Password;

    public static User getCurrentUser(){
        if(currentUser == null){
            currentUser = new User();
        }
        return currentUser;
    }

    public static void setCurrentUser(String username, String password){
        currentUser = new User();
        currentUser.Username = username;
        currentUser.Password = password;
    }
}
