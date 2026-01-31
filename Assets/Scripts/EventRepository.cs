using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectedEventArgs : EventArgs
{
    public int Value;

    public PickupCollectedEventArgs(int value)
    {
        Value = value;
    }
}

public class ActionPressedEventArgs : EventArgs
{
    public bool Value;

    public ActionPressedEventArgs(bool value)
    {
        Value = value;
    }
}


public static class EventRepository
{
    // https://www.youtube.com/watch?v=OuZrhykVytg&t=2s
    
    public static event EventHandler<PickupCollectedEventArgs> OnPickupCollected;
    public static event EventHandler<PickupCollectedEventArgs> OnKeyCollected;

    public static event EventHandler<ActionPressedEventArgs> OnActionKeyPressed;


    //public static Action OnKeyCollected;

    //public static Action OnMouseEnterButton;
    //public static Action OnMouseExitButton;
    //public static Action OnMouseSelectButton;

 

    public static void InvokeOnPickupCollected(int scoreValue, object sender)
    {
        OnPickupCollected?.Invoke(sender, new PickupCollectedEventArgs(scoreValue));
    }

    public static void InvokeOnKeyCollected(int scoreValue, object sender)
    {
        OnKeyCollected?.Invoke(sender, new PickupCollectedEventArgs(scoreValue));
    }

    public static void InvokeOnActionKeyPressed(bool isMaskOn)
    {
        OnActionKeyPressed?.Invoke(isMaskOn, new ActionPressedEventArgs(isMaskOn));
    }

  
}
