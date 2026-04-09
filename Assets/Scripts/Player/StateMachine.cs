public static class StateMachine
{
    private static MaskUse currentMaskState = MaskUse.MaskOff;
    private static Tile currentTile = Tile.Special; // uvek krece od specijalnog
    private static Mask currentMaskAvailability = Mask.Lost;
    private static PlayerControlls currentSuspendedInputState;
    public static AnyCoins currentCoinsState = AnyCoins.No;

    public static HelpMenu currentHelpMenuState = HelpMenu.Disabled;

    public static MaskUse GetMaskState() => currentMaskState;
    public static Tile GetCurrentTile() => currentTile;
    public static Mask GetMaskAvailability() => currentMaskAvailability;

    public static PlayerControlls GetPlayerInputState() => currentSuspendedInputState;

    public static AnyCoins GetCoinsState() => currentCoinsState;

    public static HelpMenu GetHelpMenuState() => currentHelpMenuState;

    public static Interaction currentInteraction = Interaction.Done;
    public static Interaction GetInteractionState() => currentInteraction;

    public static void SetInteractionState(Interaction newInteraction)
    {
        if (GetInteractionState() == newInteraction)
            return;

        currentInteraction = newInteraction;
    }

    public static void SetCoinsState(AnyCoins newState)
    {
        if (GetCoinsState() == newState)
            return;

        currentCoinsState = newState;
    }


    public static void SetMaskState(MaskUse newState)
    {
        if (GetMaskState() == newState)
            return;

        currentMaskState = newState;
    }


    public static void SetTile(Tile newTile)
    {
        if (GetCurrentTile() == newTile)
            return;

        currentTile = newTile;
    }


    public static void SetMask(Mask newMaskState)
    {
        if (GetMaskAvailability() == newMaskState)
            return;

        currentMaskAvailability = newMaskState;
    }


    public static void SetPlayerInputState(PlayerControlls newInput)
    {
        if (GetPlayerInputState() == newInput)
            return;

        currentSuspendedInputState = newInput;
    }

    public static void SetHelpMenuState(HelpMenu newState)
    {
        if (GetHelpMenuState() == newState)
            return;

        currentHelpMenuState = newState;
    }
}
