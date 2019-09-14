using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOEICVirtualExam
{
    public class MainViewModel
    {
        private IToeicTestManager testManager;
        public ObservableCollection<TestItem> testCollection { get; set; }

        public MainViewModel()
        {
            this.testCollection = new ObservableCollection<TestItem>();
            this.testManager = ToeicTestManager.GetTestManager();
            this.LoadTestList();

        }

        public void LoadTestList()
        {
            if(this.testManager == null || this.testCollection == null)
            {
                return;
            }

            this.testCollection.Clear();

            foreach (KeyValuePair<string, ToeicTest> test in this.testManager.GetAllTest())
            {
                TestItem item = new TestItem();
                item.ID = test.Key;
                item.Name = test.Value.Name;
                this.testCollection.Add(item);
            }
        }
    }

    public class TestItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
