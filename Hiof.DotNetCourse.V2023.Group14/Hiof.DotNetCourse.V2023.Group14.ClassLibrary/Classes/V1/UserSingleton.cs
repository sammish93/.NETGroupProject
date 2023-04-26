using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class UserSingleton
    {
        private V1User _loggedInUser;
        private V1User _selectedUser;
        private string _searchQuery;
        private V1Book _selectedBook;
        private byte[] _userDisplayPicture;
        private byte[] _defaultDisplayPicture;
        private byte[] _selectedUserDisplayPicture;
        private bool _isUserLibraryAltered = false;
        private V1LibraryEntryWithImage _selectedEntry;

        public V1LibraryEntryWithImage SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
            }
        }

        public V1User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
            }
        }

        public V1User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
            }
        }

        public V1Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
            }
        }

        public byte[] UserDisplayPicture
        {
            get => _userDisplayPicture;
            set
            {
                _userDisplayPicture = value;
            }
        }

        public byte[] DefaultDisplayPicture
        {
            get => _defaultDisplayPicture;
            set
            {
                _defaultDisplayPicture = value;
            }
        }

        public byte[] SelectedUserDisplayPicture
        {
            get => _selectedUserDisplayPicture;
            set
            {
                _userDisplayPicture = value;
            }
        }

        public bool IsUserLibraryAltered
        {
            get => _isUserLibraryAltered;
            set
            {
                _isUserLibraryAltered = value;
            }
        }

        public UserSingleton() { }

    }
}


