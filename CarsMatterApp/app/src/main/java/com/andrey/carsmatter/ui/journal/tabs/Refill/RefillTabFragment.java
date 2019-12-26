package com.andrey.carsmatter.ui.journal.tabs.Refill;

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
import com.andrey.carsmatter.adapters.RefillNotesAdapter;
import com.andrey.carsmatter.helpers.KeyboardHelper;
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.services.CarsRepository;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

public class RefillTabFragment extends Fragment {

    CarsRepository carsRepository;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_refill, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView refillListView = view.findViewById(R.id.refill_list_view);
        refillListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        ArrayList<RefillNote> refillNotes = this.carsRepository.GetAllRefillNotes();

        refillNotes.add(new RefillNote("Газпром",20, 10500, 866, new Date()));
        refillNotes.add(new RefillNote("Газпром",20, 10500, 866, new Date()));
        refillNotes.add(new RefillNote("Газпром",20, 10500, 866, new Date()));

        final RefillNotesAdapter adapter = new RefillNotesAdapter(getContext(), refillNotes);

        refillListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> adapterView, View view, int i, long l) {
                RefillNote selectedNote = adapter.getItem(i);
                carsRepository.DeleteRefillNote(selectedNote);
                adapter.remove(selectedNote);
                adapter.notifyDataSetChanged();
                return true;
            }
        });

        refillListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                RefillNote selectedNote = adapter.getItem(i);
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putString("date", new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'").format(selectedNote.Date));
                params.putString("location", selectedNote.Location);
                params.putFloat("petrol", selectedNote.Petrol);
                params.putLong("odo", selectedNote.Odo);
                params.putFloat("price", selectedNote.Price);

                navController.navigate(R.id.nav_refill_change, params);
                adapter.notifyDataSetChanged();
            }
        });

        refillListView.setAdapter(adapter);

        Button addRefillNoteButton = view.findViewById(R.id.add_refill_note_button);

        addRefillNoteButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
                navController.navigate(R.id.nav_refill_change);
                adapter.notifyDataSetChanged();
            }
        });

        return view;
    }

}
