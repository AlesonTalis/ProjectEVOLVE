using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectEVOLVE.Core
{
    [CustomEditor(typeof(WorldViewer))]
    public class WorldViewerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WorldViewer viewer = (WorldViewer)target;

            if (viewer == null)
                return;

            if (GUILayout.Button("Generate Single Chunk"))
                viewer.GenerateSingleChunk();
        }
    }

}