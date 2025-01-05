using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    // ▼ 이 Size 값으로 key/value의 개수를 맞춥니다.
    [SerializeField]
    private int size;

    // Inspector에서 key/value가 노출되는 리스트
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // 런타임에 쓸 진짜 Dictionary
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    /// <summary>
    /// 인스펙터에서 Size가 변경될 때, key/value 리스트 크기도 동일하게 맞춰주는 함수
    /// </summary>
    private void AdjustListSizes(int newSize)
    {
        // 1) 리스트가 더 클 경우, 뒤에서부터 잘라냅니다.
        if (keys.Count > newSize)
            keys.RemoveRange(newSize, keys.Count - newSize);

        if (values.Count > newSize)
            values.RemoveRange(newSize, values.Count - newSize);

        // 2) 리스트가 더 작을 경우, default 값으로 채워넣습니다.
        while (keys.Count < newSize)
            keys.Add(default);

        while (values.Count < newSize)
            values.Add(default);
    }

    /// <summary>
    /// Inspector에서 Size를 변경하면 set을 통해 리스트를 바로 조정할 수도 있음
    /// (직접 size 필드를 고치거나, OnValidate 등을 통해 자동 동기화 가능)
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

    // 직렬화 직전(에디터에서 Play 누르기 전, 혹은 빌드 시점)에 호출
    public void OnBeforeSerialize()
    {
        // Size와 key/value 배열 개수를 동기화
        AdjustListSizes(size);
    }

    // 역직렬화 직후(Play, 혹은 빌드 런타임 시작) 호출
    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        // key/value 배열이 같은 크기를 가진 상태로 Loop
        for (int i = 0; i < keys.Count; i++)
        {
            // Dictionary에서 Key 중복 허용 여부 체크
            if (!dictionary.ContainsKey(keys[i]))
            {
                // 중복 키가 아니면 추가
                dictionary.Add(keys[i], values[i]);
            }
            else
            {
                // 중복 키가 발견된 경우
                Debug.LogWarning(
                    $"[SerializableDictionary] Duplicate key detected at index {i}: {keys[i]} -> Skipped adding."
                );
            }
        }
    }

    /// <summary>
    /// 런타임에 Dictionary처럼 접근하고 싶을 때
    /// </summary>
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return dictionary;
    }

    /// <summary>
    /// 딕셔너리처럼 인덱서 사용 가능
    /// ex) myDictionary[key] = value;
    /// </summary>
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set
        {
            dictionary[key] = value;
            // keys/values가 싱크를 유지하도록 할 수도 있지만,
            // 필요한 경우 OnBeforeSerialize 전후로 싱크하는 방식을 유지하는 게 일반적입니다.
        }
    }
}