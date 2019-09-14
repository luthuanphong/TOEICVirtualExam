using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TOEICVirtualExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestItem _selectedItem;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            this._selectedItem = (TestItem)this.TestList.SelectedItem;
            ToeicTest _test = 
                ToeicTestManager.GetTestManager().GetTest(this._selectedItem.ID);

            this.Visibility = Visibility.Hidden;
            ExamView exView = new ExamView(_test);

            if(exView.ShowDialog() == true)
            {
                IDataAccess dbAccess = LocalDataAccess.GetDBAccess();
                Dictionary<int,List<string>> testCorrectAnswer = 
                    dbAccess.GetTestCorrectAnswer(int.Parse(this._selectedItem.ID));

                Dictionary<int,string> listeningAnswer = exView.GetListenningAnswers();
                Dictionary<int, string> readingAnswer = exView.GetReadingAnswers();

                Dictionary<int, int> listeningScoreChecklist = dbAccess.GetListeningScoreCheckList();
                Dictionary<int, int> readingScoreChecklist = dbAccess.GetReadingScoreCheckList();

                ResultView resultView = new ResultView();
                resultView.SetDataAndCalculate(
                    int.Parse(this._selectedItem.ID), 
                    testCorrectAnswer, 
                    listeningAnswer,
                    readingAnswer,
                    listeningScoreChecklist,
                    readingScoreChecklist);

                resultView.ShowDialog();
            }

            this.Visibility = Visibility.Visible;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            InsertTest insertView = new InsertTest();
            if(insertView.ShowDialog() == true)
            {
                ToeicTestManager.GetTestManager().ReLoad();
                ((MainViewModel)this.DataContext).LoadTestList();
            }
        }
    }
}
