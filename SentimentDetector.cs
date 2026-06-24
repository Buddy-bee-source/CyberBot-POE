// ============================================================
// Detects the emotional tone of the user's message.
// Returns an empathetic opening sentence
// ============================================================

using System.Collections.Generic;

namespace CybersecurityChatBot
{
    // Enum representing all possible detected sentiments
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    public class SentimentDetector
    {
        // Dictionary: Sentiment - list  of trigger words
        private Dictionary<Sentiment, List<string>> _triggerWords;

        // Constructor - populate each sentiment with its trigger words
        public SentimentDetector()
        {
            _triggerWords = new Dictionary<Sentiment, List<string>>
            {
                [Sentiment.Worried] = new List<string>
                {
                    "worried", "scared", "afraid", "anxious", "nervous",
                    "unsafe", "frightened", "concerned", "panic", "danger"
                },

                [Sentiment.Curious] = new List<string>
                {
                    "curious", "wondering", "interested", "want to know",
                    "how does", "how do", "what is", "tell me", "explain",
                    "can you", "what are"
                },

                [Sentiment.Frustrated] = new List<string>
                {
                    "frustrated", "annoyed", "confused", "don't understand",
                    "doesn't make sense", "not working", "useless",
                    "hate this", "difficult", "hard to"
                },

                [Sentiment.Happy] = new List<string>
                {
                    "great", "thanks", "helpful", "awesome", "love it",
                    "amazing", "perfect", "brilliant", "thank you",
                    "excellent", "good job"
                }
            };
        }

        // Loops through each sentiment's trigger words to find a match.
        // Returns the matching Sentiment, or Neutral if nothing found.
        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (var entry in _triggerWords)
            {
                foreach (string word in entry.Value)
                {
                    if (lower.Contains(word))
                        return entry.Key;
                }
            }

            return Sentiment.Neutral;
        }

        // Returns an empathetic opening sentence for the detected sentiment.
        // If Neutral, returns empty string so nothing is prepended.
        public string GetSentimentResponse(Sentiment sentiment)
        {
            switch (sentiment)
            {
                case Sentiment.Worried:
                    return "😟 I can hear that you're feeling worried — that's completely understandable. Let me help put your mind at ease.\n\n";

                case Sentiment.Curious:
                    return "🤔 Great question! I love your curiosity — let me shed some light on that.\n\n";

                case Sentiment.Frustrated:
                    return "😤 I'm sorry you're feeling frustrated. Let me try to explain this as clearly as possible.\n\n";

                case Sentiment.Happy:
                    return "😊 Glad to hear you're feeling positive! Here's some more useful information for you.\n\n";

                case Sentiment.Neutral:
                default:
                    return string.Empty; // Nothing prepended for neutral tone
            }
        }
    }
}
