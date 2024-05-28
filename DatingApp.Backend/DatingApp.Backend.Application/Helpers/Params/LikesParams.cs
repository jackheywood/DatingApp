namespace DatingApp.Backend.Application.Helpers.Params;

public class LikesParams : PaginationParams
{
    public int UserId { get; set; }
    public string Predicate { get; set; }
}