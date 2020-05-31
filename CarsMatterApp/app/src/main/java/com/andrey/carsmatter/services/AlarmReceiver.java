package com.andrey.carsmatter.services;

import android.app.NotificationChannel ;
import android.app.NotificationManager ;
import android.app.PendingIntent;
import android.app.Service ;
import android.content.Intent ;
import android.graphics.BitmapFactory;
import android.os.Handler ;
import android.os.IBinder ;

import androidx.annotation.Nullable;
import androidx.core.app.NotificationCompat;

import com.andrey.carsmatter.MainActivity;
import com.andrey.carsmatter.R;
import com.andrey.carsmatter.models.RememberedAccount;
import com.andrey.carsmatter.models.User;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Calendar;
import java.util.Timer ;
import java.util.TimerTask ;


public class NotificationService extends Service {
    public static final String NOTIFICATION_CHANNEL_ID = "10001";
    private final static String default_notification_channel_id = "default";
    Timer timer;
    TimerTask timerTask;
    int hoursForNextExecution = 24;
    CarsRepository carsRepository;
    String filePath;

    @Override
    public void onCreate() {
        super.onCreate();
        this.carsRepository = new CarsRepository(getApplicationContext());
        this.filePath = getApplicationContext().getFilesDir().getAbsolutePath() + File.separator + "creds.json";
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        super.onStartCommand(intent, flags, startId);
        startTimer();
        return START_STICKY;
    }

    @Override
    public void onDestroy() {
        stopTimerTask();
        super.onDestroy();
    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    //we are going to use a handler to be able to run in our TimerTask
    final Handler handler = new Handler();

    public void startTimer() {
        timer = new Timer();
        initializeTimerTask();
        // timer.schedule(timerTask, 5000, hoursForNextExecution * 1000 * 60 * 60); //
        timer.schedule(timerTask, 5000, 10 * 1000); //
    }

    public void stopTimerTask() {
        if (timer != null) {
            timer.cancel();
            timer = null;
        }
    }

    public void initializeTimerTask() {
        timerTask = new TimerTask() {
            public void run() {
                handler.post(new Runnable() {
                    public void run() {
                        createNotification();
                    }
                });
            }
        };
    }

    private void createNotification() {

        RememberedAccount account = this.GetRememberedAccount();

        if(account != null){
            User.setCurrentUser(account.Username, account.Password);
        }
        else{
            return;
        }

        new Thread(null, new Runnable() {
            @Override
            public void run() {
                boolean sendNotificationForRefill = carsRepository.SendNotificationForRefill();

                if (sendNotificationForRefill) {

                    Intent notificationIntent = new Intent(getApplicationContext(), MainActivity.class);
                    PendingIntent contentIntent = PendingIntent.getActivity(getApplicationContext(), 0, notificationIntent, PendingIntent.FLAG_CANCEL_CURRENT);

                    NotificationManager mNotificationManager = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);

                    NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(getApplicationContext(), default_notification_channel_id);

                    mBuilder.setContentTitle("Давайте заправимся");
                    mBuilder.setContentText("Не забудьте занести данные о вашей последней заправке в приложение");
                    mBuilder.setSmallIcon(R.mipmap.ic_launcher_foreground);
                    mBuilder.setLargeIcon(BitmapFactory.decodeResource(getResources(), R.mipmap.ic_launcher_round));
                    mBuilder.setAutoCancel(true);
                    mBuilder.setContentIntent(contentIntent);

                    if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
                        int importance = NotificationManager.IMPORTANCE_HIGH;
                        NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "Статус заправки", importance);
                        mBuilder.setChannelId(NOTIFICATION_CHANNEL_ID);
                        assert mNotificationManager != null;
                        mNotificationManager.createNotificationChannel(notificationChannel);
                    }
                    assert mNotificationManager != null;
                    mNotificationManager.notify((int) System.currentTimeMillis(), mBuilder.build());
                }
        }}).start();
    }

    private RememberedAccount GetRememberedAccount() {
        File file = new File(filePath);

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();

        RememberedAccount account;

        if (file.exists()) {
            try {
                InputStream fileReader = new FileInputStream(filePath);
                byte[] buffer = new byte[fileReader.available()];
                fileReader.read(buffer);
                String fileData = new String(buffer, StandardCharsets.UTF_8);
                account = gson.fromJson(fileData, RememberedAccount.class);

                if(Calendar.getInstance().getTime().getTime() > account.Date.getTime()){
                    file.delete();
                    return null;
                }

                return account;
            } catch (IOException e) {
                return null;
            }
        }
        return null;
    }
}