using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Model
{
    public class GameHistoryDatabase
    {
        private IDbConnection dbConnection;
        
        public GameHistoryDatabase()
        {
            string connectionPath = "URI=file:" + Application.dataPath + "/Databases/GameHistory.db";
            dbConnection = (IDbConnection)new SqliteConnection(connectionPath);

//            dbConnection.Open();
//            IDbCommand dbCommand = dbConnection.CreateCommand();
//            dbCommand.CommandText = "CREATE TABLE Persons(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255))";
//            dbCommand.ExecuteNonQuery();
//            dbCommand.Dispose();
//            dbConnection.Close();
//
//            dbConnection.Open();
//            dbConnection.Close();
        }

        public void OpenConnection()
        {
            dbConnection.Open();
        }
    }
}

