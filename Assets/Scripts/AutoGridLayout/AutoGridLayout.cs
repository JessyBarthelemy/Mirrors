using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[ExecuteInEditMode]
[AddComponentMenu("Layout/Auto Grid Layout Group", 152)]
public class AutoGridLayout : GridLayoutGroup
{
    public bool m_ResizeCollider;
    public int m_ColumnCount;
    public float m_Scale;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        float iRow = Mathf.Ceil(rectChildren.Count / m_ColumnCount);
        
        Vector2 vSpacing = new Vector2(50 * rectTransform.rect.width / 800, 50 * rectTransform.rect.width / 800);
        spacing = vSpacing;

        float fHeight = (rectTransform.rect.height - ((iRow - 1) * (spacing.y))) - ((padding.top + padding.bottom));
        float fWidth = (rectTransform.rect.width - ((m_ColumnCount - 1) * (spacing.x))) - ( (padding.right + padding.left));
        Vector2 vSize = new Vector2(15, 15);//(fWidth / m_ColumnCount) * m_Scale, ((fWidth) / m_ColumnCount) * m_Scale);
        cellSize = vSize;

        if (m_ResizeCollider)
        {
            Dot[] dots = transform.GetComponentsInChildren<Dot>(true);
            CircleCollider2D collider;
            foreach (Dot dot in dots)
            {
                collider = dot.GetComponent<CircleCollider2D>();
                collider.radius = (fWidth / m_ColumnCount) / 2;
            }
        }
    }
}
