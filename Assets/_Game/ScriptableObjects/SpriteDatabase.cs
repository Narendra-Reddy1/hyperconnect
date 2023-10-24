using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newSpriteDatabase",menuName ="ScriptableObject/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{
    [SerializeField] private List<Sprite> _tileImages;
    public System.Collections.ObjectModel.ReadOnlyCollection<Sprite> GetSpriteList() => _tileImages.AsReadOnly();
    public Sprite GetSpriteWithID(int id)
    {
        if (id > _tileImages.Count || id < -1)
        {
            Debug.LogError($"INVALID id ::SpriteDatabase :: GetSpriteWithDatabase:: {id}" +
                $"returning default Image!!!");
            return _tileImages[0];//to prevent from Null reference
        }
        return _tileImages[id];
    }
}
