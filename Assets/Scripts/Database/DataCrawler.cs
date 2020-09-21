using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataCrawler : MonoBehaviour
{
    // Start is called before the first frame update
    SelectionManager selectionManager;
    DataRepository repository;

    void Start()
    {
        SessionEvents.current.OnSelectionAny += CrawlData;
        selectionManager = GetComponent<SelectionManager>();
        repository = GetComponent<DataRepository>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        SessionEvents.current.OnSelectionAny -= CrawlData;
    }

    private void CrawlData()
    {
        // Debug.Log("DATA CRAWLER");
        List<string> selection = selectionManager.GetSelection();

        Regex pattern = new Regex(@"^.*Body(?<id>\d+).*$"); 
        // Extract the number from the string id
        foreach (string selected in selection)
        {
            Match match = pattern.Match(selected);
            string val = match.Groups["id"].Value;
            // Debug.Log(match.Groups["id"].Value);
            repository.search(val);
        }

    }
}
