using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Examples.Echo
{
    public class GroceryList
    {
        private static GroceryList _instance;
        public static GroceryList Instance
        {
            get
            {
                if (_instance == null) _instance = new GroceryList();
                return _instance;
            }
        }
        List<string> _list;
        List<string> _products;
        private GroceryList()
        {
            _list = new List<string>();
            InitProducts();
        }

        private void InitProducts()
        {
            _products = new List<string>();
            _products.Add("לחם/פיתות");
            _products.Add("אורז");
            _products.Add("פסטה");
            _products.Add("פתיתים");
            _products.Add("איטריות");
            _products.Add("בורגול");
            _products.Add("קוסקוס");
            _products.Add("קמח");
            _products.Add("סולת");
            _products.Add("אבקת אפיה");
            _products.Add("סוכר");
            _products.Add("סוכר וניל");
            _products.Add("מלח");
            _products.Add("פירורי לחם");
            _products.Add("קיטניות (חומוס, עדשים, תירס, שעועית)");
            _products.Add("דגני בוקר");
            _products.Add("תה");
            _products.Add("קפה");
            _products.Add("שוקולד");
            _products.Add("ריבה");
            _products.Add("דבש");
            _products.Add("תבלינים");
            _products.Add("חטיפים");
            _products.Add("טחינה גולמית");
            _products.Add("קקאו");
            _products.Add("אבקת מרק עוף");
            _products.Add("קוואקר");
            _products.Add("עוגות");
            _products.Add("עוגיות וופלים");
            _products.Add("פירות יבשים ואגוזים");
            _products.Add("פיצוחים");
            _products.Add("תחליף חלב");
            _products.Add("חיתולים");
            _products.Add("מגבונים");
            _products.Add("דייסה");
            _products.Add("גרבר");
            _products.Add("כלים חד פעמייםצלחות, כוסות, סכו");
            _products.Add("מפות חד פעמיות");
            _products.Add("שקיות לסנדויצ'ים");
            _products.Add("שקיות אשפה");
            _products.Add("ניילון נצמד");
            _products.Add("נייר אלומיניום");
            _products.Add("תבניות לתנור");
            _products.Add("מטליות לניקוי");
            _products.Add("שקיות קוקיז");
            _products.Add("נייר אפיה");
            _products.Add("מפיות נייר");
            _products.Add("נייר טואלט");
            _products.Add("נרות שבת");
            _products.Add("גפרורים");
            _products.Add("סקוטש (שקוטש לשבת)");
            _products.Add("עוף שלם");
            _products.Add("חלקי עוף אהובים");
            _products.Add("בשר בקר");
            _products.Add("בשר טחון");
            _products.Add("נקניקים");
            _products.Add("נקניקיות");
            _products.Add("דגים");
            _products.Add("שניצל");
            _products.Add("צ'יפס");
            _products.Add("מלוואח");
            _products.Add("ג'חנון");
            _products.Add("קטניות קפואות (אפונה, גזר, פול)");
            _products.Add("בצק עלים");
            _products.Add("פיצה");
            _products.Add("בורקס");
            _products.Add("גלידה");
            _products.Add("ביצים");
            _products.Add("חלב");
            _products.Add("שוקו");
            _products.Add("חמאה");
            _products.Add("מרגרינה");
            _products.Add("גבינות");
            _products.Add("חומוס");
            _products.Add("טחינה");
            _products.Add("מטבוחה");
            _products.Add("כרוב סגול");
            _products.Add("חצילים (במיונז/קלוי)");
            _products.Add("מים");
            _products.Add("קולה");
            _products.Add("מיצים");
            _products.Add("בירה");
            _products.Add("פטל");
            _products.Add("יין");
            _products.Add("שתיה חריפה");
            _products.Add("שמן");
            _products.Add("שמן זית");
            _products.Add("חומץ");
            _products.Add("שמפו ומרכך");
            _products.Add("סבון");
            _products.Add("דאודורנט");
            _products.Add("תחבושות");
            _products.Add("סכיני גילוח");
            _products.Add("צמר גפן");
            _products.Add("מקלות לאוזניים");
            _products.Add("קרם גוף/ידיים/שיער");
            _products.Add("ג'ל לשיער");
            _products.Add("מלפפון");
            _products.Add("עגבניה (רגילה/שרי/תמר)");
            _products.Add("תפוח");
            _products.Add("אגס");
            _products.Add("תפוח אדמה");
            _products.Add("בצל");
            _products.Add("לימון");
            _products.Add("פלפל");
            _products.Add("גזר");
            _products.Add("בטטה");
            _products.Add("סלק");
            _products.Add("קישואים");
            _products.Add("דלורית");
            _products.Add("בננה");
            _products.Add("חצילים");
            _products.Add("דלעת");
            _products.Add("תפוזים");
            _products.Add("קלמנטינות");
            _products.Add("אשכולית");
            _products.Add("צנון");
            _products.Add("לפת");
            _products.Add("קולורבי");
            _products.Add("שום");
            _products.Add("אבוקדו");
            _products.Add("שומר");
            _products.Add("קיווי");
            _products.Add("אפרסק");
            _products.Add("שזיף");
            _products.Add("אבטיח");
            _products.Add("ענבים");
            _products.Add("מלון");
            _products.Add("מנגו");
            _products.Add("ענבים");
            _products.Add("ירק (פטרוזילה, כוסברה, נענע, שמיר)");
            _products.Add("חסה");
            _products.Add("סלרי");
            _products.Add("פטריות");
            _products.Add("זיתים");
            _products.Add("מלפפון חמוץ");
            _products.Add("טונה");
            _products.Add("סרדינים");
            _products.Add("תירס");
            _products.Add("מיונז");
            _products.Add("קטשופ");
            _products.Add("חרדל");
            _products.Add("רסק עגבנית");
            _products.Add("פטריות");
            _products.Add("אבקת כביסה");
            _products.Add("אבקה למדיח");
            _products.Add("מלח למדיח");
            _products.Add("סבון לניקוי כלים");
            _products.Add("נוזל לניקוי רצפות");
            _products.Add("אקונומיקה");
            _products.Add("כחול לחלונות");
            _products.Add("ספריי לשירותים");
            _products.Add("ספריי נגד חרקים");
        }

        public void Add(string item)
        {
            _list.Add(item);
        }
        public void Remove(string item)
        {
            _list.Remove(item);
        }

        public List<string> GetList()
        {
            _list.Sort();
            return _list;
        }
        public List<string> Getproducts()
        {
            _products.Sort();
            return _products;
        }
    }
}
