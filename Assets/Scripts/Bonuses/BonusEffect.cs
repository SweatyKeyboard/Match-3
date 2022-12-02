using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Match-3/Bonus")]
public class BonusEffect : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private MonoScript _action;

    public Bonus Bonus => (Bonus)Activator.CreateInstance(_action.GetClass());
}
