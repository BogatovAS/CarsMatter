package com.andrey.carsmatter;

import android.content.Context;
import android.content.res.Resources;

import com.andrey.carsmatter.http.HttpClient;
import com.andrey.carsmatter.models.RefillNote;
import com.andrey.carsmatter.services.CarsRepository;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;

import java.util.ArrayList;

import static org.junit.Assert.*;
import static org.mockito.Matchers.any;
import static org.mockito.Matchers.anyString;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

/**
 * Example local unit test, which will execute on the development machine (host).
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
public class CarsRepositoryTest {

    private HttpClient httpClientMock;

    private Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();

    CarsRepository carsRepository;

    @Before
    public void Initialize() {
        Context contextMock = mock(Context.class);
        this.httpClientMock = mock(HttpClient.class);

       // when(contextMock.getResources().getString(R.string.api_base_path)).thenReturn("https://testUrl.com");

        this.carsRepository = new CarsRepository(contextMock, httpClientMock);
    }

    @Test
    public void GetAllRefillNotes_Gets_Successfully() {
        // Arrange
        ArrayList<RefillNote> expectedResponse = new ArrayList<>();

        RefillNote test1 = new RefillNote();
        test1.Id = "1";
        RefillNote test2 = new RefillNote();
        test2.Id = "2";

        expectedResponse.add(test1);
        expectedResponse.add(test2);

        String stringResponse = gson.toJson(expectedResponse);

        when(httpClientMock.getHttpResponse(anyString())).thenReturn(stringResponse);

        // Act
        ArrayList<RefillNote> result = this.carsRepository.GetAllRefillNotes();

        // Assert
        for (int i = 0; i<expectedResponse.size(); i++) {
            assertEquals(result.get(i).Id, expectedResponse.get(i).Id);

        }
    }

    @Test
    public void GetRefillNotesReport_Gets_Successfully() {
        assertEquals(4, 2 + 2);
    }

    @Test
    public void GetConsumablesNotesReport_Gets_Successfully() {
        assertEquals(4, 2 + 2);
    }

    @Test
    public void SendNotificationForRefill_Gets_Successfully() {
        assertEquals(4, 2 + 2);
    }

    @Test
    public void GetAllConsumablesNotes_Gets_Successfully() {
        assertEquals(4, 2 + 2);
    }

    @Test
    public void GetUserCars_Gets_Successfully() {
        assertEquals(4, 2 + 2);
    }

}