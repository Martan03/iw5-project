namespace IW5Forms.Web.BL;

public partial class AnswerApiClient
{
    public AnswerApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
            BaseUrl = baseUrl;
    }
}
