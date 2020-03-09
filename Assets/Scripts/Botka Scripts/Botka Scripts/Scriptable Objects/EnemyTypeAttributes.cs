using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeData", menuName = "EnmyTypeData")]
public class EnemyTypeAttributes : ScriptableObject
{
    public enum AttackType
    {
        Gun,Meele,None
    }

   [SerializeField] private AttackType attackType;
   [SerializeField] private float damage;
    [SerializeField]private float movementSpeedModifeier;


    public AttackType getAttackType()
    {
        return attackType;
    }
    public void setAttackType(AttackType attackType)
    {
        this.attackType = attackType;
    }

   
}
