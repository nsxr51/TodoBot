using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telegram.Bot.Examples.Echo
{
    public enum ExpenseDirection
    {
        OutCome,
        InCome
    }

    public enum expanseType
    {
        CreditCard,
        Cash
    }
    public class Expenses
    {
        private static Expenses _instance;
        public static Expenses Instance
        {
            get
            {
                if (_instance == null) _instance = new Expenses();
                return _instance;
            }
        }
        public List<Expense> expenses;
        static List<string> _types;
        private Expenses()
        {
            expenses = new List<Expense>();
            if (!Directory.Exists("./Files")) Directory.CreateDirectory("./Files");
            if (!File.Exists("./Files/expanses.csv")) File.Create("./Files/expanses.csv");
            LoadExpenses();
            _types = new List<string>();
            _types.Add("קניות בסופר");
            _types.Add("קניות לבית");
            _types.Add("דלק");
            _types.Add("בילויים");
            _types.Add("חופשות");
            _types.Add("חינוך");
            _types.Add("חשבונות");
            _types.Add("בגדי ילדים");
            _types.Add("בגדי מבוגרים");
            _types.Add("העברה כספית");
            _types.Add("אירוע");
            _types.Add("הצגות ומופעים");
        }
        private void LoadExpenses()
        {
            var content = File.ReadAllLines("./Files/expanses.csv");
            foreach(var line in content)
            {
                string[] expen = line.Split(',');
                AddNewExpense(new Expense(expen[0], expen[1], expen[2], expen[4], expen[3]));
            }
        }
        public static string[] GetExpensType()
        {
            return _types.ToArray();
        }
        public void AddNewExpense(Expense expense)
        {
            expenses.Add(expense);
            var file = File.Open("./Files/expanses.csv", FileMode.Append);
            file.Write(expense.ToByteArray(), 0, expense.ToByteArray().Length);
            file.Close();
        }
        public void AddNewExpenseType(string title)
        {
            if (!_types.Contains(title)) _types.Add(title);
        }
        public override string ToString()
        {
            string strToret = string.Empty;
            foreach (var ex in expenses)
            {
                strToret += $"{ex.ToString()}\r\n";
            }
            return strToret;
        }
    }
    public class Expense
    {
        public DateTime Date;
        public double Amount;
        public ExpenseDirection Direction;
        public string Subject;
        public expanseType Type;
        public Expense(ExpenseDirection dir, expanseType type, string subject)
        {
            Date = DateTime.Now;
            Direction = dir;
            Subject = subject;
            Type = type;
        }
        public Expense(string date,string dir, string type,string amount, string subject)
        {
            Date = DateTime.Parse(date);
            Direction = dir == "הוצאה" ? ExpenseDirection.OutCome: ExpenseDirection.InCome;
            Subject = subject;
            Amount = double.Parse(amount);
            Type = type == "Cash" ? expanseType.Cash : expanseType.CreditCard;
        }
        public void SetAmout(double amount)
        {
            amount = Math.Abs(amount);
            if (Direction == ExpenseDirection.InCome)
            {
                Amount = amount;
            }
            else
            {
                Amount = amount * -1;
            }
        }

        public byte[] ToByteArray()
        {
            string dir = Direction == ExpenseDirection.OutCome ? "הוצאה" : "הכנסה";
            string str = $"{Date},{dir},{Type.ToString()},{Subject},{Amount}\r\n";
            return Encoding.UTF8.GetBytes(str);
        }

        public override string ToString()
        {
            string dir = Direction == ExpenseDirection.OutCome ? "הוצאה" : "הכנסה";
            return $"{Date},{dir},{Type.ToString()},{Subject},{Amount}";
        }
    }
}
