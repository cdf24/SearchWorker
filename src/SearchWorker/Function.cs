using AdvertApi.Models.Messages;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SNSEvents;
using Nest;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SearchWorker
{
    public class Function
    {
        /*
        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Func<SNSEvent, ILambdaContext, string> func = FunctionHandler;
            using(var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new LambdaJsonSerializer()))
            using(var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FunctionHandler(string input, ILambdaContext context)
        {
            return input?.ToUpper();
        }
        */

        public Function() : this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        {

        }

        private readonly IElasticClient _client;

        public Function(IElasticClient client)
        {
            _client = client;
        }

        //public static string FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            //string input = string.Empty;

            foreach (var record in snsEvent.Records)
            {
                context.Logger.LogLine(record.Sns.Message);

                var message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
                var advertDocument = MappingHelper.Map(message);
                await _client.IndexDocumentAsync(advertDocument);

            }

            //return input?.ToUpper();
        }
    }
}
