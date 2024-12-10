namespace IW5Forms.Web.BL;

public partial class SearchApiClient
{
    public SearchApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
