using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract partial class Scene
    {
        private static readonly HashSet<IEnumerator> _ActiveRoutines = new();

        /// <summary>
        /// Begins running an asynchronous task in the background.
        /// </summary>
        /// <param name="routine">The routine to run.</param>
        public static async void StartCoroutine(IEnumerator routine)
        {
            _ActiveRoutines.Add(routine);
            while (routine.MoveNext() && _ActiveRoutines.Contains(routine))
            {
                if (routine.Current is WaitForSeconds waiter)
                    await Task.Delay(waiter._WaitTime);
            }
            if (_ActiveRoutines.Contains(routine))
                _ActiveRoutines.Remove(routine);
        }

        /// <summary>
        /// Stops all coroutines immediately.
        /// </summary>
        public static void StopAllCoroutines()
        {
            _ActiveRoutines.Clear();
        }
    }
}