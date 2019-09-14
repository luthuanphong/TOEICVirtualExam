using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOEICVirtualExam
{
    public interface IDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listen"></param>
        /// <param name="read"></param>
        /// <returns></returns>
        bool InsertTestCorrectAnswer(string name, string listen, string read, string FOLDER, SQLiteTransaction transaction = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        Dictionary<int, List<string>> GetTestCorrectAnswer(int i);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<int, List<string>> GetAllTest(); 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Open();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Close();

        SQLiteConnection GetCurrentConnection();

        Dictionary<int, int> GetListeningScoreCheckList();

        Dictionary<int, int> GetReadingScoreCheckList();
    }
}
