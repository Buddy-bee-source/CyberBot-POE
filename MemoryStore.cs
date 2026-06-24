// ============================================================
// MemoryStore.cs 
// Stores the user's name and favourite topic.
// ============================================================

using System.Collections.Generic;

namespace CybersecurityChatBot
{
    public class MemoryStore
    {
        // The user's name - set when they first introduce themselves
        public string UserName { get; set; }

        // The user's favourite cybersecurity topic - set when they show interest
        public string FavouriteTopic { get; set; }

        // General key-value store for any additional memory needs
        private Dictionary<string, string> _store = new Dictionary<string, string>();

        // Save any key-value pair to memory
        public void Store(string key, string value)
        {
            _store[key] = value;
        }

        // Retrieve a stored value by key (returns null if not found)
        public string Recall(string key)
        {
            return _store.ContainsKey(key) ? _store[key] : null;
        }

        // Builds a personalised opener using stored info.
        // If a favourite topic is known, it prepends a reference to it.
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrWhiteSpace(FavouriteTopic))
                return $"As someone interested in {FavouriteTopic}, here's what you should know: ";

            if (!string.IsNullOrWhiteSpace(UserName))
                return $"Great question, {UserName}! ";

            return string.Empty;
        }
    }
}
