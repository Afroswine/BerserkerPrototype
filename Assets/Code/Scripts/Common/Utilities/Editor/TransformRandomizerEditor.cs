using UnityEngine;
using UnityEditor;

public class TransformRandomizerEditor : EditorWindow
{
    bool _randomX, _randomY, _randomZ;
    bool _randomScale;
    float _minScale, _maxScale;

    [MenuItem("Utilities/Transform Randomizer")]
    static void Init()
    {
        TransformRandomizerEditor window = (TransformRandomizerEditor)GetWindow(typeof(TransformRandomizerEditor));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Randomize Selected Objects", EditorStyles.whiteLargeLabel);
        GUILayout.Space(10);

        GUILayout.Label("Rotations", EditorStyles.boldLabel);
        _randomX = EditorGUILayout.Toggle("    Randomize X", _randomX);
        _randomY = EditorGUILayout.Toggle("    Randomize Y", _randomY);
        _randomZ = EditorGUILayout.Toggle("    Randomize Z", _randomZ);
        GUILayout.Space(5);

        GUILayout.Label("Scaling", EditorStyles.boldLabel);
        _randomScale = EditorGUILayout.Toggle("    Randomize Scale", _randomScale);
        _minScale = EditorGUILayout.FloatField("    Minimum Scale", _minScale);
        _maxScale = EditorGUILayout.FloatField("    Maximum Scale", _maxScale);
        GUILayout.Space(5);

        if (GUILayout.Button("Randomize"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                go.transform.rotation = Quaternion.Euler(GetRandomRotations(go.transform.rotation.eulerAngles));

                if (_randomScale)
                {
                    float scaleVal = Random.Range(_minScale, _maxScale);
                    go.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
                }
            }
        }
    }

    private Vector3 GetRandomRotations(Vector3 currentRotation)
    {
        float x = _randomX ? Random.Range(0f, 360f) : currentRotation.x;
        float y = _randomY ? Random.Range(0f, 360f) : currentRotation.y;
        float z = _randomZ ? Random.Range(0f, 360f) : currentRotation.z;

        return new Vector3(x, y, z);
    }
}
