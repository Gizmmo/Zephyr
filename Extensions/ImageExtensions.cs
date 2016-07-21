using UnityEngine;

namespace Zephyr.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Sets the alpha of this image to the passed value.
        /// </summary>
        /// <param name="image">ImageExtensions to change the alpha of.</param>
        /// <param name="val">Value to change alpha to, between 1 and 0.</param>
        public static void SetAlpha(this UnityEngine.UI.Image image, float val)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, val);
        }
    }
}
