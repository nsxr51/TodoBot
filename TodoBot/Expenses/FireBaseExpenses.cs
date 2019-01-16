using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoBot.Expenses
{
    class FireBaseExpenses : IExpenses
    {
        private readonly IExpenses _ExpensesService;

        FirebaseClient firebaseClient;
        public FireBaseExpenses()
        {
            Login();
        }
        public async Task Login()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDK--Gl2kbBBDnqJqUvXliw83oCG3-5dWA"));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync("mailtolior@gmail.com", "123456");
            Console.WriteLine(auth.User.LocalId);
            firebaseClient = new FirebaseClient("https://refundwallet.firebaseio.com/", new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
            });
            LoadExpensesAsync();
        }
        public void AddNewExpense(Expense expense)
        {
            throw new NotImplementedException();
        }

        public void AddNewExpenseType(string title)
        {
            throw new NotImplementedException();
        }

        public async Task LoadExpensesAsync()
        {
            try
            {
                var y = await firebaseClient.Child("refundwallet").OnceAsync<dynamic>();
                foreach (var dino in y)
                {
                    Console.WriteLine($"{dino.Key} is {dino.Object.Height}m high.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void LoadExpenses()
        {
            LoadExpensesAsync();
        }
    }
}
