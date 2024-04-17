using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ModeDatabase : ScriptableObject
{
    public Mode[] mode;

    public int ModeCount { get { return mode.Length; } }

    public Mode getMode(int index) {  return mode[index]; }
}
