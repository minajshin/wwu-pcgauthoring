using UnityEngine;


public class AuthoringManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DbManager dbm = GetComponent<DbManager>();

        // 1. Pick pending requests
        //object[] requests = dbm.GetRequestsByStatus(ReqState.PENDING);
        
        // For now, testing with only one request
        int reqId = 1;

        // 2. Change status (INPROGRESS)
        dbm.ChangeStatus(reqId, ReqState.INPROCESS);

        // 3. Trigger generation method and get result data
        // For now, testing with hard-coded result
        string data = "/result/test.png";
        Debug.Log("AuthoringData: " + data);

        // 4. Change status (GENERATED) and update database
        dbm.ChangeStatus(reqId, ReqState.GENERATED);
        dbm.AddAuthoringData(reqId, data);
        Debug.Log("Generated");
    }
}

