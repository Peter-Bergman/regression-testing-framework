namespace RegressionTestingFramework;

public static class RegressionTestRunner
{
    // TODO: add documentation
    public static void RunTestSuite(
        Func<IEnumerable<RegressionTest>> dataBackend,
        Func<HttpRequestMessage, HttpRequestMessage> requestModifier,
        Func<HttpResponseMessage, HttpResponseMessage> responseModifier,
        Action<HttpResponseMessage, HttpResponseMessage> assertions
    ) {
        // Initialize an HTTP client
        HttpClient httpClient = new HttpClient();

        // Retrieve request and response data from the data backend.
        IEnumerable<RegressionTest> regressionTestsToRun = dataBackend();

        // Once we have the data, we can go through the main loop.
        foreach (RegressionTest regressionTest in regressionTestsToRun) {
            // Modify the request, if needed
            HttpRequestMessage modifiedRequest = requestModifier(regressionTest.request);
            // Modify response, if needed
            HttpResponseMessage expectedResponse = responseModifier(regressionTest.response);

            // Send the HttpRequestMessage to the server being tested
            HttpResponseMessage actualResponse = httpClient.Send(modifiedRequest);

            // Lastly, compare the expectedResponse and the actualResponse
            assertions(expectedResponse, actualResponse);
        }
    }
}

