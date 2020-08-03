using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;


namespace Microsoft.TrueCoderTest2
{
    public class QnABot : ActivityHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<QnABot> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public QnAMaker TrueBotQnA { get; private set; }
        public QnABot(QnAMakerEndpoint endpoint)
        {
            // connects to QnA Maker endpoint for each turn
            TrueBotQnA = new QnAMaker(endpoint);
        }
        public QnABot(IConfiguration configuration, ILogger<QnABot> logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        //private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    var results = await TrueBotQnA.GetAnswersAsync(turnContext);
        //    if (results.Any())
        //    {
        //        await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker Returned: " + results.First().Answer), cancellationToken);
        //    }
        //    else
        //    {
        //        await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
        //    }
        //}



        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {   
            var httpClient = _httpClientFactory.CreateClient();

            var qnaMaker = new QnAMaker(new QnAMakerEndpoint
            {
                KnowledgeBaseId = _configuration["67e21116-aade-4b8e-8aa3-4cf171905479"],
                EndpointKey = _configuration["3a9b23d2-34dd-41c5-83d7-c6ba8e0a6896"],
                Host = _configuration["https://customqa.azurewebsites.net/qnamaker"]
            },
            null,
            httpClient);

            _logger.LogInformation("Calling QnA Maker");

            var options = new QnAMakerOptions { Top = 1 };

            // The actual call to the QnA Maker service.
            var response = await qnaMaker.GetAnswersAsync(turnContext, options);
            if (response != null && response.Length > 0)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(response[0].Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
            }

            

        }
    }
}
