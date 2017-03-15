using UnityEngine;
using System.Collections;

public class InstructionItem : MonoBehaviour {

    public int index;

    public void MouseDown()
    {
        transform.parent.GetComponent<CommandList>().OnItemClicked(index);
    }

    public void MouseEnter()
    {
        transform.parent.GetComponent<CommandList>().OnPointerEnterItem(index);
    }
}
