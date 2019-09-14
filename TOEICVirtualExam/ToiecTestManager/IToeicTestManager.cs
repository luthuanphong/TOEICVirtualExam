using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOEICVirtualExam
{
    public interface IToeicTestManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ToeicTest GetTest(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ToeicTest> GetAllTest();

        /// <summary>
        /// 
        /// </summary>
        void ReLoad();
    }
}
