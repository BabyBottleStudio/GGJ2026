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
    public bool isMaskOn;

    public ActionPressedEventArgs(bool value)
    {
        isMaskOn = value;
    }
}

public class SpecialTileEventArgs : EventArgs
{
    public Vector3 targetPosition;

    public SpecialTileEventArgs(Vector3 position)
    {
        targetPosition = position;
    }
}


public static class EventRepository
{
    // https://www.youtube.com/watch?v=OuZrhykVytg&t=2s

    public static event EventHandler<PickupCollectedEventArgs> OnPickupCollected;
    public static event EventHandler<PickupCollectedEventArgs> OnKeyCollected;

    public static event EventHandler<ActionPressedEventArgs> OnActionKeyPressed;

    public static Action OnLevelFinished;
    public static Action OnCutsceneEnd;

    public static event EventHandler<SpecialTileEventArgs> OnTileEnter;
    public static event EventHandler<SpecialTileEventArgs> OnTileExit;

    //public static Action OnKeyCollected;

    //public static Action OnMouseEnterButton;
    //public static Action OnMouseExitButton;
    //public static Action OnMouseSelectButton;

    public static void InvokeOnCutsceneEnd()
    {
        OnCutsceneEnd?.Invoke();
    }

    public static void InvokeOnEnterTile(object sender, Vector3 position)
    {
        OnTileEnter?.Invoke(sender, new SpecialTileEventArgs(position));

    }

    public static void InvokeOnExitTile(object sender, Vector3 position)
    {
        OnTileExit?.Invoke(sender, new SpecialTileEventArgs(position));

    }

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

    public static void InvokeOnLevelFinished()
    {
        OnLevelFinished?.Invoke();
    }
}
