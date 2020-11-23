using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class PlayerData
{
    // all the data we want to save
    public float[] position;
    public  IWeapon[] arsenal;
    public int[] ammo;
    public int floorNumber;

    public PlayerData (IWeaponWielder player)
    {
        position[0] = player.GetGameObject().transform.position.x;
        position[1] = player.GetGameObject().transform.position.y;
        position[2] = player.GetGameObject().transform.position.z;

        arsenal = player.GetQuarterMaster().GetArsenal();
        ammo[0] = arsenal[0].AmmoCount;
        ammo[1] = arsenal[1].AmmoCount;
        ammo[2] = arsenal[2].AmmoCount;

        floorNumber = 1;
    }
}
