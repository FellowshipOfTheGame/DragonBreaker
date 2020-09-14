using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnergyController))]
public class EnergyControllerEditor : Editor
{
    public int expAmount = 10;
    public EnergyController controller = null;

    private void OnEnable()
    {
        controller = (EnergyController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        expAmount = EditorGUILayout.IntField("Exp amount to give", expAmount);
        if (GUILayout.Button("Give Experience"))
        {
            controller.addExperience(expAmount);
        }
    }
}
