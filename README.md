
<h1 align="center">
  💬 Chat Data System
</h1>

<p align="center">
  <img src="https://img.shields.io/badge/Version-1.0-blue?style=for-the-badge&logo=visualstudio">
  <img src="https://img.shields.io/badge/Platform-Windows-success?style=for-the-badge&logo=windows">
  <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp">
  <img src="https://img.shields.io/badge/JSON-Newtonsoft.Json-000000?style=for-the-badge&logo=json">
</p>

<p align="center">
  <b>✨ A dynamic conversation system with JSON-based message storage! ✨</b>
</p>

<p align="center">
  Load conversations from JSON, handle keyword-based replies,<br>
  and manage fallback responses automatically! 🚀
</p>

<hr>

## 🌟 Features

| Feature | Description |
| :--- | :--- |
| **💾 JSON Storage** | Saves and loads conversation data using Newtonsoft.Json |
| **🔄 Auto-Generation** | Creates placeholder data if no chat file exists |
| **🔑 Keyword Matching** | Replies based on keywords in user input |
| **🎯 Flexible Matching** | Support for complete or partial keyword matching |
| **📎 Fallback System** | Default replies when no keywords match |
| **🔗 Message Linking** | Navigate between messages via ID references |

## 🏗️ Core Components

### 📋 ChatData Class Structure

```csharp
class ChatData
{
    public List<Message> messages;
    
    public class Message
    {
        public string id;
        public string baseContent;
        public bool notNullOrEmpty;
        public List<PortentialReply> portentialReplies;
        public List<Possibility<string>> ifNothingToReply;
        
        public class PortentialReply
        {
            public string keyword;
            public bool matchCompletely;
            public List<Possibility<string>> replies;
        }
        
        public class Possibility<T>
        {
            public T item;
            public static T Random(List<Possibility<T>> list);
        }
    }
}
```

### 🔄 Message Flow

```
User Input → Keyword Matching → Found Match → Load Reply Message
                  ↓
            No Match Found
                  ↓
         Fallback (ifNothingToReply)
                  ↓
         Load Random Fallback Message
```

## 🚀 How It Works

### 1️⃣ **Data Loading**
The system first checks for a `chatData` file in the current directory:
```csharp
string path = Path.Combine(Directory.GetCurrentDirectory(), "chatData");
```

### 2️⃣ **Auto-Generation**
If no file exists, it automatically generates placeholder data:
```csharp
if (File.Exists(path))
{
    chatData = JsonConvert.DeserializeObject<ChatData>(File.ReadAllText(path));
}
if (chatData == null || chatData.messages.Count == 0)
{
    chatData = new ChatData();
    int max = 2; 
    ChatData.Message.Possibility<string> itm;
    List<Possibility<string>> replies = new List<Possibility<string>>();
    for (int i = 0; i < max; i++)
    {
        message = new ChatData.Message() { id = i.ToString(), baseContent = $"PLACEHOLDER_{i}" };
        chatData.messages.Add(message);
        itm = new ChatData.Message.Possibility<string>() { item = i.ToString() };
        replies.Add(itm);
    }
    for (int i = 0; i < max; i++)
    {
        message = chatData.messages[i];
        for (int j = 0; j < max; j++)
        {
            message.ifNothingToReply = replies;
            message.portentialReplies.Add(new ChatData.Message.PortentialReply());
            message.portentialReplies[message.portentialReplies.Count - 1].keyword = $"PLACEHOLDER_{i}";
            message.portentialReplies[message.portentialReplies.Count - 1].replies = replies;
        }
    }
    File.WriteAllText(path, JsonConvert.SerializeObject(chatData, Formatting.Indented));
    Console.WriteLine("Error! No Chat Data Found! Generated a new one! Press Enter Key To Shutdown!");
    Console.ReadLine();
    return;
}
```

### 3️⃣ **Message Indexing**
Messages are indexed by ID for quick lookup:
```csharp
Dictionary<string, ChatData.Message> keyValuePairs = new Dictionary<string, ChatData.Message>();
foreach (var item in chatData.messages)
{
    if (keyValuePairs.ContainsKey(item.id))
    {
        keyValuePairs[item.id] = item;
    }
    else
    {
        keyValuePairs.Add(item.id, item);
    }
}
```

