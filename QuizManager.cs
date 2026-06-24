using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }
    }

    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>
        {
            new QuizQuestion {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Correct! Reporting phishing emails helps prevent scams.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "True or False: You should use the same password for all accounts.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "False! Using unique passwords for each account protects you if one is compromised.",
                IsTrueFalse = true
            },
            new QuizQuestion {
                Question = "Which of these is a strong password?",
                Options = new List<string> { "A) password123", "B) john1990", "C) Tr0ub4dor&3", "D) abc123" },
                CorrectAnswer = "C",
                Explanation = "Strong passwords use a mix of letters, numbers, and symbols.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "True or False: HTTPS means a website is completely safe.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "HTTPS only means the connection is encrypted, not that the site is trustworthy.",
                IsTrueFalse = true
            },
            new QuizQuestion {
                Question = "What does two-factor authentication (2FA) do?",
                Options = new List<string> { "A) Doubles your password length", "B) Requires a second form of verification", "C) Encrypts your emails", "D) Blocks all hackers" },
                CorrectAnswer = "B",
                Explanation = "2FA adds an extra layer of security beyond just a password.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "True or False: Public Wi-Fi is safe to use for online banking.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Public Wi-Fi can be monitored by attackers. Avoid sensitive transactions on it.",
                IsTrueFalse = true
            },
            new QuizQuestion {
                Question = "What is social engineering?",
                Options = new List<string> { "A) Building social media apps", "B) Manipulating people to reveal information", "C) Engineering social networks", "D) A type of firewall" },
                CorrectAnswer = "B",
                Explanation = "Social engineering tricks people into giving up confidential information.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "True or False: Ransomware encrypts your files and demands payment.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Correct! Ransomware locks your files until you pay the attacker.",
                IsTrueFalse = true
            },
            new QuizQuestion {
                Question = "Which is the safest way to store passwords?",
                Options = new List<string> { "A) Write them on paper", "B) Save in a text file", "C) Use a password manager", "D) Memorise them all" },
                CorrectAnswer = "C",
                Explanation = "Password managers securely store and encrypt all your passwords.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "What should you do to protect your data from ransomware?",
                Options = new List<string> { "A) Pay the ransom", "B) Regularly back up your data", "C) Disable your antivirus", "D) Open all email attachments" },
                CorrectAnswer = "B",
                Explanation = "Regular backups mean you can restore your data without paying a ransom.",
                IsTrueFalse = false
            },
            new QuizQuestion {
                Question = "True or False: Clicking unknown links in emails is safe if you have antivirus.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Antivirus doesn't catch everything. Never click unknown links.",
                IsTrueFalse = true
            },
            new QuizQuestion {
                Question = "What is the best way to protect your privacy on social media?",
                Options = new List<string> { "A) Share everything publicly", "B) Review and limit your privacy settings", "C) Use your full name everywhere", "D) Accept all friend requests" },
                CorrectAnswer = "B",
                Explanation = "Reviewing privacy settings limits who can see your personal information.",
                IsTrueFalse = false
            }
        };
        }

        public QuizQuestion GetCurrentQuestion() => _questions[_currentIndex];

        public bool SubmitAnswer(string answer)
        {
            bool correct = answer.Trim().ToUpper() ==
                           _questions[_currentIndex].CorrectAnswer.ToUpper();
            if (correct) _score++;
            _currentIndex++;
            return correct;
        }

        public string GetFeedback(bool correct) =>
            correct ? "✅ " + _questions[_currentIndex - 1].Explanation
                    : "❌ Incorrect. " + _questions[_currentIndex - 1].Explanation;

        public bool IsFinished() => _currentIndex >= _questions.Count;

        public string GetFinalScore() => $"{_score} out of {_questions.Count}";

        public string GetFinalMessage() =>
            _score >= _questions.Count * 0.7
                ? "🎉 Great job! You have strong cybersecurity knowledge!"
                : "📚 Keep learning! Review the topics you missed.";

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }
        //Total questions in the cquisz
        public int TotalQuestions() => _questions.Count;
        public int CurrentIndex() => _currentIndex;
    }
}

