using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        public ObservableCollection<V1Book> Books { get; set; }
        public V1User LoggedInUser { get; set; }

        public MainPageViewModel()
        {
            Books = new ObservableCollection<V1Book>();
            LoggedInUser = App.LoggedInUser;
        }

        //public ICommand PopulateBooksCommand => new Command(async () => await populateBooks());

        public async Task PopulateBooks()
        {
            
            try
            {
                string loginUrl = $"{_apiBaseUrl}/books/GetBookByCategory?subject=fantasy";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null)
                    {
                        continue;
                    }

                    book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                    book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                    Books.Add(book);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
