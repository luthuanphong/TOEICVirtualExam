using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;

namespace TOEICVirtualExam
{
    public class ToeicTestManager : IToeicTestManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ToeicTestManager));

        private Dictionary<string, ToeicTest> _tests = new Dictionary<string, ToeicTest>();

        private static ToeicTestManager _manager = new ToeicTestManager();
        private ToeicTestManager() { this.LoadTests(); }

        public static ToeicTestManager GetTestManager()
        {
            return _manager;
        }

        public void ReLoad()
        {
            this._tests.Clear();
            this.LoadTests();
        }

        public Dictionary<string, ToeicTest> GetAllTest()
        {
            if(this._tests != null)
            {
                return this._tests;
            }

            return new Dictionary<string, ToeicTest>();
        }

        public ToeicTest GetTest(string key)
        {
            if (this._tests.ContainsKey(key))
            {
                return this._tests[key];
            }

            return new ToeicTest();
        }

        private void LoadTests()
        {
            Dictionary<int,List<string>> tests = LocalDataAccess.GetDBAccess().GetAllTest();

            for(int i = 1; i<= tests.Count; i++)
            {
                List<string> infor = tests[i];

                string testSourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, infor[1]);

                DirectoryInfo dirInfo = new DirectoryInfo(testSourcePath);

                DirectoryInfo[] parts = dirInfo.GetDirectories();

                ToeicTest test = new ToeicTest();
                this._tests.Add(i.ToString(), test);
                test.Name = infor[0];

                foreach (DirectoryInfo part in parts)
                {
                    if (string.Equals(part.Name, Common.AUDIO))
                    {
                        FileInfo[] audio = part.GetFiles();
                        if (audio.Length > 0)
                        {
                            test.Audio = audio[0].FullName;
                        }
                        else
                        {
                            this._tests.Remove(i.ToString());
                            break;
                        }
                    }
                    else if (string.Equals(part.Name, Common.LISTEN))
                    {
                        FileInfo[] listening = part.GetFiles();
                        if (listening.Length > 0)
                        {
                            test.Listening = listening[0].FullName;
                        }
                        else
                        {
                            this._tests.Remove(i.ToString());
                            break;
                        }
                    }
                    else if (string.Equals(part.Name, Common.READ))
                    {
                        FileInfo[] reading = part.GetFiles();
                        if (reading.Length > 0)
                        {
                            test.Reading = reading[0].FullName;
                        }
                        else
                        {
                            this._tests.Remove(i.ToString());
                            break;
                        }
                    }
                }
            }
        }
    }
}
