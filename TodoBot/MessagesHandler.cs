using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.Echo
{
    public delegate void newExpenseEventHandler(Expense exp);
    public delegate void newExpenseTypeEventHandler(string expenseTitle);
    public enum MessgesStatus
    {
        Begining,
        NewExpense,
        CreditCardExpans,
        ChushExpans,
        InsertAmount,
        AddNewExpensType,
        InsertExpensCategory,
        GroceryList,
        AddGroceryList,
        DeleteGroceryList
    }
    public class MessagesHandler
    {
        private Dictionary<long, Message> _messages;
        private Expenses _expenses;
        public MessagesHandler()
        {
            _messages = new Dictionary<long, Message>();
            _expenses = Expenses.Instance;
            Message.NewExpenseEvent += Message_NewExpenseEvent;
            Message.NewExpenseTypeEvent += Message_NewExpenseTypeEvent;
        }

        private void Message_NewExpenseTypeEvent(string expenseTitle)
        {
            _expenses.AddNewExpenseType(expenseTitle);
        }

        private void Message_NewExpenseEvent(Expense exp)
        {
            _expenses.AddNewExpense(exp);
        }

        public bool IsMessageInDictionary(long msgId)
        {
            return _messages.ContainsKey(msgId);
        }
        public void AddMessageToList(long id)
        {
            _messages.Add(id, new Message(id));
        }

        public Message GetMessageFromDictionary(long id)
        {
            return _messages[id];
        }
    }

    public class Message
    {
        public static Expenses _expenses = Expenses.Instance;
        public static event newExpenseEventHandler NewExpenseEvent;
        public static event newExpenseTypeEventHandler NewExpenseTypeEvent;
        long msgId;
        double amount = 0.0;
        MessgesStatus status;
        string expanseTitle;
        Expense expense;
        public Message(long id)
        {
            msgId = id;
        }
        public void ParseMessage(string msg, TelegramBotClient Bot)
        {
            if (msg == "?") status = MessgesStatus.Begining;
            switch (status)
            {
                case MessgesStatus.AddGroceryList:
                case MessgesStatus.DeleteGroceryList:
                case MessgesStatus.GroceryList:
                    HandleGroceryListMessage(msg, Bot);
                    break;
                case MessgesStatus.Begining:
                    HandleBeginingMessage(msg, Bot);
                    break;
                case MessgesStatus.NewExpense:
                    HandleNewExpenseMessage(msg, Bot);
                    break;
                case MessgesStatus.InsertExpensCategory:
                    expense.Subject = msg;
                    status = MessgesStatus.InsertAmount;
                    Bot.SendTextMessageAsync(
        msgId,
         "אנא הכנס סכום",
          replyMarkup: new ReplyKeyboardRemove());
                    break;
                case MessgesStatus.InsertAmount:
                    HandleInsertAmountMessage(msg, Bot);
                    break;
                case MessgesStatus.AddNewExpensType:
                    status = MessgesStatus.Begining;
                    NewExpenseTypeEvent.Invoke(msg);
                    HandleBeginingMessage("?", Bot);
                    break;
            }

        }

        private void HandleGroceryListMessage(string msg, TelegramBotClient bot)
        {
            switch (status)
            {
                case MessgesStatus.GroceryList:
                    switch (msg)
                    {
                        case "הוסף":
                            status = MessgesStatus.AddGroceryList;
                            ReplyKeyboardMarkup ReplyGroceryKeyboardDefult = new[]
     {
                        new[] { "סיום" },
                   };
                            bot.SendTextMessageAsync(
                   msgId,
                    "רשום את המוצרים אחד אחד" +
                    "לסיום לחץ על סיום או רשום סיום",
                    replyMarkup: ReplyGroceryKeyboardDefult);
                            break;
                        case "הסר":
                            status = MessgesStatus.DeleteGroceryList;
                            break;
                        case "הצג":
                            var types = GroceryList.Instance.GetList().ToArray();
                            List<string[]> list = new List<string[]>();
                            var completeRow = types.Length / 3;
                            var restRow = types.Length % 3;
                            for (int i = 0; i < completeRow; i++)
                            {
                                list.Add(new[] { types[(i)], types[(i) + 1], types[(i) + 2] });
                            }
                            if (restRow == 1)
                            {
                                list.Add(new[] { types[(completeRow * 3)] });
                            }
                            if (restRow == 2)
                            {
                                list.Add(new[] { types[(completeRow * 3)], types[(completeRow * 3) + 1] });
                            }
                            list.Add(new[] { "סיום" });
                            ReplyKeyboardMarkup ReplyKeyboardnewExpans = list.ToArray();
                            bot.SendTextMessageAsync(
                            msgId,
                             "לחץ על אחד המוצרים כדי לסמן שנקרא או ע סיום לחזרה לתפריט",
                              replyMarkup: ReplyKeyboardnewExpans);
                            break;
                        case "סיום":
                            status = MessgesStatus.Begining;
                            HandleBeginingMessage("?", bot);
                            break;
                        default:
                            if (GroceryList.Instance.GetList().Contains(msg))
                            {
                                GroceryList.Instance.Remove(msg);
                                types = GroceryList.Instance.GetList().ToArray();
                                list = new List<string[]>();
                                completeRow = types.Length / 3;
                                restRow = types.Length % 3;
                                for (int i = 0; i < completeRow; i++)
                                {
                                    list.Add(new[] { types[(i)], types[(i) + 1], types[(i) + 2] });
                                }
                                if (restRow == 1)
                                {
                                    list.Add(new[] { types[(completeRow * 3)] });
                                }
                                if (restRow == 2)
                                {
                                    list.Add(new[] { types[(completeRow * 3)], types[(completeRow * 3) + 1] });
                                }
                                list.Add(new[] { "סיום" });
                                ReplyKeyboardMarkup ReplyKeyboardProductsnewExpans = list.ToArray();
                                bot.SendTextMessageAsync(
msgId,
$"{msg} הוסר בהצלחה",
replyMarkup: ReplyKeyboardProductsnewExpans);
                            }
                            break;
                    }
                    break;
                case MessgesStatus.AddGroceryList:
                    if (msg == "סיום")
                    {
                        HandleBeginingMessage("רשימת קניות", bot);
                        return;
                    }
                    GroceryList.Instance.Add(msg);
                    bot.SendTextMessageAsync(
msgId,
$"{msg} נוסף בהצלחה",
 replyMarkup: new ReplyKeyboardRemove());
                    break;
                case MessgesStatus.DeleteGroceryList:
                    GroceryList.Instance.Remove(msg);
                    break;
                default:
                    break;
            }
        }

        private void HandleInsertAmountMessage(string msg, TelegramBotClient bot)
        {
            double sum = 0.0;
            if (double.TryParse(msg, out sum))
            {
                expense.SetAmout(sum);
                status = MessgesStatus.Begining;
                NewExpenseEvent?.Invoke(expense);
                bot.SendTextMessageAsync(
msgId,
"תודה רבה נרשם בהצלחה");
                HandleBeginingMessage("?", bot);
            }
            else
            {
                bot.SendTextMessageAsync(
      msgId,
       "חלה שגיאה בקבלת הסכום אנא הקלד סכום חדש או ? להתחיל מהתחלה");
                return;
            }

        }
        private void HandleNewExpenseMessage(string msg, TelegramBotClient bot)
        {
            status = MessgesStatus.InsertExpensCategory;
            switch (msg)
            {
                case "מזומן":
                    expense = new Expense(ExpenseDirection.OutCome, expanseType.Cash, "");
                    break;
                case "אשראי":
                    expense = new Expense(ExpenseDirection.OutCome, expanseType.CreditCard, "");
                    break;
            }
            int step = 3;
            var types = Expenses.GetExpensType();
            List<string[]> list = new List<string[]>();
            var completeRow = types.Length / 3;
            var restRow = types.Length % 3;
            for (int i = 0; i < completeRow; i++)
            {
                list.Add(new[] { types[(i)], types[(i) + 1], types[(i) + 2] });
            }
            if (restRow == 1)
            {
                list.Add(new[] { types[(completeRow * 3)] });
            }
            if (restRow == 2)
            {
                list.Add(new[] { types[(completeRow * 3)], types[(completeRow * 3) + 1] });
            }
            ReplyKeyboardMarkup ReplyKeyboardnewExpans = list.ToArray();
            bot.SendTextMessageAsync(
            msgId,
             "קטגוריה בבקשה ?",
              replyMarkup: ReplyKeyboardnewExpans);
            return;
        }
        private void HandleBeginingMessage(string msg, TelegramBotClient bot)
        {
            switch (msg)
            {
                case "רשימת קניות":
                    status = MessgesStatus.GroceryList;
                    ReplyKeyboardMarkup ReplyGroceryKeyboardDefult = new[]
                         {
                        new[] { "הוסף","הסר","הצג" },
                        new[] { "סיום" },
                   };
                    bot.SendTextMessageAsync(
           msgId,
            "ברוך הבא לרשימת הקניות שלך.." +
            "אנא בחרי הוסף להוספת מוצר ת הסר להסרת מוצר " +
            "הצג להצגת רשימת הקניות וסיום לחזור לתפריט הראשי",
            replyMarkup: ReplyGroceryKeyboardDefult);
                    break;
                case "הצג הוצאות":
                    bot.SendTextMessageAsync(
                            msgId,
                            _expenses.ToString()
                    );
                    break;
                case "הוצאה חדשה":
                    status = MessgesStatus.NewExpense;
                    ReplyKeyboardMarkup ReplyKeyboardnewExpans = new[] { "אשראי", "מזומן" };
                    ReplyKeyboardnewExpans.OneTimeKeyboard = false;
                    ReplyKeyboardnewExpans.ResizeKeyboard = true;
                    bot.SendTextMessageAsync(
                         msgId,
                        "אנא בחר סוג",
                        replyMarkup: ReplyKeyboardnewExpans);
                    break;
                case "הוסף סוג הוצאה":
                    status = MessgesStatus.AddNewExpensType;
                    bot.SendTextMessageAsync(
           msgId,
            "הכנס שם הוצאה",
             replyMarkup: new ReplyKeyboardRemove());
                    return;
                default:
                    ReplyKeyboardMarkup ReplyKeyboardDefult = new[]
                               {
                        new[] { "הוצאה חדשה", "הצג הוצאות","הוצאה קבועה חדשה" },
                        new[] { "כמה נשאר לי לבזבז.. ?", "האם חרגתי ? ","הוסף סוג הוצאה" },
                              new[] { "רשימת קניות" },
                    };

                    bot.SendTextMessageAsync(
                       msgId,
                       "אנא בחר",
                       replyMarkup: ReplyKeyboardDefult);
                    break;
            }
        }
    }
}
