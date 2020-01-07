using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Element : MonoBehaviour
{
    public enum ElementType
    {
        normal,
        goal,
        obstacle,
    }
    public ElementType type;
    public class Position
    {
        public int xPos;
        public int yPos;
        public Position(int x, int y)
        {
            xPos = x;
            yPos = y;
        }
        public void UpdatePosition(int xChange, int yChange)
        {
            xPos += xChange;
            yPos += yChange;
        }
    }
    public bool shouldRemove { get; private set; } = false;
    public Position positionInGrid { get; private set; }
    private TextMeshPro textMeshPro;
    public void InitElementFromText(string text)
    {
        textMeshPro = transform.Find("Text").gameObject.GetComponent<TextMeshPro>();
        string res = text;
        type = ElementType.normal;
        if (text.Contains(":"))
        {
            var texts = text.Split(':');
            switch(texts[0])
            {
                case "Goal":
                    {
                        type = ElementType.goal;
                        res = texts[1];
                    }
                    break;
            }
        }
        else if(int.TryParse(res, out int textInter))
        {
            switch (textInter)
            {
                case 0:
                    res = null;
                    break;
                case -1:
                    type = ElementType.obstacle;
                    res = null;
                    break;
            }
        }
        textMeshPro.text = res;
        IfShouldRemove();
    }
    private void IfShouldRemove()
    {
        if (type == ElementType.normal && string.IsNullOrEmpty(textMeshPro.text)) shouldRemove = true;
    }
    public void InitPosition(int xPos, int yPos)
    {
        positionInGrid = new Position(xPos, yPos);
    }
}
