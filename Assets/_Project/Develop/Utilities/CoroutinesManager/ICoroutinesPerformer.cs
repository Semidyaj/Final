using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Utilities.CoroutinesManager
{
    public interface ICoroutinesPerformer
    {
        Coroutine StartPerform(IEnumerator coroutineFunction);
        void StopPerform(Coroutine coroutine);
    }
}