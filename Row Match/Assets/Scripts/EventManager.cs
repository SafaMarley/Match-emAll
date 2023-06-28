using System;

public class EventManager : MonoSingleton<EventManager>
{
    public static Action OnPlayerMove;
}
