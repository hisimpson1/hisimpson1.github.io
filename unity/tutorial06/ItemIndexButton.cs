using UnityEngine;

public class ItemIndexButton : MonoBehaviour
{
    public int index;

    private HorizontalScrollController controller;

    void Awake()
    {
        // 부모에서 컨트롤러 찾기
        controller = GetComponentInParent<HorizontalScrollController>();
    }

    public void OnClick()
    {
        if(controller)
            controller.HandleItemButtonClick(index);
    }
}
