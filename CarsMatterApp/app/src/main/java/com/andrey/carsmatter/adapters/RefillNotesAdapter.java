package com.andrey.carsmatter.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.RefillNote;

import java.text.SimpleDateFormat;
import java.util.ArrayList;

public class RefillNotesAdapter extends BaseAdapter {

    ArrayList<RefillNote> refillNotes;
    private static LayoutInflater inflater = null;

    public RefillNotesAdapter(Context context, ArrayList<RefillNote> refillNotes) {
        this.refillNotes = refillNotes;
        inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    @Override
    public int getCount() {
        return refillNotes.size();
    }

    @Override
    public RefillNote getItem(int position) {
        return refillNotes.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    public void remove(RefillNote refillNote){
        this.refillNotes.remove(refillNote);
    }

    public void clear() {
        this.refillNotes.clear();
    }

    public void add(RefillNote refillNote) {
        this.refillNotes.add(refillNote);
    }

    public void addRange(ArrayList<RefillNote> notes) {
        this.refillNotes.addAll(notes);
    }

    @Override
    public View getView(int position, View view, ViewGroup viewGroup) {
        if (view == null) {
            view = inflater.inflate(R.layout.card_refill, null);
        }
        ((TextView)view.findViewById(R.id.refill_location)).setText(this.refillNotes.get(position).Location);
        ((TextView)view.findViewById(R.id.refill_date)).setText(new SimpleDateFormat("dd MMM yyyy").format(this.refillNotes.get(position).Date));
        ((TextView)view.findViewById(R.id.refill_petrol)).setText(this.refillNotes.get(position).Petrol + " л");
        ((TextView)view.findViewById(R.id.refill_odo)).setText(this.refillNotes.get(position).Odo + " км");
        ((TextView)view.findViewById(R.id.refill_price)).setText(this.refillNotes.get(position).Price + " руб");

        return view;
    }
}
