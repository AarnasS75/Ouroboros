using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float amplitude = 5f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private float waveSpeed = 2f;

    private TMP_TextInfo _textInfo;
    private Vector3[][] _originalVertices;

    private void Start()
    {
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;

        // Store original vertex positions
        _originalVertices = new Vector3[_textInfo.meshInfo.Length][];
        for (int i = 0; i < _originalVertices.Length; i++)
        {
            _originalVertices[i] = new Vector3[_textInfo.meshInfo[i].vertices.Length];
            System.Array.Copy(_textInfo.meshInfo[i].vertices, _originalVertices[i], _originalVertices[i].Length);
        }

        DOTween.To(() => 0f, UpdateVertexWave, 1f, 1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    private void UpdateVertexWave(float t)
    {
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;

        float time = Time.time * waveSpeed;

        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            if (!_textInfo.characterInfo[i].isVisible) continue;

            int materialIndex = _textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = _textInfo.characterInfo[i].vertexIndex;

            Vector3[] vertices = _textInfo.meshInfo[materialIndex].vertices;

            // Get original center point
            Vector3 offset = (_originalVertices[materialIndex][vertexIndex] +
                              _originalVertices[materialIndex][vertexIndex + 2]) / 2;

            float waveOffset = Mathf.Sin(time * frequency + i * 0.3f) * amplitude;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = _originalVertices[materialIndex][vertexIndex + j];
                vertices[vertexIndex + j] = orig + new Vector3(0, waveOffset, 0);
            }
        }

        // Push updated vertices to mesh
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _text.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}