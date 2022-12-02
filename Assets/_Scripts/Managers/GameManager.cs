using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Box")]
    public List<BoxData> leftBoxesUnits;
    public List<BoxData> rightBoxesUnits;

    public void ResetScene()
    {
        // CCommand has some static members, so let`s make sure that there are no commands in the Queue
        Debug.Log("Scene reloaded");
        // reset all unit IDs
        //IDHolder.ClearIDHoldersList();
        //IDManager.ResetIDs();
        CCommand.CCommandQueue.Clear();
        CCommand.CommandExecutionComplete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
