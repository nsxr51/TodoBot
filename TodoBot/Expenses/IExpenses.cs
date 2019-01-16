using System;
using System.Collections.Generic;
using System.Text;

namespace TodoBot.Expenses
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
    interface IExpenses
    {
        void LoadExpenses();
        void AddNewExpense(Expense expense);
        void AddNewExpenseType(string title);
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
        public Expense(string date, string dir, string type, string amount, string subject)
        {
            Date = DateTime.Parse(date);
            Direction = dir == "הוצאה" ? ExpenseDirection.OutCome : ExpenseDirection.InCome;
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
