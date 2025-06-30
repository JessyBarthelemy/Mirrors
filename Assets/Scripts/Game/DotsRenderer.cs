using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public abstract class DotsRenderer : MonoBehaviour
{
    private Material material;
    private Material mirroredMaterial;
    protected List<LineRenderer> lineRenderers;
    
    protected bool isTop;
    public Dot[] dots;
    public GameObject dotPrefab;
    protected string level;

    protected bool isInversed;
    protected List<int> current;
    protected int endPosition;
    public bool IsWon { get; private set; }

    protected bool isDrawing;
    protected int startIndex;
    public string Daltonian;

    public void Init(bool isTop, string inlineDots, Dot[] topDots = null, bool instanciate = true)
    {
        isInversed = false;
        string materialId = "";
        string mirroredMaterialId = "";
        
        if(isTop){
            materialId = "Materials/TopLine";
            mirroredMaterialId = "Materials/BottomLine";
        }else{
            materialId = "Materials/BottomLine";
            mirroredMaterialId = "Materials/TopLine";
        }

        if(ParameterReader.Instance.IsDaltonianModeParam){
            Daltonian = "Daltonian";
            materialId += Daltonian;
            mirroredMaterialId += Daltonian;
        }
        
        material = Resources.Load<Material>(materialId);
        mirroredMaterial = Resources.Load<Material>(mirroredMaterialId);

        lineRenderers = new List<LineRenderer>();
        AddLineRenderer(material);
        current = new List<int>();

        IsWon = false;
        level = inlineDots;
        this.isTop = isTop;

        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = LevelLoader.currentLevel.columnCount;

        if(LevelLoader.currentLevel.columnCount > 6 || LevelLoader.currentLevel.rowCount > 5)
            grid.cellSize = new Vector2(25, 25);

        int mirroredRow = 0;
        int currentIndex = 0;
        bool instantiate = false;
        if (dots == null || dots.Length == 0)
        {
            instantiate = true;
            dots = new Dot[inlineDots.Length];
        }
        Vector3 dotPosition = new Vector3(0, 0, -20);
        for (int i = 0; i < LevelLoader.currentLevel.rowCount; i++)
        {
            for (int j = 0; j < LevelLoader.currentLevel.columnCount; j++)
            {
                currentIndex = (LevelLoader.currentLevel.columnCount * i) + j;
                if (instantiate)
                {
                    dots[currentIndex] = Instantiate(dotPrefab.transform, dotPosition, Quaternion.identity, transform).GetComponent<Dot>();
                    dots[currentIndex].transform.localPosition = dotPosition;

                    if (isTop)
                    {
                        mirroredRow = LevelLoader.currentLevel.rowCount - 1 - i;

                        switch (LevelLoader.currentLevel.mode)
                        {
                            case 1:
                                dots[currentIndex].Order = dots.Length - 1 - currentIndex;
                                break;
                            default:
                                dots[currentIndex].Order = topDots[(LevelLoader.currentLevel.columnCount * mirroredRow) + j].Order;
                                break;
                        }
                    }
                    else
                    {
                        dots[currentIndex].Order = currentIndex;
                    }
                }
                    
                if (inlineDots[currentIndex] == Dot.STATE_START)
                {
                    current.Add(currentIndex);
                    dots[currentIndex].IsPicked = true;
                    dots[currentIndex].SetAnimation(DotAnimation.Selected);
                    startIndex = currentIndex;
                }
                else if (inlineDots[currentIndex] == Dot.STATE_END || inlineDots[currentIndex] == Dot.STATE_END_INV)
                {
                    dots[currentIndex].IsPicked = false;
                    endPosition = currentIndex;
                    dots[currentIndex].SetAnimation(DotAnimation.Idle);
                }
                else
                {
                    dots[currentIndex].IsPicked = false;
                    dots[currentIndex].SetAnimation(DotAnimation.Idle);
                }

                dots[currentIndex].Init(this, currentIndex, inlineDots[currentIndex], isTop);
            }
        }
    }

    protected virtual void SetStartPoint()
    {
        RectTransform rectTransform = dots[startIndex].GetComponent<RectTransform>();
        Vector3 point = rectTransform.TransformPoint(rectTransform.rect.center);
        point.z = 100;
        lineRenderers[0].SetPosition(0, point);
    }

    public virtual void AddDotPoint(Dot addedDot, bool isMirrored, bool checkTeleport)
    {
        if (isMirrored || addedDot.State != Dot.STATE_TELEPORT)
            lineRenderers.Last().positionCount++;
        RectTransform rectTransform = addedDot.GetComponent<RectTransform>();
        addedDot.IsPicked = true;
        addedDot.UpdateUI(isInversed);
        Vector3 point = rectTransform.TransformPoint(rectTransform.rect.center.x, rectTransform.rect.center.y, 0);
        point.z = 100;
        //center the current point
        if (!isMirrored)
            lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - (addedDot.State != Dot.STATE_TELEPORT ? 2 : 1), point);

        if (current != null)
        {
            dots[current.Last()].SetAnimation(DotAnimation.Idle);
            current.Add(addedDot.Id);
        }

        //center the next point
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, point);

        if (current != null)
        {
            addedDot.SetAnimation(DotAnimation.Selected);
        }

        if (addedDot.Id == endPosition)
        {
            IsWon = true;
            for (int i = 0; i < dots.Length && IsWon; i++)
            {
                if ((dots[i].State == Dot.STATE_GOAL || dots[i].State == Dot.STATE_GOAL_INV
                    || dots[i].State == Dot.STATE_END || dots[i].State == Dot.STATE_END_INV)
                    && !dots[i].IsPicked)
                {
                    IsWon = false;
                }
            }
        }
        
        if (addedDot.State == Dot.STATE_INV || (checkTeleport && addedDot.State == Dot.STATE_TELEPORT))
        {
            if (addedDot.State == Dot.STATE_INV)
            {
                if(!isMirrored)
                    lineRenderers.Last().positionCount--;
                isInversed = !isInversed;
                AddLineRenderer(isInversed ? mirroredMaterial : material, true);

                lineRenderers.Last().SetPosition(0, point);
                if (!isMirrored)
                {
                    lineRenderers.Last().positionCount++;
                    lineRenderers.Last().SetPosition(1, point);
                }
            }
            else if (addedDot.State == Dot.STATE_TELEPORT)
            {
                AddLineRenderer(isInversed ? mirroredMaterial : material, false);

                bool found = false;
                Dot teleportedDot = null;
                for (int i = 0; i < dots.Length && !found; i++)
                {
                    if (dots[i].State == Dot.STATE_TELEPORT && dots[i].Id != addedDot.Id)
                    {
                        found = true;
                        teleportedDot = dots[i];
                    }
                }

                isDrawing = false;
                AddDotPoint(teleportedDot, true, false);
            }
        }
    }

    protected bool CanAddDotPoint(Dot dot)
    {
        bool isSameColor = false;
        if (isInversed)
            isSameColor = dot.State != Dot.STATE_END && dot.State != Dot.STATE_GOAL;
        else
            isSameColor = dot.State != Dot.STATE_END_INV && dot.State != Dot.STATE_GOAL_INV;

        int last = current.Last();
        //avoid diagonal overlap
        if (dot.Id == (last - LevelLoader.currentLevel.columnCount - 1))
        {
            for (int i = 0; i < (current.Count -1); i++)
            {
                if ((current[i] == (last - 1) || current[i] == (last - LevelLoader.currentLevel.columnCount))
                    && (current[i + 1] == (last - 1) || current[i + 1] == (last - LevelLoader.currentLevel.columnCount)))
                    return false;
            }
        }
        else if (last == (dot.Id - LevelLoader.currentLevel.columnCount - 1))
        {
            for (int i = 0; i < (current.Count - 1); i++)
            {
                if ((current[i] == (dot.Id - 1) || current[i] == (dot.Id - LevelLoader.currentLevel.columnCount))
                    && (current[i + 1] == (dot.Id - 1) || current[i + 1] == (dot.Id - LevelLoader.currentLevel.columnCount)))
                    return false;
            }
        }
        else if (dot.Id == (last - LevelLoader.currentLevel.columnCount + 1))
        {
            for (int i = 0; i < (current.Count - 1); i++)
            {
                if ((current[i] == (dot.Id + 1) || current[i] == (dot.Id - LevelLoader.currentLevel.columnCount - 1))
                    && (current[i + 1] == (dot.Id + 1) || current[i + 1] == (dot.Id - LevelLoader.currentLevel.columnCount - 1)))
                    return false;
            }
        }
        else if (last == (dot.Id - LevelLoader.currentLevel.columnCount + 1))
        {
            for (int i = 0; i < (current.Count - 1); i++)
            {
                if ((current[i] == (last+ 1) || current[i] == (last - LevelLoader.currentLevel.columnCount - 1))
                    && (current[i + 1] == (last + 1) || current[i + 1] == (last - LevelLoader.currentLevel.columnCount - 1)))
                    return false;
            }
        }


        return !dot.IsPicked && dot.State != Dot.STATE_INACTIVE
             && ((dot.Id % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last + 1)
             || (last % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last - 1)
             || dot.Id == last + LevelLoader.currentLevel.columnCount
             || dot.Id == last - LevelLoader.currentLevel.columnCount
             || (dot.Id % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last + (LevelLoader.currentLevel.columnCount + 1))
             || (last % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last - (LevelLoader.currentLevel.columnCount + 1))
             || (last % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last + (LevelLoader.currentLevel.columnCount - 1))
             || (dot.Id % LevelLoader.currentLevel.columnCount != 0 && dot.Id == last - (LevelLoader.currentLevel.columnCount - 1))
             || (dot.State == Dot.STATE_TELEPORT && dots[current.Last()].State == Dot.STATE_TELEPORT))
             && (isSameColor || dot.State == Dot.STATE_TELEPORT);
    }

    public virtual void ResetLevel()
    {
        if (lineRenderers != null)
        {
            for (int i = lineRenderers.Count - 1; i >= 0; i--)
            {
                Destroy(lineRenderers[i].gameObject);
            }
        }

        Init(isTop, level);
        SetStartPoint();
    }

    public virtual void OnDotClick(Dot dot) { }

    private void AddLineRenderer(Material selectedMaterial, bool initPosition)
    {
        GameObject holder = new GameObject("gameObejct-" + (lineRenderers.Count));
        LineRenderer lineRenderer = holder.AddComponent<LineRenderer>();
        holder.transform.SetParent(transform, false);
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = selectedMaterial;
        lineRenderer.numCapVertices = 1;
        lineRenderer.numCornerVertices = 1;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.positionCount = initPosition ? 1 : 0;
        lineRenderer.startWidth = 5;
        lineRenderer.endWidth = 5;

        lineRenderers.Add(lineRenderer);
    }

    private void AddLineRenderer(Material selectedMaterial)
    {
        AddLineRenderer(selectedMaterial, true);
    }

    public void RemoveLastDot()
    {
        Dot dot = dots[current.Last()];

        //remove last position
        if (ShouldDeleteLineRenderer())
        {
            lineRenderers.RemoveAt(lineRenderers.Count - 1);
            Destroy(transform.Find("gameObejct-" + (lineRenderers.Count)).gameObject);

            if (dot.State == Dot.STATE_TELEPORT)
            {
                dot.IsPicked = false;
                dot.UpdateUI(isInversed);
                dot.SetAnimation(DotAnimation.Idle);
                isDrawing = false;
                current.RemoveAt(current.Count - 1);
            }
        }

        lineRenderers.Last().positionCount--;

        dot = dots[current.Last()];
        if (dot.State == Dot.STATE_INV)
            isInversed = !isInversed;

        dot.IsPicked = false;
        dot.UpdateUI(isInversed);

        dot.SetAnimation(DotAnimation.Idle);
        current.RemoveAt(current.Count - 1);

        dots[current.Last()].SetAnimation(DotAnimation.Selected, true);
    }

    protected abstract bool ShouldDeleteLineRenderer();
}
 