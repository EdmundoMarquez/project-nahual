using System.Collections;
using NUnit.Framework;
using ProjectNahual.PCG;
using UnityEngine;
using UnityEngine.TestTools;

public class PCGTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PCGTestsSimplePasses()
    {
        var parent = new GameObject();
        var objectToPlace = new GameObject();
        var gridCell = new GameObject();
        gridCell.AddComponent<Transform>();

        // Use the Assert class to test conditions
        PCGAlgorithm algorithm = new PCGAlgorithm();
        algorithm.Generate(parent, gridCell.transform);
        algorithm.ScatterObject(objectToPlace);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PCGTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
