using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class FetchGrafanaData : MonoBehaviour
{
    public string url = "http://localhost:3000/api/ds/query?ds_type=influxdb";
    public string apiToken = "";
    
    // Public fields for the GameObjects
    public GameObject powerObject1;
    public GameObject powerObject2;
    public GameObject co2Object;
    public GameObject temperatureObject;
    public GameObject smokeObject;

    private string queryPayload = @"
    {
        ""queries"": [
            {
                ""datasource"": {
                    ""type"": ""influxdb"",
                    ""uid"": ""ddonlgs1hz01sb""
                },
                ""query"": ""from(bucket: \""electrical system\"") |> range(start: -1h) |> filter(fn: (r) => r._measurement == \""power_meter\"") |> last()"",
                ""refId"": ""A"",
                ""datasourceId"": 1,
                ""intervalMs"": 1000,
                ""maxDataPoints"": 361
            },
            {
                ""datasource"": {
                    ""type"": ""influxdb"",
                    ""uid"": ""ddonlgs1hz01sb""
                },
                ""query"": ""from(bucket: \""electrical system\"") |> range(start: -1h) |> filter(fn: (r) => r._measurement == \""co2_sensor\"") |> last()"",
                ""refId"": ""B"",
                ""datasourceId"": 1,
                ""intervalMs"": 1000,
                ""maxDataPoints"": 361
            },
            {
                ""datasource"": {
                    ""type"": ""influxdb"",
                    ""uid"": ""ddonlgs1hz01sb""
                },
                ""query"": ""from(bucket: \""electrical system\"") |> range(start: -1h) |> filter(fn: (r) => r._measurement == \""temperature_sensor\"") |> last()"",
                ""refId"": ""C"",
                ""datasourceId"": 1,
                ""intervalMs"": 1000,
                ""maxDataPoints"": 361
            },
            {
                ""datasource"": {
                    ""type"": ""influxdb"",
                    ""uid"": ""ddonlgs1hz01sb""
                },
                ""query"": ""from(bucket: \""electrical system\"") |> range(start: -1h) |> filter(fn: (r) => r._measurement == \""smoke_detector\"") |> last()"",
                ""refId"": ""D"",
                ""datasourceId"": 1,
                ""intervalMs"": 1000,
                ""maxDataPoints"": 361
            }
        ]
    }";

    private void Start()
    {
        StartCoroutine(FetchDataCoroutine());
    }

    private IEnumerator FetchDataCoroutine()
    {
        while (true)
        {
            yield return FetchData();
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator FetchData()
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(queryPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                ProcessResponse(request.downloadHandler.text);
            }
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        var response = JObject.Parse(jsonResponse);

        // Process Power Value
        var powerValue = response["results"]["A"]["frames"][0]["data"]["values"][1][0].Value<float>();
        //Debug.Log("Power Value: " + powerValue);

        // Process CO2 Level
        var co2Value = response["results"]["B"]["frames"][0]["data"]["values"][1][0].Value<float>();
        //Debug.Log("CO2 Value: " + co2Value);

        // Process Temperature Values
        var tempValue = response["results"]["C"]["frames"][0]["data"]["values"][1][0].Value<float>();;
        //Debug.Log("Temperature Values: " + tempValue );

        // Process Smoke Detector Status
        var smokeStatus = response["results"]["D"]["frames"][0]["data"]["values"][1][0].Value<int>();
        //Debug.Log("Smoke Detector Status: " + smokeStatus);


	powerObject1.SendMessage("Update_Component", powerValue);
	powerObject2.SendMessage("Update_Component", powerValue);
	smokeObject.SendMessage("Update_Component", smokeStatus);
	temperatureObject.SendMessage("Update_Component", tempValue );
	co2Object.SendMessage("Update_Component", co2Value);

        // Update your game objects with the fetched data here
        // For example:
        // UpdatePowerValue(powerValue);
        // UpdateCO2Level(co2Value);
        // UpdateTemperatureValues(temperatureValues);
        // UpdateSmokeDetectorStatus(smokeStatus);
    }
}
