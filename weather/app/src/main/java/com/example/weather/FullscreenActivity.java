package com.example.weather;

import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;  // <-- Add this import for logging
import android.view.KeyEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class FullscreenActivity extends AppCompatActivity {

    private EditText cityEditText;
    private Button fetchWeatherButton;
    private TextView weatherTextView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_fullscreen);

        cityEditText = findViewById(R.id.cityEditText);
        fetchWeatherButton = findViewById(R.id.fetchWeatherButton);
        weatherTextView = findViewById(R.id.weatherTextView);

        fetchWeatherButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String city = cityEditText.getText().toString();
                new WeatherFetchTask().execute(city);
            }
        });

        cityEditText.setOnKeyListener(new View.OnKeyListener() {
            @Override
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                if ((event.getAction() == KeyEvent.ACTION_DOWN) && (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    String city = cityEditText.getText().toString();
                    new WeatherFetchTask().execute(city);
                    return true;
                }
                return false;
            }
        });
    }

    private class WeatherFetchTask extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            String city = params[0];
            try {
                return fetchWeatherData(city);
            } catch (IOException e) {
                Log.e("EXCEPTION", "Error in doInBackground:", e);  // <-- Log the exception
                e.printStackTrace();
                return null;
            }
        }

        @Override
        protected void onPostExecute(String result) {
            Log.d("ON_POST_EXECUTE", "Result: " + result);  // <-- Log the result

            if (result != null) {
                try {
                    JSONObject jsonObject = new JSONObject(result);

                    String currentCondition = jsonObject.getString("currentCondition");
                    int maxTemp = jsonObject.getInt("maxTemp");
                    int minTemp = jsonObject.getInt("minTemp");
                    String windDirection = jsonObject.getString("windDirection");
                    int windSpeed = jsonObject.getInt("windSpeed");
                    String outlook = jsonObject.getString("outlook");

                    // Update UI with weather information
                    String weatherInfo = getString(R.string.current_condition) + ": " + currentCondition + "\n" +
                            getString(R.string.max_temp) + ": " + maxTemp + "°C\n" +
                            getString(R.string.min_temp) + ": " + minTemp + "°C\n" +
                            getString(R.string.wind) + ": " + windDirection + " " + getString(R.string.at) + " " + windSpeed + " km/h\n" +
                            getString(R.string.outlook) + ": " + outlook;

                    weatherTextView.setText(weatherInfo);
                } catch (JSONException e) {
                    Log.e("EXCEPTION", "Error in JSON parsing:", e);  // <-- Log the exception
                    e.printStackTrace();
                }
            } else {
                weatherTextView.setText(getString(R.string.cannot_find_location));
            }
        }
    }

    private String fetchWeatherData(String city) throws IOException {
        URL url = new URL("http://192.168.0.10:7269/Weather/" + city);
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();

        try {
            InputStream stream = connection.getInputStream();
            BufferedReader reader = new BufferedReader(new InputStreamReader(stream));
            StringBuilder response = new StringBuilder();
            String line;

            while ((line = reader.readLine()) != null) {
                response.append(line);
            }

            Log.d("API_RESPONSE", response.toString());  // <-- Log the API response
            return response.toString();
        } finally {
            connection.disconnect();
        }
    }
}
