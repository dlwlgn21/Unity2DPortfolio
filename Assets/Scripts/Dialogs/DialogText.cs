using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/NewDialogContainer")]
public class DialogText : ScriptableObject
{
    public string   SpeckerName;
    [TextArea(5, 10)]
    public string[] Paragraphs;
}
