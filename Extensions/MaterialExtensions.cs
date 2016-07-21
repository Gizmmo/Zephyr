using UnityEngine;

namespace Zephyr.Extensions
{
    public static class MaterialExtensions
    {
        /// <summary>Change the Alpha of a Material</summary>
        /// <param name="mat">mat Desired Material to change</param>
        /// <param name="alpha">alpha Alpha to set material to</param>
        public static void ChangeAlpha(this Material mat, float alpha)
        {
            var oldColor = mat.color;
            var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            mat.SetColor("_Color", newColor);
        }
    }
}
