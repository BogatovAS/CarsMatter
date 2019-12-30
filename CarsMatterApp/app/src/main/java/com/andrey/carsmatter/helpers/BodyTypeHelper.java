package com.andrey.carsmatter.helpers;

public class BodyTypeHelper {

    public static String MapBodyType(int bodyTypeNumber) {
        switch (bodyTypeNumber) {
            case 0:
                return "Хэтчбек";
            case 1:
                return "Седан";
            case 2:
                return "Универсал";
            case 3:
                return "Кабриолет";
            case 4:
                return "Купе";
            case 5:
                return "Минивэн";
            case 6:
                return "Люкс";
            case 7:
                return "AWD";
            case 8:
                return "Внедорожник";
            default:
                return "Неизвестный тип кузова";
        }
    }
}
