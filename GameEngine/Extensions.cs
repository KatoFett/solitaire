using System;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Rounds the <see cref="float"/> to the nearest whole <see cref="int"/>.
        /// </summary>
        /// <param name="f">The float to round.</param>
        /// <returns>A new <see cref="int"/> with the rounded value.</returns>
        public static int ToInt(this float f)
        {
            var result = (int)MathF.Round(f);
            return result;
        }

        /// <summary>
        /// Returns a random <see cref="float"/> that's at least <paramref name="minValue"/> and less than <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The minimum value inclusive.</param>
        /// <param name="maxValue">The maximum value exclusive.</param>
        public static float NextSingle(this Random rand, float minValue, float maxValue)
        {
            var result = minValue + (rand.NextSingle() * (maxValue - minValue));
            return result;
        }

        /// <summary>
        /// Gets a vector representation of a rotation.
        /// </summary>
        /// <param name="rotation">The rotation in degrees.</param>
        /// <returns>A unit vector containing the x- and y-components of the 2D rotation.</returns>
        public static Vector2 ToRotationVector(this float rotation)
        {
            var result = new Vector2(MathF.Cos(rotation.ToRadians()), MathF.Sin(rotation.ToRadians()));
            return result;
        }

        /// <summary>
        /// Rounds both values to the nearest whole number.
        /// </summary>
        /// <returns>
        /// A new <see cref="Vector2"/> whose values are whole numbers.
        /// </returns>
        public static Vector2 Round(this Vector2 vector)
        {
            return new Vector2(MathF.Round(vector.X, MidpointRounding.ToZero), MathF.Round(vector.Y, MidpointRounding.ToZero));
        }

        /// <summary>
        /// Converts a value in degrees into radians.
        /// </summary>
        /// <param name="degrees">The value in degrees to convert.</param>
        /// <returns>A float containing the rotation in radians.</returns>
        public static float ToRadians(this float degrees)
        {
            var result = degrees / 360f * 2 * MathF.PI;
            return result;
        }

    }
}
