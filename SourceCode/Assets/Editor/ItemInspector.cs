using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemInspector : Editor
{
    bool show;
    Rect spriteRect;

    public override void OnInspectorGUI()
    {
        Item Myscript = (Item)target;
        Myscript.Name = EditorGUILayout.TextField("Name", Myscript.Name);
        EditorGUILayout.Space(2);
        EditorGUILayout.PrefixLabel("Description");
        Myscript.Description = EditorGUILayout.TextArea(Myscript.Description, GUILayout.Height(50));
        Texture2D myTexture = AssetPreview.GetAssetPreview(Myscript.artwork);
        Texture texture = null;
        texture = null;
        if(Myscript.artwork != null)
        {
            texture = Myscript.artwork;
            
        }

        
        // "target" can be any class derrived from ScriptableObject 
        // (could be EditorWindow, MonoBehaviour, etc)
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("Spawnitems");
        SerializedProperty stringsProperty2 = so.FindProperty("Usableitems");
        SerializedProperty Artwork = so.FindProperty("artwork");
        SerializedProperty Rare = so.FindProperty("Rarity");



        EditorGUILayout.BeginHorizontal();
        //GUILayout.Label(myTexture);
        GUILayout.Box(texture, GUILayout.Width(32), GUILayout.Height(32));
        EditorGUILayout.PropertyField(Artwork, false); // True means show children
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(Rare, false); // True means show children

        //Myscript.Name = EditorGUILayout.TextField("Name", Myscript.Name);
        
        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }
}
