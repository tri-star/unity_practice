using System;
using System.Collections;
using System.Collections.Generic;
using ActionSample.Components.Ui.StringAnimation;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnimationString : MonoBehaviour
{
    [SerializeField]
    private GameObject textObject;

    private TextMeshPro tmp;

    private int length;

    private Character[] characters = null;

    public void Start()
    {
        if (textObject == null)
        {
            textObject = gameObject;
        }
        tmp = textObject.GetComponent<TextMeshPro>();
    }


    public void FixedUpdate()
    {
        if (characters == null && tmp.textInfo.wordCount > 0)
        {
            Init();
        }
        if (characters == null)
        {
            return;
        }

        foreach (var c in characters)
        {
            c.UpdateVerticies(tmp.textInfo.meshInfo[0]);
        }

        tmp.UpdateVertexData();
    }


    public void Init()
    {
        length = tmp.textInfo.wordCount;
        characters = new Character[length];
        for (int i = 0; i < length; i++)
        {
            var index = tmp.textInfo.characterInfo[i].vertexIndex;
            characters[i] = new Character(tmp.textInfo.characterInfo[i], tmp.textInfo.meshInfo[0]);
        }
    }


    public class Character
    {
        private int vertexIndex;

        private MeshVertices baseVertices;

        public Character(TMP_CharacterInfo characterInfo, TMP_MeshInfo meshInfo)
        {
            vertexIndex = characterInfo.vertexIndex;
            baseVertices = new MeshVertices(
                topLeft: meshInfo.vertices[vertexIndex + 1],
                topRight: meshInfo.vertices[vertexIndex + 2],
                bottomLeft: meshInfo.vertices[vertexIndex + 0],
                bottomRight: meshInfo.vertices[vertexIndex + 3]
            );
        }

        public void UpdateVerticies(TMP_MeshInfo meshInfo)
        {
            meshInfo.vertices[vertexIndex + 1] = baseVertices.topLeft;
            meshInfo.vertices[vertexIndex + 2] = baseVertices.topRight;
            meshInfo.vertices[vertexIndex + 0] = baseVertices.bottomLeft;
            meshInfo.vertices[vertexIndex + 3] = baseVertices.bottomRight;
        }


        private void applyEasing(Func<Vector3> easingFunc, ref Vector3 vector)
        {

        }
    }
}
