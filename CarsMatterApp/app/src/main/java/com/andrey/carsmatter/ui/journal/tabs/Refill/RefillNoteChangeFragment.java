package com.andrey.carsmatter.ui.journal.tabs.Refill;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TimePicker;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.RefillNote;

import java.text.SimpleDateFormat;
import java.util.Calendar;

public class RefillNoteChangeFragment extends Fragment {

    private Calendar calendar;
    private EditText dateView;
    private EditText timeView;
    private EditText refillPriceSummary;
    private EditText refillPricePerLiter;
    private EditText petrolEditText;
    private EditText odoEditText;
    private EditText locationEditText;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_change_refill_note, container, false);

        this.dateView = view.findViewById(R.id.refill_date_edit);
        this.timeView = view.findViewById(R.id.refill_time_edit);
        this.refillPriceSummary = view.findViewById(R.id.refill_price_summary_edit);
        this.refillPricePerLiter = view.findViewById(R.id.refill_price_edit);
        this.petrolEditText = view.findViewById(R.id.refill_petrol_edit);
        this.odoEditText = view.findViewById(R.id.refill_odo_edit);
        this.locationEditText = view.findViewById(R.id.refill_location_edit);

        RefillNote currentRefillNote;
        try {
            currentRefillNote = new RefillNote();
            currentRefillNote.Location = getArguments().getString("location");
            currentRefillNote.Petrol = getArguments().getFloat("petrol");
            currentRefillNote.Odo = getArguments().getLong("odo");
            currentRefillNote.Price = getArguments().getFloat("price");
            currentRefillNote.Date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'").parse(getArguments().getString("date"));
        }catch (Exception e){
            currentRefillNote = null;
        }

        this.calendar = Calendar.getInstance();

        if(currentRefillNote != null) {
            calendar.setTime(currentRefillNote.Date);

            dateView.setText(new SimpleDateFormat("dd MMMM yyyy").format(currentRefillNote.Date));
            timeView.setText(new SimpleDateFormat("HH:mm").format(currentRefillNote.Date));
            locationEditText.setText(currentRefillNote.Location);
            petrolEditText.setText(Float.toString(currentRefillNote.Petrol));
            odoEditText.setText(Long.toString(currentRefillNote.Odo));
            refillPriceSummary.setText(Float.toString(currentRefillNote.Price));
            refillPricePerLiter.setText(Float.toString(currentRefillNote.Price / currentRefillNote.Petrol));
        }
        Button applyRefillChangeButton = view.findViewById(R.id.apply_refill_change_button);

        applyRefillChangeButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
            }
        });

        dateView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new DatePickerDialog(getContext(), new DatePickerDialog.OnDateSetListener() {
                    public void onDateSet(DatePicker datePickerView, int year, int monthOfYear, int dayOfMonth) {
                        calendar.set(Calendar.YEAR, year);
                        calendar.set(Calendar.MONTH, monthOfYear);
                        calendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);
                        dateView.setText(new SimpleDateFormat("dd MMMM yyyy").format(calendar.getTime()));
                    }
                },
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH))
                .show();
            }
        });

        timeView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new TimePickerDialog(getContext(), new TimePickerDialog.OnTimeSetListener() {
                    public void onTimeSet(TimePicker timePickerView, int hourOfDay, int minute) {
                        calendar.set(Calendar.HOUR_OF_DAY, hourOfDay);
                        calendar.set(Calendar.MINUTE, minute);
                        timeView.setText(new SimpleDateFormat("HH:mm").format(calendar.getTime()));
                    }
                },
                calendar.get(Calendar.HOUR_OF_DAY),
                calendar.get(Calendar.MINUTE), true)
                .show();
            }
        });

        refillPriceSummary.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                float summaryPrice = Float.parseFloat(refillPriceSummary.getText().toString());
                float currentPetrol = Float.parseFloat(petrolEditText.getText().toString());
                refillPricePerLiter.setText(Float.toString( summaryPrice / currentPetrol));
            }
        });

        refillPricePerLiter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                float pricePerLiter = Float.parseFloat(refillPricePerLiter.getText().toString());
                float currentPetrol = Float.parseFloat(petrolEditText.getText().toString());
                refillPriceSummary.setText(Float.toString( pricePerLiter * currentPetrol));
            }
        });

        return view;
    }
}
