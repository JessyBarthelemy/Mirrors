using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MirroredDotsRenderer : DotsRenderer
{
    public bool OnDotCollision(int position)
    {
        if (CanAddDotPoint(dots[position]))
        {
            AddDotPoint(dots[position], true, true);
            return true;
        }

        return false;
    }

    public void OnDotClick()
    {
        isDrawing = true;
    }

    protected override bool ShouldDeleteLineRenderer()
    {
        return lineRenderers.Count > 1 && lineRenderers.Last().positionCount == 1;
    }
}
