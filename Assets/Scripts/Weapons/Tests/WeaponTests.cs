using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ProjectNahual.Weapons;

class FakeDamageable : IDamageable
{
    public int DamageTaken;
    public void TakeDamage(int amount)
    {
        DamageTaken += amount;
    }
}

public class WeaponTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Fire_DealsDamageToPlayer()
    {
        // Arrange
        var weapon = new WeaponLogic(10);
        var target = new FakeDamageable();
        // Act
        weapon.Fire(target);
        //Assert
        Assert.AreEqual(10, target.DamageTaken);
    }
}
