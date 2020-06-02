package com.andrey.carsmatter.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.ConsumablesNote;

import java.text.SimpleDateFormat;
import java.util.ArrayList;

public class ConsumablesNotesAdapter extends BaseAdapter {

    ArrayList<ConsumablesNote> consumablesNotes;
    private static LayoutInflater inflater = null;

    public ConsumablesNotesAdapter(Context context, ArrayList<ConsumablesNote> consumablesNotes) {
        this.consumablesNotes = consumablesNotes;
        inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    @Override
    public int getCount() {
        return consumablesNotes.size();
    }

    @Override
    public ConsumablesNote getItem(int position) {
        return consumablesNotes.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    public void remove(ConsumablesNote consumablesNote){
        this.consumablesNotes.remove(consumablesNote);
    }

    public void clear() {
        this.consumablesNotes.clear();
    }

    public void add(ConsumablesNote consumablesNote) {
        this.consumablesNotes.add(consumablesNote);
    }

    public void addRange(ArrayList<ConsumablesNote> notes) {
        this.consumablesNotes.addAll(notes);
    }

    @Override
    public View getView(int position, View view, ViewGroup viewGroup) {
        if (view == null) {
            view = inflater.inflate(R.layout.card_consumables, null);
        }
        ((TextView)view.findViewById(R.id.consumables_date)).setText(new SimpleDateFormat("dd MMM yyyy").format(this.consumablesNotes.get(position).Date));
        ((TextView)view.findViewById(R.id.consumables_service)).setText(this.consumablesNotes.get(position).KindOfService.Name);
        ((TextView)view.findViewById(R.id.consumables_price)).setText(this.consumablesNotes.get(position).Price + " руб");
        ((TextView)view.findViewById(R.id.consumables_odo)).setText(this.consumablesNotes.get(position).Odo + " км");
        ((TextView)view.findViewById(R.id.consumables_location)).setText(this.consumablesNotes.get(position).Location);
        ((TextView)view.findViewById(R.id.consumables_notes)).setText(this.consumablesNotes.get(position).Notes);

        return view;
    }
}
