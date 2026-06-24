// ============================================================
// MainWindow.xaml.cs - GUI Code
// Its only job is to react to UI events
// ============================================================

using CybersecurityChatbot;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatBot
{
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;
        private TaskManager _taskManager;
        private QuizManager _quizManager;

        public MainWindow()
        {
            InitializeComponent();

            _chatBot = new ChatBot();
            _taskManager = new TaskManager();
            _quizManager = new QuizManager();

            PlayVoiceGreeting();
            LoadAsciiArt();

            // Load tasks from JSON on startup
            RefreshTaskList();

            string greeting = _chatBot.GetGreeting();
            AppendBotMessage(greeting);
        }

        // ============================================================
        // STARTUP HELPERS
        // ============================================================

        private void PlayVoiceGreeting()
        {
            try
            {
                // AudioPlayer audio = new AudioPlayer("greeting.wav");
                // audio.PlayGreeting();
            }
            catch { }
        }

        private void LoadAsciiArt()
        {
            AsciiArtDisplay.Text =
@"   ____      _               ____        _   
  / ___|   _| |__   ___ _ _| __ )  ___ | |_ 
 | |  | | | | '_ \ / _ \ '__|  _ \ / _ \| __|
 | |__| |_| | |_) |  __/ |  | |_) | (_) | |_ 
  \____\__, |_.__/ \___|_|  |____/ \___/ \__|
       |___/                                  ";
        }

        // ============================================================
        // CHAT TAB - Event Handlers (Part 1 & 2 preserved)
        // ============================================================

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void SendMessage()
        {
            string userText = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userText))
                return;

            AppendUserMessage(userText);
            UserInput.Clear();

            string response = _chatBot.ProcessInput(userText);

            if (response.StartsWith("EXIT:"))
            {
                string farewellMessage = response.Substring(5);
                AppendBotMessage(farewellMessage);
                SendButton.IsEnabled = false;
                UserInput.IsEnabled = false;
                return;
            }

            // If bot detected quiz intent, switch to quiz tab
            if (response.StartsWith("LAUNCH_QUIZ"))
            {
                AppendBotMessage("Launching the quiz! Head to the Quiz tab.");
                StartQuiz();
                return;
            }

            AppendBotMessage(response);

            // Refresh task list if a task was added via chat
            RefreshTaskList();
        }

        // ============================================================
        // CHAT DISPLAY HELPERS
        // ============================================================

        private void AppendUserMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"\n🧑 You: {message}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0))
            });
            ScrollToBottom();
        }

        private void AppendBotMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"\n🤖 CyberBot: {message}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 255))
            });
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run(
                "\n──────────────────────────────────────────────────────────\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 139, 139))
            });
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToBottom();
        }

        // ============================================================
        // TASK TAB - Event Handlers
        // ============================================================

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleInput.Text.Trim();
            string description = TaskDescriptionInput.Text.Trim();
            string reminder = TaskReminderInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                TaskStatusText.Text = "⚠ Please enter a task title.";
                return;
            }

            string result = _taskManager.AddTask(title, description, reminder);
            TaskStatusText.Text = "✅ " + result;

            TaskTitleInput.Clear();
            TaskDescriptionInput.Clear();
            TaskReminderInput.Clear();

            RefreshTaskList();
            StatusBar.Text = "Task added: " + title;
        }

        private void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskDisplayItem selected)
            {
                _taskManager.MarkAsComplete(selected.Id);
                RefreshTaskList();
                TaskStatusText.Text = "✅ Task marked complete.";
                StatusBar.Text = "Task completed: " + selected.Title;
            }
            else
            {
                TaskStatusText.Text = "⚠ Please select a task first.";
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskDisplayItem selected)
            {
                _taskManager.DeleteTask(selected.Id);
                RefreshTaskList();
                TaskStatusText.Text = "🗑 Task deleted.";
                StatusBar.Text = "Task deleted: " + selected.Title;
            }
            else
            {
                TaskStatusText.Text = "⚠ Please select a task first.";
            }
        }

        private void RefreshTaskList()
        {
            List<CyberTask> tasks = _taskManager.GetAllTasks();
            TaskListBox.Items.Clear();

            foreach (var task in tasks)
            {
                string status = task.IsComplete ? "✔" : "○";
                string reminder = string.IsNullOrWhiteSpace(task.Reminder)
                    ? "" : $" | ⏰ {task.Reminder}";

                TaskListBox.Items.Add(new TaskDisplayItem
                {
                    Id = task.Id,
                    Title = task.Title,
                    Display = $"{status} [{task.Id}] {task.Title}{reminder}"
                });
            }
        }

        // ============================================================
        // QUIZ TAB - Event Handlers
        // ============================================================

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }

        private void StartQuiz()
        {
            _quizManager.ResetQuiz();
            ActivityLogger.Log("Quiz started");
            QuizScoreText.Text = $"Score: 0 / {_quizManager.TotalQuestions()}";
            SubmitAnswerButton.IsEnabled = true;
            StartQuizButton.IsEnabled = false;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            QuizFeedbackText.Text = "";
            LoadCurrentQuestion();
            StatusBar.Text = "Quiz started!";
        }

        private void LoadCurrentQuestion()
        {
            if (_quizManager.IsFinished())
            {
                ShowQuizResults();
                return;
            }

            QuizQuestion q = _quizManager.GetCurrentQuestion();
            QuizQuestionText.Text = $"Q{_quizManager.CurrentIndex() + 1}: {q.Question}";
            QuizFeedbackText.Text = "";

            // Clear all radio buttons
            QuizOption1.IsChecked = false;
            QuizOption2.IsChecked = false;
            QuizOption3.IsChecked = false;
            QuizOption4.IsChecked = false;

            if (q.IsTrueFalse)
            {
                QuizOption1.Content = "True";
                QuizOption2.Content = "False";
                QuizOption1.Visibility = Visibility.Visible;
                QuizOption2.Visibility = Visibility.Visible;
                QuizOption3.Visibility = Visibility.Collapsed;
                QuizOption4.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuizOption1.Content = q.Options[0];
                QuizOption2.Content = q.Options[1];
                QuizOption3.Content = q.Options[2];
                QuizOption4.Content = q.Options[3];
                QuizOption1.Visibility = Visibility.Visible;
                QuizOption2.Visibility = Visibility.Visible;
                QuizOption3.Visibility = Visibility.Visible;
                QuizOption4.Visibility = Visibility.Visible;
            }

            SubmitAnswerButton.IsEnabled = true;
            NextQuestionButton.Visibility = Visibility.Collapsed;
        }

        private void SubmitAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            string answer = "";

            if (QuizOption1.IsChecked == true) answer = QuizOption1.Content.ToString();
            else if (QuizOption2.IsChecked == true) answer = QuizOption2.Content.ToString();
            else if (QuizOption3.IsChecked == true) answer = QuizOption3.Content.ToString();
            else if (QuizOption4.IsChecked == true) answer = QuizOption4.Content.ToString();

            if (string.IsNullOrEmpty(answer))
            {
                QuizFeedbackText.Text = "⚠ Please select an answer first.";
                return;
            }

            // Extract just the letter for multiple choice (e.g. "A) ..." → "A")
            string submitAnswer = answer.Length >= 1 ? answer.Substring(0, 1) : answer;

            bool correct = _quizManager.SubmitAnswer(submitAnswer);
            QuizFeedbackText.Text = _quizManager.GetFeedback(correct);
            QuizScoreText.Text = $"Score: {(correct ? _quizManager.CurrentIndex() : _quizManager.CurrentIndex())} / {_quizManager.TotalQuestions()}";

            SubmitAnswerButton.IsEnabled = false;
            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.IsFinished())
                ShowQuizResults();
            else
                LoadCurrentQuestion();
        }

        private void ShowQuizResults()
        {
            string score = _quizManager.GetFinalScore();
            string message = _quizManager.GetFinalMessage();

            QuizQuestionText.Text = $"🎉 Quiz Complete! You scored {score}";
            QuizFeedbackText.Text = message;
            QuizScoreText.Text = $"Final Score: {score}";

            QuizOption1.Visibility = Visibility.Collapsed;
            QuizOption2.Visibility = Visibility.Collapsed;
            QuizOption3.Visibility = Visibility.Collapsed;
            QuizOption4.Visibility = Visibility.Collapsed;

            SubmitAnswerButton.IsEnabled = false;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            StartQuizButton.IsEnabled = true;
            StartQuizButton.Content = "🔄 RESTART";

            ActivityLogger.Log($"Quiz completed - score: {score}");
            StatusBar.Text = $"Quiz finished! Score: {score}";
        }

        // ============================================================
        // ACTIVITY LOG TAB - Event Handlers
        // ============================================================

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogDisplay.Text = ActivityLogger.GetRecentLog(10);

            if (ActivityLogger.GetCount() > 10)
                ShowMoreLogButton.Visibility = Visibility.Visible;
            else
                ShowMoreLogButton.Visibility = Visibility.Collapsed;

            StatusBar.Text = "Activity log refreshed.";
        }

        private void ShowMoreLogButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogDisplay.Text = ActivityLogger.GetFullLog();
            ShowMoreLogButton.Visibility = Visibility.Collapsed;
            StatusBar.Text = "Showing full activity log.";
        }
    }

    // Helper class for ListBox display
    public class TaskDisplayItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Display { get; set; }
    }
}