using UnityEngine;

public class FeedBack : MonoBehaviour {

    public float DestroyerDistanceAtMaxFeedBackDistance = -10;
    public float DestroyerDistanceAtMinFeedBackDistance = -50;
    private float ScaleQ = 0, ScaleM = 0;
    public float MaxFeedBackDistance = -13;
    public float MinFeedBackDistance = -21;
    private Transform DestroyerTransform;
    private Transform PlayerTransform;

	void Start ()
    {
        DestroyerTransform = GameObject.Find("Player").GetComponent<Transform>();
        PlayerTransform = GameObject.Find("Destroyer").GetComponent<Transform>();
        CalculatePositionScale();
	}

	void Update ()
    {
        var posizioneLocaleX = GetPosition(PlayerTransform.position.x - DestroyerTransform.position.x);
        transform.localPosition = new Vector3(posizioneLocaleX, transform.localPosition.y, transform.localPosition.z);
        Debug.Log(posizioneLocaleX);
    }

    private float GetPosition(float distance)
    {
        var position = ((ScaleM * distance) + ScaleQ);
        return position > MaxFeedBackDistance ? MaxFeedBackDistance
            : position < MinFeedBackDistance ? MinFeedBackDistance
            : position;
    }

    private void CalculatePositionScale()
    {
        ScaleM = (MaxFeedBackDistance - MinFeedBackDistance) / (DestroyerDistanceAtMaxFeedBackDistance - DestroyerDistanceAtMinFeedBackDistance);
        ScaleQ = (MaxFeedBackDistance) - (DestroyerDistanceAtMaxFeedBackDistance * ScaleM);
    }
}