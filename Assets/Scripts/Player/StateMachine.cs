public static class StateMachine
{
    private static State currentState = State.MaskOff;
    private static Tile currentTile = Tile.Special; // uvek krece od specijalnog
    private static Mask currentMask = Mask.Lost;
    private static PlayerControlls currentSuspendedInputState;

    public static State GetCurrentState() => currentState;
    public static Tile GetCurrentTile() => currentTile;
    public static Mask GetCurrentMask() => currentMask;

    public static PlayerControlls GetPlayerInputState() => currentSuspendedInputState;


    public static void SetState(State newState)
    {
        if (GetCurrentState() == newState)
            return;

        currentState = newState;
    }


    public static void SetTile(Tile newTile)
    {
        if (GetCurrentTile() == newTile)
            return;

        currentTile = newTile;
    }


    public static void SetMask(Mask newMaskState)
    {
        if (GetCurrentMask() == newMaskState)
            return;

        currentMask = newMaskState;
    }


    public static void SetPlayerInputState(PlayerControlls newInput)
    {
        if (GetPlayerInputState() == newInput)
            return;

        currentSuspendedInputState = newInput;
    }
}
