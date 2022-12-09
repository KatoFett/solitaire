using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Animates a <see cref="Vector2"/> over time.
    /// </summary>
    public sealed class Vector2Animation : Animation<Vector2>
    {
        /// <summary>
        /// Creates a new <see cref="Vector2Animation"/>.
        /// </summary>
        /// <param name="start">The starting value.</param>
        /// <param name="end">The final value</param>
        /// <param name="setMethod">The method called to set the property value.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="callback">Callback when animation completes.</param>
        public Vector2Animation(Vector2 start, Vector2 end, Action<Vector2> setMethod, float duration, Action? callback = null)
            : base(start, end, setMethod, duration, callback) { }

        protected override void UpdateValue()
        {
            var val = Vector2.Lerp(StartValue, EndValue, Progress);
            SetMethod(val);
        }
    }
}
