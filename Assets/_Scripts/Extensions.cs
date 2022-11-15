using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        Color newColor = renderer.color;
        newColor.a = alpha;
        renderer.color = newColor;
        
        //Now we can just call SpriteRenderer.Fade(value);
    }
}
