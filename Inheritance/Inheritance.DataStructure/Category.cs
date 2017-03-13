using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Name { get; set; }
        public MessageType Type { get; set; }
        public MessageTopic Topic { get; set; }

        public Category(string name, MessageType type, MessageTopic topic)
        {
            Name = name;
            Type = type;
            Topic = topic;
        }

        public override string ToString()
        {
            return Name + "." + Type + "." + Topic;
        }

        public override bool Equals(object o)
        {
            if (!(o is Category)) return false;
            var second = (Category) o;
            return second.Name == Name && second.Type == Type && second.Topic == Topic;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + (int)Type * (int)Type + (int)Topic * (int)Topic * (int)Topic;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category)) throw new ArgumentException();
            var second = (Category) obj;

            if (Equals(second)) return 0;
            if (string.Compare(Name, second.Name, StringComparison.Ordinal) != 0)
                return string.Compare(Name, second.Name, StringComparison.Ordinal);
            if (Type != second.Type) return (int) Type - (int) second.Type;
            return (int) Topic - (int) second.Topic;
        }

        public static bool operator <= (Category first, Category second)
        {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >=(Category first, Category second)
        {
            return first.CompareTo(second) >= 0;
        }

        public static bool operator <(Category first, Category second)
        {
            return first.CompareTo(second) < 0;
        }

        public static bool operator >(Category first, Category second)
        {
            return first.CompareTo(second) > 0;
        }
    }
}
