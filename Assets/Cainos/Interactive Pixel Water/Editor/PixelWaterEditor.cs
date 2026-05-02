using UnityEngine;
using UnityEditor;

namespace Cainos.InteractivePixelWater
{
    [CustomEditor(typeof(PixelWater))]
    public class PixelWaterEditor : Cainos.LucidEditor.LucidEditor
    {
        private PixelWater water;

        protected override void OnEnable()
        {
            base.OnEnable();

            water = (PixelWater)target;

            Undo.undoRedoPerformed += OnUndoRedo;
        }


        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            // Regenerate mesh after undo/redo operations
            if (water) water.GenerateMesh();
        }

        private void OnSceneGUI()
        {
            //wireframe cube
            Handles.color = Color.white;
            Vector3 center = water.transform.position + new Vector3(0.0f, water.size.y * 0.5f, 0.0f);
            Vector3 size = new Vector3(water.size.x, water.size.y, 0.1f);
            Handles.DrawWireCube(center, size);

            //handles for width and height
            float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
            Vector3 snap = Vector3.one * 0.1f;

            //corner handle position
            Vector3[] handlePos = new Vector3[4];
            handlePos[0] = center + new Vector3(-water.size.x * 0.5f, -water.size.y * 0.5f, 0.0f);        //BL
            handlePos[1] = center + new Vector3(water.size.x * 0.5f, -water.size.y * 0.5f, 0.0f);        //BR
            handlePos[2] = center + new Vector3(-water.size.x * 0.5f, water.size.y * 0.5f, 0.0f);        //TL
            handlePos[3] = center + new Vector3(water.size.x * 0.5f, water.size.y * 0.5f, 0.0f);        //TR

            //bottom left handle
            EditorGUI.BeginChangeCheck();
            Vector3 newBL = Handles.FreeMoveHandle(handlePos[0], Quaternion.identity, handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(water, "Resize Water");
                Undo.RecordObject(water.transform, "Move Water");

                water.Size = new Vector2(handlePos[1].x - newBL.x, handlePos[2].y - newBL.y);
                water.transform.position += new Vector3((newBL.x - handlePos[0].x) * 0.5f, (newBL.y - handlePos[0].y), 0.0f);
            }

            //bottom right handle
            EditorGUI.BeginChangeCheck();
            Vector3 newBR = Handles.FreeMoveHandle(handlePos[1], Quaternion.identity, handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(water, "Resize Water");
                Undo.RecordObject(water.transform, "Move Water");

                water.Size = new Vector2(newBR.x - handlePos[0].x, handlePos[3].y - newBR.y);
                water.transform.position += new Vector3((newBR.x - handlePos[1].x) * 0.5f, (newBR.y - handlePos[1].y), 0.0f);
            }

            //top left handle
            EditorGUI.BeginChangeCheck();
            Vector3 newTL = Handles.FreeMoveHandle(handlePos[2], Quaternion.identity, handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(water, "Resize Water");
                Undo.RecordObject(water.transform, "Move Water");

                water.Size = new Vector2(handlePos[3].x - newTL.x, newTL.y - handlePos[0].y);
                water.transform.position += new Vector3((newTL.x - handlePos[2].x) * 0.5f, 0.0f, 0.0f);
            }

            //top right
            EditorGUI.BeginChangeCheck();
            Vector3 newTR = Handles.FreeMoveHandle(handlePos[3], Quaternion.identity, handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(water, "Resize Water");
                Undo.RecordObject(water.transform, "Move Water");

                water.Size = new Vector2(newTR.x - handlePos[2].x, newTR.y - handlePos[1].y);
                water.transform.position += new Vector3((newTR.x - handlePos[3].x) * 0.5f, 0.0f, 0.0f);
            }


            if (GUI.changed)
            {
                water.GenerateMesh();
            }
        }
    }
}
