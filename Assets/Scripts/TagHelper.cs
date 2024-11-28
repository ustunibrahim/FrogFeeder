using UnityEngine;

public static class TagHelper
{
    public static void AssignColorAndTag(GameObject obj, Color color)
    {
        if (color == Color.green) obj.tag = "Green";
        else if (color == Color.red) obj.tag = "Red";
        else if (color == Color.yellow) obj.tag = "Yellow";
        else if (color == Color.blue) obj.tag = "Blue";
        else if (color == new Color(139, 0, 139)) obj.tag = "Purple";
        else obj.tag = "Untagged";
    }
}
