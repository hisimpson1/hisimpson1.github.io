using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalScrollController : MonoBehaviour
{
    private ScrollRect scrollRect;
    private RectTransform content;

    private float space = 1;

    public GameObject itemPrefab;

    public List<GameObject> itemList = new List<GameObject>();

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();

        if (scrollRect != null)
            content = scrollRect.content;
    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            AddItem();
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            Button btn = itemList[i].GetComponentInChildren<Button>();
            Text btnText = itemList[i].GetComponentInChildren<Text>();
            btnText.text = (i + 1).ToString();

            ItemIndexButton itemIndex = itemList[i].GetComponent<ItemIndexButton>();
            itemIndex.index = i;
            btn.onClick.AddListener(itemIndex.OnClick);
        }

        SetSpacing(space);
        UpdateContentWidth();
    }

    public void HandleItemButtonClick(int index)
    {
        Debug.Log("버튼 클릭 index: " + index);
    }
    public void AddItem()
    {
        GameObject newUI = Instantiate(itemPrefab, content);
        itemList.Add(newUI);
    }

    public void UpdateContentWidth()
    {
        // 1. 필요한 컴포넌트 정보 가져오기
        HorizontalLayoutGroup layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
        RectTransform contentRect = content.GetComponent<RectTransform>();

        int childCount = content.childCount;

        if (childCount == 0) return;

        // 2. 아이템 하나의 가로 길이 (첫 번째 자식 기준)
        float itemWidth = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        // 3. 전체 너비 계산 공식:
        // (아이템 너비 * 개수) + (사이 간격 * (개수 - 1)) + 좌우 패딩
        float totalWidth = (itemWidth * childCount)
                           + (layoutGroup.spacing * (childCount - 1))
                           + layoutGroup.padding.left
                           + layoutGroup.padding.right;

        // 4. Content의 sizeDelta 수정 (가로 길이 반영)
        contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);
    }

    public void ClearAllItems()
    {
        foreach (var item in itemList)
        {
            Destroy(item);
        }
        itemList.Clear();
    }

    public void SetSpacing(float spacing)
    {
        HorizontalLayoutGroup layout = content.GetComponent<HorizontalLayoutGroup>();
        if (layout != null)
        {
            layout.spacing = spacing;

            // 간격이 바뀌면 전체크기가 다시 계산 되어야 하므로 UpdateContentWidth를 실행한다.
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        }
    }
}