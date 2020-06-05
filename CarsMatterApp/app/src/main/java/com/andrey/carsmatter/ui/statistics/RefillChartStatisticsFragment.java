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
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.models.RefillNotesReport;
import com.andrey.carsmatter.services.CarsRepository;
import com.github.mikephil.charting.charts.BarChart;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarDataSet;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.utils.ColorTemplate;

import java.time.Month;
import java.time.format.TextStyle;
import java.util.ArrayList;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;

public class RefillChartStatisticsFragment extends Fragment {
    private CarsRepository carsRepository;
    private ArrayList<RefillNote> refillNotes = new ArrayList<>();
    private BarChart refillChart;

    @RequiresApi(api = Build.VERSION_CODES.O)
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        this.carsRepository = new CarsRepository(getContext());

        new Thread(null, () -> {
            refillNotes = carsRepository.GetAllRefillNotes();
            this.InitializeBarChart();
        }).start();
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_refill_statistics, container, false);

        this.refillChart = view.findViewById(R.id.refill_chart);
        refillChart.animateXY(2000, 2000);
        this.refillChart.setDescription("");

        TextView totalPriceTextView = view.findViewById(R.id.refill_statistics_total_price);
        TextView pricePerDayTextView = view.findViewById(R.id.refill_statistics_price_per_day);
        TextView pricePerKmTextView = view.findViewById(R.id.refill_statistics_price_per_km);
        TextView totalVolumeTextView = view.findViewById(R.id.refill_statistics_total_volume);
        TextView averageVolumeTextView = view.findViewById(R.id.refill_statistics_average_volume);

        new Thread(null, () -> {
            RefillNotesReport report = carsRepository.GetRefillNotesReport();
            totalPriceTextView.setText(String.format("%.2f руб", report.TotalCost));
            pricePerDayTextView.setText(String.format("%.2f руб", report.CostPerDay));
            pricePerKmTextView.setText(String.format("%.2f руб", report.CostPerKm));
            totalVolumeTextView.setText(String.format("%.2f л", report.TotalVolume));
            averageVolumeTextView.setText(String.format("%.2f л/100км", report.AverageVolume));
        }).start();

        return view;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private void InitializeBarChart() {
        Map<Integer, Map<Integer, Double>> costsPerMonth = refillNotes.stream().collect(
                Collectors.groupingBy(n -> n.Date.getYear(), Collectors.groupingBy(n -> n.Date.getMonth(), Collectors.summingDouble(n -> n.Price))));

        ArrayList<BarEntry> prices = new ArrayList<>();

        int i=0;
        for(Map<Integer, Double> monthSet: costsPerMonth.values()) {
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
        refillChart.setData(data);
    }
}
