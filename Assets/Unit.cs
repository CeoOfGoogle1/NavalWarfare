using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public bool isSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected == true)
        {
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(true);

        }
        else
        {
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(false);
        }
    }
}
