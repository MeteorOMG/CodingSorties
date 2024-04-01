using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMenuDrawer : MonoBehaviour
{
    [SerializeField] private List<MenuItemHandler> itemsHandlers = new List<MenuItemHandler>();
    [SerializeField] private float elementsCount = 10;

    [Header("Selection")]
    [SerializeField] private int selectedElement = 0;
    [SerializeField] private bool active = true;

    [Header("Animation")]
    [SerializeField] private float animSpeed = 4;
    [SerializeField] private float elementSpeed = 12;
    [SerializeField] private float selectedRadius = -1;
    [SerializeField] private float idleRadius = 0;
    [SerializeField] private MenuAnimationParams activeParams;
    [SerializeField] private MenuAnimationParams inactiveParams;

    [Header("Colors")]
    [SerializeField] private Color idleColor = Color.red;
    [SerializeField] private Color selectedColor = Color.green;

    private float radius;
    private float extraRotation;
    private MenuAnimationParams currentTarget => active ? activeParams : inactiveParams;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < itemsHandlers.Count; i++)
        {
            var handler = itemsHandlers[i];
            Gizmos.color = handler.currentColor;
            handler.Update(elementSpeed);

            bool selected = selectedElement == i;
            handler.targetRadius = selected ? selectedRadius : idleRadius;
            handler.targetColor = selected ? idleColor : selectedColor;

            float x = (radius + handler.currentRadius) * Mathf.Cos((handler.angle + extraRotation) * Mathf.Deg2Rad);
            float y = (radius + handler.currentRadius) * Mathf.Sin((handler.angle + extraRotation) * Mathf.Deg2Rad);
            Gizmos.DrawWireSphere(new Vector3(x, y, 0f), 0.1f);
            Gizmos.DrawSphere(new Vector3(x, y, 0f), 0.1f);
        }

        UpdateAnimParams();
    }

    private void UpdateAnimParams()
    {
        if (currentTarget == null)
            return;

        radius = Mathf.Lerp(radius, currentTarget.targetRadius, animSpeed * Time.deltaTime);
        extraRotation = Mathf.Lerp(extraRotation, currentTarget.targetExtraRotation, animSpeed * Time.deltaTime);
    }

    [ContextMenu("Refres")]
    private void Refresh()
    {
        itemsHandlers.Clear();

        float anglePerElement = 360f / elementsCount;
        for (int i = 0; i < elementsCount; i++)
            itemsHandlers.Add(new MenuItemHandler(i, i * anglePerElement));
    }

    private void Reset()
    {
        Refresh();
    }
}

[System.Serializable]
public class MenuItemHandler
{
    public int index;
    public float angle;

    public float targetRadius;
    public float currentRadius;

    public Color targetColor;
    public Color currentColor;

    public MenuItemHandler(int index, float angle)
    {
        this.index = index;
        this.angle = angle;
    }

    public void Update(float speed)
    {
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, speed * Time.deltaTime);
        currentColor = Color.Lerp(currentColor, targetColor, speed * Time.deltaTime);
    }
}

[System.Serializable]
public class MenuAnimationParams
{
    public float targetRadius = 1;
    public float targetExtraRotation = 0;
}
