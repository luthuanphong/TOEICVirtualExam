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
    /// Interaction logic for AnswerSheet.xaml
    /// </summary>
    public partial class AnswerSheet : UserControl
    {
        private List<Answer> answers = new List<Answer>();

        private static DependencyProperty 
            NumberOfQuestionProperty = DependencyProperty.Register("NumberOfQuestion", typeof(int), typeof(AnswerSheet), new PropertyMetadata(100));

        public int NumberOfQuestion
        {
            get
            {
                return (int)GetValue(NumberOfQuestionProperty);
            }
            set
            {
                SetValue(NumberOfQuestionProperty, value);
            }
        }

        public AnswerSheet()
        {
            InitializeComponent();
            this.CreateAnswerSheet();
        }

        public Dictionary<int,int> GetAnswers()
        {
            Dictionary<int, int> answerDic = new Dictionary<int, int>();

            for(int i = 0; i < this.NumberOfQuestion; i++)
            {
                answerDic.Add(this.answers[i].QuestionNumber, this.answers[i].GetChoice());
            }

            return answerDic;
        }

        private void CreateAnswerSheet()
        {
            for(int i = 1; i <= this.NumberOfQuestion; i++)
            {
                Answer answer = new Answer(i);
                this.answers.Add(answer);
                this.AnswerList.Children.Add(answer);
            }
        }
    }
}
