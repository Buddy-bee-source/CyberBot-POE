// ============================================================
// This is the brain of the application. MainWindow.xaml.cs
// only calls ONE method on this class: ProcessInput(string).
// All routing, sentiment detection, memory, and keyword
// matching happens here — not in the UI code-behind.
// ============================================================

using System;
using System.Collections.Generic;

namespace CybersecurityChatBot
{
    public class ChatBot
    {
        // ── Dependencies (all initialised in constructor) ──
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;

        // Tracks whether we are still waiting for the user to enter their name
        private bool _awaitingName = true;

        // Tracks the last matched cybersecurity topic for follow-up handling
        private string _lastTopic = null;

        // Random fallback responses when nothing else matches
        private List<string> _fallbackResponses = new List<string>
        {
            "🤖 I'm not sure I understood that. Try asking about passwords, phishing, malware, or type 'help'.",
            "🤖 Hmm, I didn't catch that. You can ask me about VPNs, firewalls, 2FA, ransomware, and more!",
            "🤖 I'm still learning! Try rephrasing your question or type 'help' to see available topics.",
            "🤖 That's outside my expertise for now. Ask me a cybersecurity question — I'm here to help!"
        };

        private Random _random = new Random();

        // Constructor - initialise all three supporting classes
        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
        }

        // Returns the opening greeting shown in the chat when the app starts
        public string GetGreeting()
        {
            return "👋 Welcome to CyberBot — your Cybersecurity Awareness Assistant!\n\nBefore we begin, what's your name?";
        }

        // ============================================================
        // ProcessInput()
        // This handles all input in the correct priority order
        // ============================================================
        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "⚠️ Please type something — I'm here to help!";

            string lower = userInput.ToLower().Trim();

            //Capture name if we haven't yet 
            if (_awaitingName)
            {
                _memory.UserName = userInput.Trim();
                _awaitingName = false;
                return $"Nice to meet you, {_memory.UserName}! 😊\n\n" +
                       "I'm here to help you stay safe online.\n" +
                       "You can ask me about: passwords, phishing, malware, VPN, firewall, " +
                       "2FA, ransomware, privacy, scams, social engineering, or software updates.\n\n" +
                       "Type 'help' at any time to see all topics.";
            }

            //Follow-up phrases, give another response on the same topic
            if (lower.Contains("tell me more") || lower.Contains("explain more") ||
                lower.Contains("more info") || lower.Contains("elaborate"))
            {
                if (_lastTopic != null)
                {
                    string followUp = _keywords.GetResponse(_lastTopic);
                    return $"🔁 Here's more on {_lastTopic}:\n\n{followUp}";
                }
                else
                {
                    return "🤔 I'm not sure what topic to expand on. Could you ask about a specific cybersecurity topic first?";
                }
            }

            //Detect sentiment and build empathetic opener
            Sentiment detectedSentiment = _sentiment.Detect(lower);
            string sentimentOpener = _sentiment.GetSentimentResponse(detectedSentiment);

            //Run keyword matching 
            string keywordResponse = _keywords.GetResponse(lower);

            if (keywordResponse != null)
            {
                // Update last topic by finding which keyword matched
                foreach (string kw in _keywords.GetAllKeywords())
                {
                    if (lower.Contains(kw))
                    {
                        _lastTopic = kw;

                        // If user mentions interest, store as favourite topic
                        if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("love"))
                            _memory.FavouriteTopic = kw;

                        break;
                    }
                }

                // Get personalised opener from memory
                string personalOpener = _memory.GetPersonalisedOpener();

                // Combine: sentiment opener + personal opener + keyword response
                return $"{sentimentOpener}{personalOpener}{keywordResponse}";
            }

            //Handle special phrases

            if (lower == "how are you")
                return "I'm just code, but I'm running perfectly and ready to keep you safe! 😄";

            if (lower.Contains("your name") || lower.Contains("who are you"))
            {

            }
            return "I'm CyberBot — your personal Cybersecurity Awareness Assistant! 🤖";

            if (lower.Contains("purpose") || lower.Contains("what do you do") || lower.Contains("what can you do"))
            {
                string topics = string.Join(", ", _keywords.GetAllKeywords());
                return $"🎯 My purpose is to educate you about cybersecurity threats and how to stay safe online.\n\n" +
                       $"I can help you with: {topics}.";
            }

            if (lower == "hello" || lower == "hi" || lower == "hey")
                return $"Hey {_memory.UserName}! 👋 Ask me anything about cybersecurity!";

            if (lower.Contains("help") || lower == "?")
            {
                string topics = string.Join(", ", _keywords.GetAllKeywords());
                return $"💡 Here are the topics I can help you with:\n\n{topics}\n\nJust ask me anything about these!";
            }

            if (lower == "exit" || lower == "quit" || lower == "bye")
                return $"EXIT:Goodbye, {_memory.UserName}! Stay safe online! 🛡️";

            //Random fallback response 
            return sentimentOpener + _fallbackResponses[_random.Next(_fallbackResponses.Count)];
        }
    }

}
