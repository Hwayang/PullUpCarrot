using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    // �� �� Size ������ key/value�� ������ ����ϴ�.
    [SerializeField]
    private int size;

    // Inspector���� key/value�� ����Ǵ� ����Ʈ
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // ��Ÿ�ӿ� �� ��¥ Dictionary
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    /// <summary>
    /// �ν����Ϳ��� Size�� ����� ��, key/value ����Ʈ ũ�⵵ �����ϰ� �����ִ� �Լ�
    /// </summary>
    private void AdjustListSizes(int newSize)
    {
        // 1) ����Ʈ�� �� Ŭ ���, �ڿ������� �߶���ϴ�.
        if (keys.Count > newSize)
            keys.RemoveRange(newSize, keys.Count - newSize);

        if (values.Count > newSize)
            values.RemoveRange(newSize, values.Count - newSize);

        // 2) ����Ʈ�� �� ���� ���, default ������ ä���ֽ��ϴ�.
        while (keys.Count < newSize)
            keys.Add(default);

        while (values.Count < newSize)
            values.Add(default);
    }

    /// <summary>
    /// Inspector���� Size�� �����ϸ� set�� ���� ����Ʈ�� �ٷ� ������ ���� ����
    /// (���� size �ʵ带 ��ġ�ų�, OnValidate ���� ���� �ڵ� ����ȭ ����)
    /// </summary>
    public int Size
    {
        get => size;
        set
        {
            size = value;
            AdjustListSizes(size);
        }
    }

    // ����ȭ ����(�����Ϳ��� Play ������ ��, Ȥ�� ���� ����)�� ȣ��
    public void OnBeforeSerialize()
    {
        // Size�� key/value �迭 ������ ����ȭ
        AdjustListSizes(size);
    }

    // ������ȭ ����(Play, Ȥ�� ���� ��Ÿ�� ����) ȣ��
    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        // key/value �迭�� ���� ũ�⸦ ���� ���·� Loop
        for (int i = 0; i < keys.Count; i++)
        {
            // Dictionary���� Key �ߺ� ��� ���� üũ
            if (!dictionary.ContainsKey(keys[i]))
            {
                // �ߺ� Ű�� �ƴϸ� �߰�
                dictionary.Add(keys[i], values[i]);
            }
            else
            {
                // �ߺ� Ű�� �߰ߵ� ���
                Debug.LogWarning(
                    $"[SerializableDictionary] Duplicate key detected at index {i}: {keys[i]} -> Skipped adding."
                );
            }
        }
    }

    /// <summary>
    /// ��Ÿ�ӿ� Dictionaryó�� �����ϰ� ���� ��
    /// </summary>
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return dictionary;
    }

    /// <summary>
    /// ��ųʸ�ó�� �ε��� ��� ����
    /// ex) myDictionary[key] = value;
    /// </summary>
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set
        {
            dictionary[key] = value;
            // keys/values�� ��ũ�� �����ϵ��� �� ���� ������,
            // �ʿ��� ��� OnBeforeSerialize ���ķ� ��ũ�ϴ� ����� �����ϴ� �� �Ϲ����Դϴ�.
        }
    }
}