using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Data
{
    struct DBConfig
    {
        public static string ServerAddress { get; set; } = "127.0.0.1";
        public static string DatabaseName { get; set; } = "blazortodo";
        public static string UserName { get; set; } = "root";
        public static string UserPassword { get; set; } = "root";

    }


    public class DataBaseConnection : IDisposable
    {
        private bool Disposed = false;
        private string ConnectionString;
        private MySqlConnection MySqlConnection;
        public string Query { get; set; }

        public DataBaseConnection()
        {
            CreateConnectionString();
            // Create new connection
            MySqlConnection = new MySqlConnection(ConnectionString);

            // Try to open new connection
            try
            {
                MySqlConnection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

        }

        private void CreateConnectionString()
        {
            ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", DBConfig.ServerAddress, DBConfig.DatabaseName, DBConfig.UserName, DBConfig.UserPassword);
        }

        public T SelectQueryForSingleValue<T>()
        {

            if(CheckIfConnectionIsGood())
            {
                object _resultValue = null;

                T RetrunValue = default;

                using(MySqlCommand mySqlCommand = new MySqlCommand(Query, MySqlConnection))
                {
                    try
                    {
                        _resultValue = mySqlCommand.ExecuteScalar();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }

                    if(_resultValue != null)
                    {
                        RetrunValue = (T)Convert.ChangeType(_resultValue, typeof(T));
                    }
                    else
                    {
                        return default;
                    }
                }

                return RetrunValue;
            }

            return default;
        }

        public List<T> SelectQueryForSingleColumn<T>()
        {
            List<T> resultValues = new List<T>();

            if(CheckIfConnectionIsGood())
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(Query, MySqlConnection))
                {
                    try
                    {
                        using(MySqlDataReader dataReader = mySqlCommand.ExecuteReader())
                        {
                            if(dataReader.HasRows)
                            {
                                while(dataReader.Read())
                                {
                                    if(dataReader[0] == null)
                                    {
                                        resultValues.Add(default);
                                    }
                                    else
                                    {
                                        resultValues.Add((T)dataReader[0]);
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
                return resultValues;
            }

            return default;
        }



        public List<Dictionary<string, T>> SelectQuery<T>()
        {
            List<Dictionary<string, T>> results = new List<Dictionary<string, T>>();

            if(CheckIfConnectionIsGood())
            {
                using(MySqlCommand mySqlCommand = new MySqlCommand(Query, this.MySqlConnection))
                {
                    try
                    {
                        using(MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                        {
                            if(mySqlDataReader.HasRows)
                            {
                                while(mySqlDataReader.Read())
                                {
                                    var row = new Dictionary<string, T>();

                                    for(int i = 0; i < mySqlDataReader.FieldCount; ++i)
                                    {

                                        string columnName = mySqlDataReader.GetName(i);
                                        object fieldValue = mySqlDataReader[i];

                                        if(fieldValue == null)
                                        {
                                            row.Add(columnName, default);
                                        }
                                        else
                                        {
                                            row.Add(columnName, (T)Convert.ChangeType(fieldValue, typeof(T)));
                                        }
                                    }
                                    results.Add(row);
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return results;
        }


        public int InsertQuery()
        {
            // Value -1 means error, othervise contains number of rows inserted
            int returnValue = -1;

            if(CheckIfConnectionIsGood())
            {
                using(MySqlCommand mySqlCommand = new MySqlCommand(Query, this.MySqlConnection))
                {
                    try
                    {
                        returnValue = mySqlCommand.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }

            return returnValue;
        }

        public int UpdateQuery()
        {
            return InsertQuery();
        }

        public int DeleteQuery()
        {
            return InsertQuery();
        }



        private bool CheckIfConnectionIsGood()
        {
            return MySqlConnection.State == ConnectionState.Open;
        }




        public IEnumerable<T> ExecuteWithResult<T>()
        {
            List<T> records = new List<T>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
            {
                mySqlConnection.Open();
                var reader = MySqlHelper.ExecuteReader(mySqlConnection, Query);
                
                while (reader.Read())
                {
                    //reader.
                    records.Add((T)reader.GetValue(0));
                }
             
            }



            return records.AsEnumerable();
            //var reader = MySqlHelper.ExecuteReader(ConnectionString, Query);


            //if (reader != null)
            //    reader.Close();

            // reader = mySqlCommand.ExecuteReader();


        }

        public int GetLastInsertedId()
        {
            Query = "SELECT SELECT LAST_INSERT_ID()";
            return SelectQueryForSingleValue<int>();
        }

        public int ExecuteWithoutResult()
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
            {
                mySqlConnection.Open();
                return MySqlHelper.ExecuteNonQuery(mySqlConnection, Query);
            }
        }


        private void Dispose(bool disposing)
        {
            if(!Disposed)
            {
                if(disposing)
                {
                    MySqlConnection.Close();
                    MySqlConnection = null;
                    Disposed = true;
                }
            }
            Disposed = true;
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
