# twitter-statistics
1. You will need at least .NET Core 3.1.413 installed (https://dotnet.microsoft.com/download/dotnet/3.1)
2. You will need to get your bearer token for the Twitter API
3. Add said bearer token to appsettings.Development.json in the Twitter.API project under the TwitterConfig.BearerToken property.
4. At this point, you should be able to run the project in visual studio. If not running in VS you will need to set the ASPNETCORE_ENVIRONMENT variable to Development on your local machine.

There are 5 endpoints
1. GET analytics/tweet/total - This will return a 200OK and the total number of tweets the API has processed.
2. GET analytics/tweet/count - This will return a 200OK and the average tweet count per minute. It's important to note that this takes into account the current minute as well and will skew results with smaller datasets. It would be trivial to exclude the current minute if that's needed.
3. GET analytics/tweet/length - This will return a 200OK and the average tweet text length.
4. GET analytics/hashtag/top - This will return a 200OK and the top 10 hashtags in descending order.
5. GET health/ - This will return a 200OK if the API is up and accessible.
6. GET health/twitter - This will return a 200OK if the API can establish a succesful authorized connection to the Twitter API. 
