using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseModel
{
    private long id;
    private string partname;
    private string material;
    private float volume;
    private float[] size = new float[3];

    public long Id { get; set; }
    public string Partname { get; set; }
    public string Material { get; set; }
    public float Volume { get; set; }
    public float[] Size { get; set; }

    public static DatabaseModel FromString(string data, char delimiter = ',')
    {
        string[] values = data.Split(delimiter);
        DatabaseModel model = new DatabaseModel();

        try
        {
            model.Id =  long.Parse(values[0]);
            model.Partname = values[1];
            model.Material = values[2];
            model.Volume = float.Parse(values[4]);
            model.Size = new float[3] { float.Parse(values[5]),
                                        float.Parse(values[6]), 
                                        float.Parse(values[7]) };

            return model;
        }catch(System.Exception e)
        {
            Debug.LogError(e);
        }


        return null;
    }
}
