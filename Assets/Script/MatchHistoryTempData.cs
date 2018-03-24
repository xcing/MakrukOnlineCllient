using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MatchHistoryTempData : MonoBehaviour
{
    public int matchId;
    public int matchOrdinal;

    public void WatchReplay()
    {
        GameObject.Find("MatchHistoryScript").GetComponent<MatchHistory>().SetCurrentMatchId(matchId, matchOrdinal);
    }
}



