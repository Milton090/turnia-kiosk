using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TurniaKiosk.Helpers.Paging
{
    public partial class PagedItems<T> : ObservableObject
    {
        private List<T> _allItems = new();

        public int PageSize { get; }

        public ObservableCollection<T> PageItems { get; } = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasPrevious), nameof(HasNext), nameof(PageText))]
        [NotifyCanExecuteChangedFor(nameof(NextCommand), nameof(PreviousCommand))]
        private int currentPage = 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasPrevious), nameof(HasNext), nameof(PageText), nameof(ShowPager))]
        [NotifyCanExecuteChangedFor(nameof(NextCommand), nameof(PreviousCommand))]
        private int pageCount = 1;

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < PageCount;
        public bool ShowPager => PageCount > 1;

        public string PageText => $"Página {CurrentPage} de {PageCount}";
        public string PreviousText => "◀  Anterior";
        public string NextText => "Siguiente  ▶";

        public PagedItems(int pageSize)
        {
            PageSize = pageSize;
        }

        public void SetItems(IEnumerable<T> items)
        {
            _allItems = items.ToList();
            PageCount = Math.Max(1, (int)Math.Ceiling(_allItems.Count / (double)PageSize));
            CurrentPage = Math.Clamp(CurrentPage, 1, PageCount);
            RefreshPage();
        }

        [RelayCommand(CanExecute = nameof(HasNext))]
        private void Next()
        {
            CurrentPage++;
            RefreshPage();
        }

        [RelayCommand(CanExecute = nameof(HasPrevious))]
        private void Previous()
        {
            CurrentPage--;
            RefreshPage();
        }

        private void RefreshPage()
        {
            PageItems.Clear();
            foreach (var item in _allItems.Skip((CurrentPage - 1) * PageSize).Take(PageSize))
                PageItems.Add(item);
        }
    }
}
