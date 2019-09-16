using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace TOEICVirtualExam
{
    /// <summary>
    /// Interaction logic for ResultView.xaml
    /// </summary>
    public partial class ResultView : Window
    {
        public ObservableCollection<ResultItem> ListeningResult { get; set; }
        public ObservableCollection<ResultItem> ReadingResult { get; set; }
        public ResultView()
        {
            this.ListeningResult = new ObservableCollection<ResultItem>();
            this.ReadingResult = new ObservableCollection<ResultItem>();
            InitializeComponent();
        }

        public void SetDataAndCalculate(
            int ID, 
            Dictionary<int, List<string>> correctAnswer,
            Dictionary<int, string> listeningAnswer, 
            Dictionary<int, string> readingAnswer,
            Dictionary<int,int> listeningScoreChecklist,
            Dictionary<int,int> readingScoreChecklist
            )
        {
            List<string> Answers = correctAnswer[ID];

            string[] listeningCorrectAnswer = Answers[0].Split(new char[] { ',' });
            string[] readingCorrectAnswer = Answers[1].Split(new char[] { ',' });

            for(int i = 1; i < listeningAnswer.Count + 1; i++)
            {
                ResultItem item = new ResultItem()
                {
                    Number = i,
                    Correct = listeningCorrectAnswer[i - 1],
                    YourChoice = listeningAnswer[i]
                };

                this.ListeningResult.Add(item);
            }

            for(int i = 1; i < readingAnswer.Count + 1; i++)
            {
                ResultItem item = new ResultItem()
                {
                    Number = i,
                    Correct = readingCorrectAnswer[i - 1],
                    YourChoice = readingAnswer[i]
                };

                this.ReadingResult.Add(item);
            }

            int numberOfCorrectAnswerInListening = this.ListeningResult.Where(x => x.IsCorrect).Count();
            int numberOfCorrectAnswerInReading = this.ReadingResult.Where(x => x.IsCorrect).Count();
            int listeningScore = listeningScoreChecklist[numberOfCorrectAnswerInListening];
            int readingScore = readingScoreChecklist[numberOfCorrectAnswerInReading];

            this.ListeningCorrectAnswer.Text = numberOfCorrectAnswerInListening.ToString();
            this.ListeningScore.Text = listeningScore.ToString();

            this.ReadingCorrectAnswer.Text = numberOfCorrectAnswerInReading.ToString();
            this.ReadingScore.Text = readingScore.ToString();

            this.TotalScore.Text = (listeningScore + readingScore).ToString();
        }
    }

    public class ResultItem
    {
        public int Number { get; set; }
        public string Correct { get; set; }
        public string YourChoice { get; set; }
        public bool IsCorrect
        {
            get
            {
                return string.Equals(
                    Correct.Trim(), 
                    YourChoice.Trim(),
                    StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
