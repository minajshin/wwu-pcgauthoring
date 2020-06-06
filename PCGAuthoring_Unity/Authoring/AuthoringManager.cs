using UnityEngine;


public class AuthoringManager : MonoBehaviour
{
    public float targetTime = 60.0f;
    private void Update()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            StartAuthoring();
            targetTime = 60.0f;
            Debug.Log("StartAuthoring");
        }
    }
    void StartAuthoring()
    {
        DbManager dbm = GetComponent<DbManager>();

        // 1. Pick pending requests
        object[] requests = dbm.GetRequestsByStatus(ReqState.PENDING);

        // For now, testing with only one request

        int reqId = (int)requests[requests.Length - 1];
        if (dbm.GetStatus(reqId) == (int)ReqState.PENDING)
        {

            // 2. Change status (INPROGRESS)
            dbm.ChangeStatus(reqId, ReqState.INPROCESS);

            // 3. Trigger generation method and get result data
            // For now, testing with hard-coded result
            string data = "/result/Fancy4.png";
            Debug.Log("ResultData: " + data);

            // 4. Change status (GENERATED) and update database
            dbm.ChangeStatus(reqId, ReqState.GENERATED);
            dbm.AddResultData(reqId, data);
            Debug.Log("Generated");
        }
    }
}

