using UnityEngine;

[System.Serializable]
public class UnityLayer
{
    [SerializeField]
    private int m_LayerIndex = 0;

    public int LayerIndex
    {
        get { return m_LayerIndex; }
    }

    public int Mask
    {
        get
        {
            return 1 << m_LayerIndex;
        }
    }

    public UnityLayer(int layer)
    {
        Set(layer);
    }

    public static implicit operator UnityLayer(int layer)
    {
        return new UnityLayer(layer);
    }

    public static implicit operator int(UnityLayer layer)
    {
        return layer.LayerIndex;
    }

    public void Set(int _layerIndex)
    {
        if (_layerIndex > 0 && _layerIndex < 32)
        {
            m_LayerIndex = _layerIndex;
        }
    }
}