using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public static class ANNEditorUitls
    {
        [MenuItem("Assets/Create/ANN/ANN Descriptor Object")]
        public static void CreateANNDescriptorObject()
        {
            ScriptableObjectUtility.CreateAsset<ANNDescriptorObject>();
        }

        [MenuItem("Assets/Create/ANN/ANN Training Set Descriptor")]
        public static void CreateANNTrainingSetDescriptor()
        {
            ScriptableObjectUtility.CreateAsset<TrainingSetDescriptor>();
        }
    }
}
