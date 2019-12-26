package com.andrey.carsmatter.ui.journal.tabs.Consumables;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.adapters.ConsumablesNotesAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.services.CarsRepository;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

public class ConsumablesTabFragment extends Fragment {
    private CarsRepository carsRepository;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_consumables, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView consumablesListView = view.findViewById(R.id.consumables_list_view);
        consumablesListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        ArrayList<ConsumablesNote> consumablesNotes = this.carsRepository.GetAllConsumablesNotes();

        consumablesNotes.add(new ConsumablesNote(new Date(),"Замена масла",1000, 50000, "Газпром", "Первая замена масла"));
        consumablesNotes.add(new ConsumablesNote(new Date(),"Замена масла",1000, 50000, "Газпром", "Первая замена масла"));
        consumablesNotes.add(new ConsumablesNote(new Date(),"Замена масла",1000, 50000, "Газпром", "Первая замена масла"));

        final ConsumablesNotesAdapter adapter = new ConsumablesNotesAdapter(getContext(), consumablesNotes);

        consumablesListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> adapterView, View view, int i, long l) {
                ConsumablesNote selectedNote = adapter.getItem(i);
                carsRepository.DeleteConsumablesNote(selectedNote);
                adapter.remove(selectedNote);
                adapter.notifyDataSetChanged();
                return true;
            }
        });

        consumablesListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                ConsumablesNote selectedNote = adapter.getItem(i);
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putString("date", new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'").format(selectedNote.Date));
                params.putString("location", selectedNote.Location);
                params.putString("service", selectedNote.KindOfService);
                params.putLong("odo", selectedNote.Odo);
                params.putFloat("price", selectedNote.Price);
                params.putString("notes", selectedNote.Notes);

                navController.navigate(R.id.nav_consumables_change, params);
                adapter.notifyDataSetChanged();
            }
        });

        consumablesListView.setAdapter(adapter);

        Button addConsumablesNoteButton = view.findViewById(R.id.add_consumables_note_button);

        addConsumablesNoteButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                navController.navigate(R.id.nav_consumables_change);
                adapter.notifyDataSetChanged();
            }
        });

        return view;
    }
}
