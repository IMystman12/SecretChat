using Newtonsoft.Json;
using static ChatData.Message;

string path = Path.Combine(Directory.GetCurrentDirectory(), "chatData");
ChatData chatData = null; ChatData.Message message;
if (File.Exists(path))
{
    chatData = JsonConvert.DeserializeObject<ChatData>(File.ReadAllText(path));
}
if (chatData == null || chatData.messages.Count == 0)
{
    chatData = new ChatData();
    int max = 2; ChatData.Message.Possibility<string> itm;
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