using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public class DbManager : MonoBehaviour
{
    public string serverAddress = "localhost";
    public int port = 8889;
    public string databaseName = "PCGAuthoring";
    public string username = "root";
    public string password = "root";
    public bool useSSL = false;

    private MySqlConnection conn;

    void Start()
    {
        string connectionURL =
           "server=" + serverAddress
        + ";user=" + username
        + ";database=" + databaseName
        + ";port=" + port
        + ";password=" + password
        + ";encrypt=" + useSSL;

        this.conn = new MySqlConnection(connectionURL);
        this.conn.Open();

        string sql = "SELECT * FROM requests";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            Debug.Log(rdr["Id"] + ",  " + rdr["AuthoringData"]);
        }

        rdr.Close();


    }
}