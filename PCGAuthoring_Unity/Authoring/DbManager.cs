using UnityEngine;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;

public class DbManager : MonoBehaviour
{
    public string serverAddress = "localhost";
    public int port = 8889;
    public string databaseName = "PCGAuthoring";
    public string username = "root";
    public string password = "root";
    public bool useSSL = false;
    
    private MySqlConnection conn;
   
    public void ChangeStatus(int id, ReqState status)
    {
        string connectionURL = MakeConnectionURL();
        this.conn = new MySqlConnection(connectionURL);
        this.conn.Open();
        string sql = "UPDATE requests SET status="+ (int)status + " WHERE Id=" + id;
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        rdr.Close();
    }
    public void AddAuthoringData(int id, string data)
    {
        string connectionURL = MakeConnectionURL();
        this.conn = new MySqlConnection(connectionURL);
        this.conn.Open();
        string sql = "UPDATE requests SET AuthoringData='" + data + "' WHERE Id=" + id;
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        rdr.Close();
    }

    public object[] GetRequestsByStatus(ReqState status)
    {
        ArrayList requests = new ArrayList();
        
        string connectionURL = MakeConnectionURL();
        this.conn = new MySqlConnection(connectionURL);
        this.conn.Open();

        string sql = "SELECT * FROM requests WHERE Status=" + (int)status;
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            requests.Add(rdr["id"]);
        }
        return requests.ToArray();
    }

    private string MakeConnectionURL()
    {
        string connectionURL =
           "server=" + serverAddress
        + ";user=" + username
        + ";database=" + databaseName
        + ";port=" + port
        + ";password=" + password
        + ";encrypt=" + useSSL;

        return connectionURL;
    }
}
public enum ReqState
{
    PENDING,
    INPROCESS,
    GENERATED,
    COMPLETE
}