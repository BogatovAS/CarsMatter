package com.andrey.carsmatter.db;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import org.json.JSONException;
import org.json.JSONObject;

public class DBHelper extends SQLiteOpenHelper {

    public static final int DATABASE_VERSION = 1;
    public static final String DATABASE_NAME = "filmsDB";
    public static final String TABLE_FILMS = "films";

    public static final String KEY_ID = "_id";
    public static final String KEY_TITLE = "title";
    public static final String KEY_GENRE = "genre";
    public static final String KEY_DATE = "release_date";
    public static final String KEY_DESCRIPTION = "description";
    public static final String KEY_AVERAGE_VOTE = "average_vote";
    public static final String KEY_IMAGE = "image";

    public DBHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("create table " + TABLE_FILMS + "("
                + KEY_ID + " integer primary key,"
                + KEY_TITLE + " text UNIQUE ON CONFLICT ABORT,"
                + KEY_GENRE + " text,"
                + KEY_DATE + " text,"
                + KEY_DESCRIPTION + " text,"
                + KEY_AVERAGE_VOTE + " text,"
                + KEY_IMAGE + " text"
                + ")");

    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("drop table if exists " + TABLE_FILMS);
        onCreate(db);
    }

    public Cursor getFilm(String title) throws SQLException {
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor mCursor = db.query(true, TABLE_FILMS,
                new String[]{KEY_ID, KEY_TITLE, KEY_GENRE,
                        KEY_DATE, KEY_DESCRIPTION, KEY_AVERAGE_VOTE, KEY_IMAGE}, KEY_TITLE + " = '" + title + "'", null,
                null, null, null, null);
        if (mCursor != null) {
            mCursor.moveToFirst();
        }
        return mCursor;
    }

    public boolean isFilmExist(String title) throws SQLException {
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor mCursor = db.query(true, TABLE_FILMS,
                new String[]{KEY_ID, KEY_TITLE, KEY_GENRE,
                        KEY_DATE, KEY_DESCRIPTION, KEY_AVERAGE_VOTE, KEY_IMAGE}, KEY_TITLE + " = '" + title + "'", null,
                null, null, null, null);
        if (mCursor != null && mCursor.moveToNext()) {
            db.close();
            return true;
        }
        db.close();
        return false;
    }

    public boolean deleteFilm(String title) throws SQLException {
        SQLiteDatabase db = this.getWritableDatabase();
        int result = db.delete(TABLE_FILMS, KEY_TITLE + " = '" + title + "'", null);
        db.close();
        if (result == -1) {
            return false;
        } else {
            return true;
        }
    }

    public Cursor getFullTable() {
        SQLiteDatabase db = this.getWritableDatabase();
        return db.query(TABLE_FILMS, new String[]{KEY_ID,
                        KEY_TITLE, KEY_GENRE, KEY_DATE, KEY_DESCRIPTION, KEY_AVERAGE_VOTE, KEY_IMAGE}, null,
                null, null, null, null);
    }

    public long insertValues(JSONObject film) {
        ContentValues values = new ContentValues();

        try {
            values.put(KEY_TITLE, film.get("title").toString());
            values.put(KEY_AVERAGE_VOTE, film.get("vote_average").toString());
            values.put(KEY_DATE, film.get("release_date").toString());
            values.put(KEY_DESCRIPTION, film.get("overview").toString());

            values.put(KEY_GENRE, film.get("genre").toString());
            values.put(KEY_IMAGE, film.get("image").toString());

            SQLiteDatabase db = this.getWritableDatabase();

            long row = db.insertWithOnConflict(TABLE_FILMS, null, values, SQLiteDatabase.CONFLICT_ABORT);
            db.close();
            return row;
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return -1;
    }
}
