using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class PlayerData
{
    // all the data we want to save
    public float[] position;
    public  Weapon[] arsenal;
    public int[] ammo;
    public int floorNumber;

    public PlayerData (WeaponWielder player)
    {
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        arsenal = player.GetQuarterMaster().GetArsenal();
        ammo[0] = arsenal[0].ammo;
        ammo[1] = arsenal[1].ammo;
        ammo[2] = arsenal[2].ammo;

        floorNumber = 1;
        
    }
}
