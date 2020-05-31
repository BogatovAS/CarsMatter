package com.andrey.carsmatter.services;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
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


public class AlarmReceiver extends BroadcastReceiver {
    public static final String NOTIFICATION_CHANNEL_ID = "10001";
    private final static String default_notification_channel_id = "default";
    CarsRepository carsRepository;
    String filePath;

    @Override
    public void onReceive(Context context, Intent intent) {
        this.carsRepository = new CarsRepository(context);
        this.filePath = context.getFilesDir().getAbsolutePath() + File.separator + "creds.json";

        createNotification(context);
    }

    private void createNotification(final Context context) {

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

                    Intent notificationIntent = new Intent(context, MainActivity.class);
                    PendingIntent contentIntent = PendingIntent.getActivity(context, 0, notificationIntent, PendingIntent.FLAG_CANCEL_CURRENT);

                    NotificationManager mNotificationManager = (NotificationManager) context.getSystemService(context.NOTIFICATION_SERVICE);

                    NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context, default_notification_channel_id);

                    mBuilder.setContentTitle("Давайте заправимся");
                    mBuilder.setContentText("Не забудьте занести данные о вашей последней заправке в приложение");
                    mBuilder.setSmallIcon(R.mipmap.ic_launcher_foreground);
                    mBuilder.setLargeIcon(BitmapFactory.decodeResource(context.getResources(), R.mipmap.ic_launcher_round));
                    mBuilder.setAutoCancel(true);
                    mBuilder.setStyle(new NotificationCompat.BigTextStyle().bigText("Не забудьте занести данные о вашей последней заправке в приложение"));
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