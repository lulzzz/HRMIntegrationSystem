Usage:
1. Register service from Common.Api.ProxyClient in your project with url of Common project
example:
services.AddCommonHttpClient("Http://localhost:5000");
2. Use ICommonProxyClient injected from constructor and get data
example:
 _commonProxyClient.GetUnits();