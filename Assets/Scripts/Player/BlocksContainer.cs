using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField]
    private BuildBlock[] buildBlocks;
    
    public BuildBlock[] GetBlocks()
    {
        return buildBlocks;
    }
}

public enum ResourceType
{
    SandStone, Wood
}

[System.Serializable]
public struct BuildBlock
{
    public GameObject buildBlock;
    public ResourceType resourceType;
    public float price;
    public Sprite icon;
}
