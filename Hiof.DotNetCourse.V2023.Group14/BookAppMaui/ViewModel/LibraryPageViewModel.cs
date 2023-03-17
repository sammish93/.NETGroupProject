using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;


namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    internal class LibraryPageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";

       // public ObservableCollection<V1LibraryEntry> ReadBooks { get; set; }

        private V1User _user;
        private Guid _userId;

        
        public V1User User { get => _user; set => _user = value; }
        public Guid UserId { get => _userId; set => _userId = value; }


        /*
        public LibraryPageViewModel()
        {
            ReadBooks= new ObservableCollection<V1LibraryEntry>();
        }
       
        public ICommand PopulateReadBooksCommand => new Command(async () => await PopulateReadBooks());

        

        public async Task PopulateReadBooks()
        {
            try
            {
                string loginUrl = $"{_apiBaseUrl}//GetUserLibrary?userId={UserId}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var book = new V1LibraryEntry();
                if(book.ReadingStatus is ClassLibrary.Enums.V1.ReadingStatus.Completed)
                {
                    ReadBooks.Add(book);
                }
            }
            catch(Exception ex) 
            {

            }
        }
        
        */
    }
}
