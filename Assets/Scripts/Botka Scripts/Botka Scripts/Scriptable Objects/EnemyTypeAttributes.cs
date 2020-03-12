using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeData", menuName = "EnmyTypeData")]
public class EnemyTypeAttributes : ScriptableObject
{
    public enum AttackType
    {
        Gun,Meele,None, NA
    }

   [SerializeField] private AttackType attackType;
   [SerializeField] private float damage;
    [SerializeField] private float maxHealth;
    [SerializeField] [Range(0f, 10f)] private float velocityModiferier;
    [SerializeField] [Range(0f, 1000f)] private float detectionRange;


    public AttackType getAttackType()
    {
        return attackType;
    }
    public void setAttackType(AttackType attackType)
    {
        this.attackType = attackType;
    }

   
}
