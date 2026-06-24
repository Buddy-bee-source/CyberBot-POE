// ============================================================
// KeywordResponder.cs
// This class stores a dictionary where each cybersecurity
// keyword maps to a list of possible responses.
// GetResponse() randomly selects one from the list each time.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatBot
{
    public class KeywordResponder
    {
        // Dictionary: keyword -> list of possible responses
        private Dictionary<string, List<string>> _responses;

        // Used to randomly pick a response from the list
        private Random _random = new Random();

        // Constructor - Keyword recognition
        public KeywordResponder()
        {
            _responses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "🔐 Use strong passwords with uppercase, lowercase, numbers & symbols (e.g. P@ssw0rd!). Never reuse passwords across sites.",
                    "🔐 A good password is at least 12 characters long. Consider using a passphrase like 'Coffee$Runs@Dawn' — easy to remember, hard to crack!",
                    "🔐 Never share your password with anyone, including IT staff. Use a trusted password manager like Bitwarden or KeePass to store them safely."
                },

                ["phishing"] = new List<string>
                {
                    "🎣 Phishing is when attackers trick you via fake emails or websites. Always verify the sender's email address before clicking any links.",
                    "🎣 Watch out for urgency in emails — phrases like 'Your account will be closed!' are classic phishing tactics. When in doubt, go directly to the website.",
                    "🎣 Hover over links before clicking to preview the real URL. If it looks suspicious or doesn't match the company's domain, don't click it!"
                },

                ["malware"] = new List<string>
                {
                    "🦠 Malware is malicious software designed to damage or gain unauthorised access to your device. Keep your antivirus updated at all times.",
                    "🦠 Avoid downloading software from untrusted sources. Even a seemingly harmless file can contain hidden malware or trojans.",
                    "🦠 Regularly scan your device with a reputable antivirus tool. Free options like Malwarebytes can catch threats your main antivirus might miss."
                },

                ["privacy"] = new List<string>
                {
                    "🕵️ Protect your privacy by reviewing app permissions regularly. Does that flashlight app really need access to your contacts?",
                    "🕵️ Use a VPN to hide your browsing activity, especially on public Wi-Fi networks where others could be snooping.",
                    "🕵️ Be careful what you share on social media — personal details like your birthday or location can be used for identity theft."
                },

                ["scam"] = new List<string>
                {
                    "⚠️ Online scams often promise something too good to be true — prizes, refunds, or job offers. Always verify before engaging.",
                    "⚠️ Never send money or gift cards to someone you haven't met in person. This is a major red flag for romance or lottery scams.",
                    "⚠️ If someone calls claiming to be from Microsoft or your bank asking for remote access — hang up. It's almost certainly a scam."
                },

                ["vpn"] = new List<string>
                {
                    "🛡️ A VPN (Virtual Private Network) encrypts your internet traffic. Always use one when connected to public Wi-Fi.",
                    "🛡️ VPNs mask your IP address and location, making it harder for websites and hackers to track your online activity.",
                    "🛡️ Not all VPNs are trustworthy — avoid free VPNs that log your data. Reputable options include ProtonVPN and Mullvad."
                },

                ["firewall"] = new List<string>
                {
                    "🔥 A firewall monitors incoming and outgoing network traffic. Always keep your system firewall enabled to block unauthorised access.",
                    "🔥 Firewalls act as a barrier between your trusted network and untrusted external networks. They are your first line of defence.",
                    "🔥 Both hardware and software firewalls are important. Your router often has a built-in firewall — make sure it's switched on."
                },

                ["2fa"] = new List<string>
                {
                    "📱 Two-Factor Authentication (2FA) adds an extra layer of security. Even if your password is stolen, attackers can't log in without the second factor.",
                    "📱 Use an authenticator app like Google Authenticator or Authy instead of SMS codes — SIM-swapping attacks can intercept texts.",
                    "📱 Enable 2FA on all important accounts: email, banking, and social media. It takes seconds to set up and could save you from a hack."
                },

                ["ransomware"] = new List<string>
                {
                    "💰 Ransomware encrypts your files and demands payment. Back up your data regularly and never pay the ransom — it doesn't guarantee recovery.",
                    "💰 Keep your OS and software updated. Many ransomware attacks exploit known vulnerabilities that patches already fixed.",
                    "💰 The best defence against ransomware is the 3-2-1 backup rule: 3 copies, 2 different media types, 1 offsite (e.g. cloud)."
                },

                ["social engineering"] = new List<string>
                {
                    "🧠 Social engineering manipulates people into revealing confidential info. Always verify identities before sharing any sensitive data.",
                    "🧠 Attackers may impersonate your boss, IT department, or even a friend. If a request feels odd, verify it through a separate channel.",
                    "🧠 Training and awareness are the best defence against social engineering. Know the tactics so you can spot them before it's too late."
                },

                ["update"] = new List<string>
                {
                    "🔄 Always keep your software and OS updated. Patches fix security vulnerabilities that attackers actively exploit.",
                    "🔄 Enable automatic updates where possible. Waiting too long to patch leaves a window open for attackers.",
                    "🔄 Software updates aren't just for new features — they often include critical security fixes that protect your personal data."
                }
            };
        }

        // Loops through the dictionary keys and checks if the input contains the keyword.
        // If a match is found, it returns a RANDOMLY selected response from that keyword's list.
        public string GetResponse(string input)
        {
            string lower = input.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    // Pick a random response from the list for this keyword
                    List<string> options = _responses[keyword];
                    return options[_random.Next(options.Count)];
                }
            }

            // No keyword matched
            return null;
        }

        // Returns all available keyword keys - used to show the user what topics are available
        public List<string> GetAllKeywords()
        {
            return _responses.Keys.ToList();
        }
    }
}