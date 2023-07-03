using System;
using Managers.Base;

namespace Managers.LevelScene
{
    public class EventManager : MonoSingleton<EventManager>
    {
        public static Action OnPlayerMove;
        public static Action OnPlayerScore;
    }
}
