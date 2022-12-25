using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat command
public class CCommand
{
    public static Queue<CCommand> CCommandQueue = new Queue<CCommand>();
    public static bool playingQueue = false;

    public virtual void AddToQueue()
    {
        CCommandQueue.Enqueue(this);
        if (!playingQueue)
            PlayFirstCommandFromQueue();
    }

    public virtual void StartCommandExecution()
    {
        // list of everything that we have to do with this command (draw a card, play a card, play spell effect, etc...)
        // there are 2 options of timing : 
        // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
        // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
    }

    //Call this when the command is finished and ready to start the next.
    //In some command you won't call this in StartCommandExection() but you call it in other script with Command.CommandExecutionComplete()
    public static void CommandExecutionComplete()
    {
        if (CCommandQueue.Count > 0)
            PlayFirstCommandFromQueue();
        else
            playingQueue = false;
        //if (TurnManager.Instance.whoseTurn != null)
        //    TurnManager.Instance.whoseTurn.HighlightPlayableCards();
    }

    public static void PlayFirstCommandFromQueue()
    {
        playingQueue = true;
        //Debug.Log("Play : " + CCommandQueue.Peek().GetType().ToString());
        CCommandQueue.Dequeue().StartCommandExecution();
    }
}
