package com.andrey.carsmatter.ui.journal.tabs.Consumables;

import android.app.DatePickerDialog;
import android.app.Dialog;
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
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.services.CarsRepository;

import java.text.SimpleDateFormat;
import java.util.Calendar;

public class ConsumablesNoteChangeFragment extends Fragment {

    private ConsumablesNote currentConsumablesNote = new ConsumablesNote();
    private Calendar calendar;
    private EditText dateView;
    private EditText timeView;
    private EditText servicePriceEditText;
    private EditText notesEditText;
    private EditText odoEditText;
    private EditText locationEditText;
    private EditText serviceEditText;

    private CarsRepository carsRepository;

    private Dialog dialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        this.dialog = builder.create();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_change_consumables_note, container, false);

        this.dateView = view.findViewById(R.id.consumables_date_edit);
        this.timeView = view.findViewById(R.id.consumables_time_edit);
        this.serviceEditText = view.findViewById(R.id.consumables_service_edit);
        this.servicePriceEditText = view.findViewById(R.id.consumables_price_edit);
        this.notesEditText = view.findViewById(R.id.consumables_notes_edit);
        this.odoEditText = view.findViewById(R.id.consumables_odo_edit);
        this.locationEditText = view.findViewById(R.id.consumables_location_edit);

        try {
            currentConsumablesNote = new ConsumablesNote();
            currentConsumablesNote.Id = getArguments().getString("id");
            currentConsumablesNote.Date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'").parse(getArguments().getString("date"));
            currentConsumablesNote.KindOfService = getArguments().getString("service");
            currentConsumablesNote.Odo = getArguments().getInt("odo");
            currentConsumablesNote.Price = getArguments().getFloat("price");
            currentConsumablesNote.Location = getArguments().getString("location");
            currentConsumablesNote.Notes = getArguments().getString("notes");
        }catch (Exception e){
            currentConsumablesNote = null;
        }

        this.calendar = Calendar.getInstance();
        dateView.setText(new SimpleDateFormat("dd MMMM yyyy").format(calendar.getTime()));
        timeView.setText(new SimpleDateFormat("HH:mm").format(calendar.getTime()));

        if(currentConsumablesNote != null) {
            calendar.setTime(currentConsumablesNote.Date);

            dateView.setText(new SimpleDateFormat("dd MMMM yyyy").format(currentConsumablesNote.Date));
            timeView.setText(new SimpleDateFormat("HH:mm").format(currentConsumablesNote.Date));
            serviceEditText.setText(currentConsumablesNote.KindOfService);
            servicePriceEditText.setText(Float.toString(currentConsumablesNote.Price));
            odoEditText.setText(Long.toString(currentConsumablesNote.Odo));
            locationEditText.setText(currentConsumablesNote.Location);
            notesEditText.setText(currentConsumablesNote.Notes);
        }
        Button applyConsumablesChangeButton = view.findViewById(R.id.apply_consumables_change_button);

        applyConsumablesChangeButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final boolean isNewNote = currentConsumablesNote == null;

                if(isNewNote){
                    currentConsumablesNote = new ConsumablesNote();
                }

                currentConsumablesNote.Location = locationEditText.getText().toString();
                currentConsumablesNote.KindOfService = serviceEditText.getText().toString();
                currentConsumablesNote.Odo = Integer.parseInt(odoEditText.getText().toString());
                currentConsumablesNote.Price = Float.parseFloat(servicePriceEditText.getText().toString());
                currentConsumablesNote.Notes = notesEditText.getText().toString();
                currentConsumablesNote.Date = calendar.getTime();

                dialog.show();
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        if (isNewNote) {
                            carsRepository.AddConsumablesNote(currentConsumablesNote);
                        } else {
                            carsRepository.UpdateConsumablesNote(currentConsumablesNote);
                        }
                        getActivity().runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                dialog.dismiss();
                                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                                navController.navigate(R.id.nav_journal);
                            }});
                    }}).start();
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

        return view;
    }
}
