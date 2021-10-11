using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTest
{
    private Boss1 Terry = new Boss1();

    [UnityTest]
    public IEnumerator overHeal()
    {
        Terry.heal(Terry.maxHealth + 100);
        Assert.AreEqual(Terry.maxHealth, Terry.health);
        yield return null;
    }
    [UnityTest]
    public IEnumerator thatsAlotOfDamage()
    {
        Terry.damage(Terry.maxHealth + 100);
        Assert.AreEqual(0, Terry.health);
        yield return null;
    }
}
