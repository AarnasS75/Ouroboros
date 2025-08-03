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
    
    [Header("Color animate")]
    [SerializeField] private TextMeshProUGUI _textToColorRotate;

    private void Start()
    {
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;

        _originalVertices = new Vector3[_textInfo.meshInfo.Length][];
        for (int i = 0; i < _originalVertices.Length; i++)
        {
            _originalVertices[i] = new Vector3[_textInfo.meshInfo[i].vertices.Length];
            System.Array.Copy(_textInfo.meshInfo[i].vertices, _originalVertices[i], _originalVertices[i].Length);
        }

        DOTween.To(() => 0f, UpdateVertexWave, 1f, 1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        if (_textToColorRotate is null)
        {
            return;
        }
        
        var hue = 0f;
        DOTween.To(() => hue, h => {
                hue = h;
                if (hue > 1f) hue -= 1f;
                _textToColorRotate.color = Color.HSVToRGB(hue, 1f, 1f);
            }, 1f, 5f) // 5 seconds for full hue cycle; adjust as needed
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
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

            Vector3 offset = (_originalVertices[materialIndex][vertexIndex] +
                              _originalVertices[materialIndex][vertexIndex + 2]) / 2;

            float waveOffset = Mathf.Sin(time * frequency + i * 0.3f) * amplitude;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = _originalVertices[materialIndex][vertexIndex + j];
                vertices[vertexIndex + j] = orig + new Vector3(0, waveOffset, 0);
            }
        }

        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _text.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}