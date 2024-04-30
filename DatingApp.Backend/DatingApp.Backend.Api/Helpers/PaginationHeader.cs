using DatingApp.Backend.Application.Helpers;

namespace DatingApp.Backend.Api.Helpers;

public class PaginationHeader(IPagedList pagedList)
{
    public int CurrentPage { get; set; } = pagedList.CurrentPage;
    public int ItemsPerPage { get; set; } = pagedList.PageSize;
    public int TotalItems { get; set; } = pagedList.TotalCount;
    public int TotalPages { get; set; } = pagedList.TotalPages;
}