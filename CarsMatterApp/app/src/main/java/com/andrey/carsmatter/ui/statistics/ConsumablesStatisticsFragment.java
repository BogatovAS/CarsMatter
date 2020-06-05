package com.andrey.carsmatter.ui.statistics;

import android.os.Build;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.fragment.app.Fragment;

import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.ConsumablesNote;
import com.andrey.carsmatter.models.ConsumablesNotesReport;
import com.andrey.carsmatter.models.RefillNotesReport;
import com.andrey.carsmatter.services.CarsRepository;
import com.github.mikephil.charting.charts.BarChart;
import com.github.mikephil.charting.charts.PieChart;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarDataSet;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.data.DataSet;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.PieData;
import com.github.mikephil.charting.data.PieDataSet;
import com.github.mikephil.charting.utils.ColorTemplate;

import java.time.Month;
import java.time.format.TextStyle;
import java.util.ArrayList;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;

public class ConsumablesStatisticsFragment extends Fragment {
    private CarsRepository carsRepository;
    private ArrayList<ConsumablesNote> consumablesNotes = new ArrayList<>();
    private BarChart chartByPrice ;
    private PieChart chartByKindOfService;

    @RequiresApi(api = Build.VERSION_CODES.O)
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        this.carsRepository = new CarsRepository(getContext());

        new Thread(null, () -> {
            consumablesNotes = carsRepository.GetAllConsumablesNotes();

            this.InitializeBarChart();
            this.InitializePieChart();

        }).start();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_consumables_statistics, container, false);

        this.chartByPrice = view.findViewById(R.id.consumables_bar_chart);
        this.chartByPrice.animateXY(2000, 2000);
        this.chartByPrice.setDescription("");

        this.chartByKindOfService = view.findViewById(R.id.consumables_pie_chart);
        this.chartByKindOfService.animateXY(2000, 2000);
        this.chartByKindOfService.setDescription("");

        view.findViewById(R.id.show_chart_by_kind).setOnClickListener((v) -> {
            this.chartByKindOfService.setVisibility(View.VISIBLE);
            this.chartByPrice.setVisibility(View.GONE);
        });

        view.findViewById(R.id.show_chart_by_price).setOnClickListener((v) -> {
            this.chartByPrice.setVisibility(View.VISIBLE);
            this.chartByKindOfService.setVisibility(View.GONE);
        });

        TextView totalPriceTextView = view.findViewById(R.id.consumables_statistics_total_price);
        TextView pricePerDayTextView = view.findViewById(R.id.consumables_statistics_price_per_day);
        TextView pricePerKmTextView = view.findViewById(R.id.consumables_statistics_price_per_km);

        new Thread(null, () -> {
            ConsumablesNotesReport report = carsRepository.GetConsumablesNotesReport();
            totalPriceTextView.setText(String.format("%.2f руб", report.TotalCost));
            pricePerDayTextView.setText(String.format("%.2f руб", report.CostPerDay));
            pricePerKmTextView.setText(String.format("%.2f руб", report.CostPerKm));
        }).start();

        return view;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private void InitializeBarChart() {
        Map<Integer, Map<Integer, Double>> costsPerMonth = consumablesNotes.stream().collect(
                Collectors.groupingBy(n -> n.Date.getYear(), Collectors.groupingBy(n -> n.Date.getMonth(), Collectors.summingDouble(n -> n.Price))));

        ArrayList<BarEntry> prices = new ArrayList<>();

        int i = 0;
        for (Map<Integer, Double> monthSet : costsPerMonth.values()) {
            for (Double value : monthSet.values()) {
                prices.add(new BarEntry(value.floatValue(), i++));
            }
        }

        BarDataSet dataSet = new BarDataSet(prices, "Цена");
        dataSet.setColors(ColorTemplate.COLORFUL_COLORS);

        ArrayList dates = new ArrayList<>();
        for (Map.Entry<Integer, Map<Integer, Double>> year : costsPerMonth.entrySet()) {
            for (Map.Entry<Integer, Double> monthSet : year.getValue().entrySet()) {
                String monthName = Month.of(monthSet.getKey() + 1).getDisplayName(TextStyle.FULL_STANDALONE, Locale.forLanguageTag("ru"));
                dates.add(monthName + " " + (year.getKey() + 1900));
            }
        }

        BarData data = new BarData(dates, dataSet);
        chartByPrice.setData(data);
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private void InitializePieChart(){
        Map<String, Double> costsPerKind = consumablesNotes.stream().collect(
                Collectors.groupingBy(n -> n.KindOfService.Name, Collectors.summingDouble(n -> n.Price)));

        ArrayList<Entry> prices = new ArrayList<>();

        int i=0;
        for(Double cost: costsPerKind.values()) {
                prices.add(new Entry(cost.floatValue(), i++));
        }

        PieDataSet dataSet = new PieDataSet(prices, "");

        ArrayList<String> kinds = new ArrayList<>();

        for(String kind: costsPerKind.keySet()){
            kinds.add(kind);
        }

        PieData data = new PieData(kinds, dataSet);

        chartByKindOfService.setData(data);
        dataSet.setColors(ColorTemplate.COLORFUL_COLORS);
    }
}