### 4️⃣ **Conversation Loop**
The chat starts with the first message and continues:
```csharp
string id, content;
message = chatData.messages[0];
Console.WriteLine(message.baseContent);
while (message != null)
{
    content = Console.ReadLine();
    if (!string.IsNullOrEmpty(content) || !message.notNullOrEmpty)
    {
        bool flag = false;
        foreach (ChatData.Message.PortentialReply v in message.portentialReplies)
        {
            if (content.Contains(v.keyword) && (content == v.keyword || !v.matchCompletely))
            {
                flag = true;
                message = FindMessage(ChatData.Message.Possibility<string>.Random(v.replies));
                Console.WriteLine(message.baseContent);
            }
        }
        if (!flag && message.ifNothingToReply.Count > 0)
        {
            message = FindMessage(ChatData.Message.Possibility<string>.Random(message.ifNothingToReply));
            Console.WriteLine(message.baseContent);
        }
    }
}
Console.WriteLine("Error! No Message Found! Press Enter Key To Shutdown!");
Console.ReadLine();

ChatData.Message FindMessage(string id)
{
    if (keyValuePairs.ContainsKey(id))
    {
        return keyValuePairs[id];
    }
    return null;
}
```

### 5️⃣ **Keyword Matching Logic**
```csharp
foreach (PortentialReply v in message.portentialReplies)
{
    if (content.Contains(v.keyword) && (content == v.keyword || !v.matchCompletely))
    {
        // Found match!
        message = FindMessage(Possibility<string>.Random(v.replies));
    }
}
```

## 📁 JSON Data Format

Example of the generated JSON structure:

```json
{
  "messages": [
    {
      "id": "0",
      "baseContent": "PLACEHOLDER_0",
      "notNullOrEmpty": false,
      "portentialReplies": [
        {
          "keyword": "PLACEHOLDER_0",
          "matchCompletely": false,
          "replies": [
            { "item": "0" },
            { "item": "1" }
          ]
        }
      ],
      "ifNothingToReply": [
        { "item": "0" },
        { "item": "1" }
      ]
    },
    {
      "id": "1",
      "baseContent": "PLACEHOLDER_1",
      "notNullOrEmpty": false,
      "portentialReplies": [
        {
          "keyword": "PLACEHOLDER_1",
          "matchCompletely": false,
          "replies": [
            { "item": "0" },
            { "item": "1" }
          ]
        }
      ],
      "ifNothingToReply": [
        { "item": "0" },
        { "item": "1" }
      ]
    }
  ]
}
```

## 🛠️ Tech Stack

- **Development Tool**: Visual Studio 2022
- **Framework**: .NET 6.0 / .NET Framework 4.8
- **Language**: C#
- **Key Library**: Newtonsoft.Json (JSON.NET)
- **Type**: Console Application

## 📦 Installation & Usage

### Prerequisites
- Newtonsoft.Json NuGet package

### Build Instructions

```bash
# 1. Clone the repository
git clone https://github.com/yourusername/ChatDataSystem.git

# 2. Restore NuGet packages
dotnet restore

# 3. Build the project
dotnet build

# 4. Run the application
dotnet run
```

### First Run
On first launch, the system will:
1. Detect missing `chatData` file
2. Generate placeholder conversation data
3. Save to `chatData` JSON file
4. Start the conversation with message 0

## 💡 Customization Guide

### Creating Your Own Chat Data

1. **Modify the placeholder generation**:
```csharp
int max = 2; // Change this to add more messages
```

2. **Edit the generated JSON file**:
- Change message content
- Add new keywords
- Modify reply chains
- Set matchCompletely flags

3. **Custom message structure**:
```csharp
message = new ChatData.Message() 
{ 
    id = "custom_id", 
    baseContent = "Your message here",
    notNullOrEmpty = true  // Set to require non-empty input
};
```

## 🔧 Advanced Features

### Possibility System
The `Possibility<T>` class enables random selection:
```csharp
public static T Random(List<Possibility<T>> list)
{
    // Randomly selects an item
    // Can be extended for weighted probabilities
}
```

### Match Control
Control keyword matching precision with `matchCompletely`:
- `true`: Exact match required
- `false`: Partial match allowed (content.Contains)

## 🚨 Error Handling

The system includes built-in error handling:
- **No chat data**: Auto-generates and prompts user
- **Invalid message ID**: Returns null with error message
- **Empty input**: Respects `notNullOrEmpty` flag

## 🤝 Contributing

Feel free to enhance the system:
- Add weighted probabilities to Possibility class
- Implement more sophisticated matching algorithms
- Add support for multiple simultaneous conversations
- Create a GUI interface

## 📄 License

This project is open-sourced under the **MIT License**.

---

<p align="center">
  ⭐ Star this repo if you find it useful! ⭐
</p>
```
