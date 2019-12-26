package com.andrey.carsmatter.services;

import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.RefillNote;

import java.util.ArrayList;

public class CarsRepository {

    public ArrayList<RefillNote> GetAllRefillNotes() {
        return new ArrayList<RefillNote>();
    }

    public ArrayList<ConsumablesNote> GetAllConsumablesNotes() {
        return new ArrayList<ConsumablesNote>();
    }

    public boolean DeleteRefillNote(RefillNote refillNote) {
        return true;
    }

    public boolean AddRefillNote(RefillNote refillNote) {
        return true;
    }

    public boolean AddConsumablesNote(ConsumablesNote consumablesNote) {
        return true;
    }

    public boolean DeleteConsumablesNote(ConsumablesNote consumablesNote) {
        return true;
    }
}
