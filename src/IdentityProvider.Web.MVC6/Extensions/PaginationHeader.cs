namespace IdentityProvider.Web.MVC6.Extensions;

public class PaginationHeader
{
    private int currentPage;
    private int itemsPerPage;
    private int totalItems;
    private int totalPages;

    public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        this.currentPage = currentPage;
        this.itemsPerPage = itemsPerPage;
        this.totalItems = totalItems;
        this.totalPages = totalPages;
    }
}