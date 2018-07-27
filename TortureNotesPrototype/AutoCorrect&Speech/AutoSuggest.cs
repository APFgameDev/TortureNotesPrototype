using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSuggest : MonoBehaviour
{
    /*//For Testing
    
    [SerializeField]
    string query;
    [SerializeField]
    string[] suggestions;

    private void Update()
    {
        QueryForSuggestions(query, (a) => { suggestions = a; });
    }
  
   */

    string prevQuery;

    public void QueryForSuggestions(string query, Action<string[]> callBackWithSuggestions)
    {
        //no need to constantly ask database about same query
        if (prevQuery == query)
            return;

        prevQuery = query;
        StartCoroutine(GetWebSuggestion(query, callBackWithSuggestions));
    }

    IEnumerator GetWebSuggestion(string query, Action<string[]> callBackWithSuggestions)
    {
        if (query == string.Empty)
            callBackWithSuggestions(new string[0]);
        else
        {
            using (WWW www = new WWW("https://api.datamuse.com/sug?s=" + query))
            {
                yield return www;

                string rawSuggestionQueryReturn = www.text;

                rawSuggestionQueryReturn = rawSuggestionQueryReturn.Remove(0, 1);
                rawSuggestionQueryReturn = rawSuggestionQueryReturn.Remove(rawSuggestionQueryReturn.Length - 1, 1);

                string[] suggestions = rawSuggestionQueryReturn.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < suggestions.Length; i++)
                {
                    string[] split = suggestions[i].Split(new string[] { "\":\"", "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                    suggestions[i] = split[1];
                }

                callBackWithSuggestions(suggestions);
            }
        }
    }
}
