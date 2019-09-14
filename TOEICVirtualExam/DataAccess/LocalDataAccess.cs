namespace TOEICVirtualExam
{
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.SQLite;

    /// <summary>
    /// Data accessor for local DB (SQLite).
    /// </summary>
    public class LocalDataAccess : IDataAccess
    {
        /// <summary>
        /// Log outter
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LocalDataAccess));

        /// <summary>
        /// Single accessor
        /// </summary>
        private static IDataAccess _access = new LocalDataAccess();

        /// <summary>
        /// Sqlite connection string
        /// </summary>
        private string _connectionString = @"Data Source={0};Version=3;";

        /// <summary>
        /// DB connection
        /// </summary>
        private SQLiteConnection _dbConnection;

        private LocalDataAccess()
        {
            this.Open();
        }

        /// <summary>
        /// Get single accessor
        /// </summary>
        /// <returns></returns>
        public static IDataAccess GetDBAccess()
        {
            return _access;
        }

        public Dictionary<int, List<string>> GetTestCorrectAnswer(int i)
        {
            Dictionary<int, List<string>> answers = new Dictionary<int, List<string>>();

            if (this._dbConnection == null)
            {
                Logger.Error("Unable to connect to Local DB");
                return answers;
            }

            SQLiteCommand command = this._dbConnection.CreateCommand();

            StringBuilder queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append(string.Format("SELECT {0},{1},{2} FROM {3}",
                "rowid",
                Common.TEST_RESULT_LISTEN,
                Common.TEST_RESULT_READ,
                Common.TEST_RESULT_TABLE));

            queryStringBuilder.Append(string.Format(" WHERE {0} = {1}", "rowid", i));

            command.CommandText = queryStringBuilder.ToString();

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                answers.Add(Convert.ToInt32(reader["rowid"]),new List<string>() {
                    Convert.ToString(reader[Common.TEST_RESULT_LISTEN]),
                    Convert.ToString(reader[Common.TEST_RESULT_READ])
                });
            }

            return answers;
        }

        public Dictionary<int, List<string>> GetAllTest()
        {
            Dictionary<int, List<string>> tests = new Dictionary<int, List<string>>();

            if (this._dbConnection == null)
            {
                Logger.Error("Unable to connect to Local DB");
                return tests;
            }

            SQLiteCommand command = this._dbConnection.CreateCommand();

            StringBuilder queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append(string.Format("SELECT {0},{1},{2} FROM {3}", "rowid",
                Common.TEST_RESULT_TEST_NAME,
                Common.TEST_RESULT_FOLDER,
                Common.TEST_RESULT_TABLE));

            command.CommandText = queryStringBuilder.ToString();

            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                tests.Add(Convert.ToInt32(reader["rowid"]),new List<string>() {
                    Convert.ToString(reader[Common.TEST_RESULT_TEST_NAME]),
                    Convert.ToString(reader[Common.TEST_RESULT_FOLDER])});
            }

            return tests;
        }

        public bool InsertTestCorrectAnswer(string name, string listen, string read, string folder, SQLiteTransaction transaction = null)
        {
            try
            {
                if (this._dbConnection == null)
                {
                    Logger.Error("Unable to connect to Local DB");
                    return false;
                }

                SQLiteCommand command = this._dbConnection.CreateCommand();

                if(transaction != null)
                {
                    command.Transaction = transaction;
                }

                StringBuilder queryStringBuilder = new StringBuilder();

                queryStringBuilder.Append(string.Format("INSERT INTO {0} ({1},{2},{3},{4})",
                    Common.TEST_RESULT_TABLE,
                    Common.TEST_RESULT_TEST_NAME,
                    Common.TEST_RESULT_LISTEN,
                    Common.TEST_RESULT_READ,
                    Common.TEST_RESULT_FOLDER));

                queryStringBuilder.Append(string.Format("VALUES ('{0}','{1}','{2}','{3}')",
                    name, listen, read, folder)); ;

                command.CommandText = queryStringBuilder.ToString();

                int result = command.ExecuteNonQuery();

                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occurs while inserting new correct answer -{0}", ex);
                return false;
            }
        }

        public bool Open()
        {
            string _dbPath = ConfigurationManager.AppSettings[Common.DB];

            if(string.IsNullOrEmpty(_dbPath) || string.IsNullOrWhiteSpace(_dbPath))
            {
                Logger.Info("DB not found.");
                return false;
            }

            string _dbFullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dbPath);

            this._dbConnection = new SQLiteConnection(string.Format(this._connectionString, _dbFullPath));
            this._dbConnection.Open();
            return true;
        }

        public bool Close()
        {
            this._dbConnection.Close();
            return true;
        }

        public SQLiteConnection GetCurrentConnection()
        {
            if(this._dbConnection == null)
            {
                this.Open();
            }

            return this._dbConnection;
        }

        public Dictionary<int, int> GetListeningScoreCheckList()
        {
            Dictionary<int, int> checklist = new Dictionary<int, int>();

            if (this._dbConnection == null)
            {
                Logger.Error("Unable to connect to Local DB");
                return checklist;
            }

            SQLiteCommand command = this._dbConnection.CreateCommand();

            StringBuilder queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append(string.Format("SELECT {0},{1} FROM {2}",
                Common.NUM_CORRECT_ANSWER,
                Common.SCORE,
                Common.LISTENING_SCORE_TABLE));

            command.CommandText = queryStringBuilder.ToString();

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                checklist.Add(
                    Convert.ToInt32(reader[Common.NUM_CORRECT_ANSWER]),
                    Convert.ToInt32(reader[Common.SCORE])
                    );
            }

            return checklist;
        }

        public Dictionary<int, int> GetReadingScoreCheckList()
        {
            Dictionary<int, int> checklist = new Dictionary<int, int>();

            if (this._dbConnection == null)
            {
                Logger.Error("Unable to connect to Local DB");
                return checklist;
            }

            SQLiteCommand command = this._dbConnection.CreateCommand();

            StringBuilder queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append(string.Format("SELECT {0},{1} FROM {2}",
                Common.NUM_CORRECT_ANSWER,
                Common.SCORE,
                Common.READING_SCORE_TABLE));

            command.CommandText = queryStringBuilder.ToString();

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                checklist.Add(
                    Convert.ToInt32(reader[Common.NUM_CORRECT_ANSWER]),
                    Convert.ToInt32(reader[Common.SCORE])
                    );
            }

            return checklist;
        }
    }
}
