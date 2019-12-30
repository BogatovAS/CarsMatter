package com.andrey.carsmatter.ui.journal.tabs.Refill;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
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

public class RefillTabFragment extends Fragment {
    private CarsRepository carsRepository;
    private ArrayList<RefillNote> refillNotes = new ArrayList<>();
    RefillNotesAdapter adapter;

    Dialog dialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.carsRepository = new CarsRepository(getContext());
        this.adapter = new RefillNotesAdapter(getContext(), this.refillNotes);

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setView(R.layout.progress_bar_dialog);
        dialog = builder.create();

        dialog.show();
        new Thread(null, new Runnable() {
            @Override
            public void run() {
                refillNotes = carsRepository.GetAllRefillNotes();
                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        adapter.clear();
                        adapter.addRange(refillNotes);
                        adapter.notifyDataSetChanged();
                        dialog.dismiss();
                    }});
            }
        }).start();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_refill, container, false);

        KeyboardHelper.hideKeyboard(getActivity());

        ListView refillListView = view.findViewById(R.id.refill_list_view);
        refillListView.setDivider(ContextCompat.getDrawable(getActivity(), R.drawable.transparent_color));

        refillListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> adapterView, View view, int i, long l) {
                final RefillNote selectedNote = adapter.getItem(i);

                dialog.show();
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        final boolean successfully = carsRepository.DeleteRefillNote(selectedNote.Id);
                        getActivity().runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                if(successfully){
                                    adapter.remove(selectedNote);
                                    adapter.notifyDataSetChanged();
                                    Toast.makeText(getActivity().getApplicationContext(),"Запись была успешно удалена", Toast.LENGTH_LONG).show();
                                }
                                else{
                                    Toast.makeText(getActivity().getApplicationContext(),"Произошла ошибка во время удаления записи", Toast.LENGTH_LONG).show();
                                }
                                dialog.dismiss();
                            }});
                    }
                }).start();
                return true;
            }
        });

        refillListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                RefillNote selectedNote = adapter.getItem(i);
                NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);

                Bundle params = new Bundle();
                params.putInt("id", selectedNote.Id);
                params.putString("date", new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'").format(selectedNote.Date));
                params.putString("location", selectedNote.Location);
                params.putFloat("petrol", selectedNote.Petrol);
                params.putInt("odo", selectedNote.Odo);
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
