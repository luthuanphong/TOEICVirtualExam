using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOEICVirtualExam
{
    class Common
    {
        #region App_config
        public const string DB = "LOCAL_DB";
        public const string TEST_PATH = "Tests";
        #endregion

        #region Test_Asset
        public const string LISTEN = "LC";
        public const string READ = "RC";
        public const string AUDIO = "AU";
        #endregion

        #region DB Table
        public const string TEST_RESULT_TABLE = "TestResult";
        public const string TEST_RESULT_TEST_NAME = "TEST_NAME";
        public const string TEST_RESULT_LISTEN = "LISTEN";
        public const string TEST_RESULT_READ = "READ";
        public const string TEST_RESULT_FOLDER = "FOLDER";

        public const string LISTENING_SCORE_TABLE = "ListeningScore";
        public const string READING_SCORE_TABLE = "ReadingScore";
        public const string NUM_CORRECT_ANSWER = "NUM_CORRECT_ANSWER";
        public const string SCORE = "SCORE";
        #endregion
    }
}
