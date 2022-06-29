using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class SailSpeedController : MonoBehaviour
{

    public WindZone wind;
    private float[] tblWindAngles;
    private int[] tblWindSpeeds;
    private string[][] lookupTable;

    // Start is called before the first frame update
    void Start()
    {
        string path = "/home/jonathan/Downloads/polar-table-example.csv";

        string[] lines = System.IO.File.ReadAllLines(path);

        tblWindSpeeds = lines[0].Split(';').Skip(1).Select(int.Parse).ToArray();

        tblWindAngles = new float[lines.Length - 1];
        lookupTable = new string[lines.Length - 1][]; 

        int idx = 0;
        foreach(string line in lines.Skip(1)) {
            string[] values = line.Split(';');
            tblWindAngles[idx] = Int32.Parse(values[0]);
            lookupTable[idx] = values.Skip(1).ToArray();

            idx++;
        }

        Debug.Log(GetMaxSpeed(4,32));

    }

    // Update is called once per frame
    void Update()
    {
        float absWindAngle = Vector3.Angle(wind.transform.forward, transform.right);
    }

    float GetMaxSpeed(float windSpeed, float windAngle) {
        int nearestWindSpeed = tblWindSpeeds.OrderBy(x => Math.Abs(x - windSpeed)).First();
        int windSpeedIndex = Array.BinarySearch(tblWindSpeeds, nearestWindSpeed);
        
        var windAngleIndex = Array.BinarySearch(tblWindAngles, windAngle);

        // if windspeed not in table, take linear interpolation
        if (windAngleIndex < 0) {
            int leftIndex = ~windAngleIndex - 1;
            int rightIndex = ~windAngleIndex;
        } else {
            return float.Parse(lookupTable[windAngleIndex][windSpeedIndex]);
        }

        // convert knots to m/s: * 0.51


        // return Math.Min(Math.Max(index, 0), array.Length - 2);
        return -1;

    }
}
