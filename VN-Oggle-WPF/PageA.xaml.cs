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
using System.Windows.Threading;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace WPFOggle
{
    /// <summary>
    /// Interaction logic for PageA.xaml
    /// </summary>
    /// Oggle by Vinh Nguyen
    /// Oggle built upon the Navigation Demo by Janell Baxter

    public partial class PageA : Page
    {
        MainWindow window = (MainWindow)Application.Current.MainWindow;
        DispatcherTimer timer;
        TimeSpan timeSpan;

        int TimeLimit = 120;

        int row = 4 , col = 4;

        int PrevRow = -2;
        int PrevCol = -2;

        public Button[,] buttons;
        Button[,] previousbuttons;

        List<int> LastButtonRow = new List<int>();
        List<int> LastButtonCol = new List<int>();

        public List<string> PossibleWords = new List<string>();

        public List<string> SolvedWords = new List<string>();

        string[] letters = { "D", "J", "A", "T", "T", "N", "W", "R", "E", "I", "S", "N", "O", "N", "A", "I" };

        int points = 0;

        public PageA()
        {
            InitializeComponent();
            SetUp();
        }
        private void SetUp()
        {
            buttons = new Button[row, col];
            previousbuttons = new Button[row, col];
            points = 0;
            GenerateBoard();
            Counter();
            PlayerName.Text = window.player.Name;
            //DiscoveredWordsBox.Text = "";
            Indication_Text.Content  = "";
            SolvedWords.Clear();
            window.player.Points = points;
            
        }
        private string[] getWordsFromExternalFile(string path)
        => File.ReadAllLines(path);

        private void GenerateBoard()
        {
            PossibleWords = getWordsFromExternalFile("../../data/words.txt").ToList();
            Game_Board.Children.Clear();
            Game_Board.RowDefinitions.Clear();
            Game_Board.ColumnDefinitions.Clear();
            //Initialize the Gameboard's Rows and Columns
            for (int i = 0; i < row; i++)
            {
                Game_Board.RowDefinitions.Add(new RowDefinition());
                
            }
            for (int j = 0; j < col; j++)
            {
                Game_Board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Add the Buttons for the Gameboard (In a neatly fashion)
            for (int i = 0;i < row; i++)
            {
                for (int j = 0;j < col; j++)
                {
                    Button button = new Button();
                    button.Name = $"ButtonR{i}C{j}";
                    button.Background = Brushes.White;
                    button.Foreground = Brushes.Black;
                    button.Click += new RoutedEventHandler(Letters_Click);
                    //button.Content = Letter;
                    //button.Width = 40;
                    //button.Height = 40;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Margin = new Thickness(8);
                    button.Padding = new Thickness(8);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons[i,j] = button;
                    Game_Board.Children.Add(button);
                }
            }
            SetUpButtons();
        }

        private void SetUpButtons()
        {
            int rand;
            List<int> iR = new List<int>();
            for (int i = 0; i < letters.Length; i++)
            {
                iR.Add(i);
            }
            Random random = new Random();
            foreach (Button button in buttons)
            {
                rand = random.Next(iR.Count);
                button.Content = letters[iR[rand]];
                iR.RemoveAt(rand);
            }
        }

        private bool CheckPlacement(int Row, int Col)
        {
            if ((Row <= LastButtonRow[LastButtonRow.Count - 1] + 1 && Row >= LastButtonRow[LastButtonRow.Count - 1] - 1) && (Col <= LastButtonCol[LastButtonCol.Count - 1] + 1 && Col >= LastButtonCol[LastButtonCol.Count - 1] - 1))
                return true;
            return false;
        }

        private void Letters_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int CurrRow = Grid.GetRow(button);
            int CurrCol = Grid.GetColumn(button);

            if (Answer_Box.Text == "" || ((PrevRow != CurrRow || PrevCol != CurrCol) && ((PrevRow+1 >= CurrRow && CurrRow >= PrevRow-1) || (PrevCol+1 >= CurrCol && CurrCol >= PrevCol-1))))
            {
                if (LastButtonRow.Count > 0 && LastButtonCol.Count > 0)
                    if (!CheckPlacement(CurrRow, CurrCol))
                        return;
                if (button == previousbuttons[CurrRow, CurrCol]) // Reset the Answer Box should the player "break" the snake
                {
                    ClearSelectedButtons();
                    Answer_Box.Text = "";
                    return;
                }
                RegisterButton(CurrRow, CurrCol, button);
            }
            else if (CurrRow == PrevRow && CurrCol == PrevCol)
            { 
                ResignButton(CurrRow, CurrCol, button);
                if (LastButtonRow.Count != 0 && LastButtonCol.Count != 0)
                {
                    PrevRow = LastButtonRow[LastButtonRow.Count-1];
                    PrevCol = LastButtonCol[LastButtonCol.Count-1];
                    DrawButtons(PrevRow, PrevCol);
                }
                else
                {
                    ClearSelectedButtons();
                }
            }
        }

        private void ClearSelectedButtons()
        {
            LastButtonRow.Clear();
            LastButtonCol.Clear();
            Array.Clear(previousbuttons, 0, previousbuttons.Length);
            ClearButtonColors();
            PrevRow = -1;
            PrevCol = -1;
        }

        private void RegisterButton(int row, int col, Button button)
        {
            previousbuttons[row, col] = button;
            DrawButtons(row, col);
            Answer_Box.Text += button.Content;
            LastButtonRow.Add(row);
            LastButtonCol.Add(col);
            PrevRow = LastButtonRow[LastButtonRow.Count - 1];
            PrevCol = LastButtonCol[LastButtonCol.Count - 1];
        }

        private void ResignButton(int row, int col, Button button)
        {
            Answer_Box.Text = Answer_Box.Text.Remove(Answer_Box.Text.Length - 1);
            Array.Clear(previousbuttons, (row+1 * col+1) - 1, 1);
            //Array.Clear(previousbuttons, previousbuttons.GetLength(1), 1);
            LastButtonRow.RemoveAt(LastButtonRow.Count-1);
            LastButtonCol.RemoveAt(LastButtonCol.Count-1);
            previousbuttons[row,col] = null;    
        }

        private void ClearButtonColors()
        {
            foreach (Button button in buttons)
            {
                button.Background = Brushes.White;
            }
        }

        private void DrawButtons(int row, int col)
        {
            ClearButtonColors();
            buttons[row, col].Background = Brushes.Yellow;
            foreach (Button btn in previousbuttons)
            {
                if (btn != null)
                    btn.Background = Brushes.Yellow;
            }
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if ((i < buttons.GetLength(0) && i >= 0) && ( j < buttons.GetLength(1) && j >= 0) )
                    {
                        if (i != row || j != col)
                            if (buttons[i,j].Background != Brushes.Yellow)
                                buttons[i, j].Background = Brushes.Green;
                    }
                }
            }
        }

        private void Counter()
        {
            //DispatchTimer example by kmatyaszek (https://stackoverflow.com/users/1410998/kmatyaszek)
            timeSpan = TimeSpan.FromSeconds(TimeLimit);
            
            timer = new DispatcherTimer(
                new TimeSpan(0, 0, 1),
                DispatcherPriority.Normal,
                delegate
                {
                    Timer.Text = timeSpan.ToString("c");
                    if (timeSpan == TimeSpan.Zero)
                    {
                        timer.Stop();
                        this.NavigationService.Navigate(new Uri("PageB.xaml", UriKind.Relative));
                    }
                    timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
                },
                Application.Current.Dispatcher);

            timer.Start();
        }

        private string DisplaySolvedWords()
        {
            string output = "";
            string word = "";

            if (SolvedWords.Count == 0)
                return output;

            for (int i = SolvedWords.Count-1; i >= 0; i--)
            {
                word = SolvedWords[i];
                word.Replace(word[0], Convert.ToChar(word[0].ToString().ToUpper()));
                output += word + "\n";
            }

            return output;
        }
        private string SolveFor(string word)
        {
            foreach (string possibleword in PossibleWords)
            {
                if (word.ToLower().Equals(possibleword))
                {
                    if (!IsSolved(possibleword))
                    {
                        SolvedWords.Add(possibleword);
                        points += possibleword.Length;
                        possibleword.Replace(possibleword[0], Convert.ToChar(possibleword[0].ToString().ToUpper()));
                        return $"Word has been found! ({possibleword})";
                    }
                    else
                        return "The word already discovered!";
                }
            }
            return "Word is not found...";
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            SubmitAnswer();
            DiscoveredWordsBox.Text = DisplaySolvedWords();
            ClearSelectedButtons();
        }

        private bool IsSolved(string word)
        {
            foreach (string solved in SolvedWords)
            {
                if (word.ToLower().Equals(solved))
                    return true;
            }
            return false;
        }
        private void SubmitAnswer()
        {
            Indication_Text.Content = SolveFor(Answer_Box.Text.ToLower());
            Answer_Box.Text = "";
            window.player.Points = points;
            Points_Counter.Text = "Points: " + window.player.Points.ToString();
            DiscoveredWordsBox.Text = DisplaySolvedWords();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitAnswer();
            }
        }
    }
}
