namespace GameEngine
{
    /// <summary>
    /// Pauses a CoRoutine for a duration.
    /// </summary>
    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds)
        {
            _WaitTime = (int)(seconds * 1000);
        }

        internal readonly int _WaitTime;
    }
}
