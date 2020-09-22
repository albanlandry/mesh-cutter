using System.Text.RegularExpressions; 
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataRepository : MonoBehaviour
{
    string repositoryPath = "Assets/Raws/Files/Fuel_channel_C.csv";
    private Dictionary<long, DatabaseModel> models = new Dictionary<long, DatabaseModel>();
    private FileDataBase dbHandler;
    int lineSkip = 1;
    void Start()
    {
        dbHandler = new FileDataBase(repositoryPath);
        PopulateRepository(dbHandler, models);
    }

    private void PopulateRepository(FileDataBase dbHandler, Dictionary<long, DatabaseModel> models)
    {
        int count = 0;
        foreach(string line in dbHandler.ReadLine())
        {
            // We skip the first 2 lines
            if (count > lineSkip)
            {
                // Debug.Log(line);
                DatabaseModel model = DatabaseModel.FromString(line);
                models.Add(model.Id, model);
            }

            count++;
        }
    }

    public DatabaseModel search(string q)
    {
        string regex = string.Format("^Fuel Channel_C-{0}$", q);
        Regex pattern = new Regex(regex);

        try {
            long key = long.Parse(q);
            return models.ContainsKey(key) ? models[key] : null;
        } catch(Exception e)
        { Debug.LogError(e.StackTrace); }

        return null;
    }
}
