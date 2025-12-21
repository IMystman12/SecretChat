public class ChatData
{
    public List<Message> messages = new List<Message>();
    public class Message
    {
        public bool notNullOrEmpty;
        public string id;
        public string baseContent="PLACEHOLDER";
        public class Possibility<T>
        {
            public int weight = 100;
            public T item;
            public static T Random(List<Possibility<T>> list)
            {
                int num = 0, num1;
                foreach (var item1 in list)
                {
                    num += item1.weight;
                }
                num1 = new Random().Next(num);
                foreach (var item1 in list)
                {
                    if (num1 <= item1.weight)
                    {
                        return item1.item;
                    }
                    num1 -= item1.weight;
                }
                return default;
            }
        }
        public class PortentialReply
        {
            public bool matchCompletely;
            public string keyword;
            public List<Possibility<string>> replies = new List<Possibility<string>>();
        }
        public List<PortentialReply> portentialReplies = new List<PortentialReply>();
        public List<Possibility<string>> ifNothingToReply = new List<Possibility<string>>();
    }
}